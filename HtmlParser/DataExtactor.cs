using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Diagnostics;

namespace HtmlParser
{
    public class DataExtactor
    {
        /// <summary>
        /// флаг включения/отключения отладочного режима.
        /// По умолчанию режим - отключен
        /// </summary>
        private bool debugMode = false;


        public DataExtactor()
        {

        }

        public DataExtactor(bool enableDebugMode)
        {
            this.debugMode = enableDebugMode;
        }


        public Entity Extract(string data, string host, Template template)
        {
            Stopwatch watch = null;

            //Template template = new Template("template1.txt");
            //var data = File.ReadAllText(path);
            Entity ent = new Entity();
            HtmlParser parser = new HtmlParser();
            string tmpStr = string.Empty;
            int tmpInt = -1;
            if (string.IsNullOrEmpty(data)) return ent;
            //System.Diagnostics.Debug.WriteLine("starting title");

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region title
            for (int i = 0; i < template.title.Count(); i++)
                if (string.IsNullOrEmpty(ent.title))
                    ent.title = parser.GetSingleContent(data, template.title[i]);
                else
                    ent.title = parser.GetSingleContent(ent.title, template.title[i]);

            ent.title = parser.RemoveTags(ent.title).Replace("&amp;quot;", "\"");

            var charArray = ent.title.ToCharArray();
            bool convertToUppercase = false;
            if (charArray != null && charArray.Length > 0)
            {
                charArray[0] = char.ToUpper(charArray[0]);

                for (int i = 0; i < charArray.Length; i++)
                {
                    if (convertToUppercase && char.IsLetter(charArray[i]))
                    {
                        charArray[i] = char.ToUpper(charArray[i]);
                        convertToUppercase = false;
                    }
                    if (charArray[i] == '.')
                        convertToUppercase = true;
                }
                ent.title = new string(charArray);
            }

            if (string.IsNullOrEmpty(ent.title))
                ent.title = this.ExtractString(data, template.title);
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title extected in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;

            //System.Diagnostics.Debug.WriteLine("starting description");

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region description
            for (int i = 0; i < template.description.Count(); i++)
                if (string.IsNullOrEmpty(ent.description))
                    ent.description = parser.GetSingleContent(data, template.description[i]);
                else
                    ent.description = parser.GetSingleContent(ent.description, template.description[i]);

            ent.description = parser.RemoveTags(ent.description).Replace("&amp;quot;", "\""); ;

            //делаем буквы заглавными после точек
            charArray = ent.description.ToCharArray();
            if (charArray != null && charArray.Count() > 0)
            {
                charArray[0] = char.ToUpper(charArray[0]);
                convertToUppercase = false;
                for (int i = 0; i < charArray.Length; i++)
                {
                    if (convertToUppercase && char.IsLetter(charArray[i]))
                    {
                        charArray[i] = char.ToUpper(charArray[i]);
                        convertToUppercase = false;
                    }
                    if (charArray[i] == '.')
                        convertToUppercase = true;
                }
                ent.description = new string(charArray);
            }

            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title description in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region avatar
            for (int i = 0; i < template.avatar.Count(); i++)
                if (string.IsNullOrEmpty(ent.userAvatarLink))
                {
                    foreach (var item in parser.GetImgUrl(data, host, template.avatar[i]))

                        if (!string.IsNullOrEmpty(item))
                            ent.userAvatarLink = item;
                }
                else
                {
                    foreach (var item in parser.GetImgUrl(ent.userAvatarLink, host, template.avatar[i]))
                        if (!string.IsNullOrEmpty(item))
                            ent.userAvatarLink = item;
                }
            if (!string.IsNullOrEmpty(ent.userAvatarLink))
                ent.userAvatarLink = parser.RemoveTags(ent.userAvatarLink);
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title avatar in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region roomsCount
            for (int i = 0; i < template.roomsCount.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.roomsCount[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.roomsCount[i]);

            tmpStr = parser.RemoveTags(tmpStr);

            if (!int.TryParse(tmpStr, out tmpInt))
                ent.roomsCount = -1;
            else
                ent.roomsCount = tmpInt;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title roomsCount in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region sleepingPlaces
            for (int i = 0; i < template.sleepingPlaces.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.sleepingPlaces[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.sleepingPlaces[i]);

            tmpStr = parser.RemoveTags(tmpStr);

            if (!int.TryParse(tmpStr, out tmpInt))
                ent.SleepingPlaces = -1;
            else
                ent.SleepingPlaces = tmpInt;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title sleepingPlaces in " + elapsedMs.ToString());
            }



            if (this.debugMode)
                watch = Stopwatch.StartNew();

            tmpStr = string.Empty;
            double tmpDbl = -1.0;


            #region allSize
            for (int i = 0; i < template.allSize.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.allSize[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.allSize[i]);

            tmpStr = parser.RemoveTags(tmpStr);

            if (!double.TryParse(tmpStr, out tmpDbl))
                ent.allSize = -1;
            else
                ent.allSize = tmpDbl;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title allSize in " + elapsedMs.ToString());
            }

            tmpDbl = -1.0;
            tmpStr = string.Empty;

            //System.Diagnostics.Debug.WriteLine("starting houseSize");

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region houseSize
            for (int i = 0; i < template.houseSize.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.houseSize[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.houseSize[i]);

            tmpStr = parser.RemoveTags(tmpStr);

            if (tmpStr.ToLower().Contains(Dicts.KV_M.ToLower()))
                tmpStr = tmpStr.ToLower().Replace(Dicts.KV_M.ToLower(), string.Empty).Trim();
            if (!double.TryParse(tmpStr, out tmpDbl))
                ent.houseSize = -1;
            else
                ent.houseSize = tmpDbl;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title houseSize in " + elapsedMs.ToString());
            }

            tmpDbl = -1.0;
            tmpStr = string.Empty;
            //System.Diagnostics.Debug.WriteLine("starting houseGardenSize");

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region houseGardenSize
            for (int i = 0; i < template.houseGardenSize.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.houseGardenSize[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.houseGardenSize[i]);
            tmpStr = parser.RemoveTags(tmpStr);

            if (tmpStr.ToLower().Contains(Dicts.SOTOK.ToLower()))
                tmpStr = tmpStr.ToLower().Replace(Dicts.SOTOK.ToLower(), string.Empty).Trim();
            if (!double.TryParse(tmpStr, out tmpDbl))
                ent.houseGardenSize = -1;
            else
                ent.houseGardenSize = tmpDbl;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title houseGardenSize in " + elapsedMs.ToString());
            }

            tmpDbl = -1.0;
            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region distanceToCity
            for (int i = 0; i < template.distanceToCity.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.distanceToCity[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.distanceToCity[i]);

            if (tmpStr.ToLower().Contains(Dicts.KM.ToLower()))
                tmpStr = tmpStr.ToLower().Replace(Dicts.KM.ToLower(), string.Empty).Trim();

            tmpStr = parser.RemoveTags(tmpStr);

            if (!double.TryParse(tmpStr, out tmpDbl))
                ent.distanceToCity = -1;
            else
                ent.distanceToCity = tmpDbl;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title distanceToCity in " + elapsedMs.ToString());
            }

            tmpDbl = -1.0;
            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region houseCountFloor
            for (int i = 0; i < template.houseCountFloor.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.houseCountFloor[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.houseCountFloor[i]);

            tmpStr = parser.RemoveTags(tmpStr);

            if (!double.TryParse(tmpStr, out tmpDbl))
                ent.houseCountFloor = -1;
            else
                ent.houseCountFloor = tmpDbl;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title houseCountFloor in " + elapsedMs.ToString());
            }


            tmpDbl = -1.0;
            tmpStr = string.Empty;
            //System.Diagnostics.Debug.WriteLine("starting commercialObjectSize");

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region commercialObjectSize
            for (int i = 0; i < template.commercialObjectSize.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.commercialObjectSize[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.commercialObjectSize[i]);

            tmpStr = parser.RemoveTags(tmpStr);

            if (tmpStr.ToLower().Contains(Dicts.KV_M.ToLower()))
            {
                tmpStr = tmpStr.ToLower().Replace(Dicts.KV_M.ToLower(), string.Empty).Trim();
            }

            if (!double.TryParse(tmpStr, out tmpDbl))
                ent.commercialObjectSize = -1;
            else
                ent.commercialObjectSize = tmpDbl;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title commercialObjectSize in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region livingSize
            for (int i = 0; i < template.livingSize.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.livingSize[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.livingSize[i]);

            tmpStr = parser.RemoveTags(tmpStr);

            if (!double.TryParse(tmpStr, out tmpDbl))
                ent.livingSize = -1;
            else
                ent.livingSize = tmpDbl;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title livingSize in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region kitchenSize
            for (int i = 0; i < template.kitchenSize.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.kitchenSize[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.kitchenSize[i]);

            tmpStr = parser.RemoveTags(tmpStr);

            if (!double.TryParse(tmpStr, out tmpDbl))
                ent.kitchenSize = -1;
            else
                ent.kitchenSize = tmpDbl;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title kitchenSize in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region floor/count floors
            for (int i = 0; i < template.floor.Count(); i++)
            {
                if (template.floor[i].Contains(Commands.SPLIT) && !string.IsNullOrEmpty(tmpStr))
                {
                    var DataSplitter = template.floor[i].Split('=');
                    if (DataSplitter[1].ToLower().Contains(Commands.VERTICALSLASH.ToLower()))
                        DataSplitter[1] = DataSplitter[1].Replace(Commands.VERTICALSLASH, "|");

                    var splittedData = tmpStr.Split(DataSplitter[1][0]);
                    if (splittedData.Length > 1)
                    {
                        splittedData[0] = splittedData[0].Trim();
                        splittedData[1] = splittedData[1].Trim();

                        if (int.TryParse(splittedData[0], out tmpInt))
                            ent.floor = tmpInt;
                        if (int.TryParse(splittedData[1], out tmpInt))
                            ent.countFloors = tmpInt;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(tmpStr))
                        tmpStr = parser.GetSingleContent(data, template.floor[i]);
                    else
                        tmpStr = parser.GetSingleContent(tmpStr, template.floor[i]);
                }
            }
            tmpStr = tmpStr.Trim();

            tmpStr = parser.RemoveTags(tmpStr);

            if (int.TryParse(tmpStr, out tmpInt))
                ent.floor = tmpInt;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title floor/count in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;
            //System.Diagnostics.Debug.WriteLine("starting houseType");

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region houseType
            for (int i = 0; i < template.houseType.Count(); i++)
            {

                if (template.houseType[i].Contains(Commands.SPLIT) && !string.IsNullOrEmpty(ent.houseType))
                {
                    var DataSplitter = template.houseType[i].Split('=');

                    if (DataSplitter[1].ToLower().Contains(Commands.VERTICALSLASH.ToLower()))
                        DataSplitter[1] = DataSplitter[1].Replace(Commands.VERTICALSLASH, "|");


                    var splittedData = ent.houseType.Split(DataSplitter[1][0]);
                    if (splittedData.Length > 1)
                    {
                        ent.houseType = splittedData[0].Trim();
                        if (splittedData[1].Trim().Contains(Dicts.NEW_BUILDING.ToLower()))
                            ent.isNewBuilding = true;
                        else
                            ent.isNewBuilding = false;



                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(ent.houseType))
                        ent.houseType = parser.GetSingleContent(data, template.houseType[i]);
                    else
                        ent.houseType = parser.GetSingleContent(ent.houseType, template.houseType[i]);
                }
            }
            if (ent.houseType.Contains(Dicts.NEW_BUILDING.ToLower()))//возможно не указан тип здания, но указано что здание новострой
            {
                ent.isNewBuilding = true;
                ent.houseType = string.Empty;
            }

            ent.houseType = parser.RemoveTags(ent.houseType);

            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title houseType in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region wctype
            for (int i = 0; i < template.wctype.Count(); i++)
                if (string.IsNullOrEmpty(ent.wctype))
                    ent.wctype = parser.GetSingleContent(data, template.wctype[i]);
                else
                    ent.wctype = parser.GetSingleContent(ent.wctype, template.wctype[i]);
            ent.wctype = parser.RemoveTags(ent.wctype);

            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title wctype in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;
            //System.Diagnostics.Debug.WriteLine("starting balcony 1");

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region balcony 1
            for (int i = 0; i < template.balcony1.Count(); i++)
            {

                if (template.balcony1[i].Contains(Commands.SPLIT) && !string.IsNullOrEmpty(tmpStr))
                {
                    var DataSplitter = template.balcony1[i].Split('=');

                    if (DataSplitter[1].ToLower().Contains(Commands.VERTICALSLASH.ToLower()))
                        DataSplitter[1] = DataSplitter[1].Replace(Commands.VERTICALSLASH, "|");


                    var splittedData = tmpStr.Split(DataSplitter[1][0]);
                    if (splittedData.Length > 1)
                    {

                        if (splittedData[0].Trim().Contains(Dicts.KV_M.ToLower()))
                        {
                            splittedData[0] = splittedData[0].Replace(Dicts.KV_M.ToLower(), string.Empty).Trim();
                            if (int.TryParse(splittedData[0], out tmpInt))
                                ent.balconySize = tmpInt;
                        }
                        else
                            if (splittedData[0].Contains(Dicts.IS_GLASSED.ToLower()))
                                ent.isBalconyGlassed = true;

                        if (!ent.isBalconyGlassed && splittedData[1].Contains(Dicts.IS_GLASSED.ToLower()))
                            ent.isBalconyGlassed = true;

                        tmpStr = string.Empty;//clear temp var
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(tmpStr))
                        tmpStr = parser.GetSingleContent(data, template.balcony1[i]);
                    else
                        tmpStr = parser.GetSingleContent(tmpStr, template.balcony1[i]);
                }
            }
            if (!string.IsNullOrEmpty(tmpStr))
            {
                tmpStr = parser.RemoveTags(tmpStr);

                if (tmpStr.Trim().Contains(Dicts.KV_M.ToLower()))
                {
                    tmpStr = tmpStr.Replace(Dicts.KV_M.ToLower(), string.Empty).Trim();
                    if (int.TryParse(tmpStr, out tmpInt))
                        ent.balconySize = tmpInt;
                }
                else
                    if (tmpStr.Contains(Dicts.IS_GLASSED.ToLower()))
                        ent.isBalconyGlassed = true;
            }
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title balcony1 in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;
            //System.Diagnostics.Debug.WriteLine("starting balcony 2");

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region balcony 2
            if (!ent.isBalconyGlassed && ent.balconySize <= 0)
                for (int i = 0; i < template.balcony2.Count(); i++)
                {
                    //System.Diagnostics.Debug.WriteLine("starting balcony 2 "+i.ToString());

                    if (template.balcony2[i].Contains(Commands.SPLIT) && !string.IsNullOrEmpty(tmpStr))
                    {
                        //System.Diagnostics.Debug.WriteLine("starting balcony 2 first case");

                        var DataSplitter = template.balcony2[i].Split('=');

                        if (DataSplitter[1].ToLower().Contains(Commands.VERTICALSLASH.ToLower()))
                            DataSplitter[1] = DataSplitter[1].Replace(Commands.VERTICALSLASH, "|");


                        var splittedData = tmpStr.Split(DataSplitter[1][0]);
                        if (splittedData.Length > 1)
                        {

                            if (splittedData[0].Trim().Contains(Dicts.KV_M.ToLower()))
                            {
                                splittedData[0] = splittedData[0].Replace(Dicts.KV_M.ToLower(), string.Empty).Trim();
                                if (int.TryParse(splittedData[0], out tmpInt))
                                    ent.balconySize = tmpInt;
                            }
                            else
                                if (splittedData[0].Contains(Dicts.IS_GLASSED.ToLower()))
                                    ent.isBalconyGlassed = true;

                            if (!ent.isBalconyGlassed && splittedData[1].Contains(Dicts.IS_GLASSED.ToLower()))
                                ent.isBalconyGlassed = true;

                            tmpStr = string.Empty;//clear temp var
                        }
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("starting balcony 2 second case");

                        if (string.IsNullOrEmpty(tmpStr))
                            tmpStr = parser.GetSingleContent(data, template.balcony2[i]);
                        else
                            tmpStr = parser.GetSingleContent(tmpStr, template.balcony2[i]);
                    }
                }
            if (!string.IsNullOrEmpty(tmpStr))
            {
                tmpStr = parser.RemoveTags(tmpStr);

                if (tmpStr.Trim().Contains(Dicts.KV_M.ToLower()))
                {
                    tmpStr = tmpStr.Replace(Dicts.KV_M.ToLower(), string.Empty).Trim();
                    if (int.TryParse(tmpStr, out tmpInt))
                        ent.balconySize = tmpInt;
                }
                else
                    if (tmpStr.Contains(Dicts.IS_GLASSED.ToLower()))
                        ent.isBalconyGlassed = true;
            }
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title balcony2 in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;

            //System.Diagnostics.Debug.WriteLine("starting price & currency");

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region price & currency
            for (int i = 0; i < template.price.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.price[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.price[i]);

            if (tmpStr.Contains(Dicts.USD))
            {
                ent.currency = Dicts.USD;
                tmpStr = tmpStr.Replace(Dicts.USD, string.Empty).Replace(" ", string.Empty);
            }
            else if (tmpStr.Contains(Dicts.UAH))
            {
                ent.currency = Dicts.UAH;
                tmpStr = tmpStr.Replace(Dicts.UAH, string.Empty).Replace(" ", string.Empty);
            }
            else if (tmpStr.Contains(Dicts.EURO))
            {
                ent.currency = Dicts.EURO;
                tmpStr = tmpStr.Replace(Dicts.EURO, string.Empty).Replace(" ", string.Empty);
            }

            if (tmpStr.ToLower().Contains(Dicts.ZA_KV_M.ToLower().Replace(" ", string.Empty)))
            {
                ent.priceFor = Dicts.ZA_KV_M;
                tmpStr = tmpStr.ToLower().Replace(Dicts.ZA_KV_M.ToLower().Replace(" ", string.Empty), string.Empty);
            }
            else if (tmpStr.ToLower().Contains(Dicts.ZA_MONTH.ToLower().Replace(" ", string.Empty)))
            {
                ent.priceFor = Dicts.ZA_MONTH;
                tmpStr = tmpStr.ToLower().Replace(Dicts.ZA_MONTH.ToLower().Replace(" ", string.Empty), string.Empty);
            }
            else if (tmpStr.ToLower().Contains(Dicts.ZA_OBJECT.ToLower().Replace(" ", string.Empty)))
            {
                ent.priceFor = Dicts.ZA_OBJECT;
                tmpStr = tmpStr.ToLower().Replace(Dicts.ZA_OBJECT.ToLower().Replace(" ", string.Empty), string.Empty);
            }
            else if (tmpStr.ToLower().Contains(Dicts.ZA_SUTKI.ToLower().Replace(" ", string.Empty)))
            {
                ent.priceFor = Dicts.ZA_SUTKI;
                tmpStr = tmpStr.ToLower().Replace(Dicts.ZA_SUTKI.ToLower().Replace(" ", string.Empty), string.Empty);
            }



            tmpStr = parser.RemoveTags(tmpStr);

            tmpStr = tmpStr.Replace(" ", string.Empty);
            if (int.TryParse(tmpStr, out tmpInt))
                ent.price = tmpInt;

            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title price&currency in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region commision
            for (int i = 0; i < template.commision.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.commision[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.commision[i]);

            if (tmpStr.Contains(Dicts.NO_COMISSION))
                ent.noComission = true;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title commision in " + elapsedMs.ToString());
            }


            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region contact
            for (int i = 0; i < template.contact.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.contact[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.contact[i]);

            tmpStr = parser.RemoveTags(tmpStr);

            ent.contactName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tmpStr);
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title contact in " + elapsedMs.ToString());
            }

            tmpStr = string.Empty;

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region createdDate
            for (int i = 0; i < template.createddate.Count(); i++)
                if (string.IsNullOrEmpty(tmpStr))
                    tmpStr = parser.GetSingleContent(data, template.createddate[i]);
                else
                    tmpStr = parser.GetSingleContent(tmpStr, template.createddate[i]);

            tmpStr = parser.RemoveTags(tmpStr);

            ent.CreatedDate = tmpStr;
            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title createdDate in " + elapsedMs.ToString());
            }


            tmpStr = string.Empty;

            //System.Diagnostics.Debug.WriteLine("starting linkToPhotos");

            if (this.debugMode)
                watch = Stopwatch.StartNew();

            #region LinkToPhotos
            var links = new List<string>();
            for (int i = 0; i < template.photos.Count(); i++)
            {

                if (template.photos[i].Contains(Commands.LOOP) && !string.IsNullOrEmpty(tmpStr))
                {
                    template.photos[i] = parser.ClearContent(template.photos[i].Replace(Commands.LOOP, string.Empty));


                    foreach (var item in parser.GetLinks(tmpStr, host, template.photos[i]))
                        links.Add(item);

                }

                else if (template.photos[i].Contains(Commands.LOOP))
                {
                    template.photos[i] = parser.ClearContent(template.photos[i].Replace(Commands.LOOP, string.Empty));

                    if (template.photos[i].Contains(Commands.ATTRIBUTE) && template.photos[i].Contains(Commands.SPLIT))
                    {
                        //ожидаем что то  похожее на 
                        //[attribute]=onclick[split]<div class=\"preview-gallery\">*</div>
                        template.photos[i] = template.photos[i].Replace(Commands.SPLIT, "|");
                        var rowData = template.photos[i].Split('|');
                        if (rowData != null && rowData.Length == 2)
                        {
                            //получаем чистый аттрибут
                            rowData[0] = rowData[0].Replace(Commands.ATTRIBUTE, string.Empty).Replace("=", string.Empty);
                            //rowData[1] должен содержать шаблон поиска

                            foreach (var item in parser.GetAttributeValue(data, rowData[1], rowData[0]))
                                links.Add(item);

                        }

                    }
                }

                else if (links != null && links.Count() > 0)
                {
                    if (template.photos[i].Contains(Commands.REPLACE) && template.photos[i].Contains(Commands.SPLIT))
                    {
                        template.photos[i] = template.photos[i].Replace(Commands.SPLIT, "|");
                        var rowData = template.photos[i].Split('|');
                        if (rowData != null && rowData.Length == 2)
                        {
                            rowData[0] = rowData[0].Replace(Commands.REPLACE, string.Empty).Replace("=", string.Empty);
                            for (int linkCounter = 0; linkCounter < links.Count(); linkCounter++)
                            {
                                if (links[linkCounter].Contains(rowData[0]))
                                    links[linkCounter] = links[linkCounter].Replace(rowData[0], rowData[1]);
                                else
                                    links[linkCounter] = string.Empty;//намеренно обнуляем результат- т.к. там скорее всего мусор
                            }
                        }
                    }
                    else
                    {
                        for (int linkCounter = 0; linkCounter < links.Count(); linkCounter++)
                        {
                            string tmpLinkResult = parser.GetSingleContent(links[linkCounter], template.photos[i]);
                            if (!string.IsNullOrEmpty(tmpLinkResult))
                                links[linkCounter] = tmpLinkResult;
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(tmpStr))
                        tmpStr = parser.GetSingleContent(data, template.photos[i]);
                    else
                        tmpStr = parser.GetSingleContent(tmpStr, template.photos[i]);
                }
            }
            ent.LinkToPhotos = links;

            #endregion

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("Title linkToPhotos in " + elapsedMs.ToString());
            }


            ent.wall = this.ExtractString(data, template.wall);
            //ent.house_state = this.ExtractString(data, template.house_state);
            //ent.type_of_perekritiya = this.ExtractString(data, template.type_of_perekritiya);
            ent.flat_planirovka = this.ExtractString(data, template.flat_planirovka);
            ent.visota_potolka = this.ExtractString(data, template.visota_potolka);
            ent.state_of_flat = this.ExtractString(data, template.state_of_flat);
            ent.uteplenie = this.ExtractString(data, template.uteplenie);

            ent.state_of_building = this.ExtractString(data, template.state_of_building);
            ent.building_type_of_perekritiya = this.ExtractString(data, template.building_type_of_perekritiya);
            ent.building_type_of_roof = this.ExtractString(data, template.building_type_of_roof);
            ent.characteristic_of_space_planirovka = this.ExtractString(data, template.characteristic_of_space_planirovka);
            ent.characteristic_of_space_some_feature = this.ExtractString(data, template.characteristic_of_space_some_feature);
            ent.characteristic_of_space_height = this.ExtractString(data, template.characteristic_of_space_height);
            ent.characteristic_of_space_state = this.ExtractString(data, template.characteristic_of_space_state);
            ent.characteristic_of_space_gips = this.ExtractString(data, template.characteristic_of_space_gips);
            ent.communication_gas = this.ExtractString(data, template.communication_gas);
            ent.communication_water = this.ExtractString(data, template.communication_water);
            ent.communication_heating = this.ExtractString(data, template.communication_heating);
            ent.communication_water_heating = this.ExtractString(data, template.communication_water_heating);
            ent.communication_tv = this.ExtractString(data, template.communication_tv);
            ent.communication_internet = this.ExtractString(data, template.communication_internet);
            ent.communication_phone = this.ExtractString(data, template.communication_phone);
            ent.communication_conditioner = this.ExtractString(data, template.communication_conditioner);
            ent.owner_object_type = this.ExtractString(data, template.owner_object_type);
            ent.nearest_metro = this.ExtractString(data, template.nearest_metro);
            ent.nearest_metro_distance = this.ExtractString(data, template.nearest_metro_distance);
            ent.nearest_metro_howto_get = this.ExtractString(data, template.nearest_metro_howto_get);
            ent.center_city_distance = this.ExtractString(data, template.center_city_distance);
            ent.center_city_howto_get = this.ExtractString(data, template.center_city_howto_get);
            ent.doors_and_windows_indoor = this.ExtractString(data, template.doors_and_windows_indoor);
            ent.doors_and_windows_window_type = this.ExtractString(data, template.doors_and_windows_window_type);
            ent.doors_and_windows_count_glass = this.ExtractString(data, template.doors_and_windows_count_glass);
            ent.school_distance = this.ExtractString(data, template.school_distance);
            ent.school_howto_get = this.ExtractString(data, template.school_howto_get);
            ent.childrengarden_distance = this.ExtractString(data, template.childrengarden_distance);
            ent.childrengarden_howto_get = this.ExtractString(data, template.childrengarden_howto_get);
            ent.policlinic_distance = this.ExtractString(data, template.policlinic_distance);
            ent.policlinic_howto_get = this.ExtractString(data, template.policlinic_howto_get);
            ent.market_distance = this.ExtractString(data, template.market_distance);
            ent.market_howto_get = this.ExtractString(data, template.market_howto_get);
            ent.relax_zone_type = this.ExtractString(data, template.relax_zone_type);
            ent.relax_zone_distance = this.ExtractString(data, template.relax_zone_distance);
            ent.relax_zone_howto_get = this.ExtractString(data, template.relax_zone_howto_get);
            ent.other_pravo_spbst_na_nedvig = this.ExtractString(data, template.other_pravo_spbst_na_nedvig);
            ent.other_pravo_spbst_na_zemlyu = this.ExtractString(data, template.other_pravo_spbst_na_zemlyu);
            ent.other_docs_na_pravo_sobstv = this.ExtractString(data, template.other_docs_na_pravo_sobstv);
            ent.other_obstoyatelstva = this.ExtractString(data, template.other_obstoyatelstva);
            ent.building_haracter_class_object = this.ExtractString(data, template.building_haracter_class_object);
            ent.building_haracter_building_year = this.ExtractString(data, template.building_haracter_building_year);
            ent.building_haracter_state = this.ExtractString(data, template.building_haracter_state);
            ent.haracter_bussiness_pravovaya_forma = this.ExtractString(data, template.haracter_bussiness_pravovaya_forma);
            ent.haracter_bussiness_srok_okupaemosti = this.ExtractString(data, template.haracter_bussiness_srok_okupaemosti);
            ent.haracter_bussiness_average_income = this.ExtractString(data, template.haracter_bussiness_average_income);
            ent.haracter_bussiness_debit_dolg = this.ExtractString(data, template.haracter_bussiness_debit_dolg);
            ent.haracter_bussiness_credit_dolg = this.ExtractString(data, template.haracter_bussiness_credit_dolg);
            ent.haracter_bussiness_count_empl = this.ExtractString(data, template.haracter_bussiness_count_empl);
            ent.haracter_bussiness_selling_part = this.ExtractString(data, template.haracter_bussiness_selling_part);
            ent.space_poshad_hoz_pomesh = this.ExtractString(data, template.space_poshad_hoz_pomesh);
            ent.space_poshad_torg_zala = this.ExtractString(data, template.space_poshad_torg_zala);
            ent.space_poshad_sklad = this.ExtractString(data, template.space_poshad_sklad);
            ent.space_poshad_rampa = this.ExtractString(data, template.space_poshad_rampa);
            ent.parking_count_place = this.ExtractString(data, template.parking_count_place);
            ent.parking_type = this.ExtractString(data, template.parking_type);
            ent.parking_howto_get = this.ExtractString(data, template.parking_howto_get);
            ent.parking_distance = this.ExtractString(data, template.parking_distance);
            ent.offer_type = this.ExtractString(data, template.offer_type);

            if (ent.currency == null || ent.currency.Trim() == "")
                ent.currency = this.ExtractString(data, template.currency);

            if (string.IsNullOrEmpty(ent.phone1))
                ent.phone1 = this.ExtractString(data, template.phone);

            //System.Diagnostics.Debug.WriteLine("starting returning data");

            return ent;

        }
        public List<string> ExtractLinksToObjects(string data, string host)
        {
            Template template = new Template("template1.txt");
            HtmlParser parser = new HtmlParser();
            var result = new List<string>();
            template.links[0] = parser.ClearContent(template.links[0]);
            string tmpStr = string.Empty;
            foreach (var item in parser.GetLinks(data, host, template.links[0]))
            {
                tmpStr = parser.ClearContent(item).Trim();
                if (!string.IsNullOrEmpty(tmpStr))
                    result.Add(parser.ClearContent(item));
            }
            return result;

        }

        public void ExtractPhone(string content, Entity ent, string[] template)
        {
            HtmlParser parser = new HtmlParser();
            #region phone
            for (int i = 0; i < template.Count(); i++)
                if (string.IsNullOrEmpty(ent.phone1))
                    ent.phone1 = parser.GetSingleContent(content, template[i]);
                else
                    ent.phone1 = parser.GetSingleContent(ent.phone1, template[i]);

            ent.phone1 = parser.RemoveTags(ent.phone1);
            #endregion
        }

        public string ExtractString(string data, List<string> template)
        {
            Stopwatch watch = null;
            if (this.debugMode)
                watch = Stopwatch.StartNew(); 

            HtmlParser parser = new HtmlParser();
            string result = string.Empty;
            for (int i = 0; i < template.Count(); i++)
                if (string.IsNullOrEmpty(result))
                    result = parser.GetSingleContent(data, template[i]);
                else
                    result = parser.GetSingleContent(result, template[i]);
            
            result=parser.RemoveTags(result);

            if (this.debugMode)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("ExtractString in " + elapsedMs.ToString());
            }
            return result;
        }

    }
}
