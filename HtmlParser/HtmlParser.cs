using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HtmlParser
{
   public class HtmlParser
    {
        StringBuilder content = new StringBuilder();
        StringBuilder contentForImgUrls = new StringBuilder();

        public IEnumerable<string> GetLinks(string data, string host, string template)
        {
            //List<string> result = new List<string>();
            host = host.ToLower();

            template = ClearContent(template);

            var templateParts = template.ToLower().Split('*');

            //var content = new StringBuilder(data.ToLower());

            content.Clear();
            content.Append(data.ToLower());

            ClearContent(content);
            int startPos = content.Contains(templateParts[0]);
            if (startPos == -1) yield return string.Empty;
            string hrefTemplate = "href=";
            int endPos = content.Contains(templateParts[1], startPos + templateParts[0].Length - 1);
            while (startPos != -1 && endPos != -1)
            {
                startPos = content.Contains(hrefTemplate, startPos);
                if (startPos == -1) yield return string.Empty;//return result;

                endPos = content.Contains(">", startPos);
                if (endPos == -1) yield return string.Empty;  //return result;

                char[] destination = new char[endPos - (startPos + hrefTemplate.Length)];
                content.CopyTo(startPos + hrefTemplate.Length, destination, 0, endPos - (startPos + hrefTemplate.Length));
                int breakPos = -1;
                for (int i = 0; i < destination.Length; i++)
                    if (destination[i] == ' ')
                    {
                        breakPos = i;
                        break;
                    }
                if (breakPos > -1)
                {
                    Array.Resize<char>(ref destination, breakPos);
                }


                string link = new string(destination);
                if (!link.StartsWith("http"))
                {
                    link = link.Trim();
                    if (link != "" && link[0] != '/')//fix 19.05                    
                        link = host + '/' + link;
                    else
                        link = host + link;
                }
                link = link.Replace("/./", "/");

                //result.Add(link);
                startPos = content.Contains(templateParts[0], endPos);
                endPos = content.Contains(templateParts[1], startPos + templateParts[0].Length - 1);
                yield return RemoveTags(link);
            }
            yield return string.Empty;//return result;
        }


        public IEnumerable<string> GetImgUrl(string data, string host, string template)
        {
            //List<string> result = new List<string>();
            host = host.ToLower();
            var templateParts = template.ToLower().Split('*');
            templateParts[0] = ClearContent(templateParts[0]);
            templateParts[1] = ClearContent(templateParts[1]);

            //var contentForImgUrls = new StringBuilder(data.ToLower());

            contentForImgUrls.Clear();
            contentForImgUrls.Append(data.ToLower());

            ClearContent(contentForImgUrls);
            int startPos = contentForImgUrls.Contains(templateParts[0]);
            if (startPos == -1) yield return string.Empty;
            string hrefTemplate = "src=";
            int endPos = contentForImgUrls.Contains(templateParts[1], startPos + templateParts[0].Length - 1);
            while (startPos != -1 && endPos != -1)
            {
                startPos = contentForImgUrls.Contains(hrefTemplate, startPos);
                if (startPos == -1) yield return string.Empty;//return result;

                endPos = contentForImgUrls.Contains(">", startPos);
                if (endPos == -1) yield return string.Empty;  //return result;

                char[] destination = new char[endPos - (startPos + hrefTemplate.Length)];
                contentForImgUrls.CopyTo(startPos + hrefTemplate.Length, destination, 0, endPos - (startPos + hrefTemplate.Length));
                int breakPos = -1;
                for (int i = 0; i < destination.Length; i++)
                    if (destination[i] == ' ')
                    {
                        breakPos = i;
                        break;
                    }
                if (breakPos > -1)
                {
                    Array.Resize<char>(ref destination, breakPos);
                }


                string link = new string(destination);
                if (!link.StartsWith("host"))
                    link = host + '/' + link;
                link = link.Replace("/./", "/");

                //result.Add(link);
                startPos = contentForImgUrls.Contains(templateParts[0], endPos);
                endPos = contentForImgUrls.Contains(templateParts[1], startPos + templateParts[0].Length - 1);
                yield return RemoveTags(link);
            }
            yield return string.Empty;//return result;
        }



        public string RemoveTags(string content)
        {
            var templateParts = new string[] { "<", ">" };
            int startPos = content.IndexOf(templateParts[0]);
            if (startPos == -1) return content.Replace('\0', ' ').Trim(); ;
            int endPos = content.IndexOf(templateParts[1], startPos + templateParts[0].Length);
            while (startPos != -1 && endPos != -1)
            {
                content = content.Remove(startPos, endPos + 1 - startPos);
                startPos = content.IndexOf(templateParts[0]);
                if (startPos == -1) return content.Replace('\0', ' ').Trim();
                endPos = content.IndexOf(templateParts[1], startPos + templateParts[0].Length);
            }
            return content.Replace('\0', ' ').Trim();//TrimEnd('\0');
        }

        public string GetSingleContent(string data, string template)
        {
            var templateParts = template.ToLower().Split('*');
            if (templateParts != null && templateParts.Length == 2)
            {
                templateParts[0] = ClearContent(templateParts[0]);
                templateParts[1] = ClearContent(templateParts[1]);
            }
            var content = new StringBuilder(data.ToLower());
            ClearContent(content);
            int startPos = content.Contains(templateParts[0]);
            if (startPos == -1) return string.Empty;
            int endPos = content.Contains(templateParts[1], startPos + templateParts[0].Length );
            if (startPos != -1 && endPos != -1)
            {
                char[] destination = new char[endPos - startPos + templateParts[0].Length];
                content.CopyTo(startPos + templateParts[0].Length, destination, 0, endPos - (startPos + templateParts[0].Length));
                string result = new string(destination);

                content = null;

                return result;//RemoveTags(result).Trim();
            }
            content = null;
            return string.Empty;//return result;
        }




        public void ClearContent(StringBuilder content)
        {
            content.Replace('\r', ' ');
            content.Replace('\n', ' ');
            content.Replace('\0', ' ');
            content.Replace('\t', ' ');



            while (content.IndexOf("  ") >= 0)
                content.Replace("  ", " ");

            while (content.IndexOf("href =") >= 0)
                content.Replace("href =", "href=");

            while (content.IndexOf("\"") >= 0)
                content.Replace("\"", "");

            while (content.IndexOf("'") >= 0)
                content.Replace("'", "");

            while (content.IndexOf("&amp;") >= 0)
                content.Replace("&amp;", "&");

        }

        public string ClearContent(string content)
        {
            StringBuilder data = new StringBuilder(content);
            ClearContent(data);
            string tmp = data.ToString();
            data.Clear();
            data = null;
            return tmp;//data.ToString();

        }


        public IEnumerable<string> GetAttributeValue(string data, string template, string attributeName)
        {            
            template = ClearContent(template);

            var templateParts = template.ToLower().Split('*');
            var content = new StringBuilder(data.ToLower());
            ClearContent(content);
            template = ClearContent(template);
            int startPos = content.Contains(templateParts[0]);
            if (startPos == -1) yield return string.Empty;
            
            int endPos = content.Contains(templateParts[1], startPos + templateParts[0].Length - 1);

            if (startPos != -1 && endPos != -1)
            {
                content.Remove(0, startPos);
                startPos = content.Contains(templateParts[0]);
                endPos = content.Contains(templateParts[1], startPos + templateParts[0].Length - 1);
                if (startPos != -1 && endPos != -1)
                {
                    content.Remove(endPos + templateParts[1].Length, content.Length - (endPos + templateParts[1].Length));
                }
            }            

            while (startPos != -1 && endPos != -1)
            {
                startPos = content.Contains(attributeName, startPos);
                if (startPos == -1) yield return string.Empty;//return result;

                endPos = content.Contains(">", startPos);
                if (endPos == -1) yield return string.Empty;  //return result;

                char[] destination = new char[endPos - (startPos + attributeName.Length)];
                content.CopyTo(startPos + attributeName.Length, destination, 0, endPos - (startPos + attributeName.Length));
                int breakPos = -1;
                for (int i = 0; i < destination.Length; i++)
                    if (destination[i] == ' ')
                    {
                        breakPos = i;
                        break;
                    }
                if (breakPos > -1)
                {
                    Array.Resize<char>(ref destination, breakPos);
                }


                string link = new string(destination);
                //if (!link.StartsWith("host"))
                //    link = host + '/' + link;
                //link = link.Replace("/./", "/");

                //result.Add(link);
                startPos = content.Contains(attributeName, endPos);
                endPos = content.Contains(">", startPos + attributeName.Length - 1);
                yield return RemoveTags(link);
            }
            yield return string.Empty;//return result;
        }
    }
}
