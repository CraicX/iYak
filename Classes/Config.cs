﻿//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     Config.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  Contains global vars and config options
//
//
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
        static public FilterOpts VFilter        = new FilterOpts();
        static public string DefaultText        = "";

        //
        //  Define Paths
        //
        static public string RootPath    = "";
        static public string CachePath   = "";
        static public string AvatarsPath = "";
        static public string VoicesPath  = "";
        static public string ExportPath  = "";


        //
        //  Define Controls
        //
        static public System.Windows.Forms.DataGridView LVoiceSelect;
        static public System.Windows.Forms.ListView LExport;
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
            static public string key    = "";
            static public string region = "";
            static public string token  = "";
            static public bool enabled  = false;

        }

        static public class AWS
        {
            static public string accessKey     = "";
            static public string secretKey     = "";
            static public string region        = "";
            static public bool enabled         = false;
        }

        static public class GCloud
        {
            static public string key    = "";
            static public string region = "";
            static public bool enabled  = false;

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

    
    class FilterOpts
    {
        public bool male   = true;
        public bool female = true;
        public bool local  = true;
        public bool aws    = true;
        public bool gcloud = true;
        public bool azure  = true;

    }


    //
    // ────────────────────────────────────────────────────────────────────────
    //   :::::: A U D I O   F I L E
    // ────────────────────────────────────────────────────────────────────────
    //
    public class AudioFile
    {
        public string FilePath = "";
        public int FileSize    = 0;
        public string FileDate = "";

    }

}
