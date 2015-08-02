using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;

namespace CountWordsInText
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder data = new StringBuilder();

            using (FileStream fs = File.Open("bigFile.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    data.Append(line);
                }
            }

            var  words=CountWords(data);
            foreach (var item in words)
            {
                File.AppendAllText("result.txt", item.Key.ToString()+"\t"+item.Value.ToString()+Environment.NewLine);               
            }

        }


        private static SortedDictionary<string, int> CountWords(StringBuilder data)
        {
            SortedDictionary<string, int> words = new SortedDictionary<string, int>();
            StringBuilder buffer = new StringBuilder(100);
            for (int i = 0; i < data.Length; i++)
            {
                if (char.IsLetter(data[i]))
                    buffer.Append(data[i]);
                else
                {
                    if (buffer.Length > 0)
                    {
                        string tmp = buffer.ToString().ToLower();
                        if (words.ContainsKey(tmp))
                        {
                            words[tmp] = words[tmp] + 1;
                        }
                        else
                        {
                            words.Add(tmp, 1);
                        }
                        buffer.Clear();
                    }
                    else
                    {
                        if (words.ContainsKey(data[i].ToString()))
                        {
                            words[data[i].ToString()] = words[data[i].ToString()] + 1;
                        }
                        else
                        {
                            words.Add(data[i].ToString(), 1);
                        }
                    }
                }
            }

            return words;
        }
    }
}
