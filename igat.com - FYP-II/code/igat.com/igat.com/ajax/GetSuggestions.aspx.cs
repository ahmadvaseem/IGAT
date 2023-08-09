using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace igat.com.ajax
{
    public class Suggestion{
        public int ID;
        public String Name;
        public Suggestion(int id,String name)
        {
            ID = id;
            Name = name;
        }


    }
    public partial class GetSuggestions : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Suggest();
        }
        [WebMethod]
        public static List<Suggestion> Suggest(String query)
        {
            DatabaseWorker DB = new DatabaseWorker();
            List<Suggestion> list = new List<Suggestion>();
            list = DB.GetSuggestions(query);
            return list;
            /*foreach (var a in list)
            {
                Debug.WriteLine(a);
            }*/
        }
        
    }
}