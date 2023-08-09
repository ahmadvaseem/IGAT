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
using System.IO;
using igat.com.FunctionClasses;

namespace igat.com
{
    class Scraper1
    {
        Dictionary<string, string> slangDict = new Dictionary<string, string>();
        Dictionary<int, string> linksDict = new Dictionary<int, string>();
        List<string> abusesList = new List<string>();
        RemovingFilters filterObj = new RemovingFilters();
        Lexicon lex = new Lexicon();
        LexiconAspects asp = new LexiconAspects();
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
            string gameDesription = "-";
            string platform = "-";
            string developer = "-";
            string releaseDate = "-";
            string genre = "-";
            List<string> genreList = new List<string>();
            genreList = getGenreList();
            slangDictionary = DBObj.RetrieveSlangs();
            abusesList = filterInst.abuseFilter();

            linksDict = DBObj.RetrieveLinks(website);
            
            foreach (var value in linksDict)
            {
                if (value.Value.Contains(website))
                {
                    
                    HtmlDocument doc = new HtmlDocument();
                    try
                    {
                        data = client.DownloadString(value.Value);
                        doc.LoadHtml(data);
                        var node = doc.DocumentNode.SelectSingleNode("//h1");
                        var gameName = node.InnerText;
                        gameName = Regex.Replace(gameName, " review", "");
                        gameName = Regex.Replace(gameName, @"\:|\/|\\|\?|<|>", "-");
                        gameName = gameName.Trim();
                        var urlId = node.Attributes["data-remote-admin-entry-id"].Value;
                        completeUrl = firstpartUrl + urlId;
                        node = doc.DocumentNode.SelectSingleNode("//meta[@content and @property='og:image']");
                        var imgUrl = node.Attributes["content"].Value;
                        var imageAddress = imgUrl;
                        string imgLocalAdress = @"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\GameImages\" + gameName + ".jpeg";
                        client.DownloadFile(imageAddress, imgLocalAdress);
                        string imgDBAddress = @"Resources\GameImages\" + gameName + ".jpeg";
                        imgDBAddress = imgDBAddress.Trim();
                        node = doc.DocumentNode.SelectSingleNode("//tr[@class='platform']");
                        platform = node.InnerText.Trim();
                        platform = platform.Replace("Platform", "");
                        platform = platform.Trim();
                        node = doc.DocumentNode.SelectSingleNode("//tr[@class='developer']");
                        developer = node.InnerText.Trim();
                        developer = developer.Replace("Developer", "");
                        developer = developer.Trim();
                        node = doc.DocumentNode.SelectSingleNode("//tr[@class='release']");
                        releaseDate = node.InnerText.Trim();
                        releaseDate = releaseDate.Replace("Release Date", "");
                        releaseDate = releaseDate.Trim();
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

                        DBObj.InsertGamesDetail(gameName,gameDesription,platform,developer,releaseDate, imgDBAddress, genre,value.Key);
                        gameId = DBObj.getGameId(gameName);
                        getComments(completeUrl);
                        filterObj.FilterData(gameId);
                        //lex.CallSentiment(gameId);
                        //asp.callSentiment(gameId);
                    }
                    catch
                    {                  
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
