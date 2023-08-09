using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace igat.com
{
    public partial class Details : System.Web.UI.Page
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
           
            DataSet dt = dw1.getGameNamesAsDataSet(Convert.ToInt32(ID));
            rptGameName.DataSource = dt;
            rptGameName.DataBind();

            double rating = dw1.getGameRating(Convert.ToInt32(ID));
            if ((rating >= 0 && rating <= 0.3)) { spnRating.Attributes.Add("class", "rating r0"); }
            if ((rating >= 0.7 && rating <= 1.3)) { spnRating.Attributes.Add("class", "rating r1"); }
            if ((rating >= 1.7 && rating <= 2.3)) { spnRating.Attributes.Add("class", "rating r2"); }
            if ((rating >= 2.7 && rating <= 3.3)) { spnRating.Attributes.Add("class", "rating r3"); }
            if ((rating >= 3.7 && rating <= 4.3)) { spnRating.Attributes.Add("class", "rating r4"); }
            if ((rating >= 4.7 && rating <= 5.3)) { spnRating.Attributes.Add("class", "rating r5"); }
            if ((rating >= 5.7 && rating <= 6.3)) { spnRating.Attributes.Add("class", "rating r6"); }
            if ((rating >= 6.7 && rating <= 7.3)) { spnRating.Attributes.Add("class", "rating r7"); }
            if ((rating >= 7.7 && rating <= 8.3)) { spnRating.Attributes.Add("class", "rating r8"); }
            if ((rating >= 8.7 && rating <= 9.3)) { spnRating.Attributes.Add("class", "rating r9"); }

            if ((rating >= 0.3 && rating <= 0.7)) { spnRating.Attributes.Add("class", "rating r05"); }
            if ((rating >= 1.3 && rating <= 1.7)) { spnRating.Attributes.Add("class", "rating r15"); }
            if ((rating >= 2.3 && rating <= 2.7)) { spnRating.Attributes.Add("class", "rating r25"); }
            if ((rating >= 3.3 && rating <= 3.7)) { spnRating.Attributes.Add("class", "rating r35"); }
            if ((rating >= 4.3 && rating <= 4.7)) { spnRating.Attributes.Add("class", "rating r45"); }
            if ((rating >= 5.3 && rating <= 5.7)) { spnRating.Attributes.Add("class", "rating r55"); }
            if ((rating >= 6.3 && rating <= 6.7)) { spnRating.Attributes.Add("class", "rating r65"); }
            if ((rating >= 7.3 && rating <= 7.7)) { spnRating.Attributes.Add("class", "rating r75"); }
            if ((rating >= 8.3 && rating <= 8.7)) { spnRating.Attributes.Add("class", "rating r85"); }
            if ((rating >= 9.3 && rating <= 9.7)) { spnRating.Attributes.Add("class", "rating r95"); }
            if ((rating >= 9.7)) { spnRating.Attributes.Add("class", "rating r10"); }

            lblRate.Text = "(" + Math.Round(rating,1).ToString() + " /<span style='font-size:18px'>10</span>)";

            string imgAdress = dw1.getGamePhoto(Convert.ToInt32(ID));
            Debug.WriteLine(imgAdress);
            imgGameImage.ImageUrl = imgAdress;

            double graphicsRating = dw1.getGraphicsRating(Convert.ToInt32(ID));
            graphicsRating = Math.Round(graphicsRating,1); 
            lblGraphics.Text = graphicsRating.ToString();
            double performanceRating = dw1.getPerformanceRating(Convert.ToInt32(ID));
            performanceRating = Math.Round(performanceRating, 1);
            lblPerformance.Text = performanceRating.ToString();
            double gameplayRating = dw1.getGameplayRating(Convert.ToInt32(ID));
            gameplayRating = Math.Round(gameplayRating, 1);
            lblGameplay.Text = gameplayRating.ToString();

            string developer = dw1.getGameDeveloper(Convert.ToInt32(ID));
            lblDeveloper.Text = developer;
            string platform = dw1.getGamePlatform(Convert.ToInt32(ID));
            lblPlatform.Text = platform;
            string releaseDate = dw1.getGameReleaseDate(Convert.ToInt32(ID));
            lblReleaseDate.Text = releaseDate;
            string genre = dw1.getGameGenre(Convert.ToInt32(ID));
            lblGenre.Text = genre;
            string description = dw1.getGameDescription(Convert.ToInt32(ID));
            lblDescription.Text = description;

            Recommandation rec = new Recommandation();
            DataSet dt1 = rec.Recommond(Convert.ToInt32(ID));
            rptRecommond.DataSource = dt1;
            rptRecommond.DataBind();
        }

        protected void rptFeature_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                HiddenField hdnF = (HiddenField)e.Item.FindControl("hdnID");
                Response.Redirect("Details.aspx?ID=" + hdnF.Value);

            }
        }

        //protected void rptRating_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName == "View")
        //    {
        //        HiddenField hdnF = (HiddenField)e.Item.FindControl("hdnID");
        //        Response.Redirect("Details.aspx?ID=" + hdnF.Value);

        //    }
        //}

        protected void rptGameName_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                HiddenField hdnF = (HiddenField)e.Item.FindControl("hdnID");
                Response.Redirect("Details.aspx?ID=" + hdnF.Value);

            }
        }

        protected void rptImages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                HiddenField hdnF = (HiddenField)e.Item.FindControl("hdnID");
                Response.Redirect("Details.aspx?ID=" + hdnF.Value);

            }
        }

        //protected void rptRating_ItemDataBound(object source, RepeaterCommandEventArgs e) {
        //    Label lblRating = (Label)e.Item.FindControl("lblRating");
        //    double rating = Convert.ToDouble(lblRating.Text);
        //    spnRating.Attributes.Add("class", "r9");
        //}
    }
}