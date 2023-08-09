using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;
using igat.com.ajax;

namespace igat.com
{
    public class DatabaseWorker
    {
        string connectionString = @"Data Source=VACEEM-PC\AHMADSQLSERVER;Initial Catalog=Ratingdb1;Integrated Security=True";   //Connection string for sql

        public void CreateWebsite()
        {
            string query = "CREATE TABLE GameWebsites(Id int IDENTITY(1,1) PRIMARY KEY, Link nchar(200) NOT NULL)";
            TryCreateTable(query);
            query = "IF NOT EXISTS(SELECT Link FROM GameWebsites WHERE Link LIKE @value) INSERT into GameWebsites (Link) values (@value)";
            InsertTable("http://www.eurogamer.net/", query);
            InsertTable("http://www.polygon.com/games/reviewed/", query);
            //query = "ALTER  TABLE  GameWebsites WITH CHECK ADD CONSTRAINT UQ_GameWebsite UNIQUE (Link);";
        }

        void CreateLinkTable()
        {
            string query = "CREATE TABLE PageLinks(Id int IDENTITY(1,1) PRIMARY KEY, Link nchar(200) NOT NULL, WebsiteId int )";
            TryCreateTable(query);

        }
        void CreateGamesTable()
        {
            string query = "CREATE TABLE Games(Id int IDENTITY(1,1) PRIMARY KEY, Name varchar(200) NOT NULL, LinkId int )";
            TryCreateTable(query);
            //query = "ALTER  TABLE  Games WITH CHECK ADD CONSTRAINT UQ_Games UNIQUE (Name)";

        }
        void CreateDetailedGameTable()
        {
            string query = "CREATE TABLE GamesDetail(Id int IDENTITY(1,1) PRIMARY KEY, Name varchar(300) NOT NULL,Description varchar(2500), Platform varchar(100),Developer varchar(100),ReleaseDate varchar(15),ImageAdress varchar(800),Genre varchar(150),LinkId int)";
            TryCreateTable(query);
        }
        void CreateCommentsTable()
        {
            string query = "CREATE TABLE Comments(Id int IDENTITY(1,1) PRIMARY KEY, Comment varchar(MAX) NOT NULL, GameId int  FOREIGN KEY REFERENCES GamesDetail(Id) )";
            TryCreateTable(query);
        }
        void CreateCleanCommentsTable()
        {
            string query = "CREATE TABLE CleanComments(Id int IDENTITY(1,1) PRIMARY KEY, Comment varchar(MAX) NOT NULL, GameId int FOREIGN KEY REFERENCES GamesDetail(Id))";
            TryCreateTable(query);
        }
        void CreateSlangTable()
        {
            string query = "CREATE TABLE SlangWords(Slang varchar(100) NOT NULL, Abbreviations varchar(200) NOT NULL)";
            TryCreateTable(query);
        }
        void CreatePositiveTable()
        {
            string query = "CREATE TABLE PositiveWords(Word varchar(100) NOT NULL)";
            TryCreateTable(query);
        }
        void CreateNegativeTable()
        {
            string query = "CREATE TABLE NegativeWords(Word varchar(100) NOT NULL)";
            TryCreateTable(query);
        }
        void CreateRatingTable()
        {
            string query = "CREATE TABLE GamesRating (Id int IDENTITY(1,1) PRIMARY KEY, Rating float NOT NULL, GameId int FOREIGN KEY REFERENCES GamesDetail(Id))";
            TryCreateTable(query);
        }
        public void CreateFeatureRatingTable()
        {
            string query = "CREATE TABLE GamesFeatureRating (Id int IDENTITY(1,1) PRIMARY KEY, Graphics float NOT NULL, Gameplay float NOT NULL,Performance float NOT NULL,GameId int FOREIGN KEY REFERENCES GamesDetail(Id))";
            TryCreateTable(query);
        }
        public void InsertPageLinks(string value1, int value2)
        {
            string query = "IF NOT EXISTS(SELECT Link FROM PageLinks WHERE Link LIKE @value1) INSERT into PageLinks (Link, WebsiteId) VALUES (@value1, @value2)";
            InsertTable(query, value1, value2);
        }
        public void InsertGames(string value1, int value2)
        {
            string query = "IF NOT EXISTS(SELECT Name FROM Games WHERE Name LIKE @value1) INSERT INTO Games (Name, LinkId) VALUES (@value1, @value2)";
            InsertTable(query, value1, value2);
        }
        public void InsertGamesDetail(string value1, string value2, string value3, string value4, string value5, string value6, string value7, int value8)
        {
            string query = "IF NOT EXISTS(SELECT Name FROM GamesDetail WHERE Name LIKE @value1) INSERT INTO GamesDetail(Name, Description, Platform, Developer, ReleaseDate,ImageAdress, Genre, LinkId) VALUES(@value1, @value2,@value3,@value4,@value5,@value6,@value7,@value8)";
            InsertTable(query, value1, value2, value3, value4, value5, value6, value7, value8);
        }
        public void InsertComments(string value1, int value2)
        {
            string query = "INSERT INTO Comments (Comment,GameId) VALUES (@value1,@value2)";
            InsertTable(query, value1, value2);
        }
        public void InsertCleanComments(string value1, int value2)
        {
            string query = "INSERT INTO CleanComments (Comment,GameId) VALUES (@value1,@value2)";
            InsertTable(query, value1, value2);
        }
        public void InsertSlangs(string value1, string value2)
        {
            string query = "IF NOT EXISTS(SELECT Slang FROM SlangWords WHERE Slang LIKE @value1) INSERT INTO SlangWords (Slang, Abbreviations) VALUES (@value1, @value2)";
            InsertTable(query, value1, value2);
        }
        public void InsertPositiveSet(string value1)
        {
            string query = "IF NOT EXISTS(SELECT Word FROM PositiveWords WHERE Word LIKE @value1) INSERT INTO PositiveWords (Word) VALUES (@value1)";
            InsertTable(value1, query);
        }
        public void InsertNegativeSet(string value1)
        {
            string query = "IF NOT EXISTS(SELECT Word FROM NegativeWords WHERE Word LIKE @value1) INSERT INTO NegativeWords (Word) VALUES (@value1)";
            InsertTable(value1, query);
        }
        public void InsertRatings(float value1, int value2)
        {
            string query = "IF NOT EXISTS(SELECT GameId FROM GamesRating WHERE GameId LIKE @value2) INSERT INTO GamesRating (Rating, GameId) VALUES (@value1, @value2)";
            InsertTable(query, value1, value2);
        }
        public void InsertFeatureRatings(float value1, float value2, float value3, int value4)
        {
            string query = "INSERT INTO GamesFeatureRating (Graphics,Gameplay,Performance, GameId) VALUES (@value1, @value2, @value3, @value4)";

            InsertTable(query, value1, value2, value3, value4);
        }
        void TryCreateTable(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch
                { }
            }

        }


        void InsertTable(string value1, string query)
        {

            CreatePositiveTable();
            CreateNegativeTable();
            //Saving interested Urls into Database            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value1", value1);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
        void InsertTable(string query, string value1, int value2)
        {
            CreateLinkTable();
            CreateGamesTable();
            CreateCommentsTable();
            CreateCleanCommentsTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@value1", value1);
                    command.Parameters.AddWithValue("@value2", value2);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    { }
                }
            }
        }
        void InsertTable(string query, string value1, string value2, string value3, string value4, string value5, string value6, string value7, int value8)
        {
            CreateDetailedGameTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@value1", value1);
                    command.Parameters.AddWithValue("@value2", value2);
                    command.Parameters.AddWithValue("@value3", value3);
                    command.Parameters.AddWithValue("@value4", value4);
                    command.Parameters.AddWithValue("@value5", value5);
                    command.Parameters.AddWithValue("@value6", value6);
                    command.Parameters.AddWithValue("@value7", value7);
                    command.Parameters.AddWithValue("@value8", value8);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }
        }
        void InsertTable(string query, string value1, string value2)
        {
            CreateSlangTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@value1", value1);
                    command.Parameters.AddWithValue("@value2", value2);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    { }
                }
            }
        }
        void InsertTable(string query, float value1, int value2)
        {
            CreateRatingTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@value1", value1);
                    command.Parameters.AddWithValue("@value2", value2);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    { Debug.WriteLine(e.Message); }
                }
            }
        }
        void InsertTable(string query, float value1, float value2, float value3, int value4)
        {
            CreateFeatureRatingTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@value1", value1);
                    command.Parameters.AddWithValue("@value2", value2);
                    command.Parameters.AddWithValue("@value3", value3);
                    command.Parameters.AddWithValue("@value4", value4);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    { Debug.WriteLine(e.Message); }
                }
            }
        }
        public Dictionary<int, string> RetrieveLinks(string name)
        {
            Dictionary<int, string> values = new Dictionary<int, string>();
            string query = "SELECT Id,Link FROM PageLinks WHERE Link LIKE '%" + name + "%'";
            values = RetrieveData(query);
            return values;
        }
        public Dictionary<int, string> RetrieveWebsite()
        {
            Dictionary<int, string> values = new Dictionary<int, string>();
            string query = "SELECT Id,Link FROM GameWebsites";
            values = RetrieveData(query);
            return values;
        }
        public Dictionary<int, string> RetrieveGames()
        {
            Dictionary<int, string> values = new Dictionary<int, string>();
            string query = "SELECT Id,Name FROM Games";
            values = RetrieveData(query);
            return values;
        }
        public Dictionary<int, string> RetrieveComments()
        {
            Dictionary<int, string> values = new Dictionary<int, string>();
            string query = "SELECT Id,Comment FROM Comments";
            values = RetrieveData(query);
            return values;
        }
        public List<KeyValuePair<string, int>> RetrieveCommentsToClean(int gameID)
        {
            List<KeyValuePair<string, int>> values = new List<KeyValuePair<string, int>>();
            string query = "SELECT Comment, GameId FROM Comments WHERE GameId =" + gameID;
            values = RetrieveData2(query);
            return values;
        }
        public List<KeyValuePair<string, int>> RetrieveCleanComments(int gameID)
        {
            List<KeyValuePair<string, int>> comments = new List<KeyValuePair<string, int>>();
            string query = "SELECT Comment,GameId FROM CleanComments WHERE GameId =" + gameID;
            comments = RetrieveData2(query);
            return comments;
        }
        public string GetGame(int gameId)
        {
            string name = null;
            string query = "SELECT Name From Games WHERE Id =" + gameId;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        name = reader.GetString(0);
                    }
                }
            }
            return name;
        }
        public DataSet getCommentsAsDataSet(int ID)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "SELECT Id,Comment FROM Comments Where GameId = " + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "Comments");
                }
            }
            catch (Exception e)
            {

            }

            return ds;
        }
        public DataSet getGameNamesAsDataSet()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "SELECT Id,Name FROM Games";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "Games");
                }
            }
            catch (Exception e)
            {

            }

            return ds;
        }
        public DataSet getTopGamesAsDataSet()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "SELECT TOP 5 Id,Name FROM GamesDetail";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesDetail");
                }
            }
            catch (Exception e)
            {

            }

            return ds;
        }
        public DataSet getGenresAsDataSet()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "SELECT Id,Genre FROM Genres";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "Genres");
                }
            }
            catch (Exception e)
            {

            }

            return ds;
        }
        public DataSet getGenreGamesAsDataSet(string ID)
        {
            DataSet ds = new DataSet();
            try
            {
                //string query = "SELECT Id,Name FROM GamesDetail WHERE Genre LIKE '%"+ID+"%'";
                string query = "select top 10 percent Id,Name,ImageAdress from [GamesDetail] order by newid()";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesDetail");
                }
            }
            catch (Exception e)
            {

            }

            return ds;
        }

        public List<Suggestion> GetSuggestions(String searchVal)
        {
            List<Suggestion> games = new List<Suggestion>();
            string query = "SELECT TOP 5 * from [Ratingdb1].[dbo].[GamesDetail] WHERE Name LIKE '%" + searchVal + "%'";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                games.Add(new Suggestion((int)sdr["Id"], sdr["Name"].ToString())
                                {});
                            }
                            }
                    }
                        connection.Close();
                }

            }
            catch { }
            return games;
        }
        public DataSet getGameNamesAsDataSet(int ID)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "SELECT Id,Name FROM GamesDetail WHERE Id =" + ID ;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesDetail");
                }
            }
            catch (Exception e)
            {

            }

            return ds;
        }
        public DataSet getGamePhotosAsDataSet()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "SELECT TOP 5 Id,Name,ImageAdress FROM GamesDetail order by newid()";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesDetail");
                }
            }
            catch (Exception e)
            {

            }

            return ds;
        }
        public string getGamePhoto(int ID)
        {
            string adress = null;
            try
            {
                string query = "SELECT ImageAdress FROM GamesDetail WHERE Id =" + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        adress = command.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception e)
            {

            }

            return adress;
        }
        public string getGameDeveloper(int ID)
        {
            string developer = null;
            try
            {
                string query = "SELECT Developer FROM GamesDetail WHERE Id =" + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        developer = command.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception e)
            {

            }

            return developer;
        }
        public string getGamePlatform(int ID)
        {
            string platform = null;
            try
            {
                string query = "SELECT Platform FROM GamesDetail WHERE Id =" + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        platform = command.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception e)
            {

            }

            return platform;
        }
        public string getGameReleaseDate(int ID)
        {
            string date = null;
            try
            {
                string query = "SELECT ReleaseDate FROM GamesDetail WHERE Id =" + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        date = command.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception e)
            {

            }

            return date;
        }
        public string getGameDescription(int ID)
        {
            string description = null;
            try
            {
                string query = "SELECT Description FROM GamesDetail WHERE Id =" + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        description = command.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception e)
            {

            }

            return description;
        }
        public string getGameGenre(int ID)
        {
            string genre = null;
            try
            {
                string query = "SELECT Genre FROM GamesDetail WHERE Id =" + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        genre = command.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception e)
            {

            }

            return genre;
        }
        public double getGameRating(int ID)
        {
            double rating = 0.0;
            try
            {
                string query = "SELECT Rating FROM GamesRating Where GameId = " + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        rating = (double)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception e)
            {

            }

            return rating;
        }
        public double getGameplayRating(int ID)
        {
            double rating = 0.0;
            try
            {
                string query = "SELECT Gameplay FROM GamesFeatureRating Where GameId = " + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        rating = (double)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception e)
            {

            }

            return rating;
        }
        public double getGraphicsRating(int ID)
        {
            double rating = 0.0;
            try
            {
                string query = "SELECT Graphics FROM GamesFeatureRating Where GameId = " + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        rating = (double)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception e)
            {

            }

            return rating;
        }
        public double getPerformanceRating(int ID)
        {
            double rating = 0.0;
            try
            {
                string query = "SELECT Performance FROM GamesFeatureRating Where GameId = " + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        rating = (double)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception e)
            {

            }

            return rating;
        }
        public DataSet getGameFeatureRatingAsDataSet(int ID)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "SELECT Id,Graphics,Gameplay,Performance FROM GamesFeatureRating Where GameId = " + ID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesFeatureRating");
                }
            }
            catch (Exception e)
            {

            }

            return ds;
        }
        public int getGameId(string gameName)
        {
            int id = 0;
            try
            {
                string query = "SELECT Id FROM GamesDetail where Name LIKE '%" + gameName + "%'";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            id = reader.GetInt32(0);
                        }

                    }
                }
            }
            catch (Exception e)
            {

            }

            return id;
        }
        Dictionary<int, string> RetrieveData(string query)
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string url = reader.GetString(1);
                        try
                        {
                            data.Add(id, url);
                        }
                        catch { }
                    }

                }
            }
            return data;
        }
        List<KeyValuePair<string, int>> RetrieveData2(string query)
        {
            List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string url = reader.GetString(0);
                        int id = reader.GetInt32(1);
                        try
                        {
                            data.Add(new KeyValuePair<string, int>(url, id));
                        }
                        catch (Exception ex) { Debug.Print(ex.Message); }
                    }

                }
            }
            return data;
        }
        public Dictionary<string, string> RetrieveSlangs()
        {
            string query = "SELECT Slang, Abbreviations FROM SlangWords";
            Dictionary<string, string> words = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        string id = reader.GetString(0);
                        string url = reader.GetString(1);
                        try
                        {
                            words.Add(id, url);
                        }
                        catch { }
                    }

                }
            }
            return words;
        }

        public List<string> positiveRetrieve()
        {
            List<string> posRetrieve = new List<string>();
            string query = "SELECT Word FROM PositiveWords";
            posRetrieve = DataRetrieve(query);
            return posRetrieve;
        }
        public List<string> negativeRetrieve()
        {
            List<string> negRetrieve = new List<string>();
            string query = "SELECT Word FROM NegativeWords";
            negRetrieve = DataRetrieve(query);
            return negRetrieve;
        }
        public List<string> DataRetrieve(string query)
        {
            List<string> data = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        string word = reader.GetString(0);
                        try
                        {
                            data.Add(word);
                        }
                        catch { }
                    }

                }
                return data;
            }
        }

        public DataSet GetTopRecommendedGames(double rating, double graphicRating, double gameplayRating, double performanceRating,string genre,string platform)
        {
           
            DataSet ds = new DataSet();
            try
            {
                string query = "Select TOP 5 Id,Name,ImageAdress from GamesDetail where Id IN ("+
               " SELECT GameId from GamesRating Where Rating >"+ rating+ "AND Rating<"+ (rating +2) +"AND GameId IN"+
                "(Select GameId from GamesFeatureRating WHERE(Performance >"+ performanceRating + "AND Performance <"+ (performanceRating +2) +") AND ("+
                " Graphics >"+graphicRating+"AND Graphics<"+(graphicRating+2)+ ") AND (Gameplay >" + gameplayRating + "AND Gameplay <" + (gameplayRating + 2) + ") AND " + "GameId IN"+
                "(SELECT Id FROM GamesDetail where Platform LIKE '%"+platform+"%' AND Genre LIKE '%"+genre+"%')))";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesDetail");
                }
            }
            catch (Exception e)
            {

            }
            return ds;
        }
        public DataSet GetTopRecommendedGames(double rating, double graphicRating, double gameplayRating, double performanceRating, string genre)
        {

            DataSet ds = new DataSet();
            try
            {
                string query = "Select TOP 5 Id,Name,ImageAdress from GamesDetail where Id IN (" +
               " SELECT GameId from GamesRating Where Rating >" + rating + "AND Rating<" + (rating + 2) + "AND GameId IN" +
                "(Select GameId from GamesFeatureRating WHERE(Performance >" + performanceRating + "AND Performance <" + (performanceRating + 2) + ") AND (" +
                " Graphics >" + graphicRating + "AND Graphics<" + (graphicRating + 2) + ") AND (Gameplay >" + gameplayRating + "AND Gameplay <" + (gameplayRating + 2) + ") AND " + "GameId IN" +
                "(SELECT Id FROM GamesDetail where Genre LIKE '%" + genre + "%')))";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesDetail");
                }
            }
            catch (Exception e)
            {

            }
            return ds;
        }
        public DataSet GetTopRecommendedGames(double rating, double graphicRating, double gameplayRating, double performanceRating)
        {

            DataSet ds = new DataSet();
            try
            {
                string query = "Select TOP 5 Id,Name,ImageAdress from GamesDetail where Id IN (" +
               " SELECT GameId from GamesRating Where Rating >" + rating + "AND Rating<" + (rating + 2) + "AND GameId IN" +
                "(Select GameId from GamesFeatureRating WHERE(Performance >" + performanceRating + "AND Performance <" + (performanceRating + 2) + ") AND (" +
                " Graphics >" + graphicRating + "AND Graphics<" + (graphicRating + 2) + ") AND (Gameplay >" + gameplayRating + "AND Gameplay <" + (gameplayRating + 2) + ") ))";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesDetail");
                }
            }
            catch (Exception e)
            {

            }
            return ds;
        }
        public DataSet GetTopRecommendedGames(double rating, double graphicRating, double gameplayRating)
        {

            DataSet ds = new DataSet();
            try
            {
                string query = "Select TOP 5 Id,Name,ImageAdress from GamesDetail where Id IN (" +
               " SELECT GameId from GamesRating Where Rating >" + rating + "AND Rating<" + (rating + 2) + "AND GameId IN" +
                "(Select GameId from GamesFeatureRating WHERE( Graphics >" + graphicRating + "AND Graphics<" + (graphicRating + 2) + ") AND (Gameplay >" + gameplayRating + "AND Gameplay <" + (gameplayRating + 2) + ") ))";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesDetail");
                }
            }
            catch (Exception e)
            {

            }
            return ds;
        }
        public DataSet GetTopRecommendedGames(double rating, double graphicRating)
        {

            DataSet ds = new DataSet();
            try
            {
                string query = "Select TOP 5 Id,Name,ImageAdress from GamesDetail where Id IN (" +
                " SELECT GameId from GamesRating Where Rating >" + rating + "AND Rating<" + (rating + 2) + "AND GameId IN" +
                 "(Select GameId from GamesFeatureRating WHERE( Graphics >" + graphicRating + "AND Graphics<" + (graphicRating + 2) + ") ))";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesDetail");
                }
            }
            catch (Exception e)
            {

            }
            return ds;
        }
        public DataSet GetTopRecommendedGames(double rating)
        {

            DataSet ds = new DataSet();
            try
            {
                string query = "Select TOP 5 Id,Name,ImageAdress from GamesDetail where Id IN (" +
                 " SELECT GameId from GamesRating Where Rating >" + rating + "AND Rating<" + (rating + 2)+")";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesDetail");
                }
            }
            catch (Exception e)
            {

            }
            return ds;
        }
        
        

        public DataSet GetRecommendedGames()
        {

            DataSet ds = new DataSet();
            try
            {
                string query = "Select TOP 5 Id,Name,ImageAdress from GamesDetail";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter sqlda = new SqlDataAdapter(query, connection);
                    sqlda.Fill(ds, "GamesDetail");
                }
            }
            catch (Exception e)
            {

            }
            return ds;
        }
    }
}