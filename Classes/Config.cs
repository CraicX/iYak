using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iYak.Classes
{
    class Config
    {

        static public ViewSplash splasher;
        static public Settings frmSettings;
        static public string AppName            = "iYak";
        static public List<Voice> Voices        = new List<Voice>();
        static public List<String> Avatars      = new List<String>();
        static public Voice CurrentVoice        = new Voice();
        static public Playlist CurrentPlaylist  = new Playlist();


        //
        //  Define Paths
        //
        static public string RootPath    = "";
        static public string CachePath   = "";
        static public string AvatarsPath = "";
        static public string VoicesPath  = "";

        //
        //  Define Controls
        //
        static public System.Windows.Forms.ListView LVoices;
        static public System.Windows.Forms.FlowLayoutPanel FAvatars;
        static public System.Windows.Forms.FlowLayoutPanel FActors;
        static public System.Windows.Forms.PictureBox CurrentFace;
        static public System.Windows.Forms.FlowLayoutPanel FScripts;
        static public Main mainRef;




    }

    class CloudWS
    {
        static public class Azure
        {
            static public string key      = "";
            static public string region   = "";
            static public bool enabled = false;

            
        }
    }

    class Playlist
    {
        public int Uid              = 0;
        public string Name          = "";
        public string Created       = "";
        public string positions     = "";
        public string LastUpdated   = "";

    }

    


}
