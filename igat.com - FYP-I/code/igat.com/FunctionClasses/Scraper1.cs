///-----------------------------------------------------------------
///   Namespace:      igat.com
///   Class:          Scraper1
///   Description:    A class that is scraping the html pages and parsing the comments from Polygon website that are available for each game, these games
///                     comments will be saved in database by calling Database worker class method that is maintaning databse functions.
///   Author:         Ahmad Waseem                    Date: 8/4/2016
///   Notes:          version 5.4
///   Revision History 
///   Version: 4.0           Date: 29/5/2016
///   Version: 3.0           Date: 23/5/2016
///   Version: 2.0           Date: 17/4/2016 
///   Version: 1.0           Date: 8/4/2016        
///-----------------------------------------------------------------
using System;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Diagnostics;

namespace igat.com
{
    class Scraper1
    {
        Dictionary<string, string> slangDict = new Dictionary<string, string>();
        Dictionary<int, string> linksDict = new Dictionary<int, string>();
        List<string> abusesList = new List<string>();
        string website = "polygon";
        DatabaseWorker DBObj = new DatabaseWorker();
        public Dictionary<string, string> slangDictionary
        {
            get { return slangDict; }
            set { slangDict = value; }
        }
        int gameId = 0;
        Filters filterInst = new Filters();
        /// <summary>
        /// 
        /// </summary>
        public void getCommentsUrl()
        {
            string completeUrl = null;
            WebClient client = new WebClient();
            string firstpartUrl = "http://www.polygon.com/comments/load_comments/";
            string data = null;

            slangDictionary = DBObj.RetrieveSlangs();
            abusesList = filterInst.abuseFilter();
            linksDict = DBObj.RetrieveLinks(website);

        ReaderPoint:
            foreach (var value in linksDict)
            {
                if (value.Value.Contains(website))
                {
                    data = client.DownloadString(value.Value);
                    HtmlDocument doc = new HtmlDocument();
                    try
                    {
                        doc.LoadHtml(data);
                        var node = doc.DocumentNode.SelectSingleNode("//h1");
                        var gameName = node.InnerText;
                        gameName = Regex.Replace(gameName, " review", "");
                        var urlId = node.Attributes["data-remote-admin-entry-id"].Value;
                        completeUrl = firstpartUrl + urlId;

                        DBObj.InsertGames(gameName, value.Key);
                        gameId = DBObj.getGameId(gameName);
                        getComments(completeUrl);
                    }
                    catch
                    {
                        goto ReaderPoint;
                    }
                }
            }
        }

        void getComments(string compltUrl)
        {
            WebClient c = new WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            string json = c.DownloadString(compltUrl);
            JToken dat = JToken.Parse(@json);
            JArray arr = (JArray)dat.SelectToken("comments");
            foreach (JObject parsedObject in arr.Children<JObject>())
            {
                foreach (JProperty parsedProperty in parsedObject.Properties())
                {
                    string propertyName = parsedProperty.Name;
                    if (propertyName.Equals("body"))
                    {
                        string propertyValue = (string)parsedProperty.Value;
                        //Console.WriteLine("Name: {0}, Value: {1}", propertyName, propertyValue);
                        propertyValue = Regex.Replace(propertyValue, @"<[^>]*>|@[-_a-zA-Z0-9]*|([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+)|<[^>]*>|(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*|((?::|;|=)(?:-)?(?:\)|d|p|/|\\|\*|D|P)) ", "");
                        propertyValue = WebUtility.HtmlDecode(propertyValue);
                        foreach (var entry in slangDictionary)
                        {
                            if (propertyValue.Contains(entry.Key))
                            {
                                propertyValue.Replace(entry.Key, entry.Value);
                            }

                        }
                        foreach (var entry in abusesList)
                        {
                            if (propertyValue.Contains(entry))
                            {
                                propertyValue = propertyValue.Replace(entry, "");
                            }
                        }
                        DBObj.InsertComments(propertyValue, gameId);
                    }
                }
            }
        }

    }
}
