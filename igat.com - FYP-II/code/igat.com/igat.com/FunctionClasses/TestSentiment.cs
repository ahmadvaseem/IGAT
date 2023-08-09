/*using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace igat.com.FunctionClasses
{
    public class TestSentiment
    {

        List<KeyValuePair<string, int>> sentimentData = new List<KeyValuePair<string, int>>();
        Dictionary<string, string> listWordPos = new Dictionary<string, string>();
        Dictionary<string, string> listWordNeg = new Dictionary<string, string>();
        DatabaseWorker dbObj = new DatabaseWorker();
        List<string> posWords = new List<string>();
        List<string> negWords = new List<string>();
        List<string> negationWords = new List<string>();
        double positiveListProb, negativeListProb;
        //List<string> sentimentData = new List<string>();
        double positiveScore = 1, negativeScore = 1;
        int positiveCount = 0, negativeCount = 0;
        public void callSentiment(int gameID)
        {
            Sentiment(gameID);
        }
        void Sentiment(int gameID)
        {
            sentimentData = dbObj.RetrieveCleanComments(gameID);

            //sentimentData.Add("not absurd game ever");
            //sentimentData.Add("game not bad");
            //sentimentData.Add("playful game. pleasant graphics. robust performance");
            //sentimentData.Add("swank results. tact move. vital life. woo game.");

            //sentimentData.Add("absurd game");
            //sentimentData.Add("abort gameplay");
           // sentimentData.Add("disapprove version");
            //sentimentData.Add("imperfect gameplay. indecent graphics. mangle moves");
            //sentimentData.Add("insufficient memories, indecent gameplay, misread conceptions");

           // sentimentData.Add("awesome gameplay. bad graphics");
            //sentimentData.Add("worst gameplay. nice graphics");
            //sentimentData.Add("awesome gameplay. bad graphics, excallent performance");
             //sentimentData.Add("worst gameplay. nice graphics, indiscreet performance");


            posWords = dbObj.positiveRetrieve();
            negWords = dbObj.negativeRetrieve();
            negationWords = negationWordsFilter();
            // var words = posWords.Zip(negWords, (p, n) => new { pos = p, neg = n });
            foreach (var word in posWords)
            {
                string a = word.Trim();
                try
                {
                    listWordPos.Add(a, "Positive");
                }
                catch { }
            }
            foreach (var word in negWords)
            {
                string b = word.Trim();
                try
                {
                    listWordNeg.Add(b, "Negative");
                }
                catch { }
                
            }
            
            positiveListProb = (double)listWordPos.Count / (listWordNeg.Count + listWordPos.Count);
            negativeListProb = (double)listWordNeg.Count / (listWordNeg.Count + listWordPos.Count);

            foreach (var oneData in sentimentData)
            {
                int index = sentimentData.IndexOf(oneData);
                /*if (negationWords.Any(w => test.Contains(w)))
                {
                    negCheck = true;
                    Regex reg = new Regex("not\\s\\w+");
                    MatchCollection matches = reg.Matches(test);
                    foreach (Match match in matches)
                    {
                        string sub = match.Value;
                        sub = sub.Replace("not", "");
                        listWordPos.Keys.Any(y => sub.Contains(y));
                    }
                }*/
            /*    string oneReview = oneData.Key;
                var splittedData = oneReview.Split(' ');
                positiveScore = PositiveScore(splittedData);
                negativeScore =  NegativeScore(splittedData);

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
                //Debug.WriteLine(positiveScore);
                //Debug.WriteLine(negativeScore);
                if (sentimentData[index].Value != sentimentData[index+1].Value)
                {                    
                    Debug.WriteLine(dbObj.GetGame(oneData.Value) );
                    Debug.WriteLine(" Rating: " + getRating(positiveCount, negativeCount));
                    dbObj.InsertRatings(getRating(positiveCount, negativeCount), oneData.Value);
                    positiveCount = 0;negativeCount = 0;
                }
            }
            
        }
       public float getRating(double pos, double neg)
        {
            float rat = (float)(pos / (pos + neg )) * 10;
            return rat;
        }
       public double PositiveScore(string[] test1)
        {
            int foundP = 0;
            double probAWordPos = 1;
            double probP = 1;
            int count = 1;
            bool negationCheck = false;
            bool posCheck = false;
            foreach (string t in test1)
            {
                foreach (var neg in negationWords)
                {
                    if ((Regex.Match(t, "^" + neg + "$").Success))
                        negationCheck = true;
                }

                foreach (var c in listWordPos.Keys)
                {
                    count += Regex.Matches(t, "^" + c + "$").Count;
                    if (Regex.Match(t, "^" + c + "$").Success)
                    {
                        if (negationCheck == true)
                        {
                            posCheck = true;
                            foundP = 0;
                            negationCheck = false;
                        }
                        else foundP = 1;
                        //count++;
                        break;
                    }
                    else foundP = 0;

                }

                //if ((Regex.Match(t, "(?::|;|=)(?:-)?(?:\\)|[bBdDeEpPoOxX*>@#//)]").Success))
                  //  foundP = 1;
                    probAWordPos = wordProbablity(foundP, listWordPos.Count, listWordNeg.Count, listWordPos.Count);//(double)(foundP + 1) / (listWordPos.Count + (listWordNeg.Count + listWordPos.Count));
                if (posCheck)
                {
                    probAWordPos -= 1;
                    posCheck = false;
                }
                //if (count > 1) count -= 1;
                probP += (double) probAWordPos + count;
                //Console.WriteLine(probP);
            }
            return probP;
        }
       public double NegativeScore(string[] test1)
        {
            int foundN = 0;
            double probAWordNeg = 1;
            double probN = 1;
            int count = 1;
            bool negationCheck = false;
            bool negCheck = false;
            foreach (string t in test1)
            {
                count = 1;
                foreach (var neg in negationWords)
                {
                    if ((Regex.Match(t, "^" + neg + "$").Success))
                        negationCheck = true;
                }
                foreach (var c in listWordNeg.Keys)
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
                    }
                    else foundN = 0;
                }
                if ((Regex.Match(t, "(?::|;|=|o)(?:-)?(?:\\|[eEoO<\\(])").Success))
                    foundN = 1;
                probAWordNeg = wordProbablity(foundN, listWordNeg.Count, listWordNeg.Count, listWordPos.Count);//(double)(foundN + 1) / (listWordNeg.Count + (listWordNeg.Count + listWordPos.Count));
                if (negCheck)
                {
                    probAWordNeg -= 1;
                    negCheck = false;
                }
                //if (count > 1) count -= 1;
                probAWordNeg *= -1;
                probN += (double) probAWordNeg + count;
                //Console.WriteLine(probN);


            }
            negationCheck = false;
            return probN;
        }
        double wordProbablity(int found, int count,int countN, int countP)
        {
            return (double)(found) / (count + (countN + countP));
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
}*/