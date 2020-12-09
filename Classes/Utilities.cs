﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iYak.Classes;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
namespace iYak.Classes
{
    class Utilities
    {


        /**
         *  START UP
         *  
         *  Create Paths, Datatables and download Avatars
         */
        static public void StartUp() 
        {

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

            LoadCloudCreds();

            Config.Voices = RoboVoice.GetVoiceList();

            FillVoiceList(Config.Voices);

        }

        static public void FillVoiceList(List<Voice> VList) 
        {

            int imgNum       = 0; 
            string tmpLoc    = "";
            Color FontColor;
            string BackCol   = "";

            foreach (Voice vItem in VList)
            {

                Console.WriteLine(vItem.Handle);
                if (vItem.Gender == System.Speech.Synthesis.VoiceGender.Male) {
                    imgNum = 0;
                    FontColor = Color.Blue;
                    BackCol = (vItem.VoiceType == Voice.EVoiceType.Neural) ? "LightSteelBlue" : "SkyBlue";

                } else {
                    imgNum = 1;
                    FontColor = Color.MediumVioletRed;
                    BackCol = (vItem.VoiceType == Voice.EVoiceType.Neural ? "Plum" : "LightSteelBlue");
                }

                ListViewItem VoiceItem = new ListViewItem(vItem.Id, imgNum);
                VoiceItem.UseItemStyleForSubItems = false;
                //~ VoiceItem.ForeColor = Color.FromName(FontColor);

                if (vItem.Host == Voice.EHost.Azure) {
                    VoiceItem.BackColor = Color.FromName(BackCol);
                    tmpLoc = "C";
                }
                tmpLoc = "-";

                VoiceItem.SubItems.Add(tmpLoc);

                ListViewItem.ListViewSubItem tmpHost = new ListViewItem.ListViewSubItem(VoiceItem, Voice.GetHost(vItem.Host));

                tmpHost.ForeColor = Color.Black;

                VoiceItem.SubItems.Add(tmpHost);

                switch( vItem.Gender) {
                    case System.Speech.Synthesis.VoiceGender.Male:
                        VoiceItem.Group = Config.LVoices.Groups[0];
                        break;

                    case System.Speech.Synthesis.VoiceGender.Female:
                        VoiceItem.Group = Config.LVoices.Groups[1];
                        break;

                    default:
                        VoiceItem.Group = Config.LVoices.Groups[2];
                        break;
                }
                VoiceItem.SubItems[0].ForeColor = FontColor;
                VoiceItem.SubItems[0].Font = new Font(VoiceItem.SubItems[0].Font, VoiceItem.SubItems[1].Font.Style | FontStyle.Bold);
                VoiceItem.SubItems[2].Font = new Font("Microsoft Sans Serif", 10, GraphicsUnit.Pixel);
                VoiceItem.SubItems[2].ForeColor = Color.MidnightBlue;

                Config.LVoices.Items.Add(VoiceItem);
            }

        }


        /**
         * LOAD AVATARS
         * 
         * Builds a list of available avatars to use
         * Downloads new ones if necessary
         * 
         */
        static public List<String> LoadAvatars() 
        {

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

        static public void LoadCloudCreds()
        {
            String iniFile = Helpers.JoinPath(Config.RootPath, "Cloud.ini");

            if (!File.Exists(iniFile)) return;

            StreamReader sr = new StreamReader(iniFile);

            while (!sr.EndOfStream) {
                String lineProperty = sr.ReadLine();
                Console.WriteLine(lineProperty);

                if (lineProperty.Contains('=') ) {

                    String[] pieces = lineProperty.Split('=');

                    switch( pieces[0].Trim() ) {
                        case "AzureKey":
                            CloudWS.Azure.key = pieces[1];
                            break;

                        case "AzureRegion":
                            CloudWS.Azure.region = pieces[1];
                            break;

                        case "AzureEnabled":
                            CloudWS.Azure.enabled = (pieces[1].Trim() == "1");
                            break;

                    }

                }

            }

            sr.Close();



        }

        static public void SaveCloudCreds()
        {
            String iniFile = Helpers.JoinPath(Config.RootPath, "Cloud.ini");

            StreamWriter sw = new StreamWriter(iniFile, false);

            sw.WriteLine("AzureKey = "       + CloudWS.Azure.key);
            sw.WriteLine("AzureRegion = "    + CloudWS.Azure.region);
            sw.WriteLine("AzureEnabled = "   + (CloudWS.Azure.enabled ? "1":"0"));

            sw.Close();

        }

    }

}
