using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace igat.com
{
    public class Recommandation
    {
        
        DatabaseWorker DBObj = new DatabaseWorker();
        public DataSet Recommond(int id)
        {
            string platform = DBObj.getGamePlatform(id);
            string genre = DBObj.getGameGenre(id);
            double rating = DBObj.getGameRating(id);
            double graphicRating = DBObj.getGraphicsRating(id);
            double gameplayRating = DBObj.getGameplayRating(id);
            double performanceRating = DBObj.getPerformanceRating(id);
            
            DataSet ds = DBObj.GetTopRecommendedGames(rating,graphicRating,gameplayRating,performanceRating,genre,platform);

            if (ds.Tables[0].Rows.Count == 0)
            {
                ds = DBObj.GetTopRecommendedGames(rating, graphicRating, gameplayRating, performanceRating,genre);

            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                ds = DBObj.GetTopRecommendedGames(rating, graphicRating, gameplayRating, performanceRating);

            }
           if (ds.Tables[0].Rows.Count == 0)
            {
                ds = DBObj.GetTopRecommendedGames(rating, graphicRating, gameplayRating);

            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                ds = DBObj.GetTopRecommendedGames(rating, graphicRating);

            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                ds = DBObj.GetTopRecommendedGames(rating);

            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                ds = DBObj.GetRecommendedGames();
            }

            return ds;
        }
    }
}