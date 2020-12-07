using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iYak.Classes;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace iYak.Classes
{
    class Utilities
    {


        /**
         *  START UP
         *  
         *  Create Paths, Datatables and download Avatars
         */
        static public void StartUp() {

            Application.DoEvents();


            //
            //  Check/create folders
            //
            Config.RootPath = Helpers.JoinPath(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                Config.AppName
            );

            Config.CachePath = Helpers.JoinPath(Config.RootPath, "Cache");
            
            Config.AvatarsPath = Helpers.JoinPath(Config.RootPath, "Avatars");
            
            Config.VoicesPath = Helpers.JoinPath(Config.RootPath, "voices.json");

            if (!Directory.Exists(Config.CachePath))    Directory.CreateDirectory(Config.CachePath);
            if (!Directory.Exists(Config.AvatarsPath))  Directory.CreateDirectory(Config.AvatarsPath);

            //
            //  Check/Download Avatars
            //
            Config.Avatars = LoadAvatars();

            Config.splasher.Hide();

            //
            //  Initialize SQLite DataTables
            //
            Datax.InitializeDatabase();

        }


        /**
         * LOAD AVATARS
         * 
         * Builds a list of available avatars to use
         * Downloads new ones if necessary
         * 
         */
        static public List<String> LoadAvatars() {

            List<string> TempAvatars = Helpers.GlobList(Config.AvatarsPath);

            if (TempAvatars.Count == 0) TempAvatars = DownloadAvatars();

            return TempAvatars;

        }


        /**
         * DOWNLOAD AVATARS
         * 
         * Downloads avatars from the web to the local Avatars folder
         * 
         */ 
        static public List<String> DownloadAvatars()
        {

            List<String> TempAvatars = new List<String>();

            String[] avatarList = System.IO.File.ReadAllLines(@"AvatarList.txt");

            int numAvatar = avatarList.Length;

            int curAvatar = 1;

            foreach (String avatarPath in avatarList) { 

                Config.splasher.ShowStatus("Downloading avatar image (" + curAvatar + " / " + numAvatar + ")", curAvatar, numAvatar);
                
                Helpers.DownloadImage(avatarPath, Config.AvatarsPath);

                curAvatar++;

            }


            return TempAvatars;


        }

    }

}
