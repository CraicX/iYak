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
    public class RoboVoice
    {

        static private Boolean isImported  = false;
        //static private Boolean AzureReady  = false;
        //static private Boolean AzureEnabled = false;
        //static private Boolean GCloudReady = false;
        //static private String AzureKey     = "";
        //static private String AzureRegion  = "";

        static private SpeechConfig AzureConfig;

        static private System.Speech.Synthesis.SpeechSynthesizer LocalSynth = new System.Speech.Synthesis.SpeechSynthesizer();

        public Voice voice = new Voice();


        public RoboVoice() 
        {
            if (RoboVoice.isImported == false) {

                RoboVoice.InitAzure();

                RoboVoice.isImported = true;

            }
        }

        public RoboVoice(Voice _voice)
        {
            this.voice = _voice;
        }


        public bool Say(string sayText) 
        {
            if (voice.Handle == null || voice.Handle == "") return false;

            System.Speech.Synthesis.SpeechSynthesizer tmpVoice = new System.Speech.Synthesis.SpeechSynthesizer();
            tmpVoice.SetOutputToDefaultAudioDevice();
            tmpVoice.SelectVoice(voice.Handle);
            tmpVoice.Volume = voice.Volume;
            tmpVoice.Rate = (voice.Rate * 2) - 10;
            tmpVoice.SpeakAsync(sayText);

            return true;

        }

        static void InitAzure() 
        {
            if (CloudWS.Azure.key == "") return;

            // SpeechConf = SpeechConfig.FromSubscription(CloudKey, CloudRegion)

            RoboVoice.AzureConfig = SpeechConfig.FromSubscription(CloudWS.Azure.key, CloudWS.Azure.region);

           // if (!RoboVoice.AzureReady) RoboVoice.AzureReady = true;

            
        }


        static public List<Voice> GetVoiceList()
        {
            var VoiceList = new List<Voice>();
            var TempVoice = new Voice();


            foreach (InstalledVoice iVoice in LocalSynth.GetInstalledVoices()) {

                TempVoice = new Voice
                {
                    Active = true,
                    Id     = Voice.GenerateName(iVoice.VoiceInfo.Name),
                    Handle = iVoice.VoiceInfo.Name,
                    Gender = iVoice.VoiceInfo.Gender,
                    Host   = Voice.EHost.Local
                };

                VoiceList.Add(TempVoice);


                
            }


            return VoiceList;


        }
    }



    //
    // ────────────────────────────────────────────────────────────────────────
    //   :::::: V O I C E 
    // ────────────────────────────────────────────────────────────────────────
    //
    public class Voice
    {

        public string Id       = "";
        public string Handle   = "";
        public string Avatar   = "";
        public string Nickname = "";
        public bool Active     = true;
        public int Rate        = 5;
        public int Pitch       = 5;
        public int Volume      = 100;

        public System.Speech.Synthesis.VoiceGender Gender = System.Speech.Synthesis.VoiceGender.NotSet;
        public EVoiceType VoiceType                       = EVoiceType.Standard;
        public EHost Host                                 = EHost.Local;

        public Voice Copy()
        {
            Voice _voice = new Voice()
            {
                Id        = this.Id,
                Handle    = this.Handle,
                Avatar    = this.Avatar,
                Nickname  = this.Nickname,
                Active    = this.Active,
                Rate      = this.Rate,
                Pitch     = this.Pitch,
                Volume    = this.Volume,
                Gender    = this.Gender,
                VoiceType = this.VoiceType,
                Host      = this.Host
            };

            return _voice;
        }

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

        public enum EHost
        {
            Local,
            Azure,
            GCloud,
            AWS
        }

        static public string GetGender(System.Speech.Synthesis.VoiceGender which)
        {
            if (which == VoiceGender.Male)      return "Male";
            if (which == VoiceGender.Female)    return "Female";
            if (which == VoiceGender.Neutral)   return "Neu";

            return "unknown";
            
        }

        static public string GetType (EVoiceType which)
        {
            if (which == EVoiceType.Standard) return "Standard";
            if (which == EVoiceType.Neural)   return "Neural";

            return "unknown";

        }

        static public string GetHost(EHost which)
        {
            if (which == EHost.Azure)  return "Azure";
            if (which == EHost.GCloud) return "GCloud";
            if (which == EHost.AWS)    return "AWS";
            if (which == EHost.Local)  return "Local";

            return "unknown";

        }



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
                    Host      = tmpVoice.Host;
                    Rate      = tmpVoice.Rate;
                    Pitch     = tmpVoice.Pitch;
                    Volume    = tmpVoice.Volume;
                    Nickname  = tmpVoice.Id;
                }


            }

        }


    }

    //
    // ────────────────────────────────────────────────────────────────────────
    //   :::::: V O I C E   S E T
    // ────────────────────────────────────────────────────────────────────────
    //
    public class VoiceSet
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
