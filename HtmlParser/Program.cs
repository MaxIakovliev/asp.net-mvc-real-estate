using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HtmlParser.Crawler;

namespace HtmlParser
{
    class Program
    {
        static void Main(string[] args)
        {
            ////Template t = new Template("template1.txt");
            //HtmlParser parser = new HtmlParser();
            //var content = File.ReadAllText("test2.txt");
            //DataExtactor extractor = new DataExtactor();
            //var result =extractor.Extract(content, "http://fn.ua");
            
            //File.Delete("result.txt");
            //File.AppendAllText("result.txt", result.title+"\r\n");
            //File.AppendAllText("result.txt", result.description + "\r\n");
            //File.AppendAllText("result.txt", "комнат: " + result.roomsCount.ToString() + "\r\n");
            //File.AppendAllText("result.txt", "Общая: " + result.allSize.ToString() + "\r\n");
            //File.AppendAllText("result.txt", "Жилая: " + result.livingSize.ToString() + "\r\n");
            //File.AppendAllText("result.txt", "Кухня: " + result.kitchenSize.ToString() + "\r\n");
            //File.AppendAllText("result.txt", "Тип дома: " + result.houseType.ToString()+" "+result.isNewBuilding.ToString() + "\r\n");
            //File.AppendAllText("result.txt", "Этаж/Этажность: " + result.floor.ToString() + " " + result.countFloors.ToString() + "\r\n");
            //File.AppendAllText("result.txt", "Балкон " + result.balconySize.ToString() +" "+result.isBalconyGlassed.ToString() +"\r\n");
            //File.AppendAllText("result.txt", "Санузел: " + result.wctype.ToString() + "\r\n");
            //File.AppendAllText("result.txt", "цена: " + result.price.ToString() + result.currency + " " + result.priceFor+ "\r\n");
            //File.AppendAllText("result.txt", "commision: " + result.noComission.ToString() + "\r\n");
            //File.AppendAllText("result.txt", "Контакт: " + result.contactName.ToString() + "\r\n");
            //File.AppendAllText("result.txt", "Подано " + result.CreatedDate.ToString() + "\r\n");
            //File.AppendAllText("result.txt", "Аватар " + result.userAvatarLink + "\r\n");
            //File.AppendAllText("result.txt", "Спальных мест " + result.SleepingPlaces + "\r\n");
            //File.AppendAllText("result.txt", "houseSize " + result.houseSize + "\r\n");
            //File.AppendAllText("result.txt", "houseGardenSize " + result.houseGardenSize + "\r\n");
            //File.AppendAllText("result.txt", "distanceToCity " + result.distanceToCity + "\r\n");
            //File.AppendAllText("result.txt", "houseCountFloor " + result.houseCountFloor + "\r\n");
            //File.AppendAllText("result.txt", "commercialObjectSize " + result.commercialObjectSize + "\r\n");

            //content = File.ReadAllText("test.txt");
            //var links = extractor.ExtractLinksToObjects(content, "http://fn.ua");
            //int i = 0;
            //foreach (var item in links)
            //    Console.WriteLine((++i).ToString()+". "+item);

           // Hash hash = new Hash("stopWords.txt" ,3);
           // var text1 = File.ReadAllText("text_forAnalyze1.txt");
           // var h1=hash.ComputeHash(text1);
           // //Console.WriteLine(h1.Count().ToString());
           // float percentage = hash.IsExist(h1);
           //// Console.WriteLine(percentage);
           // if (percentage < 90)
           // {
           //     hash.AddNewHash(h1);
           // }

           // text1 = File.ReadAllText("text_forAnalyze2.txt");
           // h1 = hash.ComputeHash(text1);
           // //Console.WriteLine(h1.Count().ToString());
           // percentage = hash.IsExist(h1);
           // Console.WriteLine("Совпадение: " + (int)percentage);
           // if (percentage < 90)
           // {
           //     hash.AddNewHash(h1);
           // }


           // Console.WriteLine(hash.GetCountObjectInStorage());





            Template t1 = new Template("ria_flat_template.txt");
            var _extractor = new DataExtactor();
            var content1 = File.ReadAllText("ria_flat_img_links_test.txt");
            var result = _extractor.Extract(content1, "dom.ria.ua", t1);
            return;




            Hash hash = new Hash("stopWords.txt","", 10);
            List<ulong> h1=null;



            WebRealty.Models.RealtyDb _db = new WebRealty.Models.RealtyDb();

            var q = from s in _db.PropertyObjects.AsEnumerable()
                    select s.PropertyDescription;

            int counter = 0;
            foreach (var item in q)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    h1 = hash.ComputeHash(item);
                    float percentage = hash.IsExist(h1);
                    if (percentage < 90.0)
                    {
                        hash.AddNewHash(h1);
                    }
                    else
                    {
                        File.AppendAllText("dublicates.txt", item + Environment.NewLine);
                        File.AppendAllText("dublicates.txt", "------------------------------");
                        
                        Console.WriteLine("Dublicate!!!!!!!!!!!!!!!!!!!!!");
                    }
                    Console.WriteLine(counter++);
                }
            }


















            Console.WriteLine("done");

            Console.ReadKey();
            return;


            EntryPoint point = new EntryPoint(Directory.GetCurrentDirectory());
            point.Run();


            //working code DO NOT REMOVE IT
            //EntryPoint point = new EntryPoint(Directory.GetCurrentDirectory());
            //point.Run();

        }
    }
}
