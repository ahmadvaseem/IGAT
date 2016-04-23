using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Crwal_v0._2
{
    class test2
    {
        static Queue<string> urls = new Queue<string>();
        static public Queue<string> URLs
        {
            get { return urls;  }
            set { urls = value; }
        }
        static void Main0(string[] args)
        {
            WebClient client = new WebClient();
            client.DownloadString("http://www.eurogamer.net/?type=review");
            URLs.Enqueue("http://www.eurogamer.net/?type=review");
            string l = "http://www.eurogamer.net/";
            int a = 0;
            while (a != 10)
            {

                string url = urls.Dequeue();
                string html = client.DownloadString(url);
                MatchCollection matches = Regex.Matches(html, "(?<URL>(?:(?!javascript)(?!#))[a-zA-Z0-9\\~\\!\\@\\#\\$" +
                         "\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]+)" +
                         "(?<!(?:\\.png|\\.js|\\.jpg|\\.jpeg|\\.css|\\.ico|\\.gif|\\.php|\\.pn|\\.net|\\.zip|\\.r|\\.jp" +
                         "ar))(?:$||\\s)");

                for (int i = 0; i < matches.Count; i++)
                {
                    Match oMatch = matches[i];
                    if (oMatch.Value.Contains("-review") && !oMatch.Value.Contains("facebook") && !oMatch.Value.Contains("twitter") && !oMatch.Value.Contains("whatsapp") && !oMatch.Value.Contains("reddit") && !oMatch.Value.Contains("mailto:") && !oMatch.Value.Contains("image"))
                    {
                        URLs.Enqueue(l+oMatch.Value);
                    }
                }
                a++;
                foreach (string i in URLs)
                {
                    Console.WriteLine(i);
                }

            }
            Console.WriteLine("\n");
            Console.ReadLine();
        }
    }
}
