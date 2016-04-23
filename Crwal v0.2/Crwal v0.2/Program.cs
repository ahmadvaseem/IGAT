using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Crwal_v0._2
{
    class Program
    {
        static WebCrawler crawlerObj = new WebCrawler();
        static WebCrawler2 crawler2 = new WebCrawler2();
            static void Main0(string[] args)
        {
            //crawlerObj.getPages();
            crawler2.getPages();
            Console.ReadLine();     
        }
    }
}
