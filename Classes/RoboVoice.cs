using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using Microsoft.CognitiveServices.Speech;


namespace iYak.Classes
{
    class RoboVoice
    {

        static private Boolean isImported  = false;
        static private Boolean AzureReady  = false;
        //static private Boolean AzureEnabled = false;
        //static private Boolean GCloudReady = false;
        //static private String AzureKey     = "";
        //static private String AzureRegion  = "";

        static private SpeechConfig AzureConfig;

        static private System.Speech.Synthesis.SpeechSynthesizer LocalSynth = new System.Speech.Synthesis.SpeechSynthesizer();



        public RoboVoice() 
        {
            if (RoboVoice.isImported == false) {

                RoboVoice.InitAzure();

                RoboVoice.isImported = true;

            }

            if
            
        }


        static void InitAzure() 
        {
            if (CloudWS.Azure.key == "") return;

            // SpeechConf = SpeechConfig.FromSubscription(CloudKey, CloudRegion)

            RoboVoice.AzureConfig = SpeechConfig.FromSubscription(CloudWS.Azure.key, CloudWS.Azure.region);

            RoboVoice.AzureReady = true;

            
        }


        static public List<Voice> GetVoiceList()
        {
            var VoiceList = new List<Voice>();
            var TempVoice = new Voice();


            foreach (InstalledVoice iVoice in LocalSynth.GetInstalledVoices()) {

                TempVoice = new Voice
                {
                    Active = true,
                    Id = Voice.GenerateName(iVoice.VoiceInfo.Name),
                    Handle = iVoice.VoiceInfo.Name,
                    Gender = iVoice.VoiceInfo.Gender
                };

                VoiceList.Add(TempVoice);


                
            }


            return VoiceList;


        }
    }


    class Voice
    {
        public enum EGender
        {
            Male,
            Female,
            Neutral
        }

        public enum EVoiceType
        {
            Standard,
            Neural

        }

        public enum ELoc
        {
            Local,
            Cloud
        }


        public string Id                                  = "";
        public string Handle                              = "";
        public bool Active                                = true;
        public System.Speech.Synthesis.VoiceGender Gender = System.Speech.Synthesis.VoiceGender.NotSet;
        public EVoiceType VoiceType                       = EVoiceType.Neural;
        public ELoc Loc                                   = ELoc.Local;
        public int Rate                                   = 5;
        public int Pitch                                  = 5;
        public int Volume                                 = 5;

        static public string GenerateName(string HandleStr)
        {

            string replaceList = "VE,American,English,22kHz,Mobile,British,Desktop,Microsoft,_, ";

            foreach (string badWord in replaceList.Split(','))
            {
                HandleStr = HandleStr.Replace(badWord, "");
            }

            return HandleStr;



        }
        static public System.Speech.Synthesis.VoiceGender ToGender(string GenderStr)
        {
            switch (GenderStr)
            {
                case "male":
                    return System.Speech.Synthesis.VoiceGender.Male;
                case "female":
                    return System.Speech.Synthesis.VoiceGender.Female;
                default:
                    return System.Speech.Synthesis.VoiceGender.Neutral;
            }
        }
        public void SetVoice(String Lookup)
        {

            foreach (Voice tmpVoice in Config.Voices)
            {

                if (tmpVoice.Id == Lookup)
                {
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

    class VoiceSet
    {

        public int Uid       = 0;
        public int Volume    = 100;
        public int Rate      = 5;
        public int Pitch     = 5;
        public string Avatar = "";
        public string Say    = "";
        public Voice Voice   = new Voice();


    }
}
