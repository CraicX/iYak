using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace iYak.Classes
{
    class Datax
    {

        static public SqliteConnectionStringBuilder ConnectionStr = new SqliteConnectionStringBuilder();

        static public SqliteConnection DBH;


        static public void InitializeDatabase() 
        {

            List<String> QueryList = new List<String>();

            ConnectionStr.Mode = SqliteOpenMode.ReadWriteCreate;
            ConnectionStr.Cache = SqliteCacheMode.Private;
            ConnectionStr.DataSource = Helpers.JoinPath(Config.CachePath, "iYak.db");

            DBH = new SqliteConnection(ConnectionStr.ConnectionString);


            //':
            //':  Define Queries for creating tables and indexes
            //':
            QueryList.Add(@"
                CREATE TABLE If Not Exists [Playlists] (
                    [id]           INTEGER PRIMARY KEY AUTOINCREMENT,
                    [name]         NVARCHAR(50)     NOT NULL,
                    [date_created] DateTime         DEFAULT CURRENT_TIMESTAMP NOT NULL,
                    [last_updated] DateTime         DEFAULT CURRENT_TIMESTAMP NOT NULL
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

            //DropTables("Actors")
            //TruncateTables()


            DBH.Open();

            SqliteCommand MyCommand = DBH.CreateCommand();

            foreach (string query in QueryList) {
                MyCommand.CommandText = query;
                MyCommand.ExecuteNonQuery();
            }

            DBH.Close();



        }


    }
}
