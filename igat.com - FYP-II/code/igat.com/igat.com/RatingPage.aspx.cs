using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace igat.com
{
    public partial class RatingPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["ID"] != null)
                {
                    ID = Request.QueryString["ID"];
                    BindRepeater();
                }
            }
        }
        
        protected void BindRepeater()
        {
            DatabaseWorker dw1 = new DatabaseWorker();
            //DataSet ds1 = dw1.getGameRatingAsDataSet(Convert.ToInt32(ID));
            //rptRating.DataSource = ds1;
            //rptRating.DataBind();
            //DataSet dt = dw1.getGameNamesAsDataSet(Convert.ToInt32(ID));
            //rptGameName.DataSource = dt;
            //rptGameName.DataBind();
            DataSet dc = dw1.getGameFeatureRatingAsDataSet(Convert.ToInt32(ID));
            rptFeature.DataSource = dc;
            rptFeature.DataBind();

        }

        protected void rptGameName_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}