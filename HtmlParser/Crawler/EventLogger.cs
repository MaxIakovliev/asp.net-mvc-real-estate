using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace HtmlParser.Crawler
{
   public class EventLogger
    {
        /// <summary>
        /// очередь линков 
        /// </summary>
        private Queue<string> eventQueue;
        /// <summary>
        /// локер для linksQueue
        /// </summary>
        private readonly object eventQueueLocker;
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


        public EventLogger()
        {
            eventQueue = new Queue<string>();
            eventQueueLocker = new object();
            fileReaderLock = new object();
            logNotification = new object();
            terminateLogLock = new object();
            countPackSize = 10;
            this.logFileName = "eventLog.txt"; //System.Configuration.ConfigurationManager.AppSettings["logFileName"];
            Thread t1 = new Thread(DoLog);

            t1.IsBackground = true;

            t1.Start();

        }

        public void TerminateLog()
        {

            lock (terminateLogLock)
            {
                terminateLog = true;
            }

            lock (logNotification)
            {
                Monitor.PulseAll(logNotification);//пробуждаем поток  работы с файлом
            }
        }


        /// <summary>
        /// добавляет  линк в очередь логирования
        /// </summary>
        /// <param name="link"></param>
        public void AddEventToLog(string link)
        {
            bool flushToFile = false;//надо ли сбрасывать  содержимое очереди на диск
            lock (eventQueueLocker)
            {
                eventQueue.Enqueue(link);
                if (eventQueue.Count() >= countPackSize)
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
        /// сброс в файл данных  из очереди на логирование
        /// </summary>
        private void DoLog()
        {
            bool stopLoop = false;
            while (!stopLoop)
            {
                string[] workData = null;
                lock (eventQueueLocker)
                {
                    workData = new string[eventQueue.Count()];
                    for (int i = 0; i < workData.Length; i++)
                        workData[i] = eventQueue.Dequeue();//переносим  с общей очереди во временный буфер
                }
                lock (fileReaderLock)//лочим обращение  к файлу на запись
                {
                    using (StreamWriter file = File.AppendText(logFileName))//logFileName -потоко безопасная т.к.  помечена readonly
                    {
                        for (int i = 0; i < workData.Length; i++)
                        {
                            file.WriteLine(DateTime.Now.ToString()+"- "+workData[i]);
                        }
                        file.Flush();
                    }
                }

               

                lock (terminateLogLock)
                {
                    stopLoop = terminateLog;
                }

                lock (logNotification)
                {
                    Monitor.Wait(logNotification);//ожидаем  следующего пробуждения
                }
            }
            return;
        }
    }
}
