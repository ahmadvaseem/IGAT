using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;

namespace igat.com
{
    class WebCrawler2
    {
        Queue<string> urls = new Queue<string>();
        HashSet<string> _savingUrls = new HashSet<string>();
        public Queue<string> URLs
        {
            get { return urls; }
            set { urls = value; }
        }
        public HashSet<string> SavedUrls
        {
            get { return _savingUrls; }
            set { _savingUrls = value; }
        }
        DatabaseWorker DBObj = new DatabaseWorker();

        public void getLinks()
        {
            //Original URL Example www.eurogamer.net/ajax.php?action=frontpage&page=1&platform=&type=review&topic=
            string firstPartUrl = "http://www.eurogamer.net/ajax.php?action=frontpage&page=";
            string secondPartUrl = "&platform=&type=review&topic=";
            string mainLink = "http://www.eurogamer.net/";
            int pageCounter = 1;
            int id = 2;
            bool isIN;
            string webUrl = "eurogamer";
            string url;
            string data;

            Regex notContains = new Regex("mailto|reddit|image|digitalfoundry|;|jpg|gif|png|comment");
            string keyword = "articles";
            Regex contains = new Regex(keyword);
            string urlRegexTxt = "(?<URL>(?:(?!javascript)(?!#))[a-zA-Z0-9\\~\\!\\@\\#\\$" +
                         "\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]+)" + "(?:$||\\s)";
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            do
            {
                url = firstPartUrl + pageCounter + secondPartUrl;
                data = client.DownloadString(url);

                MatchCollection matches = Regex.Matches(data, urlRegexTxt);
                for (int i = 0; i < matches.Count; i++)
                {
                    Match oMatch = matches[i];
                    if (contains.IsMatch(oMatch.Value) && !notContains.IsMatch(oMatch.Value))
                    {
                        int index = oMatch.Value.IndexOf("/");

                        if (oMatch.Value.Contains("www"))
                        {
                            isIN = SavedUrls.Add(oMatch.Value);
                            /*if (isIN)
                                URLs.Enqueue(oMatch.Value);*/
                        }
                        else
                        {
                            if (index == 0)
                            {
                                mainLink = mainLink.TrimEnd('/');
                            }
                            isIN = SavedUrls.Add(mainLink + oMatch.Value);
                            if (isIN)
                                URLs.Enqueue(mainLink + oMatch.Value);
                        }
                    }
                }
                pageCounter++;
            }
            while (data.Contains(keyword));

            Dictionary<int, string> websites = new Dictionary<int, string>();
            websites = DBObj.RetrieveWebsite();
            foreach (var urlSet in websites)
            {
                if (urlSet.Value.Contains(webUrl))
                    id = urlSet.Key;
            }
            foreach (string _url in SavedUrls)
            {
                DBObj.InsertPageLinks(_url, id);
            }

        }
    }
}
