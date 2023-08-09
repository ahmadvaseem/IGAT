using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace igat.com
{
    public partial class Games : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindRepeater();

            }
        }
        protected void BindRepeater()
        {
            DatabaseWorker dw = new DatabaseWorker();
            DataSet ds1 = dw.getGenreGamesAsDataSet(ID);
            rptGenreGame.DataSource = ds1;
            rptGenreGame.DataBind();

        }
    }
}