using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using igat.com.FunctionClasses.External;
using Iveonik.Stemmers;

namespace igat.com.FunctionClasses
{
    public class RemovingFilters
    {
        List<string> stopWordsList = new List<string>();
        string pattern = @"\s+(?:what|why|when|how|where|who|which|whose|does|do|was|are|am|were|can|should|is|how|whom)[^\?\.]*(?:\?|\.)";
        List<KeyValuePair<string, int>> comments = new List<KeyValuePair<string, int>>();
        DatabaseWorker DBObj = new DatabaseWorker();
        //Stemmer s = new Stemmer();
        public void FilterData(int gameID)
        {

            string propertyValue;
            int gameId = 0;
            stopWordsList = stopWordsFilter();
            comments = DBObj.RetrieveCommentsToClean(gameID);
            foreach (var comment in comments)
            {
                //Removing Interrogative sentences
                propertyValue = comment.Key;
                propertyValue = propertyValue.Trim();
                propertyValue = Regex.Replace(propertyValue, pattern, "", RegexOptions.IgnoreCase);
                //Removing Stopwords
                var words = propertyValue.Split();
                var newWords = words.Except(stopWordsList, StringComparer.InvariantCultureIgnoreCase);
                propertyValue = string.Join(" ", newWords);
                string temp = null;
                //temp = propertyValue;

                /*foreach (string word in propertyValue.Split(' '))
                 {
                     temp = obj.Stem(word);
                     temp1 = temp1 + " " + temp;
                 }
                 propertyValue = temp1;*/

                 //Stemming the sentences
                foreach (string word in propertyValue.Split(' '))
                {
                    var a = word.ToCharArray();                    
                    for (int i = 0; i < a.Length; i++)
                    {
                        //s.add(a[i]);
                    }
                    //s.stem();
                    //temp = temp + " " + s.ToString();
                }
                propertyValue = temp;
                gameId = comment.Value;
                DBObj.InsertCleanComments(propertyValue, gameId);
            }
            
        }
        List<string> stopWordsFilter()
        {
            string line;
            List<string> stopWords = new List<string>();
            using (StreamReader reader = new StreamReader(@"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\stopwords.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    stopWords.Add(line);
                }
            }
            return stopWords;
        }
    }
}