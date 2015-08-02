using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HtmlParser;
using System.IO;
using System.Diagnostics;

namespace TestHtmlParser.Ria_ua_source
{
    [TestFixture]
    class TestFetchingData
    {
        private string sourceFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\ContentForParser\\ria_flat_all_params_source.html";
        private string linkSourceFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\ContentForParser\\ria_flat_img_links_test.txt";
        private string linkSourceFilePath1 = AppDomain.CurrentDomain.BaseDirectory + "\\ContentForParser\\1ria_flat_img_links_test.txt";
        private string linkSourceFilePath3 = AppDomain.CurrentDomain.BaseDirectory + "\\ContentForParser\\3ria_flat_img_links_test.txt";
        private string linkSourceFilePath8 = AppDomain.CurrentDomain.BaseDirectory + "\\ContentForParser\\8ria_flat_img_links_test.txt";
        private string linkSourceFilePath9 = AppDomain.CurrentDomain.BaseDirectory + "\\ContentForParser\\9ria_flat_img_links_test.txt";
        private string linkSourceFilePath10 = AppDomain.CurrentDomain.BaseDirectory + "\\ContentForParser\\10ria_flat_img_links_test.txt";
        private string templateFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\ContentForParser\\ria_flat_template.txt";
        private string linksToDetailsAndPagesPath = AppDomain.CurrentDomain.BaseDirectory + "\\ContentForParser\\ria_flat_links_and_pages.txt";
        private string noLinksToDetailsAndPagesPath = AppDomain.CurrentDomain.BaseDirectory + "\\ContentForParser\\ria_flat_no_links_topages.txt";
        //ria_flat_no_links_topages.txt

        private string ListOfFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\ContentForParser\\TestForNoExceptions";



        private Template _riaTemplate;
        DataExtactor _extractor;
        string _content;
        Entity result;

        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Setup fired");

            if (_riaTemplate == null)
            {
                var watch = Stopwatch.StartNew();

                _riaTemplate = new Template(templateFilePath);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("template constractor takes " + elapsedMs.ToString());

            }
            if (_extractor == null)
            {
                var watch = Stopwatch.StartNew();

                _extractor = new DataExtactor(true);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("DataExtactor constractor takes " + elapsedMs.ToString());
            }
            if (_content == null)
            {
                var watch = Stopwatch.StartNew();

                _content = File.ReadAllText(this.sourceFilePath);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("reading file sourceFilePath takes " + elapsedMs.ToString());
            }
            if (result == null)
            {
                var watch = Stopwatch.StartNew();

                result = _extractor.Extract(_content, "dom.ria.ua", _riaTemplate);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("_extractor.Extract routines takes " + elapsedMs.ToString());
            }
        }


        [Test]
        public void TestTemplateFileAvalability()
        {
            bool result = File.Exists(templateFilePath);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TestSourceDataFileAvalability()
        {
            bool result = File.Exists(sourceFilePath);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Test_other_obstoyatelstva()
        {
            Template riaTemplate = new Template(templateFilePath);
            var content = File.ReadAllText(this.sourceFilePath);
            var result = _extractor.Extract(content, "dom.ria.ua", riaTemplate);
            //Console.WriteLine(result.other_obstoyatelstva);
            Assert.AreEqual("в кредите;", result.other_obstoyatelstva);
        }

        [Test]
        public void Test_other_docs_na_pravo_sobstv()
        {
            Console.WriteLine(result.other_docs_na_pravo_sobstv);
            Assert.AreEqual("свидетельство о праве собственности;", result.other_docs_na_pravo_sobstv);
        }

        [Test]
        public void Test_other_pravo_spbst_na_zemlyu()
        {
            Console.WriteLine(result.other_pravo_spbst_na_zemlyu);
            Assert.AreEqual("частное (гос-акт постоянного пользования);", result.other_pravo_spbst_na_zemlyu);
        }

        [Test]
        public void Test_other_pravo_spbst_na_nedvig()
        {
            Console.WriteLine(result.other_pravo_spbst_na_nedvig);
            Assert.AreEqual("оформлено на юр.лицо;", result.other_pravo_spbst_na_nedvig);
        }

        [Test]
        public void Test_relax_zone_howto_get()
        {
            Console.WriteLine(result.relax_zone_howto_get);
            Assert.AreEqual("пешком;", result.relax_zone_howto_get);
        }

        [Test]
        public void Test_relax_zone_distance()
        {
            Console.WriteLine(result.relax_zone_distance);
            Assert.AreEqual("до 15-ти минут;", result.relax_zone_distance);
        }

        [Test]
        public void Test_relax_zone_type()
        {
            Console.WriteLine(result.relax_zone_type);
            Assert.AreEqual("пляж, река / озеро;", result.relax_zone_type);
        }

        [Test]
        public void Test_market_howto_get()
        {
            Console.WriteLine(result.market_howto_get);
            Assert.AreEqual("пешком;", result.market_howto_get);
        }

        [Test]
        public void Test_market_distance()
        {
            Console.WriteLine(result.market_distance);
            Assert.AreEqual("до 20-ти минут;", result.market_distance);
        }

        [Test]
        public void Test_policlinic_howto_get()
        {
            Console.WriteLine(result.policlinic_howto_get);
            Assert.AreEqual("на транспорте;", result.policlinic_howto_get);
        }

        [Test]
        public void Test_policlinic_distance()
        {
            Console.WriteLine(result.policlinic_distance);
            Assert.AreEqual("до 10-ти минут;", result.policlinic_distance);
        }

        [Test]
        public void Test_childrengarden_howto_get()
        {
            Console.WriteLine(result.childrengarden_howto_get);
            Assert.AreEqual("на транспорте;", result.childrengarden_howto_get);
        }

        [Test]
        public void Test_childrengarden_distance()
        {
            Console.WriteLine(result.childrengarden_distance);
            Assert.AreEqual("до 15-ти минут;", result.childrengarden_distance);
        }

        [Test]
        public void Test_parking_count_place()
        {
            Console.WriteLine(result.parking_count_place);
            Assert.AreEqual("1;", result.parking_count_place);
        }

        [Test]
        public void Test_parking_type()
        {
            Console.WriteLine(result.parking_type);
            Assert.AreEqual("стихийная парковка;", result.parking_type);
        }

        [Test]
        public void Test_parking_howto_get()
        {
            Console.WriteLine(result.parking_howto_get);
            Assert.AreEqual("пешком;", result.parking_howto_get);
        }

        [Test]
        public void Test_parking_distance()
        {
            Console.WriteLine(result.parking_distance);
            Assert.AreEqual("до 15-ти минут;", result.parking_distance);
        }

        [Test]
        public void Test_school_howto_get()
        {
            Console.WriteLine(result.school_howto_get);
            Assert.AreEqual("пешком;", result.school_howto_get);
        }

        [Test]
        public void Test_school_distance()
        {
            Console.WriteLine(result.school_distance);
            Assert.AreEqual("до 5-ти минут;", result.school_distance);
        }

        [Test]
        public void Test_doors_and_windows_count_glass()
        {
            Console.WriteLine(result.doors_and_windows_count_glass);
            Assert.AreEqual("2;", result.doors_and_windows_count_glass);
        }

        [Test]
        public void Test_doors_and_windows_window_type()
        {
            Console.WriteLine(result.doors_and_windows_window_type);
            Assert.AreEqual("деревянные;", result.doors_and_windows_window_type);
        }

        [Test]
        public void Test_doors_and_windows_indoor()
        {
            Console.WriteLine(result.doors_and_windows_indoor);
            Assert.AreEqual("деревянная;", result.doors_and_windows_indoor);
        }

        [Test]
        public void Test_center_city_howto_get()
        {
            Console.WriteLine(result.center_city_howto_get);
            Assert.AreEqual("на транспорте;", result.center_city_howto_get);
        }

        [Test]
        public void Test_center_city_distance()
        {
            Console.WriteLine(result.center_city_distance);
            Assert.AreEqual("до 20-ти минут;", result.center_city_distance);
        }

        [Test]
        public void Test_nearest_metro_howto_get()
        {
            Console.WriteLine(result.nearest_metro_howto_get);
            Assert.AreEqual("на транспорте;", result.nearest_metro_howto_get);
        }

        [Test]
        public void Test_nearest_metro_distance()
        {
            Console.WriteLine(result.nearest_metro_distance);
            Assert.AreEqual("до 10-ти минут;", result.nearest_metro_distance);
        }

        [Test]
        public void Test_nearest_metro()
        {
            Console.WriteLine(result.nearest_metro);
            Assert.AreEqual("Нивки;".ToLower(), result.nearest_metro);
        }

        [Test]
        public void Test_owner_object_type()
        {
            Console.WriteLine(result.owner_object_type);
            Assert.AreEqual("от посредника", result.owner_object_type);
        }

        [Test]
        public void Test_communication_conditioner()
        {
            Console.WriteLine(result.communication_conditioner);
            Assert.AreEqual("сплит-система;", result.communication_conditioner);
        }

        [Test]
        public void Test_communication_phone()
        {
            Console.WriteLine(result.communication_phone);
            Assert.AreEqual("есть возможность подключения;", result.communication_phone);
        }

        [Test]
        public void Test_communication_internet()
        {
            Console.WriteLine(result.communication_internet);
            Assert.AreEqual("проводной;", result.communication_internet);
        }

        [Test]
        public void Test_communication_tv()
        {
            Console.WriteLine(result.communication_tv);
            Assert.AreEqual("кабельное;", result.communication_tv);
        }

        [Test]
        public void Test_communication_water_heating()
        {
            Console.WriteLine(result.communication_water_heating);
            Assert.AreEqual("бойлер;", result.communication_water_heating);
        }

        [Test]
        public void Test_communication_heating()
        {
            Console.WriteLine(result.communication_heating);
            Assert.AreEqual("централизованное;", result.communication_heating);
        }

        [Test]
        public void Test_communication_water()
        {
            Console.WriteLine(result.communication_water);
            Assert.AreEqual("централизованное (водопровод);", result.communication_water);
        }

        [Test]
        public void Test_communication_gas()
        {
            Console.WriteLine(result.communication_gas);
            Assert.AreEqual("есть;", result.communication_gas);
        }

        [Test]
        public void Test_characteristic_of_space_gips()
        {
            Console.WriteLine(result.characteristic_of_space_gips);
            Assert.AreEqual("потолок;", result.characteristic_of_space_gips);
        }

        [Test]
        public void Test_characteristic_of_space_state()
        {
            Console.WriteLine(result.characteristic_of_space_state);
            Assert.AreEqual("требует ремонта;", result.characteristic_of_space_state);
        }

        [Test]
        public void Test_characteristic_of_space_height()
        {
            Console.WriteLine(result.characteristic_of_space_height);
            Assert.AreEqual("2.7;", result.characteristic_of_space_height);
        }

        [Test]
        public void Test_characteristic_of_space_some_feature()
        {
            Console.WriteLine(result.characteristic_of_space_some_feature);
            Assert.AreEqual("перепланировка;", result.characteristic_of_space_some_feature);
        }

        [Test]
        public void Test_characteristic_of_space_planirovka()
        {
            Console.WriteLine(result.characteristic_of_space_planirovka);
            Assert.AreEqual("смежные комнаты;", result.characteristic_of_space_planirovka);
        }

        [Test]
        public void Test_building_type_of_roof()
        {
            Console.WriteLine(result.building_type_of_roof);
            Assert.AreEqual("рубероидная кровля;", result.building_type_of_roof);
        }

        [Test]
        public void Test_building_type_of_perekritiya()
        {
            Console.WriteLine(result.building_type_of_perekritiya);
            Assert.AreEqual("смешанное;", result.building_type_of_perekritiya);
        }

        [Test]
        public void Test_state_of_building()
        {
            Console.WriteLine(result.state_of_building);
            Assert.AreEqual("нормальное;", result.state_of_building);
        }

        [Test]
        public void Test_uteplenie()
        {
            Console.WriteLine(result.uteplenie);
            Assert.AreEqual("внутреннее;", result.uteplenie);
        }

        [Test]
        public void Test_state_of_flat()
        {
            Console.WriteLine(result.state_of_flat);
            Assert.AreEqual("требует ремонта;", result.state_of_flat);
        }

        [Test]
        public void Test_visota_potolka()
        {
            Console.WriteLine(result.visota_potolka);
            Assert.AreEqual("2.7;", result.visota_potolka);
        }

        [Test]
        public void Test_flat_planirovka()
        {
            Console.WriteLine(result.flat_planirovka);
            Assert.AreEqual("смежные комнаты;", result.flat_planirovka);
        }


        [Test]
        public void Test_wall()
        {
            Console.WriteLine(result.wall);
            Assert.AreEqual("монолит", result.wall);
        }

        [Test]
        public void Test_phone()
        {
            Console.WriteLine(result.phone1);
            Assert.AreEqual("(066) 248-74-15", result.phone1);
        }

        [Test]
        public void Test_createddate()
        {
            Console.WriteLine(result.CreatedDate);
            Assert.AreEqual("07.06.2013", result.CreatedDate);
        }

        [Test]
        public void Test_contact()
        {
            Console.WriteLine(result.contactName);
            Assert.AreEqual("Valik", result.contactName);
        }

        [Test]
        public void Test_offer_type()
        {
            Console.WriteLine(result.offer_type);
            Assert.AreEqual("от посредника", result.offer_type);
        }

        [Test]
        public void Test_commision()
        {
            Console.WriteLine(result.noComission);
            Assert.AreEqual(false, result.noComission);
        }

        [Test]
        public void Test_sleepingplaces()
        {
            Console.WriteLine(result.SleepingPlaces);
            Assert.AreEqual(-1, result.SleepingPlaces);
        }

        [Test]
        public void Test_commercialobjectsize()
        {
            Console.WriteLine(result.commercialObjectSize);
            Assert.AreEqual(-1.0d, result.commercialObjectSize);
        }

        [Test]
        public void Test_housecountfloor()
        {
            Console.WriteLine(result.houseCountFloor);
            Assert.AreEqual(-1.0d, result.houseCountFloor);
        }

        [Test]
        public void Test_distancetocity()
        {
            Console.WriteLine(result.distanceToCity);
            Assert.AreEqual(-1.0d, result.distanceToCity);
        }

        [Test]
        public void Test_housegardensize()
        {
            Console.WriteLine(result.houseGardenSize);
            Assert.AreEqual(-1.0d, result.houseGardenSize);
        }

        [Test]
        public void Test_housesize()
        {
            Console.WriteLine(result.houseSize);
            Assert.AreEqual(-1.0d, result.houseSize);
        }

        [Test]
        public void Test_currency()
        {
            Console.WriteLine(result.currency);
            Assert.AreEqual("грн", result.currency);
        }

        [Test]
        public void Test_price()
        {
            Console.WriteLine(result.price);
            Assert.AreEqual(759335, result.price);
        }

        [Test]
        public void Test_wctype()
        {
            Console.WriteLine(result.wctype);
            Assert.AreEqual("совмещенный;", result.wctype);
        }

        [Test]
        public void Test_houseType()
        {
            Console.WriteLine(result.houseType);
            Assert.AreEqual("малосемейка;", result.houseType);
        }

        [Test]
        public void Test_floor()
        {
            Console.WriteLine(result.floor);
            Assert.AreEqual(7, result.floor);
            Assert.AreEqual(9, result.countFloors);

        }

        [Test]
        public void Test_avatar()
        {
            Console.WriteLine(result.userAvatarLink);
            Assert.AreEqual(null, result.userAvatarLink);
        }

        [Test]
        public void Test_kitchenSize()
        {
            Console.WriteLine(result.kitchenSize);
            Assert.AreEqual(0, result.kitchenSize);
        }

        [Test]
        public void Test_livingSize()
        {
            Console.WriteLine(result.livingSize);
            Assert.AreEqual(0, result.livingSize);
        }

        [Test]
        public void Test_allSize()
        {
            Console.WriteLine(result.allSize);
            Assert.AreEqual(45, result.allSize);
        }

        [Test]
        public void Test_roomsCount()
        {
            Console.WriteLine(result.roomsCount);
            Assert.AreEqual(2, result.roomsCount);
        }

        [Test]
        public void Test_description()
        {
            Console.WriteLine(result.description);
            Assert.AreEqual("Хорошое состояние, раздельная, консьерж, чистая продажа, рядом метро.", result.description);
        }

        [Test]
        public void Test_title()
        {
            Console.WriteLine(result.title);
            Assert.AreEqual("Киев, .", result.title);
        }



        [Test]
        public void TestLinksCount()
        {
            Template riaTemplate = new Template(templateFilePath);
            var content = File.ReadAllText(this.linkSourceFilePath);
            var result = _extractor.Extract(content, "dom.ria.ua", riaTemplate);
            //Console.WriteLine(result.other_obstoyatelstva);
            for (int i = 0; i < result.LinkToPhotos.Count(); i++)
                if (string.IsNullOrEmpty(result.LinkToPhotos[i]))
                    result.LinkToPhotos.RemoveAt(i);
            Assert.AreEqual(25, result.LinkToPhotos.Count);
        }

        [Test]
        public void TestLinksCount1()
        {
            Template riaTemplate = new Template(templateFilePath);
            var content = File.ReadAllText(this.linkSourceFilePath1);
            var result = _extractor.Extract(content, "dom.ria.ua", riaTemplate);
            //Console.WriteLine(result.other_obstoyatelstva);
            for (int i = 0; i < result.LinkToPhotos.Count(); i++)
                if (string.IsNullOrEmpty(result.LinkToPhotos[i]))
                {
                    result.LinkToPhotos.RemoveAt(i);
                    i = 0;
                }

            for (int i = 0; i < result.LinkToPhotos.Count(); i++)
                Console.WriteLine(result.LinkToPhotos[i]);
            Console.WriteLine("---------------");


            Assert.AreEqual(1, result.LinkToPhotos.Count);
        }

        [Test]
        public void TestLinksCount3()
        {
            Template riaTemplate = new Template(templateFilePath);
            DataExtactor extractor = new DataExtactor();
            var content = File.ReadAllText(this.linkSourceFilePath3);
            var result = extractor.Extract(content, "dom.ria.ua", riaTemplate);
            //Console.WriteLine(result.other_obstoyatelstva);
            for (int i = 0; i < result.LinkToPhotos.Count(); i++)
                if (string.IsNullOrEmpty(result.LinkToPhotos[i]))
                    result.LinkToPhotos.RemoveAt(i);
            Assert.AreEqual(3, result.LinkToPhotos.Count);
        }

        [Test]
        public void TestLinksCount8()
        {
            Template riaTemplate = new Template(templateFilePath);
            DataExtactor extractor = new DataExtactor();
            var content = File.ReadAllText(this.linkSourceFilePath8);
            var result = extractor.Extract(content, "dom.ria.ua", riaTemplate);
            //Console.WriteLine(result.other_obstoyatelstva);
            for (int i = 0; i < result.LinkToPhotos.Count(); i++)
                if (string.IsNullOrEmpty(result.LinkToPhotos[i]))
                    result.LinkToPhotos.RemoveAt(i);
            Assert.AreEqual(8, result.LinkToPhotos.Count);
        }

        [Test]
        public void TestLinksCount9()
        {
            Template riaTemplate = new Template(templateFilePath);
            var content = File.ReadAllText(this.linkSourceFilePath9);
            var result = _extractor.Extract(content, "dom.ria.ua", riaTemplate);
            //Console.WriteLine(result.other_obstoyatelstva);
            for (int i = 0; i < result.LinkToPhotos.Count(); i++)
                if (string.IsNullOrEmpty(result.LinkToPhotos[i]))
                    result.LinkToPhotos.RemoveAt(i);
            Assert.AreEqual(9, result.LinkToPhotos.Count);
        }

        [Test]
        public void TestLinksCount10()
        {
            Template riaTemplate = new Template(templateFilePath);
            var content = File.ReadAllText(this.linkSourceFilePath10);
            var result = _extractor.Extract(content, "dom.ria.ua", riaTemplate);
            for (int i = 0; i < result.LinkToPhotos.Count(); i++)
                if (string.IsNullOrEmpty(result.LinkToPhotos[i]))
                    result.LinkToPhotos.RemoveAt(i);
            Assert.AreEqual(10, result.LinkToPhotos.Count);
        }

        //[Test]
        public void TestSetOfFilesNoException()
        {
            Template riaTemplate = new Template(templateFilePath);
            string[] filePaths = Directory.GetFiles(ListOfFilePath);
            for (int i = 0; i < filePaths.Length; i++)
            {
                Console.WriteLine(filePaths[i]);
                var content = File.ReadAllText(filePaths[i]);
                var result = _extractor.Extract(content, "dom.ria.ua", riaTemplate);


                Assert.AreEqual(true, result != null);
            }
        }
        //linksToDetailsAndPagesPath

        [Test]
        public void TestLinksToDetail()
        {
            Template riaTemplate = new Template(templateFilePath);
            var content = File.ReadAllText(this.linksToDetailsAndPagesPath);
            HtmlParser.HtmlParser parser = new HtmlParser.HtmlParser();

            int counter = 0;
            foreach (var link in parser.GetLinks(content, "http://dom.ria.ua", riaTemplate.links[0]))
            {
                Console.WriteLine(link);
                if (!string.IsNullOrEmpty(link))
                    counter++;
            }
            Assert.AreEqual(10, counter);
        }

        [Test]
        public void TestLinksToPages()
        {
            Template riaTemplate = new Template(templateFilePath);
            var content = File.ReadAllText(this.linksToDetailsAndPagesPath);
            HtmlParser.HtmlParser parser = new HtmlParser.HtmlParser();

            int counter = 0;
            foreach (var link in parser.GetLinks(content, "http://dom.ria.ua", riaTemplate.pages[0]))
            {
                Console.WriteLine(link);
                if (!string.IsNullOrEmpty(link))
                    counter++;
            }
            Assert.AreEqual(1, counter);
        }

        [Test]
        public void TestNoLinksToPages()
        {
            Template riaTemplate = new Template(templateFilePath);
            var content = File.ReadAllText(this.noLinksToDetailsAndPagesPath);
            HtmlParser.HtmlParser parser = new HtmlParser.HtmlParser();

            int counter = 0;
            foreach (var link in parser.GetLinks(content, "http://dom.ria.ua", riaTemplate.pages[0]))
            {
                Console.WriteLine(link);
                if (!string.IsNullOrEmpty(link))
                    counter++;
            }
            Assert.AreEqual(0, counter);
        }
    }
}
