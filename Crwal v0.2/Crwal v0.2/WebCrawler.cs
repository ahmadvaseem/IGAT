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
    class WebCrawler
    {
        Queue<string> urls = new Queue<string>();
        Regex contains = new Regex("facebook|twitter|whatsapp|mailto|reddit");
        HashSet<string> _savingUrls = new HashSet<string>();
        SqlConnection connection = new SqlConnection(@"Data Source=VACEEM-PC\AHMADSQLSERVER;Initial Catalog=Ratingdb1;Integrated Security=True");
        public Queue<string> URLs
        {
            get { return urls; }
            set { urls = value; }
        }

        public void getPages()
        {
            string siteLink = "http://www.pcgamer.com/reviews";
            _savingUrls.Add(siteLink);
            URLs.Enqueue(siteLink);
            getLinks(siteLink);
        }
        public void getLinks(string url)
        {
            WebClient client = new WebClient();
            //client.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
            int a = 0;
            while (a!=10)
            {
                url = urls.Dequeue();
                string html = client.DownloadString(url);
                MatchCollection matches = Regex.Matches(html, "(?<URL>(?:(?!javascript)(?!#))[a-zA-Z0-9\\~\\!\\@\\#\\$" +
                         "\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]+)" +
                         "(?<!(?:\\.png|\\.js|\\.jpg|\\.jpeg|\\.css|\\.ico|\\.gif|\\.php|\\.pn|\\.net|\\.jp|\\.zip|\\.r" +
                         "ar))(?:$||\\s)");

                for (int i = 0; i < matches.Count; i++)
                {
                    Match oMatch = matches[i];
                    if (oMatch.Value.Contains("pcgamer") && oMatch.Value.Contains("-review") && !contains.IsMatch(oMatch.Value))
                    {
                        URLs.Enqueue(oMatch.Value);
                        int lastIndex = oMatch.Value.LastIndexOf('w');
                        try
                        {
                            char m = oMatch.Value.ElementAt(lastIndex + 1);

                            if (m == '/')
                                _savingUrls.Add(oMatch.Value);


                        }
                        catch (Exception e)
                        {
                            _savingUrls.Add(oMatch.Value + '/');
                        }

                    }

                }
                Console.WriteLine(a);
                a++;
            }
            connection.Open();

            foreach (string i in _savingUrls)
            {
                //SqlCommand command = new SqlCommand("INSERT into pageLinks (Link) VALUES ('"+i+"')", connection);
                //command.ExecuteNonQuery();
                Console.WriteLine(i);
            }
            connection.Close();
        }
    }
}
