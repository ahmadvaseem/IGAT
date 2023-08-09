using igat.com.FunctionClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace igat.com
{
    public class LexiconAspects
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

        List<string> graphicsAspects = new List<string>();
        List<string> gameplayAspects = new List<string>();
        List<string> performanceAspects = new List<string>();
        List<string> splitter = new List<string>();
        double positiveListProb, negativeListProb;
        int graphicsPCount = 0, gameplayPCount = 0, performancePCount = 0;
        int graphicsNCount = 0, gameplayNCount = 0, performanceNCount = 0;
        string locations;

        //TestSentiment testSentiment = new TestSentiment();
        public void callSentiment(int gameID)
        {
            CalcSentiment(gameID);
        }
        void CalcSentiment(int gameID)
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
                string word = Regex.Replace(values[2], "#[a-zA-Z0-9]", "");
                listPos.Add(posScore);
                listNeg.Add(negScore);
                listWords.Add(word);
            }
            sentimentData = dbObj.RetrieveCleanComments(gameID);
            locations = @"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\negations.txt";
            //negationWords = testSentiment.negationWordsFilter();//getFileData(locations);
            locations = @"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\graphics.txt";
            graphicsAspects = getFileData(locations);
            locations = @"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\gameplay.txt";
            gameplayAspects = getFileData(locations);
            locations = @"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\performance.txt";
            performanceAspects = getFileData(locations);

            splitter = getSplitter();

            foreach (var oneData in sentimentData)
            {
                int index = sentimentData.IndexOf(oneData);
                string oneReview = oneData.Key;
                bool graphics = false, gameplay = false, performance = false;
                var splitted = oneReview.Split(new string[] { ", ", ". ", " and ", " but ", "&", ";", " or ", " although ", " hence " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var splitt in splitted)
                {
                    var splittedData = splitt.Split(' ');
                    ///////
                    foreach (string data in splittedData)
                    {
                        var mlist = graphicsAspects.Where(match => match.Contains(data));
                        if (mlist.Count() > 0) { graphics = true; }
                        mlist = gameplayAspects.Where(match => match.Contains(data));
                        if (mlist.Count() > 0)
                        { gameplay = true; }
                        mlist = performanceAspects.Where(match => match.Contains(data));
                        if (mlist.Count() > 0)
                        { performance = true; }
                    }

                    ///////
                    positiveScore = PositiveScore(splittedData);
                    negativeScore = NegativeScore(splittedData);

                    if (graphics == true)
                    {
                        if (negativeScore > positiveScore)
                        {
                            //Debug.WriteLine("Graphics Negative: " + negativeScore);
                            graphicsNCount += 1;
                        }
                        else if (negativeScore < positiveScore)
                        {
                            //Debug.WriteLine("Graphics Positive: " + positiveScore);
                            graphicsPCount += 1;
                        }
                        else
                            graphics = false;
                    }
                    //else Debug.WriteLine("Graphics not found");
                    if (gameplay == true)
                    {
                        if (negativeScore > positiveScore)
                        {
                            //Debug.WriteLine("Gameplay Negative: " + negativeScore);
                            gameplayNCount += 1;
                        }
                        else if (negativeScore < positiveScore)
                        {
                            //Debug.WriteLine("Gameplay Positive: " + positiveScore);
                            gameplayPCount += 1;
                        }
                        gameplay = false;
                    }
                    //else Debug.WriteLine("Gameplay not found");
                    if (performance == true)
                    {
                        if (negativeScore > positiveScore)
                        {
                            //Debug.WriteLine("Performance Negative: " + negativeScore);
                            performanceNCount += 1;
                        }
                        else if (negativeScore < positiveScore)
                        {
                            //Debug.WriteLine("Performance Positive: " + positiveScore);
                            performancePCount += 1;
                        }
                        performance = false;
                    }
                    //else Debug.WriteLine("Performance not found");
                }
                //Debug.WriteLine("Next: \n");
                try
                {
                    if (sentimentData[index].Value != sentimentData[index + 1].Value)
                    {
                        Debug.WriteLine(dbObj.GetGame(oneData.Value));
                        if (graphicsNCount + graphicsPCount > 10)
                            Debug.WriteLine("Graphics Rating: " + getRating(graphicsPCount, graphicsNCount));
                        else
                            Debug.WriteLine("Graphics Rating: Not enough reviews for Rating");
                        if (gameplayPCount + gameplayNCount > 10)
                            Debug.WriteLine("Gameplay Rating: " + getRating(gameplayPCount, gameplayNCount));
                        else
                            Debug.WriteLine("Gameplay Rating: Not enough reviews for Rating");
                        if (performancePCount + performanceNCount > 10)
                            Debug.WriteLine("Performance Rating: " + getRating(performancePCount, performanceNCount));
                        else
                            Debug.WriteLine("Performance Rating: Not enough reviews for Rating");
                        dbObj.InsertFeatureRatings(getRating(graphicsPCount, graphicsNCount), getRating(gameplayPCount, gameplayNCount), getRating(performancePCount, performanceNCount), oneData.Value);
                        graphicsPCount = 0; graphicsNCount = 0; gameplayPCount = 0; gameplayNCount = 0; performancePCount = 0; performanceNCount = 0;
                    }
                    
                }
                catch
                {
                    Debug.WriteLine(dbObj.GetGame(oneData.Value));
                    if (graphicsNCount + graphicsPCount > 10)
                        Debug.WriteLine("Graphics Rating: " + getRating(graphicsPCount, graphicsNCount));
                    else
                        Debug.WriteLine("Graphics Rating: Not enough reviews for Rating");
                    if (gameplayPCount + gameplayNCount > 10)
                        Debug.WriteLine("Gameplay Rating: " + getRating(gameplayPCount, gameplayNCount));
                    else
                        Debug.WriteLine("Gameplay Rating: Not enough reviews for Rating");
                    if (performancePCount + performanceNCount > 10)
                        Debug.WriteLine("Performance Rating: " + getRating(performancePCount, performanceNCount));
                    else
                        Debug.WriteLine("Performance Rating: Not enough reviews for Rating");
                    dbObj.InsertFeatureRatings(getRating(graphicsPCount, graphicsNCount), getRating(gameplayPCount, gameplayNCount), getRating(performancePCount, performanceNCount), oneData.Value);
                    graphicsPCount = 0; graphicsNCount = 0; gameplayPCount = 0; gameplayNCount = 0; performancePCount = 0; performanceNCount = 0;
                }
            }
        }
        float getRating(double pos, double neg)
        {
            float rat = (float)(pos / (pos + neg)) * 10;
            rat = (float)Math.Round(rat, 1);
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
            int index; double score = 0, wordScore = 0;
            foreach (string t in test1)
            {

                foreach (var neg in negationWords)
                {
                    if ((Regex.Match(t, "^" + neg + "$").Success))
                        negationCheck = true;
                }

                foreach (var c in listWords)
                {

                    if (Regex.Match(t, "^" + c + "$").Success)
                    {
                        if (negationCheck == true)
                        {
                            posCheck = true;
                            foundP = 0;
                            negationCheck = false;
                        }
                        else foundP = 1;
                        index = listWords.IndexOf(c);
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
        public List<string> getFileData(string fileLocation)
        {
            string line;
            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader(fileLocation))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            return list;
        }

        public List<string> getSplitter()
        {
            string line;
            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader(@"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\separators.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            return list;
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