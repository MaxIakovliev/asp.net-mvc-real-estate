using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace NetLayer
{
    public class NetFetcher 
    {

        public byte[] getDataAsByteArray(string Url)
        {
            byte[] downloadedData = new byte[0];
            WebResponse response = null;
            WebRequest req = null;
            Stream stream = null;
            MemoryStream memStream = null;
            try
            {
                //Get a data stream from the url
                req = WebRequest.Create(Url);
                response = req.GetResponse();
                stream = response.GetResponseStream();

                //responseUrl = response.ResponseUri.ToString();

                //Download in chuncks
                byte[] buffer = new byte[1024];

                //Get Total Size
                int dataLength = (int)response.ContentLength;

                //Download to memory
                //Note: adjust the streams here to download directly to the hard drive
                memStream = new MemoryStream();
                while (true)
                {
                    //Try to read the data
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        break;
                    }
                    else
                    {
                        //Write the downloaded data
                        memStream.Write(buffer, 0, bytesRead);
                    }
                }

                //Convert the downloaded stream to a byte array
                downloadedData = memStream.ToArray();

                //Clean up
                stream.Close();
                memStream.Close();
                response.Close();

                stream.Dispose();
                memStream.Dispose();
                buffer = null;
                

            }
            catch (Exception e)
            {
                //responseUrl = string.Empty;
                return new byte[0];
            }
            finally
            {
                if (req != null)
                    req = null;

                if (response != null)
                    response = null;

                if (stream != null)
                    stream = null;

                if (memStream != null)
                    memStream = null;
            }

            return downloadedData;
        }


        public string GetContentAsString(string url, out string responseUrl, string codePage, out string errMsg)
        {
            string result = string.Empty;
            responseUrl = errMsg = string.Empty;
            try
            {
                result = DownloadWebPage(url, out responseUrl);
            }
            catch (Exception ex)
            {
                errMsg = url + "; error-" + ex.Message;
            }

            return result;

        }





        public string DownloadWebPage(string url, Encoding codePage)
        {
            
            // Open a connection
            HttpWebRequest webRequestObject = (HttpWebRequest)HttpWebRequest.Create(url);

            // You can also specify additional header values like
            // the user agent or the referer:
            webRequestObject.UserAgent = "Googlebot/1.0 (googlebot@googlebot.com http://googlebot.com/)";
            webRequestObject.Referer = "http://www.google.com/";

            // Will contain the content of the page
            string pageContent = string.Empty;

            // Request response:
            using (WebResponse response = webRequestObject.GetResponse())
            {
                url= response.ResponseUri.ToString();
                // Open stream and create reader object:
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), codePage))//Encoding.GetEncoding(codePage < 0 ? 1251 : codePage)
                {
                    // Read the entire stream content:
                    pageContent = reader.ReadToEnd();
                }
            }

            return pageContent;
        }


        public string DownloadWebPage(string Url, out string responseUrl)
        {
            // Open a connection
            HttpWebRequest webRequestObject = (HttpWebRequest)HttpWebRequest.Create(Url);

            // You can also specify additional header values like
            // the user agent or the referer:
            webRequestObject.UserAgent = "Googlebot/1.0 (googlebot@googlebot.com http://googlebot.com/)";
            webRequestObject.Referer = "http://www.google.com/";

            // Will contain the content of the page
            string pageContent = string.Empty;

            // Request response:
            using (WebResponse response = webRequestObject.GetResponse())
            {
                responseUrl = response.ResponseUri.ToString();
                // Open stream and create reader object:
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    // Read the entire stream content:
                    pageContent = reader.ReadToEnd();
                }
            }

            return pageContent;
        }
    }

}
