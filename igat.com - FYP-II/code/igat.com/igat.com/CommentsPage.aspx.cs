using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace igat.com
{
    public partial class CommentsPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                if (Request.QueryString["ID"] != null) {
                    ID = Request.QueryString["ID"];
                    BindRepeater();
                }   
            }
        }

        protected void BindRepeater() {
            DatabaseWorker dw = new DatabaseWorker();
            DataSet ds = dw.getCommentsAsDataSet(Convert.ToInt32(ID));
            rptComments.DataSource = ds;
            rptComments.DataBind();

        }
    }
}