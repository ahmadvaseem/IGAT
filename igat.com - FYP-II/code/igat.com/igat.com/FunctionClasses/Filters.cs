///-----------------------------------------------------------------
///   Namespace:      igat.com
///   Class:          Filters
///   Description:    Class that is taking abusive words from file and saving it into a List so that can be send to scraper classes
///                   so, it can filter the comments.  
///   Author:         Ahmad Waseem                    Date: 3/6/2016
///   Notes:          version 2.0
///   Revision History 
///   Version: 1.0           Date: 3/6/2016        
///-----------------------------------------------------------------
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace igat.com
{
    public class Filters
    {
        
        List<string> abusesList = new List<string>();

        public List<string> abuseFilter()
        {
            string line;
            using (StreamReader reader = new StreamReader(@"C:\Users\Ahmad Vaceem\Documents\Visual Studio 2015\Projects\igat.com\igat.com\Resources\abusiveWords.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    abusesList.Add(line);
                }
            }
            return abusesList;
        }
        
    }
}