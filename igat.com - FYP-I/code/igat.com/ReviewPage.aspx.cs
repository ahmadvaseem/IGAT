using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace igat.com
{
    public partial class ReviewPage : System.Web.UI.Page
    {
        DatabaseWorker DBObj = new DatabaseWorker();
        Dictionary<int, string> dictComments = new Dictionary<int, string>();

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
                Response.Redirect("CommentsPage.aspx?ID=" + hdnF.Value);

            }
        }
    }
}