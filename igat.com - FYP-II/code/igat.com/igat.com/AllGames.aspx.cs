using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace igat.com
{
    public partial class AllGames : System.Web.UI.Page
    {
        DatabaseWorker DBObj = new DatabaseWorker();
        //Dictionary<int, string> dictGames = new Dictionary<int, string>();
      
       /* protected void Page_Load(object sender, EventArgs e)
        {
            dictGames = DBObj.RetrieveGames();
            //CreateTable();
        }*/

        protected void Page_Load(object sender, EventArgs e)
        {
            // dictComments = DBObj.RetrieveComments();
            if (!Page.IsPostBack)
            {
                BindRepeater();

            }
        }

        protected void BindRepeater()
        {
            DatabaseWorker dw = new DatabaseWorker();
            DataSet ds = dw.getGameNamesAsDataSet();
            rptGames.DataSource = ds;
            rptGames.DataBind();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {

        }


        protected void rptGames_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                HiddenField hdnF = (HiddenField)e.Item.FindControl("hdnID");
                Response.Redirect("RatingPage.aspx?ID=" + hdnF.Value);

            }
        }
        protected void btnRating_Click(object sender, EventArgs e)
        {
            
        }
        /*public void CreateTable()
        {
            int count = 0;
            foreach (var link in dictGames)
            {
                TableCell c = new TableCell();
                TableRow dr = new TableRow();
                c.Controls.Add(new LiteralControl(link.Value));
                dr.Cells.Add(c);
                Table1.Rows.Add(dr);
                Button b = new Button();
                b.Text = "Rating ";
                b.ID = "btnKill_";
                b.Click += new EventHandler(btnRating_Click);
                TableCell tc = new TableCell();
                tc.Controls.Add(b);
                dr.Cells.Add(tc);
                if (count == 300)
                    break;
                count++;

            }

        }*/
    }
}