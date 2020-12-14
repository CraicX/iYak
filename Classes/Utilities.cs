using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iYak.Classes;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using iYak.Controls;
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

            Config.CachePath   = Helpers.JoinPath(Config.RootPath, "Cache");
            Config.AvatarsPath = Helpers.JoinPath(Config.RootPath, "Avatars");
            Config.VoicesPath  = Helpers.JoinPath(Config.RootPath, "voices.json");

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

            ListAvatars();

        }

        static public void FillVoiceList(List<Voice> VList) 
        {

            int imgNum;
            string tmpLoc;
            Color FontColor;
            string BackCol;

            foreach (Voice vItem in VList)
            {

                if (vItem.Gender == System.Speech.Synthesis.VoiceGender.Male) {
                    imgNum    = 0;
                    FontColor = Color.Blue;
                    BackCol   = (vItem.VoiceType == Voice.EVoiceType.Neural) ? "LightSteelBlue" : "SkyBlue";

                } else {
                    imgNum    = 1;
                    FontColor = Color.MediumVioletRed;
                    BackCol   = (vItem.VoiceType == Voice.EVoiceType.Neural ? "Plum" : "LightSteelBlue");
                }

                ListViewItem VoiceItem            = new ListViewItem(vItem.Id, imgNum) { 
                    UseItemStyleForSubItems = false
                };

                if (vItem.Host == Voice.EHost.Azure) {
                    VoiceItem.BackColor = Color.FromName(BackCol);
                    tmpLoc = "C";
                }
                tmpLoc = "-";

                VoiceItem.SubItems.Add(tmpLoc);

                ListViewItem.ListViewSubItem tmpHost = new ListViewItem.ListViewSubItem(VoiceItem, Voice.GetHost(vItem.Host))
                {
                    ForeColor = Color.Black
                };

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
                VoiceItem.SubItems[0].Font      = new Font(VoiceItem.SubItems[0].Font, VoiceItem.SubItems[1].Font.Style | FontStyle.Bold);
                VoiceItem.SubItems[2].Font      = new Font("Microsoft Sans Serif", 10, GraphicsUnit.Pixel);
                VoiceItem.SubItems[2].ForeColor = Color.MidnightBlue;

                Config.LVoices.Items.Add(VoiceItem);
            }

        }


        static public void ListAvatars()
        {

            if (Config.Avatars.Count <= 0) return;

            List<string> AvatarList = Config.Avatars;

            Helpers.Shuffle(ref AvatarList);


            foreach (string avatarPath in AvatarList)
            {
                string ATag = "";

                if (avatarPath.Contains('\\')) {
                    ATag = avatarPath.Substring(avatarPath.LastIndexOf('\\') + 1);
                }


                PictureBox anAvatar = new PictureBox()
                {
                    Tag         = ATag,
                    AutoSize    = false,
                    Width       = 48,
                    Height      = 48,
                    Margin      = new Padding(1, 1, 1, 1),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor   = System.Drawing.Color.Transparent,
                    SizeMode    = PictureBoxSizeMode.StretchImage,
                    Image       = Helpers.LoadImage(avatarPath),
                    Cursor      = System.Windows.Forms.Cursors.Hand
                };


                Config.FAvatars.Controls.Add(anAvatar);

                anAvatar.Click += new EventHandler(Avatar_Click);


            }


        }

        static void Avatar_Click( object sender, EventArgs e )
        {
            PictureBox cAvatar         = sender as PictureBox;
            Config.CurrentFace.Image   = cAvatar.Image;
            Config.CurrentFace.Tag     = cAvatar.Tag;
            Config.CurrentVoice.Avatar = cAvatar.Tag.ToString();
            
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

            String[] avatarList = System.IO.File.ReadAllLines(@"AvatarList.txt");

            int numAvatar = avatarList.Length;

            int curAvatar = 1;

            foreach (String avatarPath in avatarList) { 

                Config.splasher.ShowStatus("Downloading avatar image (" + curAvatar + " / " + numAvatar + ")", curAvatar, numAvatar);
                
                Helpers.DownloadImage(avatarPath, Config.AvatarsPath);

                curAvatar++;

            }

            List<string> TempAvatars = Helpers.GlobList(Config.AvatarsPath);

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

        static public void AddActor(Voice voice)
        {
            RoboActor actor = new RoboActor(voice, RoboActor.ControlType.Actor)
            {
                Width = 64,
                Height = 80,
                Margin = new Padding(1)
            };


            Config.FActors.Controls.Add(actor);

        }

        static public void AddSpeech(Voice voice, string speech)
        {
            RoboActor roboA = new RoboActor(
                0,
                speech,
                voice
            );

            
            Config.FScripts.Controls.Add(roboA);
            RoboActor.Activate(roboA);


        }

    }

}
