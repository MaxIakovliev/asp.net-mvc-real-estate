using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HtmlParser.Infrastructure
{
   public class FileAvailability
    {
        private string[] files = new string[] { "\\stopWords.txt", "\\CacheHash.txt", "\\logLinks.txt" };
        private string filePath;


        /// <summary>
        /// проверяет доступность требуемых файлов
        /// </summary>
        /// <param name="filePath"><путь к файлам/param>
        /// <param name="msg">сообщение об ошибке</param>
        /// <returns>результат</returns>
        public bool CheckFilePaths(string filePath, out string msg)
        {
            this.filePath = filePath;
            msg = string.Empty;
            for (int i = 0; i < this.files.Length; i++)
            {
                if (!File.Exists((filePath + files[i]).Replace("\\\\","\\")))
                {
                    msg = string.Format("Error- {0} not found", files[i]);
                    return false;
                }

            }
            return true;
        }

        public string GetFileCacheHash()
        {
            return this.filePath+files[(int)EFiles.CacheHash];
        }

        public string GetStopWordsFile()
        {
            return this.filePath + files[(int)EFiles.stopWords];
        }

        public string GetLogLinksFile()
        {
            return this.filePath + files[(int)EFiles.logFileName];
        }
    }
}
