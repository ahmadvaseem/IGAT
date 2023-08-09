using System.Net;
using HtmlAgilityPack;

namespace igat.com
{
    public class SlangCrawler
    {
        WebClient client = new WebClient();
        int count = 1;
        char _char = 'a';
        string data;
        string website1 = "http://www.abbreviations.com/acronyms/GAMING/";
        DatabaseWorker DBObj = new DatabaseWorker();
        public void getGamingSlangs()
        {
            do
            {
                data = client.DownloadString(website1 + count);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(data);
                var nodeSlang = doc.DocumentNode.SelectNodes("//td[@class='tal tm']");
                var nodeText = doc.DocumentNode.SelectNodes("//td[@class='tal dm']");

                try
                {
                    for (int i = 0; i < nodeSlang.Count; i++)
                    {
                        var slangWord = WebUtility.HtmlDecode(nodeSlang[i].InnerText);
                        var slangAbbr = WebUtility.HtmlDecode(nodeText[i].InnerText);
                        slangWord = slangWord.Trim();
                        slangAbbr = slangAbbr.Trim();
                        DBObj.InsertSlangs(slangWord, slangAbbr);
                    }
                }
                catch
                {
                    continue;
                }

                count++;
            }
            while (data.Contains("tal tm"));
        }

        public void getSlangs()
        {
            do
            {
                GSlang(_char);
                _char++;
            }
            while (_char != 'z' + 1);
            _char = '1';
            GSlang(_char);
        }
        void GSlang(char ch)
        {
            string data = client.DownloadString("http://www.noslang.com/dictionary/" + ch);
            HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(data);
            var nodeSlang = doc.DocumentNode.SelectNodes("//strong");
            var nodeText = doc.DocumentNode.SelectNodes("//dd");
            try
            {
                for (int i = 0; i < nodeSlang.Count; i++)
                {
                    var slangWord = WebUtility.HtmlDecode(nodeSlang[i].InnerText);
                    var slangAbbr = WebUtility.HtmlDecode(nodeText[i].InnerText);
                    DBObj.InsertSlangs(slangWord, slangAbbr);

                }
            }
            catch { }
        }
    }
}