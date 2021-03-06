﻿//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     VoiceImport.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  Contains functions to install/import new voices
//
//
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Win32;

namespace iYak.Classes
{
    public class VoiceImport
    {

        public const string TTS_ROOT = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\Voices\\Tokens\\";

        public static List<VoiceSynth> ListTTS = new List<VoiceSynth>();

        public static List<string> AddedVoices = new List<string>();


        public class VoiceSynth
        {
            public string Name { get; set; }
            public string TokenPath { get; set; }
            public string FullName { get; set; }
            public string VoicePath { get; set; }
            public string LangDataPath { get; set; }
            public string Region { get; set; }
            public string Gender { get; set; }
            public string Language { get; set; }
            public bool Installed { get; set; }



        }

        public static List<VoiceSynth> GetAvailableVoices() 
        {

            string LocalJsonFile = Helpers.JoinPath(AppDomain.CurrentDomain.BaseDirectory, "LocalVoices.json");

            if (!File.Exists(LocalJsonFile)) return new List<VoiceSynth>();

            string voiceJson = File.ReadAllText(LocalJsonFile);

            VoiceImport.ListTTS = JsonConvert.DeserializeObject<List<VoiceSynth>> (voiceJson);

            foreach( VoiceSynth tmpVoice in ListTTS ) 
            {

                tmpVoice.Installed = false;
                foreach( Voice voice in Config.Voices ) 
                {
                    if (voice.Handle.ToLower() == tmpVoice.Name.ToLower()) tmpVoice.Installed = true;
                }

                foreach( string beenInstalled in AddedVoices )
                {
                    if (beenInstalled == tmpVoice.Name) tmpVoice.Installed = true;
                }

            }

            return ListTTS;


        }


        public static bool InstallTTS(string voiceHandle)
        {

            VoiceSynth synth = new VoiceSynth();

            foreach ( VoiceSynth tmpSynth in GetAvailableVoices())
            {
                if (tmpSynth.Name.ToLower() != voiceHandle.ToLower()) continue;

                synth  = tmpSynth;

            }

            if (synth == null) return false;

           
            if (synth.Installed) return true;

            Console.WriteLine("INSTALLING NEW TTS VOICE: " + synth.Name);

            string rkey = TTS_ROOT + synth.TokenPath;

            try
            {
                Registry.SetValue(rkey, "(Default)", synth.Name);
                Registry.SetValue(rkey, "409", synth.FullName);
                Registry.SetValue(rkey, "CLSID", "{179F3D56-1B0B-42B2-A962-59B7EF59FE1B}");
                Registry.SetValue(rkey, "LangDataPath", synth.LangDataPath);
                Registry.SetValue(rkey, "VoicePath", synth.VoicePath);
                Registry.SetValue(rkey + "\\Attributes", "Age", "Adult");
                Registry.SetValue(rkey + "\\Attributes", "Gender", synth.Gender);
                Registry.SetValue(rkey + "\\Attributes", "Language", synth.Language);
                Registry.SetValue(rkey + "\\Attributes", "Name", synth.Name);


                AddedVoices.Add(synth.Name);

                RoboVoice.RefreshVoiceList();

                Config.Voices = RoboVoice.GetVoiceList();

                Utilities.FillVoiceList(Config.Voices);

                Settings.MyForm.ShowLocalVoices();
                


            } catch (Exception e) {
                string msg = e.Message + "\n\nAccess to the following registry key is needed:\n\n"
                            + "HKLM\\SOFTWARE\\Microsoft\\Speech\\Voices\\Tokens\\\n\n"
                            + "You can manually adjust the permissions for that key to give your user full control "
                            + "or Re-run this application as Admin and try again.";
                Helpers.Alert(msg, "RESTRICTED REGISTRY ACCESS");
            }

            return true;

        }

    }

}
