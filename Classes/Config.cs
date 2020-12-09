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
        static public string AppName       = "iYak";
        static public List<Voice> Voices   = new List<Voice>();
        static public List<String> Avatars = new List<String>();


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




    }

    class CloudWS
    {
        static public class Azure
        {
            static public String key      = "";
            static public String region   = "";
            static public Boolean enabled = false;
            
        }
    }

    class Playlist
    {
        public int Uid            = 0;
        public string Name        = "";
        public string Created     = "";
        public string LastUpdated = "";

    }

    


}
