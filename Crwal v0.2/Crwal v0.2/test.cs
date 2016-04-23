using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using HtmlAgilityPack;
namespace Crwal_v0._2
{
    class test
    {
        static void Main0(string[] args)
        {
            int a = 0;
            string url = "http://www.pcgamer.com/dark-souls-3-review/";
            string page = "http://disqus.com/embed/comments/?base=default&version=31ddd45cf078ea8508c60e43c9c9fce8&f=pcgamerfte&t_u=";

            WebClient client = new WebClient();
            client.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
            client.Encoding = Encoding.UTF8;

            var html = client.DownloadString(new Uri("http://www.eurogamer.net/?type=review"));
            // client.DownloadFile ("http://www.eurogamer.net",@"1.txt");

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(page+url);
            
            var bod = doc.DocumentNode.SelectSingleNode("//body");
            //var layout = bod.SelectSingleNode("//div[@id='layout']");
            //var sec = layout.SelectSingleNode("//section[@id='conversation']");
           /* var posts = sec.SelectSingleNode("//div[@id='posts']");
            var postList = posts.SelectSingleNode("//ul[@id='post-list']");
            var post = postList.SelectSingleNode("//li[@class='post']");
            var postContent = post.SelectSingleNode("//div[@class='post-content']");
            var postBody = postContent.SelectSingleNode("//div[@class='post-body']");
            var postBodyInner = postBody.SelectSingleNode("//div[@class='post-body-inner']");
            var postMessageCon = postBodyInner.SelectSingleNode("//div[@class='post-message-container']");
            var publisherAnchorColor = postMessageCon.SelectSingleNode("//div[@class='publisher-anchor-color']");
            var postMessage = publisherAnchorColor.SelectSingleNode("//div[@class='post-message']");
            var pTag = postMessage.SelectSingleNode("//p");*/
            //HtmlNode nod = doc.DocumentNode.SelectSingleNode("//div[@class='post-message']");
           // foreach (HtmlNode nod in doc.DocumentNode.SelectNodes("//p"))
            {
                //Console.WriteLine(a++ +":"+bod.InnerHtml);
                Console.WriteLine(html);
            }

            Console.WriteLine("End");
            Console.ReadLine();
         }
        
    }
}
