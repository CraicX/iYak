//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     Utilities.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  Static class containing misc utilities
//
//
using iYak.Classes;
using iYak.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace iYak.Classes
{
    static class Utilities
    {


        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    S T A R T U P
        // ────────────────────────────────────────────────────────────────────────
        //
        // Create paths, datatables, download avatars
        //
        static public void StartUp() 
        {

            Application.DoEvents();

            //  Check/create folders
            //
            Config.RootPath    = Helpers.JoinPath(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                Config.AppName
            );

            Config.CachePath   = Helpers.JoinPath(Config.RootPath, "Cache");
            Config.AvatarsPath = Helpers.JoinPath(Config.RootPath, "Avatars");
            Config.VoicesPath  = Helpers.JoinPath(Config.RootPath, "LocalVoices.json");

            Config.ExportPath  = Helpers.JoinPath(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Config.AppName
            );

            if (!Directory.Exists(Config.CachePath))    Directory.CreateDirectory(Config.CachePath);
            if (!Directory.Exists(Config.AvatarsPath))  Directory.CreateDirectory(Config.AvatarsPath);
            if (!Directory.Exists(Config.ExportPath))   Directory.CreateDirectory(Config.ExportPath);

            //  Check/Download Avatars
            //
            Config.Avatars = LoadAvatars();

            Config.splasher.Hide();

            LoadSettings();

            //  Initialize SQLite DataTables
            //
            Datax.InitializeDatabase();

            LoadCloudCreds();

            ListAvatars();

            LoadPlaylist(Config.CurrentPlaylist.Uid);

            FillExported();

            //  Update form based on prev settings
            //
            Config.mainRef.SayBox.Text = Config.DefaultText;

            if (CloudWS.Azure.enabled) AzureVoice.Init();

            Config.Voices = RoboVoice.GetVoiceList();

            FillVoiceList(Config.Voices);

            


        }



        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::     L O A D   P L A Y L I S T
        // ────────────────────────────────────────────────────────────────────────
        //
        // Grabs Actors and Scripts from the database
        //
        static public void LoadPlaylist(int playlistId)
        {
            List<Voice> Speeches = Datax.GetSpeeches(playlistId);

            if(Speeches.Count > 0)
            {
                foreach(Voice voice in Speeches)
                {
                    Utilities.AddSpeech(voice);
                }
            }

            List<Voice> Actors = Datax.GetActors(playlistId);

            if(Actors.Count > 0)
            {
                foreach(Voice voice in Actors)
                {
                    Utilities.AddActor(voice);
                }

            }
        }



        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    L I S T   E X P O R T E D
        // ────────────────────────────────────────────────────────────────────────
        //
        // Adds Items to the main voices ViewListBox
        //
        static public List<AudioFile> ListExported(string folderPath="")
        {
            var AudioList = new List<AudioFile>();
        
            if (folderPath == "") folderPath = Config.ExportPath;

            List<string> FileList = Helpers.GlobList(folderPath);
            foreach(string filename in FileList)
            {
                if( Regex.IsMatch(filename, "\\.(wav|mp3)") ) {
                    AudioFile afile = Helpers.GetAudioFileInfo(filename);

                    AudioList.Add(afile);

                }
            }

            return AudioList;

        }

        static public void FillExported()
        {
            Config.LExport.Items.Clear();

            List<AudioFile> AudioList = ListExported();

            foreach(AudioFile afile in AudioList)
            {
                string shortname = afile.FilePath.Substring(afile.FilePath.LastIndexOf("\\")+1);

                ListViewItem litem = new ListViewItem(shortname)
                {
                    Tag                     = afile.FilePath,
                    UseItemStyleForSubItems = false
                };

                litem.SubItems.Add(afile.FileDate);

                Config.LExport.Items.Add(litem);



            }
        }


        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    F I L L    V O I C E L I S T
        // ────────────────────────────────────────────────────────────────────────
        //
        // Adds Items to the main voices ViewListBox
        //
        static public void FillVoiceList(List<Voice> VList)
        {
            //  Set Filter Options
            Config.mainRef.RefreshFilters();
            Config.LVoiceSelect.Rows.Clear();

            Color FontColor;
            string BackCol;

            
            var HostMap = new Dictionary<Voice.EHost, int>() {
                    { Voice.EHost.AWS, 0 },
                    { Voice.EHost.Azure, 1 },
                    { Voice.EHost.GCloud, 3 },
                    { Voice.EHost.Local, 5 }
                };

            var LocaleMap = new Dictionary<string, int>()
            {
                {"au", 6},
                {"ca", 7},
                {"cn", 8},
                {"gb", 9},
                {"ie", 10},
                {"in", 11},
                {"us", 12},
            };

            
            foreach (Voice vItem in VList)
            {
                if (vItem.Nickname == "") continue;

                if (Config.VFilter.male && vItem.Gender == Voice.EGender.Male) continue;
                if (Config.VFilter.female && vItem.Gender == Voice.EGender.Female) continue;
                if (Config.VFilter.local && vItem.Host == Voice.EHost.Local) continue;
                if (Config.VFilter.azure && vItem.Host == Voice.EHost.Azure) continue;
                if (Config.VFilter.aws && vItem.Host == Voice.EHost.AWS) continue;
                if (Config.VFilter.gcloud && vItem.Host == Voice.EHost.GCloud) continue;

                string CountryCode = GetCountryFromLocale(vItem.Locale);

                DataGridViewRow dgvr = new DataGridViewRow() { };
                DataGridViewImageCell cGender = new DataGridViewImageCell() { };
                DataGridViewTextBoxCell cName = new DataGridViewTextBoxCell()
                {
                    Value = vItem.Nickname,
                };

                cName.Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));


                DataGridViewImageCell cType = new DataGridViewImageCell() {
                    Value = Config.mainRef.ImgListVS.Images[vItem.VoiceType == Voice.EVoiceType.Neural ? 13 : 14],
                };
                DataGridViewImageCell cLoc = new DataGridViewImageCell() {
                    Value = Config.mainRef.ImgListVS.Images[CountryCode + ".png"],
                };
                DataGridViewImageCell cHost = new DataGridViewImageCell()
                {
                    Value = Config.mainRef.ImgListVS.Images[HostMap[vItem.Host]],
                };
                DataGridViewTextBoxCell cGenderHidden = new DataGridViewTextBoxCell()
                {
                    Value = Voice.GetGender(vItem.Gender),
                };
                DataGridViewTextBoxCell cLocHidden = new DataGridViewTextBoxCell()
                {
                    Value = CountryCode,
                };
                DataGridViewTextBoxCell cTypeHidden = new DataGridViewTextBoxCell()
                {
                    Value = Voice.GetVoiceType(vItem.VoiceType),
                };
                DataGridViewTextBoxCell cHostHidden = new DataGridViewTextBoxCell()
                {
                    Value = vItem.Host,
                };

                if (vItem.Gender == Voice.EGender.Male)
                {
                    FontColor     = Color.Blue;
                    BackCol       = "SkyBlue";
                    cGender.Value = Config.mainRef.ImgListVS.Images[4];

                }
                else
                {
                    FontColor     = Color.MediumVioletRed;
                    BackCol       = "Plum";
                    cGender.Value = Config.mainRef.ImgListVS.Images[2];
                }

                dgvr.DefaultCellStyle.BackColor = Color.FromName(BackCol);
                dgvr.DefaultCellStyle.ForeColor = FontColor;


                dgvr.Cells.AddRange( new DataGridViewCell[]{cName, cGender, cLoc, cHost, cType,  cGenderHidden, cLocHidden, cTypeHidden, cHostHidden} );

                Config.LVoiceSelect.Rows.Add(dgvr);
            }

            Config.LVoiceSelect.Sort(Config.LVoiceSelect.Columns[0], System.ComponentModel.ListSortDirection.Ascending);


        }



        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    L I S T   A V A T A R S
        // ────────────────────────────────────────────────────────────────────────
        //
        // Adds the avatar objects to the flow panel
        //
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




        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    D O W N L O A D    A V A T A R S
        // ────────────────────────────────────────────────────────────────────────
        //
        // Downloads avatars from the web to the local Avatars folder
        //
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



        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    L O A D   S E T T I N G S
        // ────────────────────────────────────────────────────────────────────────
        //
        // Loads User Settings
        //
        static public void LoadSettings()
        {
            String iniFile = Helpers.JoinPath(Config.RootPath, "Settings.ini");

            if (!File.Exists(iniFile)) return;

            StreamReader sr = new StreamReader(iniFile);

            while (!sr.EndOfStream)
            {
                String lineProperty = sr.ReadLine();

                if (lineProperty.Contains('='))
                {

                    String[] pieces = lineProperty.Split('=');

                    pieces[0] = pieces[0].Trim();
                    pieces[1] = pieces[1].Trim();

                    switch( pieces[0] ) {

                        case "ExportPath":
                            if (Directory.Exists(pieces[1])) Config.ExportPath = pieces[1];
                            break;

                    }
                }

            }

            sr.Close();

            string DSpeechFile = Helpers.JoinPath(Config.RootPath, "DefaultSpeech.txt");

            if( File.Exists(DSpeechFile) ) {

                Config.DefaultText = File.ReadAllText(@DSpeechFile).Trim();

            }

        }



        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    S A V E   S E T T I N G S
        // ────────────────────────────────────────────────────────────────────────
        //
        // Saves User Settings
        //
        static public void SaveSettings()
        {
            String iniFile = Helpers.JoinPath(Config.RootPath, "Settings.ini");

            StreamWriter sw = new StreamWriter(iniFile, false);

            sw.WriteLine("ExportPath = " + Config.ExportPath);

            sw.Close();

            string DSpeechFile = Helpers.JoinPath(Config.RootPath, "DefaultSpeech.txt");

            File.WriteAllText(DSpeechFile, Config.DefaultText);

        }



        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    L O A D   C L O U D   C R E D S
        // ────────────────────────────────────────────────────────────────────────
        //
        // Loads Webservice credentials from INI file 
        //
        static public void LoadCloudCreds()
        {
            String iniFile = Helpers.JoinPath(Config.RootPath, "Cloud.ini");

            if (!File.Exists(iniFile)) return;

            StreamReader sr = new StreamReader(iniFile);

            while (!sr.EndOfStream) {
                String lineProperty = sr.ReadLine();

                if (lineProperty.Contains('=') ) {

                    String[] pieces = lineProperty.Split('=');

                    pieces[0] = pieces[0].Trim();
                    pieces[1] = pieces[1].Trim();

                    switch( pieces[0] ) {
                        case "AzureKey":
                            CloudWS.Azure.key = pieces[1];
                            break;

                        case "AzureRegion":
                            CloudWS.Azure.region = pieces[1];
                            break;

                        case "AzureEnabled":
                            CloudWS.Azure.enabled = (pieces[1] == "1");
                            break;

                        case "AWSKey":
                            CloudWS.AWS.key = pieces[1];
                            break;

                        case "AWSRegion":
                            CloudWS.AWS.region = pieces[1];
                            break;

                        case "AWSEnabled":
                            CloudWS.AWS.enabled = (pieces[1] == "1");
                            break;

                        case "GCloudKey":
                            CloudWS.GCloud.key = pieces[1];
                            break;

                        case "GCloudRegion":
                            CloudWS.GCloud.region = pieces[1];
                            break;

                        case "GCloudEnabled":
                            CloudWS.GCloud.enabled = (pieces[1] == "1");
                            break;

                    }

                }

            }

            sr.Close();

        }


        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    S A V E   C L O U D   C R E D S
        // ────────────────────────────────────────────────────────────────────────
        //
        // Saves Webservice credentials to INI file 
        //
        static public void SaveCloudCreds()
        {
            String iniFile = Helpers.JoinPath(Config.RootPath, "Cloud.ini");

            StreamWriter sw = new StreamWriter(iniFile, false);

            sw.WriteLine("AzureKey = "          + CloudWS.Azure.key);
            sw.WriteLine("AzureRegion = "       + CloudWS.Azure.region);
            sw.WriteLine("AzureEnabled = "      + (CloudWS.Azure.enabled ? "1":"0"));

            sw.WriteLine("AWSKey = "            + CloudWS.AWS.key);
            sw.WriteLine("AWSRegion = "         + CloudWS.AWS.region);
            sw.WriteLine("AWSEnabled = "        + (CloudWS.AWS.enabled ? "1" : "0"));

            sw.WriteLine("GCloudKey = "         + CloudWS.GCloud.key);
            sw.WriteLine("GCloudRegion = "      + CloudWS.GCloud.region);
            sw.WriteLine("GCloudEnabled = "     + (CloudWS.GCloud.enabled ? "1" : "0"));

            sw.Close();

        }



        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    ADD   ACTOR
        // ────────────────────────────────────────────────────────────────────────
        //
        // Creates a new actor control in the flow panel
        //
        static public void AddActor(Voice _voice, bool autoSave=false)
        {

            Voice voice = (autoSave) ? _voice.Copy() : _voice;
            
            RoboActor actor = new RoboActor(voice, RoboActor.ControlType.Actor)
            {
                Width  = 64,
                Height = 80,
                Margin = new Padding(1)
            };


            Config.FActors.Controls.Add(actor);

            if(autoSave) Datax.AddActor( voice, Config.CurrentPlaylist.Uid );

        }


        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    ADD   SPEECH
        // ────────────────────────────────────────────────────────────────────────
        //
        // Creates a new voice control in the flow panel for speech/scripts
        //
        static public void AddSpeech(Voice voice, string speech)
        {
            RoboActor roboA = new RoboActor(
                0,
                speech,
                voice
            );

            
            Config.FScripts.Controls.Add(roboA);
            Datax.AddSpeech(voice, speech, Config.CurrentPlaylist.Uid);
            RoboActor.Activate(roboA);


        }

        static public void AddSpeech(Voice voice, bool autoSave=false)
        {
            RoboActor roboA = new RoboActor(
                0,
                voice.Speech,
                voice
            );


            Config.FScripts.Controls.Add(roboA);
            RoboActor.Activate(roboA);

            if(autoSave) Datax.AddSpeech(voice, voice.Speech, Config.CurrentPlaylist.Uid);

        }


        static public string GetCountryFromLocale(string locale)
        {

            if (locale.Length < 2) return "us";

            var CountryMap = new Dictionary<string, string>()
            {
                {"at", "at"},
                {"au", "au"},
                {"bg", "bg"},
                {"br", "br"},
                {"ca", "ca"},
                {"ch", "ch"},
                {"cn", "cn"},
                {"cz", "cz"},
                {"de", "de"},
                {"dk", "dk"},
                {"eg", "eg"},
                {"es", "es"},
                {"fi", "fi"},
                {"fr", "fr"},
                {"gb", "gb"},
                {"gr", "gr"},
                {"hk", "hk"},
                {"hr", "hr"},
                {"hu", "hu"},
                {"id", "id"},
                {"ie", "ie"},
                {"it", "it"},
                {"jp", "jp"},
                {"kr", "kr"},
                {"mx", "mx"},
                {"my", "my"},
                {"nl", "nl"},
                {"no", "no"},
                {"pl", "pl"},
                {"pt", "pt"},
                {"ro", "ro"},
                {"ru", "ru"},
                {"sa", "sa"},
                {"se", "se"},
                {"si", "si"},
                {"sk", "sk"},
                {"th", "th"},
                {"tr", "tr"},
                {"tw", "tw"},
                {"us", "us"},
                {"vn", "vn"},
                {"states", "us"},
                {"ireland", "ie"},
                {"india", "in"},
                {"kingdom", "gb"},
                {"britain", "gb"},
                {"australia", "au"},
                {"china", "cn"},
            };

            locale = locale.ToLower();

            string lastTwo = locale.Substring(locale.Length - 2);

            foreach (KeyValuePair<string, string> entry in CountryMap)
            {
                if (lastTwo == entry.Key) return entry.Value;
            }

            foreach (KeyValuePair<string, string> entry in CountryMap)
            {
                if (locale.Contains(entry.Key)) return entry.Value;
            }

            return "us";
        }

    }

}
