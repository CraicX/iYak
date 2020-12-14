using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;


namespace iYak.Classes
{
    class Datax
    {

        static public SqliteConnectionStringBuilder ConnectionStr = new SqliteConnectionStringBuilder();

        static public SqliteConnection DBH;


        static public void InitializeDatabase() 
        {


            List<String> QueryList = new List<String>();

            ConnectionStr.Mode       = SqliteOpenMode.ReadWriteCreate;
            ConnectionStr.Cache      = SqliteCacheMode.Private;
            ConnectionStr.DataSource = Helpers.JoinPath(Config.CachePath, "iYak.db");
            Console.WriteLine(Helpers.JoinPath(Config.CachePath, "iYak.db"));
            
            DBH = new SqliteConnection(ConnectionStr.ConnectionString);


            //':
            //':  Define Queries for creating tables and indexes
            //':
            QueryList.Add(@"
                CREATE TABLE If Not Exists [Playlists] (
                    [id]           INTEGER PRIMARY KEY AUTOINCREMENT,
                    [name]         NVARCHAR(50)     NOT NULL,
                    [date_created] DateTime         DEFAULT CURRENT_TIMESTAMP NOT NULL,
                    [last_updated] DateTime         DEFAULT CURRENT_TIMESTAMP NOT NULL,
                    [positions]    TEXT             DEFAULT """" NOT NULL
                ); 
            ");


            QueryList.Add(@"
                CREATE TABLE If Not Exists [Speeches] (
                    [id]            INTEGER PRIMARY KEY AUTOINCREMENT,
                    [playlist]      INTEGER         DEFAULT 0    NOT NULL,
                    [voice_handle]  NVARCHAR(100)   DEFAULT """" NOT NULL,
                    [voice_name]    NVARCHAR(16)    DEFAULT """" NOT NULL,
                    [volume]        TINYINT(3)      DEFAULT 100  NOT NULL,
                    [rate]          TINYINT(3)      DEFAULT 5    NOT NULL,
                    [pitch]         TINYINT(3)      DEFAULT 5    NOT NULL,
                    [avatar]        NVARCHAR(50)    DEFAULT """" NOT NULL,
                    [say]           TEXT            DEFAULT """" NOT NULL
                ); 
            ");

            QueryList.Add(@"
                CREATE TABLE If Not Exists [Actors] (
                    [id]            INTEGER PRIMARY KEY AUTOINCREMENT,
                    [nickname]      NVARCHAR(50)    DEFAULT """" NOT NULL,
                    [playlist]      INTEGER         DEFAULT 0    NOT NULL,
                    [voice_handle]  NVARCHAR(100)   DEFAULT """" NOT NULL,
                    [voice_name]    NVARCHAR(16)    DEFAULT """" NOT NULL,
                    [volume]        TINYINT(3)      DEFAULT 100  NOT NULL,
                    [rate]          TINYINT(3)      DEFAULT 5    NOT NULL,
                    [pitch]         TINYINT(3)      DEFAULT 5    NOT NULL,
                    [avatar]        NVARCHAR(50)    DEFAULT """" NOT NULL
                ); 
            ");

            QueryList.Add("CREATE INDEX IF Not EXISTS idx_RosterP ON Actors (playlist);");

            DropTables("Playlists,Actors,Speeches");
            //TruncateTables()


            DBH.Open();

            SqliteCommand MyCommand = DBH.CreateCommand();

            //foreach (string query in QueryList) {
            //    MyCommand.CommandText = query;
            //    MyCommand.ExecuteNonQuery();
            //}

            DBH.Close();



        }

        static private void DropTables(string tableList)
        {
            string[] Tables = tableList.Split(',');

            foreach (string tableName in Tables) {
                Query("DROP TABLE IF EXISTS " + tableName.Trim());
            }
            
        }


        static public void Query(string query)
        {
            DBH.Open();

            SqliteCommand command = DBH.CreateCommand();

            command.CommandText = query;
            command.ExecuteNonQuery();

            DBH.Close();

        }

    }
}
