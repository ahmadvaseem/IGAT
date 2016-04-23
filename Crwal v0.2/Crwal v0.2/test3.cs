using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
namespace Crwal_v0._2
{
    class test3
    {
        static void Main(string[] args)
        {
            
            string l = "http://www.eurogamer.net/ajax.php?action=json-comments&aid=1822104&start=0&limit=1000&filter=all&order=asc";
            string w = "http://www.eurogamer.net/articles/2016-04-06-hardcore-henry-review";
            WebClient c = new WebClient();
            string p =  c.DownloadString(w);
            Regex r = new Regex("'aid': [0-9]{2-8}");
            MatchCollection matches = Regex.Matches(p, @"((href)://)+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,15})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?");
            for (int i = 0; i < matches.Count; i++)
            {
                Match mat = matches[i];
                Console.WriteLine(mat.Value);
            }
                
            Console.ReadLine();
            
        }
    }
}
