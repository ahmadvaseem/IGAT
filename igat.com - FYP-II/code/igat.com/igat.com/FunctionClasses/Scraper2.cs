///-----------------------------------------------------------------
///   Namespace:      igat.com
///   Class:          Scraper2
///   Description:    A class that is scraping the html pages and parsing the comments from Eurogamer website that are available for each game, these games
///                     comments will be saved in database by calling Database worker class method that is maintaning databse functions.
///   Author:         Ahmad Waseem                    Date: 10/4/2016
///   Notes:          version 5.2
///   Revision History 
///   Version: 4.0           Date: 30/5/2016
///   Version: 3.0           Date: 24/5/2016
///   Version: 2.0           Date: 18/4/2016 
///   Version: 1.0           Date: 10/4/2016        
///-----------------------------------------------------------------
using System;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

namespace igat.com
{
    class Scraper2
    {
        Dictionary<string, string> slangDict = new Dictionary<string, string>();
        Dictionary<int, string> linksDict = new Dictionary<int, string>();
        List<string> abusesList = new List<string>();
        string website = "eurogamer";
        DatabaseWorker DBObj = new DatabaseWorker();
        public Dictionary<string, string> slangDictionary
        {
            get { return slangDict; }
            set { slangDict = value; }
        }
        int gameId = 0;
        Filters filterInst = new Filters();
        /// <summary>
        /// Getting Comments and saving the games into database
        /// </summary>
        public void getCommentsUrl()
        {
            string completeUrl = null;
            WebClient client = new WebClient();
            //Oroginal URL Example www.eurogamer.net/ajax.php?action=json-comments&aid=1826441&start=0&limit=1000&filter=all&order=asc
            string firstpartEg = "http://www.eurogamer.net/ajax.php?action=json-comments&aid=";
            string secondpartEg = "&start=0&limit=1000&filter=all&order=asc";
            string data = null;
            string gameDescription = "-";
            string developer = "-";
            string platform = "-";
            string releaseDate = "-";
            string genre = null;
            List<string> genreList = new List<string>();
            genreList = getGenreList();

            slangDictionary = DBObj.RetrieveSlangs();
            abusesList = filterInst.abuseFilter();
            linksDict = DBObj.RetrieveLinks(website);
            
            foreach (var value in linksDict)
            {
                if (value.Value.Contains("eurogamer"))
                {
                    try
                    {
                        data = client.DownloadString(value.Value);
                       
                        HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(data);
                        genre = "";
                        var node = doc.DocumentNode.SelectSingleNode("//h2"); 
                        var gameName = node.InnerText;
                        gameName = gameName.Trim();
                        Match match = Regex.Match(data, "'aid': (?<value>[0-9]*)");
                        match = Regex.Match(match.Value, "[\\d]+");
                        completeUrl = firstpartEg + match.Value + secondpartEg;
                        node = doc.DocumentNode.SelectSingleNode("//meta[@content and @property='og:image']");

                        var imgUrl = node.Attributes["content"].Value;
                        var imageAddress = imgUrl;
                        string imgLocalAdress = @"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\GameImages\" + gameName + ".jpeg";
                        client.DownloadFile(imageAddress, imgLocalAdress);
                        string imgDBAddress = @"Resources\GameImages\" + gameName + ".jpeg";

                        node = doc.DocumentNode.SelectSingleNode("//p[@itemprop='summary']");
                        gameDescription = node.InnerText;
                        gameDescription = gameDescription.Trim();
                        node = doc.DocumentNode.SelectSingleNode("//li[@class='has-packshot']").SelectSingleNode(".//p");
                        platform = node.InnerText.Trim();
                        int c = 0;
                        foreach (string a in genreList)
                        {
                            if (data.Contains(a))
                            {
                                if (c < 1)
                                {
                                    genre = a;                                    
                                }
                                else
                                {
                                    genre += "/";
                                    genre = genre + a;                                    
                                }
                                c++;
                            }
                            
                        }

                        //DBObj.InsertGames(gameName, value.Key);
                        DBObj.InsertGamesDetail(gameName,gameDescription,platform,developer,releaseDate, imgDBAddress, genre, value.Key);
                        gameId = DBObj.getGameId(gameName);
                        getComments(completeUrl);
                    }
                    catch
                    {}
                }
            }
        }

        void getComments(string compltUrl)
        {
            WebClient c = new WebClient();
            c.Encoding = System.Text.Encoding.UTF8;
            string json = null;
            json = c.DownloadString(compltUrl);
            var arr = JArray.Parse(@json);
            foreach (JObject parsedObject in arr.Children<JObject>())
            {
                foreach (JProperty parsedProperty in parsedObject.Properties())
                {
                    string propertyName = parsedProperty.Name;
                    if (propertyName.Equals("p"))
                    {
                        var propertyValue = (string)parsedProperty.Value;
                        propertyValue = Regex.Replace(propertyValue, @"<[^>]*>|@[-_a-zA-Z0-9]*|([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+)|<[^>]*>|(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*|_|&|'|,|\\|/\|(|)|{|}|[|]|\-|\+|\=|\:|\;|\!|\@|\$|\%|\*|<|> ", "");
                        propertyValue = WebUtility.HtmlDecode(propertyValue);
                        propertyValue = propertyValue.Trim();
                        foreach (var entry in slangDictionary)
                        {
                            if (propertyValue.Contains(entry.Key))
                            {
                                propertyValue = propertyValue.Replace(entry.Key, entry.Value);
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

        public List<string> getGenreList()
        {
            string line;
            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader(@"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\GenreList.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            return list;
        }
    }
}
