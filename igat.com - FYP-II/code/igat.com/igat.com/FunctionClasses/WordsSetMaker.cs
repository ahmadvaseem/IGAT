
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace igat.com.FunctionClasses
{
    public class WordsSetMaker
    {
        DatabaseWorker dbObj = new DatabaseWorker();
        public void wordsList()
        {
            positiveWords();
            negativeWords();
        }
        List<string> positiveWords()
        {
            string line;
            List<string> pWords = new List<string>();
            using (StreamReader reader = new StreamReader(@"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\positive-words.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    dbObj.InsertPositiveSet(line);
                    pWords.Add(line);
                }
            }
            return pWords;
        }
        List<string> negativeWords()
        {
            string line;
            List<string> nWords = new List<string>();
            using (StreamReader reader = new StreamReader(@"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\negative-words.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    dbObj.InsertNegativeSet(line);
                    nWords.Add(line);
                }
            }
            return nWords;
        }
    }
}