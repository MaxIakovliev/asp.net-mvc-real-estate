using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace HtmlParser.Crawler
{
    public class Hash : Cleaner
    {
        private readonly int countWords;

        private SortedDictionary<ulong, List<ulong>> storage;
        private readonly string fileCachePath;

        public Hash(string stopWordsFilePath, string fileCachePath, int countWords)
            : base(stopWordsFilePath)
        {
            this.countWords = countWords;
            this.storage = new SortedDictionary<ulong, List<ulong>>();

            this.fileCachePath = fileCachePath;
            RestoreDataFromCache();

        }

       

        /// <summary>
        /// восстанавливаем данные из кеша
        /// </summary>
        private void RestoreDataFromCache()
        {
            if (!File.Exists(this.fileCachePath)) return;

            using (FileStream fs = File.Open(this.fileCachePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;//skipping empty lines

                    var data = line.Split('|');
                    if (data == null || data.Length != 3)
                        throw new Exception("RestoreDataFromCache Error: data should have 3 cells");

                    var key = Convert.ToUInt64(data[0]);
                    var listOfHashes = data[1].Split(',');
                    var uList = new List<ulong>();

                    for (int i = 0; i < listOfHashes.Length; i++)
                        uList.Add(Convert.ToUInt64(listOfHashes[i]));

                    this.storage.Add(key, uList);

                    uList = null;
                    listOfHashes = null;
                    data = null;

                }
            }
        }

        /// <summary>
        /// сохранение  кеша в файл
        /// </summary>
        public void StoreCache()
        {
            if (File.Exists(this.fileCachePath))
            {
                string backUpFilePath = this.fileCachePath + DateTime.Today.ToString();
                if (File.Exists(backUpFilePath))
                {
                    File.Delete(backUpFilePath);
                }
                    File.Move(this.fileCachePath, backUpFilePath);
                    
                StringBuilder sb = new StringBuilder();
                foreach (var item in this.storage)
                {
                    sb.Append(item.Key.ToString() + "|");
                    for (int i = 0; i < item.Value.Count(); i++){
                        sb.Append(item.Value[i].ToString());
                        if (i < item.Value.Count() - 1)
                            sb.Append(",");
                    }
                    sb.Append("|" + DateTime.Today.ToString()+Environment.NewLine);
                    File.AppendAllText(this.fileCachePath, sb.ToString());
                    sb.Clear();
                }
                
            }
        }

        public List<ulong> ComputeHash(string data)
        {
            var cleanData = base.CleanString2(data);//очищаем входную строку 
            if (cleanData == null || cleanData.Length == 0)
                return null;

            //для отладки
            //var tmp=cleanData.Split(' ');
            //Array.Sort(tmp);
            //System.IO.File.WriteAllLines("dopolnitelno.txt", tmp);



            List<ulong> result = new List<ulong>();
            var uEncode = new UnicodeEncoding();
            var sha = new System.Security.Cryptography.SHA256Managed();
            byte[] bytes = null;

            StringBuilder sb = new StringBuilder(data.Length);
            int internetWordsCounter = 0;
            for (int i = 0; i < cleanData.Length; i++)
            {
                if (internetWordsCounter == this.countWords - 1)//пробелов=количество слов-1;
                {
                    bytes = uEncode.GetBytes(sb.ToString());
                    bytes = sha.ComputeHash(bytes);//считаем хеш
                    result.Add(GetUInt64(bytes, 0));//добавляем в результат


                    for (int j = 0; j < sb.Length; j++)
                    {
                        if (sb[j] == ' ')
                        {
                            sb.Remove(0, j);//удаляем все кроме последнего слова, чтобы получить слова в "нахлёст"

                            if (sb.Length > 0 && sb[0] == ' ')
                                sb.Remove(0, 1);//удаляем все кроме последнего слова, чтобы получить слова в "нахлёст"

                            internetWordsCounter--;//сбрасываем счетчик
                            break;
                        }
                    }
                    ////удаляем всё кроме последнего слова                    
                    //for (int j = sb.Length - 1; j >= 0; j--)
                    //{
                    //    if (sb[j] == ' ')
                    //    {
                    //        sb.Remove(0, j);//удаляем все кроме последнего слова, чтобы получить слова в "нахлёст"
                    //        internetWordsCounter = 0;//сбрасываем счетчик
                    //        break;
                    //    }
                    //}
                }

                if (!Char.IsLetterOrDigit(cleanData[i]))
                    internetWordsCounter++;

                sb.Append(cleanData[i]);
            }


            if (internetWordsCounter > 0)//если в буфере осталось больше одного слова
            {
                bytes = uEncode.GetBytes(sb.ToString());
                bytes = sha.ComputeHash(bytes);//считаем хеш
                result.Add(GetUInt64(bytes, 0));//добавляем в результат
                sb.Clear();
                sb = null;
            }

            result.Sort();
            return result;
        }

        public float IsExist(List<ulong> newHashes)
        {
            if (newHashes == null || newHashes.Count() == 0) return 100;
            if (this.storage.Keys.Contains(newHashes[0]))
            {
                int countMatch = 0;
                int newCount = newHashes.Count();
                int oldCount = this.storage[newHashes[0]].Count();
                for (int i = 0; i < newCount; i++)
                    for (int j = 0; j < oldCount; j++)
                        if (newHashes[i] == this.storage[newHashes[0]][j])
                            countMatch++;

                float result = ((float)countMatch / (float)newCount) * 100;
                return result;

            }
            else
                return 0;
        }

        public void AddNewHash(List<ulong> newHashes)
        {
            if (this.storage.Keys.Contains(newHashes[0]))
            {


                int newCount = newHashes.Count();
                for (int i = 0; i < newCount; i++)
                    if (!this.storage.Keys.Contains(newHashes[i]))
                    {
                        this.storage.Add(newHashes[i], newHashes);
                        return;
                    }

                //int oldCount = this.storage[newHashes[0]].Count();
                //for (int i = 0; i < newCount; i++)
                //    for (int j = 0; j < oldCount; j++)
                //        if (newHashes[i] != this.storage[newHashes[0]][j])
                //        {
                //            this.storage.Add(newHashes[i], newHashes);
                //            return;
                //        }
            }
            else
            {
                this.storage.Add(newHashes[0], newHashes);
            }
        }


        public int GetCountObjectInStorage()
        {
            return this.storage.Count();
        }

        unsafe public ulong GetUInt64(byte[] bb, int pos)
        {
            // we only read aligned longs, so a simple casting is enough
            fixed (byte* pbyte = &bb[pos])
            {
                return *((ulong*)pbyte);
            }
        }
    }
}
