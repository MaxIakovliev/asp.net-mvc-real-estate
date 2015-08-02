using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections;

namespace HtmlParser.Crawler
{
    public class LinkLogger
    {
        /// <summary>
        /// очередь линков 
        /// </summary>
        private Queue<string> linksQueue;
        /// <summary>
        /// локер для linksQueue
        /// </summary>
        private readonly object linksQueueLocker;
        /// <summary>
        /// минимальный размер очереди для сброса в файл
        /// </summary>
        private readonly int countPackSize;
        /// <summary>
        /// название файла лога
        /// </summary>
        private readonly string logFileName;
        /// <summary>
        /// локер работы с файлом логирования
        /// </summary>
        private readonly object fileReaderLock;
        /// <summary>
        /// ноттификация  потока логирования
        /// </summary>
        private readonly object logNotification;

        private bool terminateLog = false;
        private readonly object terminateLogLock;

        public LinkLogger(string logFileName)
        {
            linksQueue = new Queue<string>();
            linksQueueLocker = new object();
            fileReaderLock = new object();
            logNotification = new object();
            terminateLogLock = new object();
            countPackSize = 10;
            this.logFileName = logFileName; //System.Configuration.ConfigurationManager.AppSettings["logFileName"];
            Thread t1 = new Thread(DoLog);

            t1.IsBackground = true;

            t1.Start();

        }//

        public void TerminateLog()
        {
            lock (terminateLogLock)
            {
                terminateLog = true;
            }
        }


        /// <summary>
        /// добавляет  линк в очередь логирования
        /// </summary>
        /// <param name="link"></param>
        public void AddLinkToLog(string link)
        {
            bool flushToFile = false;//надо ли сбрасывать  содержимое очереди на диск
            lock (linksQueueLocker)
            {
                linksQueue.Enqueue(link);
                if (linksQueue.Count() >= countPackSize)
                    flushToFile = true;//надо сбросить содержимое
            }
            if (flushToFile)
            {
                lock (logNotification)
                {
                    Monitor.PulseAll(logNotification);//пробуждаем поток  работы с файлом
                }
            }
        }

        /// <summary>
        /// чтение обработанных линков из лога
        /// </summary>
        /// <returns>список линков</returns>
        public Hashtable GetLinksFromLog()
        {
            var result = new Hashtable();
            lock (fileReaderLock)//лочим обращение  к файлу на запись
            {
                if (!File.Exists(logFileName))
                    return result;

                using (StreamReader sr = new StreamReader(logFileName))//logFileName -потоко безопасная т.к.  помечена readonly
                {
                    while (sr.Peek() >= 0)
                    {
                        string link = sr.ReadLine();
                        if (!result.ContainsKey(link))
                            result.Add(link, true);
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// сброс в файл данных  из очереди на логирование
        /// </summary>
        private void DoLog()
        {
            bool stopLoop = false;
            while (!stopLoop)
            {
                string[] workData = null;
                lock (linksQueueLocker)
                {
                    workData = new string[linksQueue.Count()];
                    for (int i = 0; i < workData.Length; i++)
                        workData[i] = linksQueue.Dequeue();//переносим  с общей очереди во временный буфер
                }
                lock (fileReaderLock)//лочим обращение  к файлу на запись
                {
                    using (StreamWriter file = File.AppendText(logFileName))//logFileName -потоко безопасная т.к.  помечена readonly
                    {
                        for (int i = 0; i < workData.Length; i++)
                        {
                            file.WriteLine(workData[i]);
                        }
                        file.Flush();
                    }
                }

                lock (logNotification)
                {
                    Monitor.Wait(logNotification);//ожидаем  следующего пробуждения
                }

                lock (terminateLogLock)
                {
                    stopLoop = terminateLog;
                }
            }
            return;
        }


    }
}
