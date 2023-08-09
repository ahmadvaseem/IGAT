/*using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace igat.com.FunctionClasses
{
    public class TestAspectsSentiment
    {

        Dictionary<string, string> trainWordPos = new Dictionary<string, string>();
        Dictionary<string, string> trainWordNeg = new Dictionary<string, string>();
        DatabaseWorker dbObj = new DatabaseWorker();
        List<string> posWords = new List<string>();
        List<string> negWords = new List<string>();
        List<KeyValuePair<string, int>> sentimentData = new List<KeyValuePair<string, int>>();
        List<string> negationWords = new List<string>();
        List<string> graphicsAspects = new List<string>();
        List<string> gameplayAspects = new List<string>();
        List<string> performanceAspects = new List<string>();
        List<string> splitter = new List<string>();
        double positiveListProb, negativeListProb;
        double positiveProbability = 1, negativeProbability = 1;
        int graphicsPCount = 0, gameplayPCount = 0, performancePCount = 0;
        int graphicsNCount = 0, gameplayNCount = 0, performanceNCount = 0;
        string locations;

        TestSentiment testSentiment = new TestSentiment();
        public void callSentiment(int gameID)
        {
            CalcSentiment(gameID);
        }
        void CalcSentiment(int gameID)
        {
            sentimentData = dbObj.RetrieveCleanComments(gameID);

            //sentimentData.Add("not absurd game ever");
            //sentimentData.Add("game not bad");
            //sentimentData.Add("playful game. pleasant . robust performance");
            // sentimentData.Add("swank results. tact move. vital life. woo game.");

            //sentimentData.Add("absurd game");
            /*sentimentData.Add("abort gameplay");
            sentimentData.Add("imperfect gameplay. indecent graphics. mangle moves");
            sentimentData.Add("insufficient memories, indecent gameplay, misread conceptions");

            sentimentData.Add("awesome gameplay. bad graphics");
            sentimentData.Add("worst gameplay. nice graphics");
            sentimentData.Add("awesome gameplay. bad graphics, excallent performance");
            sentimentData.Add("worst gameplay. nice graphics, indiscreet performance");*/

         /*   posWords = dbObj.positiveRetrieve();
            negWords = dbObj.negativeRetrieve();

            locations = @"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\negations.txt";
            negationWords = testSentiment.negationWordsFilter();//getFileData(locations);
            locations = @"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\graphics.txt";
            graphicsAspects = getFileData(locations);
            locations = @"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\gameplay.txt";
            gameplayAspects = getFileData(locations);
            locations = @"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\performance.txt";
            performanceAspects = getFileData(locations);

            splitter = getSplitter();


            foreach (var word in posWords)
            {
                string a = word.Trim();
                try
                {
                    trainWordPos.Add(a, "Positive");
                }
                catch { }
            }
            foreach (var word in negWords)
            {
                try
                {
                    string b = word.Trim();
                    trainWordNeg.Add(b, "Negative");
                }
                catch { }
            }

            positiveListProb = (double)trainWordPos.Count / (trainWordNeg.Count + trainWordPos.Count);
            negativeListProb = (double)trainWordNeg.Count / (trainWordNeg.Count + trainWordPos.Count);



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
                    positiveProbability = PositiveProbability(splittedData);
                    negativeProbability = NegativeProbability(splittedData);

                    if (graphics == true)
                    {
                        if (negativeProbability > positiveProbability)
                        {
                            //Debug.WriteLine("Graphics Negative: " + negativeProbability);
                            graphicsNCount += 1;
                        }
                        else if (negativeProbability < positiveProbability)
                        {
                            //Debug.WriteLine("Graphics Positive: " + positiveProbability);
                            graphicsPCount += 1;
                        }
                        else 
                        graphics = false;
                    }
                    //else Debug.WriteLine("Graphics not found");
                    if (gameplay == true)
                    {
                        if (negativeProbability > positiveProbability)
                        {
                            //Debug.WriteLine("Gameplay Negative: " + negativeProbability);
                            gameplayNCount += 1;
                        }
                        else if (negativeProbability < positiveProbability)
                        {
                            //Debug.WriteLine("Gameplay Positive: " + positiveProbability);
                            gameplayPCount += 1;
                        }
                        gameplay = false;
                    }
                    //else Debug.WriteLine("Gameplay not found");
                    if (performance == true)
                    {
                        if (negativeProbability > positiveProbability)
                        {
                            //Debug.WriteLine("Performance Negative: " + negativeProbability);
                            performanceNCount += 1;
                        }
                        else if(negativeProbability < positiveProbability)
                        {
                            //Debug.WriteLine("Performance Positive: " + positiveProbability);
                            performancePCount += 1;
                        }
                        performance = false;
                    }
                    //else Debug.WriteLine("Performance not found");
                }
                //Debug.WriteLine("Next: \n");
                if (sentimentData[index].Value != sentimentData[index + 1].Value)
                {
                    Debug.WriteLine(dbObj.GetGame(oneData.Value));
                    if (graphicsNCount + graphicsPCount > 5)
                        Debug.WriteLine("Graphics Rating: " + getRating(graphicsPCount, graphicsNCount));
                    else
                        Debug.WriteLine("Graphics Rating: Not enough reviews for Rating");
                    if (gameplayPCount + gameplayNCount > 5)
                        Debug.WriteLine("Gameplay Rating: " + getRating(gameplayPCount, gameplayNCount));
                    else
                        Debug.WriteLine("Gameplay Rating: Not enough reviews for Rating");
                    if (performancePCount + performanceNCount > 5)
                        Debug.WriteLine("Performance Rating: " + getRating(performancePCount, performanceNCount));
                    else
                        Debug.WriteLine("Performance Rating: Not enough reviews for Rating");
                    dbObj.InsertFeatureRatings(getRating(graphicsPCount, graphicsNCount), getRating(gameplayPCount, gameplayNCount), getRating(performancePCount, performanceNCount), oneData.Value);
                    graphicsPCount = 0; graphicsNCount = 0; gameplayPCount = 0; gameplayNCount = 0; performancePCount = 0; performanceNCount = 0;
                }
            }
           /* double ratPG = getPercentage(graphicsPCount);
            double ratNG = getPercentage(graphicsNCount);
            double ratGmN = getPercentage(gameplayPCount);
            double ratGmP = getPercentage(gameplayPCount);
            double ratPN = getPercentage(performanceNCount);
            double ratPP = getPercentage(performancePCount);*/

       
    /*    }
        float getRating(double pos, double neg)
        {
            float rat = (float)(pos / (pos + neg)) * 10;
            return rat;
        }
        /*double getPercentage(int count)
        {
            double x;
            x = ((double)count / sentimentData.Count) * 10;
            return x;
        }*/

     /*  double PositiveProbability(string[] test1)
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
                foreach (var c in trainWordPos.Keys)
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
                probAWordPos = wordProbablity(foundP, trainWordPos.Count, trainWordNeg.Count, trainWordPos.Count);//(double)(foundP + 1) / (trainWordPos.Count + (trainWordNeg.Count + trainWordPos.Count));
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

        double NegativeProbability(string[] test1)
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
                foreach (var c in trainWordNeg.Keys)
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

                probAWordNeg = wordProbablity(foundN, trainWordNeg.Count, trainWordNeg.Count, trainWordPos.Count);//(double)(foundN + 1) / (trainWordNeg.Count + (trainWordNeg.Count + trainWordPos.Count));
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

        double wordProbablity(int found, int count, int countN, int countP)
        {
            return (double)(found + 1) / (count + (countN + countP));
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
    }
}*/