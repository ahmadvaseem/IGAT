using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace igat.com
{
    public partial class AllLinks : System.Web.UI.Page
    {
        DatabaseWorker DBObj = new DatabaseWorker();
        Dictionary<int, string> dictLinks = new Dictionary<int, string>();
   
        protected void Page_Load(object sender, EventArgs e)
        {
            dictLinks = DBObj.RetrieveLinks("Eurogamer");
            CreateTable();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }

        public void CreateTable()
        {
            int count = 0;
            foreach (var link in dictLinks)
            {
                TableCell c = new TableCell();
                TableRow dr = new TableRow();
                c.Controls.Add(new LiteralControl(link.Value));
                dr.Cells.Add(c);
                Table1.Rows.Add(dr);
                if (count == 200)
                    break;
                count++;

            }

        }
    }
}