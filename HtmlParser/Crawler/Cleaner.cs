using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HtmlParser.Crawler
{
    /// <summary>
    /// Отвечает за подготовку очистку входной строки
    /// Ссылается на HtmlParser
    /// </summary>
    public class Cleaner
    {
        private string filePath = string.Empty;
        private string[] keywords;
        public Cleaner(string filePath)
        {
            this.filePath = filePath;
            if (!File.Exists(filePath))
                throw new FileNotFoundException("file with stop words not found on " + filePath);

            keywords = File.ReadAllLines(filePath);//fill list of stop words

            for (int i = 0; i < this.keywords.Length; i++)// do lowercase to insure we match all stopwords without additional overheads
                this.keywords[i] = this.keywords[i].ToLower();
        }


        /// <summary>
        /// подготавливает строку к хешированию
        /// </summary>
        /// <param name="data">входной набор символов</param>
        /// <returns>StringBuilder or null</returns>
        private StringBuilder CleanString(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            HtmlParser parser = new HtmlParser();
            var result = new StringBuilder(data.ToLower());
            parser.ClearContent(result);
            for (int i = 0; i < this.keywords.Length; i++)
            {
                if (this.keywords[i] != "")
                {
                    while (result.Contains(this.keywords[i]) >= 0)
                        result.Replace(this.keywords[i], string.Empty);
                }
            }
            parser.ClearContent(result);//еще раз зачищаем строку
            return result;
        }




        public string CleanString2(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            HtmlParser parser = new HtmlParser();
            //var result = new StringBuilder(data.ToLower());
            data = data.ToLower();
            var tmpData = parser.ClearContent(data).Split(' ').ToList<string>();

            for (int i = 0; i < this.keywords.Length; i++)
            {
                if (this.keywords[i] != "")
                {
                    int j = 0;
                    while (j < tmpData.Count())
                    {
                        if (this.keywords[i].Length == 1 && !Char.IsLetter(this.keywords[i][0]))
                        {
                            tmpData[j] = tmpData[j].Replace(this.keywords[i], " ");
                            if(tmpData[j].Length==0)
                                tmpData.RemoveAt(j);
                            j++;
                        }
                        else if (tmpData[j] == this.keywords[i])
                        {
                            tmpData.RemoveAt(j);
                        }
                        else { j++; }

                    }
                }
            }

            StringBuilder sb = new StringBuilder(100);
            for (int i = 0; i < tmpData.Count(); i++)
            {
                sb.Append(tmpData[i] + " ");
            }
            string result = parser.ClearContent(sb.ToString().Trim());//еще раз зачищаем строку
            return result;
        }

    }
}
