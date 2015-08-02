using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using NetLayer;
using System.Threading;
using WebRealty.Models;
using RealtyDomainObjects;
using System.Collections;
using HtmlParser.DBLayer;
using HtmlParser.Infrastructure;

namespace HtmlParser.Crawler
{
    public class EntryPoint
    {
        private Queue<Instruction> instructions { get; set; }
        private readonly object instructionLocker;

        private Queue<Instruction> linksToObjects = new Queue<Instruction>();//содержит ссылку на объект а также инструкцию принадлежности
        private readonly object linksToObjectLocker;
        private readonly object linksToObjectLimitLocker;
        private const int LimitList = 100;
        private const int minLimitList = 50;

        private LinkLogger linkLogger;
        private Hashtable processedLinks;//список обработанных линков
        private readonly object processedLinksLocker;

        EventLogger eventLog = new EventLogger();//логер событий


        private Queue<Entity> entities = new Queue<Entity>();
        private readonly object entitiesLocker;

        Hash hash;
        private readonly object hashLocker;

        public EntryPoint(string pathToInstructions)
        {
            eventLog.AddEventToLog("Инициализация");
            instructionLocker = new object();
            linksToObjectLocker = new object();
            collectEntityLock = new object();
            entitiesLocker = new object();
            dbLoaderLock = new object();
            linksToObjectLimitLocker = new object();
            hashLocker = new object();
            processedLinksLocker = new object();

            FileAvailability fileChecker = new FileAvailability();
            string errMsg = string.Empty;

            //проверяем доступность требуемых файлов
            if (!fileChecker.CheckFilePaths(pathToInstructions, out errMsg))
            {
                throw new FileNotFoundException(errMsg);
            }

            linkLogger = new LinkLogger(fileChecker.GetLogLinksFile());//создаём логер обработанных линков
            Console.WriteLine("Restoring processed links from file....");
            processedLinks = linkLogger.GetLinksFromLog();//восстанавливаем линки из файла
            
            eventLog.AddEventToLog("Восстановленно обработанных ссылок" + processedLinks.Count.ToString());

            Console.WriteLine("Restoring hashes....");

            hash = new Hash(pathToInstructions + fileChecker.GetStopWordsFile(),
                            pathToInstructions + fileChecker.GetFileCacheHash(), 10);// хранитель/обработчик дубликатов


            Console.WriteLine("Loading instructions");
            this.instructions = new Queue<Instruction>();
            string path = pathToInstructions + "\\" + "instructions.xml";
            if (!File.Exists(path))
                throw new FileNotFoundException("instructions.xml not found  in path: " + pathToInstructions);





        }




        private readonly object collectEntityLock;
        private readonly object dbLoaderLock;

        Thread t1 = null;
        Thread t2 = null;
        Thread t3 = null;
        public void Run()
        {
            List<Instruction> dataForVerification = new List<Instruction>();


            #region populate instructions
            XmlDocument doc = new XmlDocument();
            doc.Load("instructions.xml");
            XmlElement root = doc.DocumentElement;
            var nodes = root.SelectSingleNode("/Instructions");
            foreach (XmlNode node in nodes)
            {
                var instruction = new Instruction();
                instruction.host = node["Host"].InnerText;
                instruction.link = node["Link"].InnerText;
                instruction.propertyAction = node["PropertyAction"].InnerText;
                instruction.propertyType = node["PropertyType"].InnerText;
                instruction.city = node["City"].InnerText;
                instruction.district = node["District"].InnerText;

                this.instructions.Enqueue(instruction);
                dataForVerification.Add(instruction);
            }
            eventLog.AddEventToLog("Прочитано инструкций: " + this.instructions.Count.ToString());

            #endregion

            #region validate instructions coparing with DB values
            if (!this.ValidateData(dataForVerification))
            {
                Console.ReadKey();
            }
            dataForVerification = null;
            #endregion


            Template t = new Template("template1.txt");
            t.HOST = "http://fn.ua";
            t1 = new Thread(CollectLinksToObjects);
            t1.Start(t);

            t2 = new Thread(CollectEntities);
            t2.Start(t);

            t3 = new Thread(StoreEntity);
            t3.Start();

            t1.IsBackground = true;
            t2.IsBackground = true;
            t3.IsBackground = true;

            t1.Join();
            t2.Join();
            t3.Join();



            Console.WriteLine("All treads were terminated");

            Console.WriteLine("сохраняем существующие хеши.....");
            hash.StoreCache();

            eventLog.AddEventToLog("Работа завершена.");
            eventLog.AddEventToLog("-------------------------------------------------------------------");
            eventLog.TerminateLog();
            Console.WriteLine("Нажмите любую клавишу для окончания работы паука");
            Console.ReadKey();//remove it

            //CollectLinksToObjects(t);
            //CollectEntities(t);
            //StoreEntity();
        }

        private void StoreEntity()
        {


            lock (dbLoaderLock)
            {
                Monitor.Wait(dbLoaderLock);
            }

            RealtyDb _db = new RealtyDb();
            bool stopLoop = false;
            Entity ent = null;
            int countStoredObjects = 0;

            var city = (from s in _db.Cities
                        select s).ToArray<City>();

            var district = (from s in _db.CityDistricts.Include("City")
                            select s).ToList<CityDisctict>();

            var propertyTypes = (from s in _db.PropertyTypes
                                 select s).ToList<PropertyType>();

            var propertyActions = (from s in _db.PropertyActions
                                   select s).ToList<PropertyAction>();

            var propertyObjects = (from s in _db.PropertyObjects
                                   select s).ToList<PropertyObject>();

            var periods = (from s in _db.Periods
                           select s).ToList<Periods>();

            var buildingTypes = (from s in _db.BuildingTypes
                                 select s).ToList<BuildingType>();

            var wcTypes = (from s in _db.WCTypes
                           select s).ToList<WCType>();

            var currencyTypes = (from s in _db.CurrencyTypes
                                 select s).ToList<CurrencyType>();

            var priceForTypes = (from s in _db.PriceForTypes
                                 select s).ToList<PriceForType>();

            var landCommunications = (from s in _db.LandCommunications
                                      select s).ToList<LandCommunication>();

            var landFunctions = (from s in _db.LandFunctions
                                 select s).ToList<LandFunction>();

            var commercialPropertyTypes = (from s in _db.CommercialPropertyTypes
                                           select s).ToList<CommercialPropertyType>();

            var serviceTypes = (from s in _db.ServiceTypes
                                select s).ToList<ServiceType>();


            _db.Dispose();
            _db = null;

            string connStr = "server=localhost; Port =3306; user id=root; password=mysqlPass; database=RealtyDb; pooling=true; CharSet=utf8; Connection Timeout=10000; Protocol=socket;";
            SqlObjectManipulation sql = new SqlObjectManipulation(connStr);
            WebRealty.Common.ImageProcessing imgHelper = new WebRealty.Common.ImageProcessing();

            DataValidator validate = new DataValidator();
            //int countInstructions = 0;
            //int countLinksToObject = 0;
            int countEntities = 0;
            int tmpInt = -1;
            do
            {
                lock (entitiesLocker)
                {
                    if (entities.Count > 0)
                        ent = entities.Dequeue();
                    countEntities = entities.Count;
                }

                //if (countEntities < minLimitList)
                //{
                //    lock (collectEntityLock)
                //    {
                //        Monitor.PulseAll(collectEntityLock);
                //    }
                //}



                //lock (instructionLocker)//получаем количество доступных инструкций
                //{
                //    countInstructions = instructions.Count();
                //}

                if (ent == null && t2.IsAlive)//засыпаем, пока нес не позовут
                {
                    eventLog.AddEventToLog("StoreEntity- сслыку на объект получить не удалось, поток собиратель объетов еще живой- засыпаем");
                    lock (dbLoaderLock)
                    {
                        Monitor.Wait(dbLoaderLock);
                    }
                    continue;
                }

                if (ent != null)
                {
                    var houseTypeObj = (from s in buildingTypes.AsEnumerable()
                                        where s.BuildingTypeName.ToLower() == ent.houseType.ToLower().Trim()
                                        select s).SingleOrDefault<BuildingType>();

                    var wcTypeObj = (from s in wcTypes.AsEnumerable()
                                     where s.WCTypeName.ToLower() == ent.wctype.ToLower().Trim()
                                     select s).SingleOrDefault<WCType>();

                    var currencyObj = (from s in currencyTypes
                                       where s.CurrencyTypeName.ToLower() == (ent.currency != null ? ent.currency.ToLower().Trim() : "у.е")
                                       select s).SingleOrDefault<CurrencyType>();

                    PriceForType priceForObj = null;
                    if (ent.priceFor != null)
                        priceForObj = (from s in priceForTypes
                                       where s.PriveForTypeName.ToLower().Replace(".", "") == ent.priceFor.ToLower().Trim().Replace(".", "")
                                       select s).FirstOrDefault<PriceForType>();

                    var cityObj = (from s in city
                                   where s.CityName.ToLower() == ent.City.ToLower().Trim()//"Киев".ToLower()//
                                   select s).SingleOrDefault<City>();

                    var districtObj = (from s in district
                                       where s.District.ToLower() == ent.District.ToLower().Trim()//"Голосеевский р-н.".ToLower()//
                                       && s.City.Id == cityObj.Id
                                       select s).SingleOrDefault<CityDisctict>();

                    var propertyTypeObj = (from s in propertyTypes
                                           where s.PropertyTypeName.ToLower() == ent.propertyType.ToLower().Trim()//"Квартиры".ToLower()//
                                           select s).SingleOrDefault<PropertyType>();

                    var propertyActionObj = (from s in propertyActions
                                             where s.PropertyActionName.ToLower() == ent.propertyAction.ToLower().Trim()//"Продажа".ToLower()//
                                             && s.PropertyType.Id == propertyTypeObj.Id
                                             select s).SingleOrDefault<PropertyAction>();

                    PropertyObject po = new PropertyObject()
                    {
                        BalconAvailable = ent.balconyAvailable,
                        BalconSpace = ent.balconySize,
                        BuildingTypeName = houseTypeObj,
                        City = cityObj,
                        CityDistrict = districtObj,
                        CommercialPropertyType = null,
                        ContactName = ent.contactName,
                        CountFloors = ent.countFloors, //???????
                        CountPhotos = (ent.photos != null ? ent.photos.Count() : 0),//ent.LinkToPhotos.Count(),
                        //CreatedDate=
                        Currency = currencyObj,
                        DistanceToCity = ent.distanceToCity,
                        Floor = ent.floor,
                        IsActive = true,
                        isBalconGlassed = ent.isBalconyGlassed,
                        IsNewBuilding = ent.isNewBuilding,
                        KitchenSpace = ent.kitchenSize,
                        LivingSpace = ent.livingSize,
                        NoCommission = ent.noComission,
                        //Periods=
                        Phone1 = ent.phone1,
                        Price = ent.price,
                        PriceForTypeName = priceForObj,
                        PropertyAction = propertyActionObj,
                        PropertyDescription = ent.description,
                        PropertyType = propertyTypeObj,
                        RoomCount = ent.roomsCount,
                        ServiceType = null,
                        Title = ent.title,
                        TotalSpace = ent.allSize,
                        WCType = wcTypeObj,
                        LinkOfObjectGrab = ent.linkToOriginalObject

                    };



                    if (po.CountFloors <= 0)
                        po.CountFloors = (int)ent.houseCountFloor;


                    if (po.LivingSpace <= 0)
                        po.LivingSpace = ent.houseGardenSize;

                    if (po.TotalSpace <= 0)
                        po.TotalSpace = ent.houseSize;

                    if (po.TotalSpace <= 0)
                        po.TotalSpace = ent.commercialObjectSize;




                    var datePart = ent.CreatedDate.Split('-');
                    if (datePart != null && datePart.Length > 1)
                    {
                        var dateSplitted = datePart[0].Split('.');
                        var timeSplitted = datePart[1].Split(':');
                        if (dateSplitted.Length == 3)
                        {
                            DateTime createdDate = new DateTime(Convert.ToInt32(dateSplitted[2]),
                                Convert.ToInt32(dateSplitted[1]),
                                Convert.ToInt32(dateSplitted[0]),
                                Convert.ToInt32(timeSplitted[0]),
                                Convert.ToInt32(timeSplitted[1]),
                                0, DateTimeKind.Local
                                );
                            po.CreatedDate = createdDate;
                        }
                    }

                    validate.ValidateAndFix(po);
                    try
                    {
                        int objectId = sql.AddPropertyObject(po);//uncomment it

                        for (int i = 0; i < ent.photos.Count(); i++)
                        {
                            if (ent.photos[i] != null && ent.photos[i].Length > 0)
                            {
                                //uncomment it
                                sql.Mysql_File_Save(objectId, 0, i.ToString() + ".jpg", "", 521, 521,
                                     ent.photos[i], imgHelper.ResizeImage(ent.photos[i], new System.Drawing.Size(120, 90)), false);
                            }
                        }
                        ent = null;
                        po = null;
                        Console.WriteLine("Stored objects : " + (++countStoredObjects).ToString());
                    }
                    catch (Exception ex)
                    {
                        eventLog.AddEventToLog("ОШИБКА сохранения объекта в БД: " + ex.Message.ToString() + " стэк ошибки: " + ex.StackTrace.ToString());
                    }
                }



                if (!t2.IsAlive && countEntities == 0)
                {
                    stopLoop = true;
                    linkLogger.TerminateLog();
                }

            } while (!stopLoop);

            Console.WriteLine("StoreEntity terminated");
            eventLog.AddEventToLog("StoreEntity завершает работу");
            return;

        }

        private void CollectEntities(Object o)
        {
            lock (collectEntityLock)//лочим текущий поток чтобы не обгонять поток получения ссылок на объекты
            {
                Monitor.Wait(collectEntityLock);
            }

            NetFetcher net = new NetFetcher();
            bool stopLoop = false;
            Template template = o as Template;
            if (template == null)
                return;
            HtmlParser parser = new HtmlParser();
            DataExtactor extractor = new DataExtactor();
            Instruction currentUrl = null;

            var sha = new System.Security.Cryptography.SHA256Managed();
            var uEncode = new UnicodeEncoding();
            List<ulong> h1 = null;

            int countLinksToObject = 0;

            try
            {
                do
                {
                    lock (linksToObjectLocker)
                    {
                        if (linksToObjects.Count > 0)
                            currentUrl = linksToObjects.Dequeue();//пытаемся получить ссылку на объект
                        countLinksToObject = linksToObjects.Count();
                    }

                    if (countLinksToObject <= minLimitList)
                    {
                        lock (linksToObjectLimitLocker)
                        {
                            Monitor.PulseAll(linksToObjectLimitLocker);
                        }
                    }

                    if (currentUrl == null)//если ссылку получить не удалось
                    {
                        if (!t2.IsAlive)//пришло время останавливаться
                        {
                            eventLog.AddEventToLog("CollectEntities- Нет больше ссылок на объекты,  поток собиратель ссылок- завршился- завершаем работу и мы");
                            break;//останавливаем главный цикл
                        }
                        else//поток собирающий ссылки еще живой
                        {
                            lock (linksToObjectLimitLocker)
                            {
                                Monitor.PulseAll(linksToObjectLimitLocker);
                            }


                            //eventLog.AddEventToLog("CollectEntities- Нет ссылок на объекты,  поток собиратель еще живой- зысыпаем");

                            lock (collectEntityLock)//останавливаемся и ждем когда нас опять позовут
                            {
                                Monitor.Wait(collectEntityLock);
                            }
                            continue;//начинаем  с начала цикла
                        }
                    }
                    else//если ссылку получили
                    {
                        if (string.IsNullOrEmpty(currentUrl.link))//если урл ссылка пустая
                        {
                            eventLog.AddEventToLog("CollectEntities- ОШИБКА - пришла пустая ссылка");
                            continue;// начинаем цикл по новой
                        }

                        //если добрались сюда- значит ссылку получили и  можем с ней работать

                        #region проверяем линк на дубликаты
                        bool dublicateLink = false;
                        currentUrl.link = currentUrl.link.ToLower();
                        lock (processedLinksLocker)
                        {
                            if (processedLinks.ContainsKey(currentUrl.link))
                            {
                                dublicateLink = true;
                                currentUrl = null;//обнуляем текущую  ссылку т.к. найден дубликат
                                Console.WriteLine("dublicat found");
                            }
                        }
                        if (dublicateLink)//если нашли дубликат- начнаем с начала  цикла
                            continue;
                        else//если дубликата не найден- добавлеям ссылку в список обработанных
                        {
                            lock (processedLinksLocker)
                            {
                                processedLinks.Add(currentUrl.link, true);
                                linkLogger.AddLinkToLog(currentUrl.link);
                            }
                        }
                        #endregion

                        string content = net.DownloadWebPage(currentUrl.link, Encoding.UTF8);//скачиваем страницу
                        Template tmpTemplate = Template.DeepClone<Template>(template);//делаем дубликат шаблона

                        Entity ent = extractor.Extract(content, template.HOST, tmpTemplate);//template);//извлекаем объект со страницы

                        if (ent != null)//если объект найден
                        {

                            #region проверяем текст объявления на дубликаты

                            string clearContent = parser.ClearContent(ent.description);

                            if (string.IsNullOrEmpty(clearContent))//если текст  объявления после очистки пустой- пропускаем такое объявление
                            {
                                eventLog.AddEventToLog("ВНИМАНИЕ!!!Пустое объявление по ссылке " + ent.linkToOriginalObject);
                                continue;
                            }

                            h1 = hash.ComputeHash(clearContent);//считаем хеши
                            if (h1 == null)
                            {
                                eventLog.AddEventToLog("ВНИМАНИЕ!!!Ошибка получения хеша для  " + clearContent);
                                continue;
                            }
                            float percentage = hash.IsExist(h1);//проверяем на наличие похожих хешей
                            if (percentage < 90.0)
                            {
                                hash.AddNewHash(h1);//добавляем если предел не превышен
                            }
                            else// найден дублика объявления
                            {
                                Console.WriteLine("Text Dublicate found");
                                ent = null;
                                tmpTemplate = null;
                                content = null;
                                currentUrl = null;
                                continue;
                            }
                            #endregion

                            ent.linkToOriginalObject = currentUrl.link;//сохраняем линк на оригинал объявления

                            if (string.IsNullOrEmpty(ent.phone1))//если  телефон1 не заполнен еще- пытаемся его получить
                            {
                                //пытаемся получить телефоны
                                foreach (var item in parser.GetLinks(content, template.HOST, template.phone[0]))
                                {
                                    content = net.DownloadWebPage(item, Encoding.UTF8);//скачиваем указанную страницу для получения телефона
                                    var newPhoneTemplate = new string[1];
                                    newPhoneTemplate[0] = template.phone[1];
                                    extractor.ExtractPhone(content, ent, newPhoneTemplate);
                                    break;
                                }
                            }

                            #region получаем фотографии
                            if (ent.LinkToPhotos != null)
                            {
                                ent.photos = new List<byte[]>();
                                for (int i = 0; i < ent.LinkToPhotos.Count; i++)
                                {
                                    if (!string.IsNullOrEmpty(ent.LinkToPhotos[i]))
                                        ent.photos.Add(
                                            net.getDataAsByteArray(ent.LinkToPhotos[i])
                                            );
                                }
                            }
                            #endregion

                            //дописываем вспомогательную информацию
                            ent.propertyType = currentUrl.propertyType;
                            ent.propertyAction = currentUrl.propertyAction;
                            ent.City = currentUrl.city;
                            ent.District = currentUrl.district;

                            int countEntities = 0;
                            lock (entitiesLocker)//добавляем полученый объект в очередь на сохранение
                            {
                                entities.Enqueue(ent);
                                countEntities = entities.Count();
                            }
                            ent = null;


                            lock (dbLoaderLock)
                            {
                                Monitor.PulseAll(dbLoaderLock);
                            }

                            if (countEntities > LimitList)//если  превысили лимит  объектов в списке лочим текущий поток
                            {


                                lock (collectEntityLock)
                                {
                                    Monitor.Wait(collectEntityLock);
                                }
                            }
                        }
                        else
                        {
                            eventLog.AddEventToLog("ОШИБКА объект по сслыке не найден: " + currentUrl.link);
                        }
                    }


                    if (!t1.IsAlive && countLinksToObject == 0)
                        stopLoop = true;




                } while (!stopLoop);//пока все условия соблюдаются
            }
            catch (Exception ex)
            {
                eventLog.AddEventToLog("ИСКЛЮЧЕНИЕ!!! CollectEntities " + ex.Message);
                hash.StoreCache();//сохряняем хеши
            }
            finally
            {

                lock (dbLoaderLock)//разлочиваем поток загрузки объектов в БД на прощание
                {
                    Monitor.PulseAll(dbLoaderLock);
                }

                Console.WriteLine("CollectEntities terminated");
            }
            return;

        }

        private void CollectLinksToObjects(Object o)
        {
            //const int dublicateCountObjsToRecheck = 20;
            //string[] dublicateLastAddedLinks = new string[dublicateCountObjsToRecheck];
            //int dublicateLoopCounter = 0;
            //bool[] dublicateMatrix = new bool[dublicateCountObjsToRecheck];
            Instruction instruction = null;
            NetFetcher net = new NetFetcher();
            bool stopLoop = false;
            Template template = o as Template;
            if (template == null)
                return;
            HtmlParser parser = new HtmlParser();
            int countLinksObtained = -1;

            int countInstructions = 0;

            SortedList<int, bool> existingKeys = new SortedList<int, bool>();//список добавленных ключей 

            do
            {

                if (countLinksObtained == 0)//если не нашли ниодной ссылки- сбрасываем текущие  инструкции
                    instruction = null;

                //if (dublicateLoopCounter >= dublicateCountObjsToRecheck - 1)
                //{
                //    instruction = null;
                //    dublicateLoopCounter = 0;
                //}

                if (instruction == null || string.IsNullOrEmpty(instruction.link.Trim()))
                {
                    //instruction = null;
                    lock (instructionLocker)
                    {
                        if (instructions.Count() == 0)
                        {
                            Console.WriteLine("All instructions were processed, CollectLinksToObjects thread going to terminate");
                            eventLog.AddEventToLog("CollectLinksToObjects Инструкции закончились,  завершаем работу");
                            break;
                        }
                        else
                        {
                            instruction = instructions.Dequeue();//берем инструкцию с очереди
                            countInstructions = instructions.Count();
                            countLinksObtained = -1;
                        }
                    }
                }

                eventLog.AddEventToLog("CollectLinksToObjects количество инструкций в очереди: " + countInstructions.ToString());

                string content = string.Empty;
                if (!stopLoop && instruction != null)
                {
                    content = net.DownloadWebPage(instruction.link, Encoding.UTF8);//скачиваем страницу по линку из инструкций

                    countLinksObtained = 0;//сбрасываем счетчик полученных линков
                    foreach (var item in parser.GetLinks(content, instruction.host, template.links[0]))//получаем линки на объявления
                    {
                        if (string.IsNullOrEmpty(item)) continue;

                        bool canAddToQueue = true;
                        int currentKey = SearchIdFromURL(item.ToLower(), "ad_id");
                        if (existingKeys.ContainsKey(currentKey))
                        {
                            canAddToQueue = false;
                            eventLog.AddEventToLog("Найден дубликат в ключах url для " + currentKey.ToString());
                        }
                        else
                            existingKeys.Add(currentKey, true);
                        //if(existingKeys.ContainsKey(
                        //for (int i = 0; i < dublicateCountObjsToRecheck; i++)
                        //    if (dublicateLastAddedLinks[i] == item)
                        //    {
                        //        dublicateMatrix[i] = true;//dublicate found
                        //        canAddToQueue = false;//can't add current link to global queue
                        //        dublicateLoopCounter++;//считаем количество дубликатов
                        //        break;
                        //    }

                        if (canAddToQueue)//in case no dublication for last elements
                        {
                            //if (dublicateLoopCounter >= dublicateCountObjsToRecheck - 1)
                            //    dublicateLoopCounter = 0;

                            //dublicateLastAddedLinks[dublicateLoopCounter++] = item;


                            int countLinksToObject = -1;

                            Instruction linkToObj = new Instruction()
                            {
                                city = instruction.city,
                                district = instruction.district,
                                host = instruction.host,
                                link = item,
                                propertyAction = instruction.propertyAction,
                                propertyType = instruction.propertyType
                            };
                            lock (linksToObjectLocker)
                            {
                                linksToObjects.Enqueue(linkToObj);//adding url to object in global queue                           
                                countLinksToObject = linksToObjects.Count;
                            }
                            Console.WriteLine("item: " + item);
                            countLinksObtained++;

                            linkToObj = null;


                            lock (collectEntityLock)//разлочиваем сборку объектов
                            {
                                Monitor.PulseAll(collectEntityLock);
                            }

                            if (countLinksToObject >= LimitList)//если превышен лимит ссылок на объеты -лочимся
                            {
                                //lock (collectEntityLock)//разлочиваем сборку объектов
                                //{
                                //    Monitor.PulseAll(collectEntityLock);
                                //}

                                lock (linksToObjectLimitLocker)
                                {
                                    Monitor.Wait(linksToObjectLimitLocker);
                                }
                            }


                        }
                    }

                    string currentLink = instruction.link;

                    if (template.pages[0].Count() > 1)
                    {
                        string tmpData = string.Empty;
                        for (int i = 0; i < template.pages.Count - 1; i++)
                        {
                            if (string.IsNullOrEmpty(tmpData))
                                tmpData = parser.GetSingleContent(content, template.pages[i]);
                            else
                                tmpData = parser.GetSingleContent(tmpData, template.pages[i]);

                        }


                        foreach (var item in parser.GetLinks(tmpData, instruction.host, template.pages[template.pages.Count() - 1]))
                        {
                            instruction.link = item;
                            break;
                        }


                    }
                    else
                    {
                        foreach (var item in parser.GetLinks(content, instruction.host, template.pages[0]))
                        {
                            instruction.link = item;
                            break;
                        }
                    }



                    if (currentLink == instruction.link && countInstructions == 0)//если предыдущая ссылка и новая  совпадают- достигли конца
                        stopLoop = true;

                    if (countLinksObtained == 0 && countInstructions == 0)//если предыдущая ссылка и новая  совпадают- достигли конца
                        stopLoop = true;

                }

                //if (string.IsNullOrEmpty(instruction.link) || stopLoop)
                //{
                //    stopLoop = false;
                //    lock (instructionLocker)
                //    {
                //        if (instructions.Count > 0)
                //            instruction = instructions.Dequeue();
                //        else stopLoop = true;
                //    }
                //}

            } while (!stopLoop);


            lock (collectEntityLock)//разлочиваем ожидающий поток на прощание
            {
                Monitor.PulseAll(collectEntityLock);
            }


            Console.WriteLine("CollectLinksToObjects terminated");
            return;

        }




        private int SearchIdFromURL(string url, string keyAttr)
        {
            int pos = url.IndexOf(keyAttr);
            if (pos < 0) return -1;
            pos = url.IndexOf('=', pos + keyAttr.Length);
            if (pos < 0) return -1;
            List<char> cArray = new List<char>();
            for (int i = pos + 1; i < url.Length; i++)
                if (Char.IsNumber(url[i]))
                    cArray.Add(url[i]);
                else
                    break;
            string result = new string(cArray.ToArray());
            return Convert.ToInt32(result);
        }

        private bool ValidateData(List<Instruction> data)
        {

            RealtyDb _db = new RealtyDb();

            var city = (from s in _db.Cities
                        select s).ToArray<City>();

            var district = (from s in _db.CityDistricts.Include("City")
                            select s).ToList<CityDisctict>();

            var propertyTypes = (from s in _db.PropertyTypes
                                 select s).ToList<PropertyType>();

            var propertyActions = (from s in _db.PropertyActions
                                   select s).ToList<PropertyAction>();

            var propertyObjects = (from s in _db.PropertyObjects
                                   select s).ToList<PropertyObject>();


            if (data == null || data.Count() == 0)
            {
                Console.WriteLine("Отсутствуют  инструкции на проверки, работа остановлена");
                return false;
            }
            int countErrors = 0;
            for (int i = 0; i < data.Count(); i++)
            {

                if (!string.IsNullOrEmpty(data[i].city.ToLower().Trim()))
                {
                    var cityObj = (from s in city
                                   where s.CityName.ToLower() == data[i].city.ToLower().Trim()//"Киев".ToLower()//
                                   select s).SingleOrDefault<City>();
                    if (cityObj == null)
                    {
                        Console.WriteLine("Не найден  город " + data[i].city);
                        countErrors++;
                    }

                    if (!string.IsNullOrEmpty(data[i].district.ToLower().Trim()))
                    {
                        var districtObj = (from s in district
                                           where s.District.ToLower() == data[i].district.ToLower().Trim()
                                           && s.City.Id == cityObj.Id
                                           select s).SingleOrDefault<CityDisctict>();

                        if (districtObj == null)
                        {
                            if (cityObj != null)
                                Console.WriteLine("Не найден  район " + data[i].district + " для города " + cityObj.CityName);
                            else
                                Console.WriteLine("Не найден  район " + data[i].district);
                            countErrors++;
                        }
                    }
                }

                var propertyTypeObj = (from s in propertyTypes
                                       where s.PropertyTypeName.ToLower() == data[i].propertyType.ToLower().Trim()//"Квартиры".ToLower()//
                                       select s).SingleOrDefault<PropertyType>();

                if (propertyTypeObj == null)
                {
                    Console.WriteLine("Не найден тип недвижимости" + data[i].propertyType);
                    countErrors++;
                }

                var propertyActionObj = (from s in propertyActions
                                         where s.PropertyActionName.ToLower() == data[i].propertyAction.ToLower().Trim()//"Продажа".ToLower()//
                                         && s.PropertyType.Id == propertyTypeObj.Id
                                         select s).SingleOrDefault<PropertyAction>();

                if (propertyActionObj == null)
                {
                    Console.WriteLine("Не найден тип действия с недвижимостью" + data[i].propertyAction);
                    countErrors++;
                }


            }

            _db.Dispose();
            _db = null;
            return countErrors == 0 ? true : false;
        }

    }
}
