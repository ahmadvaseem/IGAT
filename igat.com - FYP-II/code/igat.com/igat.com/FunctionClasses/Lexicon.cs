using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace igat.com.FunctionClasses
{
    public class Lexicon
    {
        List<KeyValuePair<string, int>> sentimentData = new List<KeyValuePair<string, int>>();
        DatabaseWorker dbObj = new DatabaseWorker();
        List<string> negationWords = new List<string>();
        //List<string> sentimentData = new List<string>();
        double positiveScore = 1, negativeScore = 1;
        int positiveCount = 0, negativeCount = 0;
        List<float> listPos = new List<float>();
        List<float> listNeg = new List<float>();
        List<string> listWords = new List<string>();
        public void CallSentiment(int gameID)
        {     

            sentimentData = dbObj.RetrieveCleanComments(gameID);
            negationWords = negationWordsFilter();

            var reader = new StreamReader(File.OpenRead(@"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\List.csv"));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                float posScore = float.Parse(values[0], CultureInfo.InvariantCulture.NumberFormat);
                float negScore = float.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat);
                string word = Regex.Replace(values[2],"#[a-zA-Z0-9]","");
                listPos.Add(posScore);
                listNeg.Add(negScore);
                listWords.Add(word);
            }
            foreach (var oneData in sentimentData)
            {
                int index = sentimentData.IndexOf(oneData);
                string oneReview = oneData.Key;
                var splittedData = oneReview.Split(' ');

                positiveScore = PositiveScore(splittedData);
                negativeScore = NegativeScore(splittedData);
                if (negativeScore > positiveScore)
                {
                    //Debug.WriteLine("Negative Sentence");
                    negativeCount += 1;
                }
                else if (negativeScore < positiveScore)
                {
                    //Debug.WriteLine("Positive Sentence");
                    positiveCount += 1;
                }
                try
                {
                    if (sentimentData[index].Value != sentimentData[index + 1].Value)
                    {
                        Debug.WriteLine(dbObj.GetGame(oneData.Value));
                        Debug.WriteLine(" Rating: " + getRating(positiveCount, negativeCount));
                        dbObj.InsertRatings(getRating(positiveCount, negativeCount), oneData.Value);
                        positiveCount = 0; negativeCount = 0;
                    }
                }
                catch {
                    Debug.WriteLine(dbObj.GetGame(oneData.Value));
                    Debug.WriteLine(" Rating: " + getRating(positiveCount, negativeCount));
                    dbObj.InsertRatings(getRating(positiveCount, negativeCount), oneData.Value);
                    positiveCount = 0; negativeCount = 0;
                }
            }

        }
        public float getRating(double pos, double neg)
        {
            float rat = (float)(pos / (pos + neg)) * 10;
            rat = (float) Math.Round(rat,2);
            return rat;
        }
        public double PositiveScore(string[] test1)
        {
            int foundP = 0;
            double probAWordPos = 1;
            double probP = 1;
            int count = 1;
            string checkWord = "NOT_";
            bool negationCheck = false;
            bool posCheck = false;
            int index;double score = 0, wordScore = 0;
            foreach (string t in test1)
            {
                
                foreach (var neg in negationWords)
                {
                    
                    if ((Regex.Match(t, "^" + neg + "$").Success))
                        negationCheck = true;
                }

                foreach (var checkWords in listWords)
                {
                    
                    if (Regex.Match(t, "^" + checkWords + "$").Success)
                    {
                        if (negationCheck == true)
                        {
                            checkWord = checkWord + t;
                            posCheck = true;
                            foundP = 0;
                            negationCheck = false;
                        }
                        else foundP = 1;
                        index = listWords.IndexOf(checkWords);
                        wordScore = listPos[index];
                        //count++;
                        break;
                    }
                    else foundP = 0;

                }

                //if ((Regex.Match(t, "(?::|;|=)(?:-)?(?:\\)|[bBdDeEpPoOxX*>@#//)]").Success))
                //  foundP = 1;
                if (posCheck)
                {
                    wordScore *= -1;
                    posCheck = false;
                }
                //if (count > 1) count -= 1;
                score += wordScore;
                //Console.WriteLine(probP);
            }
            return score;
        }
        public double NegativeScore(string[] test1)
        {
            int foundN = 0;
            double probAWordNeg = 1;
            double probN = 1;
            int count = 1;
            bool negationCheck = false;
            bool negCheck = false;
            int index; double score = 0, wordScore = 0;
            foreach (string t in test1)
            {
                count = 1;
                foreach (var neg in negationWords)
                {
                    if ((Regex.Match(t, "^" + neg + "$").Success))
                        negationCheck = true;
                }
                foreach (var c in listWords)
                {

                    count += Regex.Matches(t, "^" + c + "$").Count;
                    if (Regex.Match(t, "^" + c + "$").Success)
                    {
                        if (negationCheck == true)
                        {
                            negCheck = true;
                            foundN = 0;
                            negationCheck = false;
                        }
                        else foundN = 1;
                        index = listWords.IndexOf(c);
                        wordScore = listNeg[index];
                        break;
                    }
                    else foundN = 0;
                    
                }
                if ((Regex.Match(t, "(?::|;|=|o)(?:-)?(?:\\|[eEoO<\\(])").Success))
                    foundN = 1;

                if (negCheck)
                {
                    wordScore *= -1;
                    negCheck = false;
                }
                //if (count > 1) count -= 1;


                score += wordScore;
                //Console.WriteLine(probN);


            }
            negationCheck = false;
            return score;
        }
        public List<string> negationWordsFilter()
        {
            string line;
            List<string> negationWords = new List<string>();
            using (StreamReader reader = new StreamReader(@"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\negations.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    negationWords.Add(line);
                }
            }
            return negationWords;
        }
    }
}