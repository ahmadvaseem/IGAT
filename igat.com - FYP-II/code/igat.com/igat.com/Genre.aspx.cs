using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace igat.com
{
    public partial class Genre : System.Web.UI.Page
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
            DataSet ds = dw.getGenresAsDataSet();
            rptGenres.DataSource = ds;
            rptGenres.DataBind();

        }

        protected void rptGames_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
        protected void OnClickGenreButton(object sender, EventArgs e)
        {

        }
    }
}