using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;

namespace igat.com
{
    class WebCrawler1
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
            #region variableInitialization
            string firstPartUrl = "http://www.polygon.com/games/reviewed/";
            int pageCounter = 1;
            int id = 1;
            bool isIN;
            string webUrl = "polygon";
            string url;
            string data;
            Regex keywordsNot = new Regex("mailto|reddit|image|;|jpg|gif|png|comment|js|css");
            Regex keywords = new Regex("(?=.*polygon)(?=.*-review)");
            string urlRegexTxt = "(?<URL>(?:(?!javascript)(?!#))[a-zA-Z0-9\\~\\!\\@\\#\\$" +
                         "\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]+)" + "(?:$||\\s)";
            string checkPoly = "game__body";
            WebClient client = new WebClient();
            #endregion

            client.Encoding = System.Text.Encoding.UTF8;                            // Encoding the text to UTF-8 (Unicode Transformation Format) to make all text in same format
            //Loop for Crawling, Getting Links
            do
            {
                url = firstPartUrl + pageCounter;
                data = client.DownloadString(url);
                //Matching Regular expressions with data string to extract Urls
                MatchCollection matches = Regex.Matches(data, urlRegexTxt);
                for (int i = 0; i < matches.Count; i++)
                {
                    Match oMatch = matches[i];
                    if (keywords.IsMatch(oMatch.Value) && !keywordsNot.IsMatch(oMatch.Value))
                    {
                        isIN = SavedUrls.Add(oMatch.Value);
                        if (isIN)
                            URLs.Enqueue(oMatch.Value);
                    }
                }
                pageCounter++;
            }
            while (data.Contains(checkPoly));

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
