﻿//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     RoboVoice.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  Interface for the various Text-to-speech services
//
//  Currently supported services:
//  SAPI    (LocalVoice.cs)
//  Azure   (AzureVoice.cs)
//
//
using System;
using System.Collections.Generic;


namespace iYak.Classes
{
    public class RoboVoice
    {

        static public Boolean AzureReady     = false;
        static public Boolean AzureEnabled   = false;
        static public Boolean AWSReady       = false;
        static public Boolean AWSEnabled     = false;


        public Voice voice = new Voice();


        public RoboVoice()
        {
                

        }

        public RoboVoice(Voice _voice)
        {
            this.voice = _voice;
        }

        

        public static bool Speak(Voice voice) 
        {

            if (voice.Host == Voice.EHost.Local) return LocalVoice.Speak(voice);
            if (voice.Host == Voice.EHost.Azure) { AzureVoice.Speak(voice); return true; }
            if (voice.Host == Voice.EHost.AWS) { AWSVoice.Speak(voice); return true; }

            return false;
        
       }

        public static void ExportSpeech(Voice voice, VoiceExport voiceExport=null)
        {
            if (voiceExport == null)
            {
                voiceExport = new VoiceExport() 
                { 
                    FileName = Helpers.GenerateFileName(voice),
                    FilePath = Config.ExportPath,
                };
                

            }

            Action callback;

            callback = Utilities.FillExported;

            if (voice.Host == Voice.EHost.Local)
            {
                LocalVoice.Export(voice, voiceExport, callback);
            }
            else if (voice.Host == Voice.EHost.Azure)
            {
                AzureVoice.Export(voice, voiceExport, callback);
            }
            else if (voice.Host == Voice.EHost.AWS)
            {
                AWSVoice.Export(voice, voiceExport, callback);
            }


        }
        

        static public List<Voice> GetVoiceList(bool ForceRefresh=false)
        {
            List<Voice> VoiceList = new List<Voice>();

            List<Voice> TempList  = LocalVoice.GetVoiceList();

            foreach( Voice TempVoice in TempList)
            {
                VoiceList.Add(TempVoice);
            }

            if(AzureReady)
            {
                TempList = AzureVoice.GetVoiceList(ForceRefresh);

                foreach (Voice TempVoice in TempList)
                {
                    VoiceList.Add(TempVoice);
                }

            }

            if (AWSReady)
            {
                TempList = AWSVoice.GetVoiceList(ForceRefresh);

                foreach (Voice TempVoice in TempList)
                {
                    VoiceList.Add(TempVoice);
                }

            }

            return VoiceList;

        }

        
       

        static public void RefreshVoiceList()
        {

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
        public string Locale   = "";
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
                Uid       = this.Uid,
                Id        = this.Id,
                Handle    = this.Handle,
                Avatar    = this.Avatar,
                Nickname  = this.Nickname,
                Active    = this.Active,
                Locale    = this.Locale,
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
        public string GetVoiceType() {   return Voice.GetVoiceType(this.VoiceType); }
        public string GetHost() {   return Voice.GetHost(this.Host); }

        static public string GetGender(EGender which)
        {
            if (which == EGender.Male)      return "Male";
            if (which == EGender.Female)    return "Female";
            if (which == EGender.Neutral)   return "Female";
            if (which == EGender.NotSet)    return "NotSet";

            return "unknown";

        }
         
         static public EGender FromGender(string which)
            {
            if (which == "Male")    return EGender.Male;
            if (which == "Female")  return EGender.Female;
            if (which == "Neutral") return EGender.Female;
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


        static public string GetVoiceType(EVoiceType which)
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

        

        public void SetVoice(string LookupId, EHost LookupHost=EHost.Local, EVoiceType LookupType=EVoiceType.Standard)
        {

            foreach (Voice tmpVoice in Config.Voices)
            {

                if (tmpVoice.Id == LookupId && tmpVoice.Host == LookupHost && tmpVoice.VoiceType == LookupType)
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

    public class VoiceExport
    {
        public string FileName            = "";
        public string FilePath            = Config.ExportPath;
        public OutputFormats OutputFormat = OutputFormats.mp3;
        public KhzOptions Khz             = KhzOptions.khz_24;

        public enum KhzOptions{
            khz_16,
            khz_24,
        }
        public enum OutputFormats{
            wav,
            mp3,
        }

        public static string GetFilePath(VoiceExport voiceExport)
        {
            string filePath = Helpers.JoinPath(voiceExport.FilePath, voiceExport.FileName);

            string fileExtension = "";

            if (voiceExport.OutputFormat == OutputFormats.mp3)      fileExtension = "mp3";
            else if (voiceExport.OutputFormat == OutputFormats.wav) fileExtension = "wav";

            filePath += "." + fileExtension;

            return filePath;

        }
        
    }


    

}
