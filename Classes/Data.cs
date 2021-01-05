//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     Data.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  Class for utilizing an SqlLite database
//
//
using System;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;


namespace iYak.Classes
{
    class Datax
    {

        static public SqliteConnectionStringBuilder ConnectionStr = new SqliteConnectionStringBuilder();

        static public SqliteConnection DBH;

        const string qUpdateTime = "UPDATE [Playlists] SET last_updated = DATE('now') WHERE id=\"{0}\"";

        static public void InitializeDatabase() 
        {


            List<String> QueryList = new List<String>();

            ConnectionStr.Mode       = SqliteOpenMode.ReadWriteCreate;
            ConnectionStr.Cache      = SqliteCacheMode.Private;
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
                    [last_updated] DateTime         DEFAULT CURRENT_TIMESTAMP NOT NULL,
                    [positions]    TEXT             DEFAULT """" NOT NULL
                ); 
            ");


            QueryList.Add(@"
                CREATE TABLE If Not Exists [Speeches] (
                    [id]            INTEGER PRIMARY KEY AUTOINCREMENT,
                    [playlist]      INTEGER         DEFAULT 0    NOT NULL,
                    [nickname]      NVARCHAR(16)    DEFAULT 50   NOT NULL,
                    [voice_handle]  NVARCHAR(128)   DEFAULT """" NOT NULL,
                    [voice_name]    NVARCHAR(16)    DEFAULT """" NOT NULL,
                    [voice_type]    NVARCHAR(32)    DEFAULT """" NOT NULL,
                    [voice_host]    NVARCHAR(32)    DEFAULT """" NOT NULL,
                    [gender]        NVARCHAR(8)     DEFAULT """" NOT NULL,
                    [volume]        TINYINT(3)      DEFAULT 100  NOT NULL,
                    [rate]          TINYINT(3)      DEFAULT 5    NOT NULL,
                    [pitch]         TINYINT(3)      DEFAULT 5    NOT NULL,
                    [avatar]        NVARCHAR(64)    DEFAULT """" NOT NULL,
                    [say]           TEXT            DEFAULT """" NOT NULL
                ); 
            ");

            QueryList.Add(@"
                CREATE TABLE If Not Exists [Actors] (
                    [id]            INTEGER PRIMARY KEY AUTOINCREMENT,
                    [playlist]      INTEGER         DEFAULT 0    NOT NULL,
                    [nickname]      NVARCHAR(16)    DEFAULT """" NOT NULL,
                    [voice_handle]  NVARCHAR(128)   DEFAULT """" NOT NULL,
                    [voice_name]    NVARCHAR(16)    DEFAULT """" NOT NULL,
                    [voice_type]    NVARCHAR(32)    DEFAULT """" NOT NULL,
                    [voice_host]    NVARCHAR(32)    DEFAULT """" NOT NULL,
                    [gender]        TINYINT(3)      DEFAULT 100  NOT NULL,
                    [volume]        TINYINT(3)      DEFAULT 100  NOT NULL,
                    [rate]          TINYINT(3)      DEFAULT 5    NOT NULL,
                    [pitch]         TINYINT(3)      DEFAULT 5    NOT NULL,
                    [avatar]        NVARCHAR(64)    DEFAULT """" NOT NULL
                ); 
            ");

            QueryList.Add("CREATE INDEX IF Not EXISTS idx_RosterP ON Actors (playlist);");

            //DropTables("Playlists,Actors,Speeches");
            //TruncateTables()


            DBH.Open();

            SqliteCommand MyCommand = DBH.CreateCommand();

            foreach (string query in QueryList)
            {
                MyCommand.CommandText = query;
                MyCommand.ExecuteNonQuery();
            }

            DBH.Close();



        }

        //static private void DropTables(string tableList)
        //{
        //    string[] Tables = tableList.Split(',');

        //    foreach (string tableName in Tables) {
        //        Query("DROP TABLE IF EXISTS " + tableName.Trim());
        //    }
            
        //}


        static public void Query(string query)
        {
            DBH.Open();

            SqliteCommand command = DBH.CreateCommand();

            command.CommandText = query;

            command.ExecuteNonQuery();

            DBH.Close();

        }

        static public Object Fetch(string query)
        {
            DBH.Open();

            SqliteCommand command = DBH.CreateCommand();

            command.CommandText = query;

            Object Response = command.ExecuteScalar();

            return Response;


        }

        static public QueryResult AddActor(Voice actor, int playlistId)
        {

            bool doUpdate = false;

            var qResult   = new QueryResult();

            DBH.Open();

            const string qUpdateActor = @"
                UPDATE [Actors] SET 
                    playlist        = ""{0}"",
                    nickname        = ""{1}"",
                    voice_handle    = ""{2}"",
                    voice_name      = ""{3}"",
                    voice_type      = ""{4}"",
                    voice_host      = ""{5}"",
                    gender          = ""{6}"",
                    volume          = ""{7}"",
                    rate            = ""{8}"",
                    pitch           = ""{9}"",
                    avatar          = ""{10}""
                WHERE id            = ""{11}""
            ";

            const string qInsertActor = @"
                INSERT INTO [Actors] (playlist, nickname, voice_handle, voice_name, voice_type, voice_host, gender, volume, rate, pitch, avatar)
                VALUES (""{0}"", ""{1}"", ""{2}"", ""{3}"", ""{4}"", ""{5}"", ""{6}"", ""{7}"", ""{8}"", ""{9}"", ""{10}"");

                SELECT last_insert_rowid();
            ";

            const string qValidateUpdate = @"
                SELECT nickname FROM [Actors] 
                WHERE id = ""{0}"";
            ";


            SqliteCommand cmd = DBH.CreateCommand();

            if (actor.Uid > 0)
            {
                string nickname = (string) Fetch(String.Format(qValidateUpdate, actor.Uid));

                if (nickname == actor.Nickname) doUpdate = true;

            }

            if (doUpdate)
            {

                qResult.queryMethod = QueryResult.QueryMethod.Update;

                cmd.CommandText = String.Format(qUpdateActor,
                    playlistId,
                    actor.Nickname,
                    actor.Handle,
                    actor.Id,
                    actor.GetVoiceType(),
                    actor.GetHost(),
                    actor.GetGender(),
                    actor.Volume,
                    actor.Rate,
                    actor.Pitch,
                    actor.Avatar,
                    actor.Uid);


                cmd.ExecuteNonQuery();

                qResult.recordKey   = actor.Uid;
                qResult.numAffected = 1;

            }
            else
            {

                qResult.queryMethod = QueryResult.QueryMethod.Insert;

                cmd.CommandText = String.Format(qInsertActor,
                    playlistId,
                    actor.Nickname,
                    actor.Handle,
                    actor.Id,
                    actor.GetVoiceType(),
                    actor.GetHost(),
                    actor.GetGender(),
                    actor.Volume,
                    actor.Rate,
                    actor.Pitch,
                    actor.Avatar);

                Object actor_uid = cmd.ExecuteScalar();

                actor.Uid = int.Parse(actor_uid.ToString());
                
                qResult.recordKey   = actor.Uid;
                qResult.numAffected = 1;
            }

            cmd.CommandText = String.Format(qUpdateTime, playlistId);
                
            cmd.ExecuteNonQuery();

            DBH.Close();

            return qResult;

        }


        static public void DeleteSpeech(int voiceId, int playlistId)
        {
            DBH.Open();

            const string qDeleteSpeech = @"
                DELETE FROM [Speeches] 
                WHERE id = {0} 
                    AND playlist = {1}
            ";

            SqliteCommand cmd = DBH.CreateCommand();

            cmd.CommandText = String.Format(qDeleteSpeech, voiceId, playlistId);

            cmd.ExecuteNonQuery();

            DBH.Close();

        }

        static public void DeleteActor(int voiceId, int playlistId)
        {
            DBH.Open();

            const string qDeleteActor = @"
                DELETE FROM [Actors] 
                WHERE id = {0} 
                    AND playlist = {1}
            ";

            SqliteCommand cmd = DBH.CreateCommand();

            cmd.CommandText = String.Format(qDeleteActor, voiceId, playlistId);
            Console.WriteLine(cmd.CommandText);
            cmd.ExecuteNonQuery();

            DBH.Close();

        }



        static public int AddSpeech(Voice speech, string say, int playlistId, bool forceNew=false)
        {

            DBH.Open();

            const string qUpdateSpeech = @"
                UPDATE [Speeches] SET 
                    playlist        = ""{0}"",
                    nickname        = ""{1}"",
                    voice_handle    = ""{2}"",
                    voice_name      = ""{3}"",
                    voice_type      = ""{4}"",
                    voice_host      = ""{5}"",
                    gender          = ""{6}"",
                    volume          = ""{7}"",
                    rate            = ""{8}"",
                    pitch           = ""{9}"",
                    avatar          = ""{10}"",
                    say             = ""{11}""
                WHERE id            = ""{12}""
            ";

            const string qInsertSpeech = @"
                INSERT INTO [Speeches] (playlist, nickname, voice_handle, voice_name, voice_type, voice_host, gender, volume, rate, pitch, avatar, say)
                VALUES (""{0}"", ""{1}"", ""{2}"", ""{3}"", ""{4}"", ""{5}"", ""{6}"", ""{7}"", ""{8}"", ""{9}"", ""{10}"", ""{11}"");

                SELECT last_insert_rowid();
            ";

            SqliteCommand cmd = DBH.CreateCommand();

            if (speech.Uid > 0 && !forceNew)
            {

                cmd.CommandText = String.Format(qUpdateSpeech,
                    playlistId,
                    speech.Nickname,
                    speech.Handle,
                    speech.Id,
                    speech.GetVoiceType(),
                    speech.GetHost(),
                    speech.GetGender(),
                    speech.Volume,
                    speech.Rate,
                    speech.Pitch,
                    speech.Avatar,
                    say,
                    speech.Uid);

                cmd.ExecuteNonQuery();

            }
            else
            {

                cmd.CommandText = String.Format(qInsertSpeech, 
                    playlistId,
                    speech.Nickname,
                    speech.Handle,
                    speech.Id,
                    speech.GetVoiceType(),
                    speech.GetHost(),
                    speech.GetGender(),
                    speech.Volume, 
                    speech.Rate, 
                    speech.Pitch, 
                    speech.Avatar, 
                    say);

                Object speech_uid = cmd.ExecuteScalar();

                speech.Uid = int.Parse(speech_uid.ToString());

            }

            Console.WriteLine(cmd.CommandText);

            cmd.CommandText = String.Format(qUpdateTime, playlistId);

            cmd.ExecuteNonQuery();

            DBH.Close();

            return speech.Uid;

        }

        static public List<Voice> GetSpeeches(int playlistId)
        {
            DBH.Open();

            const string qGetSpeeches = @"
                SELECT id, nickname, voice_handle, voice_name, voice_type, voice_host, gender, volume, rate, pitch, avatar, say
                FROM [Speeches] 
                WHERE playlist = ""{0}""
            ";

            List<Voice> Speeches = new List<Voice>();

            SqliteCommand cmd = DBH.CreateCommand();
            
            cmd.CommandText = String.Format(qGetSpeeches, playlistId);

            SqliteDataReader reader = cmd.ExecuteReader();

            while( reader.Read() ) {

                Voice voice = new Voice()
                {
                    Uid       = int.Parse(reader.GetString(0)),
                    Nickname  = reader.GetString(1),
                    Handle    = reader.GetString(2),
                    Id        = reader.GetString(3),
                    VoiceType = Voice.FromType(reader.GetString(4)),
                    Host      = Voice.FromHost(reader.GetString(5)),
                    Gender    = Voice.FromGender(reader.GetString(6)),
                    Volume    = int.Parse(reader.GetString(7)),
                    Rate      = int.Parse(reader.GetString(8)),
                    Pitch     = int.Parse(reader.GetString(9)),
                    Avatar    = reader.GetString(10),
                    Speech    = reader.GetString(11)
                };

                Speeches.Add(voice);

            }

            reader.Close();

            DBH.Close();

            return Speeches;


        }

        static public List<Voice> GetActors(int playlistId)
        {
            DBH.Open();

            const string qGetActors = @"
                SELECT id, nickname, voice_handle, voice_name, voice_type, voice_host, gender, volume, rate, pitch, avatar
                FROM [Actors] 
                WHERE playlist = ""{0}""
            ";

            List<Voice> Actors = new List<Voice>();

            SqliteCommand cmd = DBH.CreateCommand();

            cmd.CommandText = String.Format(qGetActors, playlistId);

            SqliteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                Voice voice = new Voice()
                {
                    Uid       = int.Parse(reader.GetString(0)),
                    Nickname  = reader.GetString(1),
                    Handle    = reader.GetString(2),
                    Id        = reader.GetString(3),
                    VoiceType = Voice.FromType(reader.GetString(4)),
                    Host      = Voice.FromHost(reader.GetString(5)),
                    Gender    = Voice.FromGender(reader.GetString(6)),
                    Volume    = int.Parse(reader.GetString(7)),
                    Rate      = int.Parse(reader.GetString(8)),
                    Pitch     = int.Parse(reader.GetString(9)),
                    Avatar    = reader.GetString(10)
                };

                Actors.Add(voice);

            }

            reader.Close();

            DBH.Close();

            return Actors;

        }

        public class QueryResult 
        {

            public QueryMethod queryMethod = QueryMethod.Unknown;

            public int numAffected = 0;

            public int recordKey = 0;

            public string data = "";

            public enum QueryMethod {
                Delete,
                Insert,
                Update,
                Unknown,
            }


        }

    }

}
