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
using System.Speech.AudioFormat;
using System.Speech.Synthesis;


namespace iYak.Classes
{
    class LocalVoice
    {


        // ────────────────────────────────────────────────────────────────────────
        //   :::    SPEAK
        // ────────────────────────────────────────────────────────────────────────
        public static bool Speak(Voice voice)
        {
            SpeechSynthesizer TSynth = new SpeechSynthesizer();

            TSynth.SetOutputToDefaultAudioDevice();

            TSynth.SelectVoice(voice.Handle);

            TSynth.Volume = voice.Volume;

            TSynth.Rate   = (voice.Rate * 2) - 10;

            TSynth.SpeakAsync(voice.Speech);

            return true;
        }


        // ────────────────────────────────────────────────────────────────────────
        //   :::    EXPORT
        // ────────────────────────────────────────────────────────────────────────
        public static void Export(Voice voice, VoiceExport voiceExport, Action callback=null)
        {

            //  Currently only supporting WAV format
            voiceExport.OutputFormat = VoiceExport.OutputFormats.wav;

            //
            //  set our export file name/path
            //
            string FilePath = VoiceExport.GetFilePath(voiceExport);

            SpeechAudioFormatInfo outputFormat;

            int hz = 96000;

            if(voiceExport.OutputFormat == VoiceExport.OutputFormats.wav)
            {
                if (voiceExport.Khz == VoiceExport.KhzOptions.khz_16)       hz = 96000;
                else if (voiceExport.Khz == VoiceExport.KhzOptions.khz_24)  hz = 96000;

                outputFormat = new SpeechAudioFormatInfo(
                    hz, 
                    AudioBitsPerSample.Sixteen, 
                    AudioChannel.Stereo
                );

            } else outputFormat = new SpeechAudioFormatInfo(hz, AudioBitsPerSample.Sixteen, AudioChannel.Stereo);

            SpeechSynthesizer localSynth = new SpeechSynthesizer()
            {
                Volume = voice.Volume,
                Rate   = (voice.Rate * 2) - 10
            };

            localSynth.SetOutputToDefaultAudioDevice();
            localSynth.SelectVoice(voice.Handle);

            localSynth.SetOutputToWaveFile(FilePath, outputFormat);
            localSynth.Speak(voice.Speech);
            
            callback?.Invoke();

        }



        public static List<Voice> GetVoiceList()
        {
            Voice TempVoice;

            List<Voice> VoiceList    = new List<Voice>();
            SpeechSynthesizer TSynth = new SpeechSynthesizer();

            foreach (InstalledVoice iVoice in TSynth.GetInstalledVoices())
            {
                TempVoice = new Voice
                {
                    Active   = true,
                    Id       = Voice.GenerateName(iVoice.VoiceInfo.Name),
                    Nickname = Voice.GenerateName(iVoice.VoiceInfo.Name),
                    Handle   = iVoice.VoiceInfo.Name,
                    Gender   = ConvertGenderFromLocal(iVoice.VoiceInfo.Gender),
                    Host     = Voice.EHost.Local,
                    Locale   = iVoice.VoiceInfo.Culture.DisplayName
                };

                VoiceList.Add(TempVoice);

            }

            return VoiceList;
        }


        public static Voice.EGender ConvertGenderFromLocal(VoiceGender gender)
        {
            if (gender == VoiceGender.Male)     return Voice.EGender.Male;
            if (gender == VoiceGender.Female)   return Voice.EGender.Female;
            if (gender == VoiceGender.Neutral)  return Voice.EGender.Female;
            if (gender == VoiceGender.NotSet)   return Voice.EGender.NotSet;

            return Voice.EGender.NotSet;

        }

    }

}
