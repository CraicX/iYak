using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using System.Text.RegularExpressions;


namespace iYak.Classes
{
    public class RoboVoice
    {

        static private Boolean isImported   = false;
        static public Boolean AzureReady   = false;
        static public Boolean AzureEnabled = false;
        //static private Boolean GCloudReady = false;


        public Voice voice = new Voice();


        public RoboVoice()
        {
                

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
            tmpVoice.Rate   = (voice.Rate * 2) - 10;
            tmpVoice.SpeakAsync(sayText);

            return true;

        }

        public static AudioFile ExportSpeech(Voice voice, string FileName="")
        {
            if (FileName == "") FileName = RoboVoice.GenerateFileName(voice);
            string FilePath = Helpers.JoinPath(Config.ExportPath, FileName);

            if (voice.Host == Voice.EHost.Local)        LocalVoice.Export(voice, FileName);
            else if (voice.Host == Voice.EHost.Azure)   AzureVoice.Export(voice, FileName);

            AudioFile afile = Helpers.GetAudioFileInfo(FilePath);

            return afile;

        }
        


        static public string GenerateFileName(Voice voice) 
        {

            string FileName = voice.Nickname + "_";

            string words    = Regex.Replace(voice.Speech, "[^a-zA-Z]", " ").Replace("  ", " ");
            
            if (words.Length > 20) words = words.Substring(0, 20);

            string timestamp = DateTime.UtcNow.Ticks.ToString();

            timestamp = timestamp.Substring(timestamp.Length - 5);

            FileName += words + "_" + timestamp;

            FileName = FileName.Replace(" ", "-").ToLower();

            if (FileName.Substring(FileName.Length - 1) == "-") FileName = FileName.Substring(0, FileName.Length - 2);

            return FileName;

        }

        static public void InitAzure()
        {

            
            //if (CloudWS.Azure.key == "") return;
            //AzureConfig = SpeechConfig.FromSubscription(CloudWS.Azure.key, CloudWS.Azure.region);
            //AzureSynth = new Microsoft.CognitiveServices.Speech.SpeechSynthesizer(AzureConfig);
            //RoboVoice.AzureConfig = SpeechConfig.FromSubscription(CloudWS.Azure.key, CloudWS.Azure.region);
            //RoboVoice.AzureReady = true;

            

        }


        static public List<Voice> GetVoiceList()
        {
            List<Voice> VoiceList = new List<Voice>();

            List<Voice> TempList = LocalVoice.GetVoiceList();
            foreach( Voice TempVoice in TempList)
            {
                VoiceList.Add(TempVoice);
            }

            return VoiceList;


        }

        
       

        static public void RefreshVoiceList()
        {
           // LocalSynth = new System.Speech.Synthesis.SpeechSynthesizer();
            //GetVoiceList();

        }



    }



    //
    // ────────────────────────────────────────────────────────────────────────
    //   :::::: V O I C E 
    // ────────────────────────────────────────────────────────────────────────
    //
    public class Voice
    {

        public int Uid         = 0;
        public string Id       = "";
        public string Handle   = "";
        public string Avatar   = "";
        public string Nickname = "";
        public string Speech   = "";
        public bool Active     = true;
        public int Rate        = 5;
        public int Pitch       = 5;
        public int Volume      = 100;

        public EGender Gender       = EGender.NotSet;
        public EVoiceType VoiceType = EVoiceType.Standard;
        public EHost Host           = EHost.Local;

        public Voice Copy()
        {
            Voice _voice = new Voice()
            {
                Uid       = 0,
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
                Host      = this.Host,
                Speech    = this.Speech
            };

            return _voice;
        }

        public enum EGender
        {
            Male,
            Female,
            Neutral,
            NotSet
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

        public string GetGender() { return Voice.GetGender(this.Gender); }
        public string GetType() {   return Voice.GetType(this.VoiceType); }
        public string GetHost() {   return Voice.GetHost(this.Host); }

        static public string GetGender(EGender which)
        {
            if (which == EGender.Male)      return "Male";
            if (which == EGender.Female)    return "Female";
            if (which == EGender.Neutral)   return "Neutral";
            if (which == EGender.NotSet)    return "NotSet";

            return "unknown";

        }
         
         static public EGender FromGender(string which)
            {
            if (which == "Male")    return EGender.Male;
            if (which == "Female")  return EGender.Female;
            if (which == "Neutral") return EGender.Neutral;
            if (which == "NotSet")  return EGender.NotSet;

            return EGender.NotSet;
        }

        static public string GetHost(EHost which)
        {
            if (which == EHost.Azure)   return "Azure";
            if (which == EHost.GCloud)  return "GCloud";
            if (which == EHost.AWS)     return "AWS";
            if (which == EHost.Local)   return "Local";

            return "unknown";

        }

        static public EHost FromHost(string which)
        {
            if (which == "Azure")   return EHost.Azure;
            if (which == "GCloud")  return EHost.GCloud;
            if (which == "AWS")     return EHost.AWS;
            if (which == "Local")   return EHost.Local;

            return EHost.Local;
        }


        static public string GetType(EVoiceType which)
        {
            if (which == EVoiceType.Standard)   return "Standard";
            if (which == EVoiceType.Neural)     return "Neural";

            return "unknown";

        }
        static public EVoiceType FromType(string which)
        {
            if (which == "Standard")    return EVoiceType.Standard;
            if (which == "Neural")      return EVoiceType.Neural;

            return EVoiceType.Standard;
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

        static public EGender convertGenderFromLocal(System.Speech.Synthesis.VoiceGender gender)
        {
            if (gender == VoiceGender.Male)     return EGender.Male;
            if (gender == VoiceGender.Female)   return EGender.Female;
            if (gender == VoiceGender.Neutral)  return EGender.Neutral;
            if (gender == VoiceGender.NotSet)   return EGender.NotSet;

            return EGender.NotSet;

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


    

}
