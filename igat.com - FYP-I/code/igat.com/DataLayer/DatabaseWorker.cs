using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;

namespace igat.com
{
    public class DatabaseWorker
    {
        string connectionString = @"Data Source=VACEEM-PC\AHMADSQLSERVER;Initial Catalog=Ratingdb1;Integrated Security=True";   //Connection string for sql
         
        public void CreateWebsite()
        {
            string query = "CREATE TABLE GameWebsites(Id int IDENTITY(1,1) PRIMARY KEY, Link nchar(200) NOT NULL)";
            TryCreateTable(query);
            query = "INSERT into GameWebsites (Link) values (@value)";
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
            string query = "CREATE TABLE Games(Id int IDENTITY(1,1) PRIMARY KEY, Name nchar(200) NOT NULL, LinkId int )";
            TryCreateTable(query);
            //query = "ALTER  TABLE  Games WITH CHECK ADD CONSTRAINT UQ_Games UNIQUE (Name)";

        }
        void CreateCommentsTable()
        {
            string query = "CREATE TABLE Comments(Id int IDENTITY(1,1) PRIMARY KEY, Comment varchar(MAX) NOT NULL, GameId int )";
            TryCreateTable(query);
        }
        void CreateSlangTable()
        {
            string query = "CREATE TABLE SlangWords(Slang nchar(100) NOT NULL, Abbreviations nchar(200) NOT NULL)";
            TryCreateTable(query);
        }
        public void InsertPageLinks(string value1,int value2) 
        {
            string query = "INSERT into PageLinks (Link, WebsiteId) VALUES (@value1, @value2)";
            InsertTable(query, value1, value2);
        }
        public void InsertGames(string value1, int value2)
        {
            string query = "INSERT INTO Games (Name, LinkId) VALUES (@value1, @value2)";
            InsertTable(query, value1, value2);
        }
        public void InsertComments(string value1, int value2)
        {
            string query = "INSERT INTO Comments (Comment,GameId) VALUES (@value1,@value2)";
            InsertTable(query, value1,value2);
        }
        public void InsertSlangs(string value1, string value2)
        {
            string query = "INSERT INTO SlangWords (Slang, Abbreviations) VALUES (@value1, @value2)";
            InsertTable(query, value1, value2);
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
                {  }
            }

        }
        

        void InsertTable(string value1, string query)
        {
            
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
                    {  }
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
                    {  }
                }
            }
        }

        public Dictionary<int, string> RetrieveLinks(string name)
        {
            Dictionary<int, string> values = new Dictionary<int, string>();
            string query  = "SELECT Id,Link FROM PageLinks WHERE Link LIKE '%"+name+"%'";
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

        public DataSet getCommentsAsDataSet(int ID) {
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

        public int getGameId(string gameName)
        {
            int id = 0;
            try
            {
                string query = "SELECT Id FROM Games where Name LIKE '%"+ gameName+"%'";
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
        Dictionary<int,string> RetrieveData(string query)
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

    }
}