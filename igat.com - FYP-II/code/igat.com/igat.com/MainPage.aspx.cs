using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace igat.com
{
    public partial class MainPage : System.Web.UI.Page
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
           // DataSet ds = dw.getTopGamesAsDataSet();
            //rptTopGames.DataSource = ds;
            //rptTopGames.DataBind();
            
            DataSet ds1 = dw.getGamePhotosAsDataSet();
            rptPhtots.DataSource = ds1;
            rptPhtots.DataBind();

        }
    }
}