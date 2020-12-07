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

        static public string AppName     = "iYak";
        static public List<Voice> Voices = new List<Voice>();
        static public List<String> Avatars = new List<String>();


        //
        //  Define Paths
        //
        static public string RootPath    = "";
        static public string CachePath   = "";
        static public string AvatarsPath = "";
        static public string VoicesPath  = "";


    }

    class Playlist
    {
        public int Uid            = 0;
        public string Name        = "";
        public string Created     = "";
        public string LastUpdated = "";

    }

    class Voice
    {
        public enum EGender {
            Male,
            Female,
            Neutral
        }

        public enum EVoiceType {
            Standard,
            Neural

        }

        public enum ELoc {
            Local,
            Cloud
        }


        public string Id            = "";
        public string Handle        = "";
        public string Active        = "";
        public EGender Gender       = EGender.Neutral;
        public EVoiceType VoiceType = EVoiceType.Neural;
        public ELoc Loc             = ELoc.Local;
        public int Rate             = 5;
        public int Pitch            = 5;
        public int Volume           = 5;
        
        public string GenerateName(string HandleStr) {

            string replaceList = "VE,American,English,22kHz,Mobile,British,Desktop,Microsoft,_, ";

            foreach (string badWord in replaceList.Split(',')) {
                HandleStr = HandleStr.Replace(badWord, "");
            }

            return HandleStr;



        }

        public void SetVoice(String Lookup) {

            foreach (Voice tmpVoice in Config.Voices) {

                if (tmpVoice.Id == Lookup) {
                    Id        = tmpVoice.Id;
                    Handle    = tmpVoice.Handle;
                    Gender    = tmpVoice.Gender;
                    VoiceType = tmpVoice.VoiceType;
                    Loc       = tmpVoice.Loc;
                    Rate      = tmpVoice.Rate;
                    Pitch     = tmpVoice.Pitch;
                    Volume    = tmpVoice.Volume;
                }
                

            }

        }
        

    }

    class VoiceSet {

        public int Uid       = 0;
        public int Volume    = 100;
        public int Rate      = 5;
        public int Pitch     = 5;
        public string Avatar = "";
        public string Say    = "";
        public Voice Voice   = new Voice();

        
    }


}
