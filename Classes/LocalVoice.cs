//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     LocalVoice.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  For supporting locally installed voices 
//  utilizing Microsoft's Speech API (SAPI)
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;


namespace iYak.Classes
{
    class LocalVoice
    {

        private static SpeechSynthesizer Synth = new SpeechSynthesizer();


        public static void Speak(Voice voice)
        {
            Synth.SetOutputToDefaultAudioDevice();

            Synth.SelectVoice(voice.Handle);

            Synth.Volume = voice.Volume;

            Synth.Rate   = (voice.Rate * 2) - 10;

            Synth.SpeakAsync(voice.Speech);
        }



        public static AudioFile Export(Voice voice, string FileName = "")
        {

            if (FileName == "") FileName = RoboVoice.GenerateFileName(voice) + ".wav";

            string FilePath = Helpers.JoinPath(Config.ExportPath, FileName);

            var SaveFormat = new SpeechAudioFormatInfo(
                96000,
                AudioBitsPerSample.Sixteen,
                AudioChannel.Stereo
            );

            SpeechSynthesizer ExportVoice = new SpeechSynthesizer()
            {
                Volume = voice.Volume,
                Rate = (voice.Rate * 2) - 10
            };
            ExportVoice.SetOutputToDefaultAudioDevice();
            ExportVoice.SelectVoice(voice.Handle);

            ExportVoice.SetOutputToWaveFile(FilePath, SaveFormat);
            ExportVoice.Speak(voice.Speech);

            AudioFile afile = Helpers.GetAudioFileInfo(FilePath);

            return afile;


        }



        public static List<Voice> GetVoiceList()
        {
            Voice TempVoice;

            List<Voice> VoiceList = new List<Voice>();

            foreach (InstalledVoice iVoice in Synth.GetInstalledVoices())
            {
                TempVoice = new Voice
                {
                    Active = true,
                    Id = Voice.GenerateName(iVoice.VoiceInfo.Name),
                    Handle = iVoice.VoiceInfo.Name,
                    Gender = Voice.convertGenderFromLocal(iVoice.VoiceInfo.Gender),
                    Host = Voice.EHost.Local
                };

                VoiceList.Add(TempVoice);

            }

            return VoiceList;
        }
    }
}
