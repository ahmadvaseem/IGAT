using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using igat.com.FunctionClasses;

namespace igat.com
{
    public partial class Admin2 : System.Web.UI.Page
    {
        WebCrawler1 webCrawl1Inst = new WebCrawler1();
        WebCrawler2 webCrawl2Inst = new WebCrawler2();
        Scraper1 scarper1Inst = new Scraper1();
        Scraper2 scarper2Inst = new Scraper2();
        SlangCrawler slangObj = new SlangCrawler();
        DatabaseWorker db = new DatabaseWorker();
        RemovingFilters filterObj = new RemovingFilters();
        WordsSetMaker setObj = new WordsSetMaker();
        //TestSentiment TsentObj = new TestSentiment();
        //TestAspectsSentiment aspects = new TestAspectsSentiment();
        Lexicon lex = new Lexicon();
        LexiconAspects asp = new LexiconAspects();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //db.CreateWebsite();
            //slangObj.getGamingSlangs();
            //slangObj.getSlangs();
            //ThreadStart childref = new ThreadStart(webCrawl1Inst.getLinks);
            //Thread childThread = new Thread(childref);
            //childThread.Start();
            //webCrawl2Inst.getLinks();

            //ThreadStart childref = new ThreadStart(scarper1Inst.getCommentsUrl);
            //Thread childThread = new Thread(childref);
            //childThread.Start();
            scarper1Inst.getCommentsUrl();
            // scarper2Inst.getCommentsUrl();

            //removing stopwords
            //filterObj.FilterData();
            //adding positive and negative words in DB
            //setObj.wordsList();
            //lex.CallSentiment();
            //TsentObj.callSentiment();
            //aspects.callSentiment();
            //asp.callSentiment(2026);
            dataAdded.Visible = true;
        }
    }
}