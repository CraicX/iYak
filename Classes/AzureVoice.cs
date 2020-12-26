//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     AzureVoice.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  For supporting Azure's remote Speech Synthesis 
//  using Microsoft CognitiveServices & Azure SDK
//
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Newtonsoft.Json;


namespace iYak.Classes
{
    class AzureVoice
    {

        public static SpeechConfig AzureConfig;
        public static SpeechSynthesizer Synth;
        public static List<Voice> VoiceList = new List<Voice>();

        //
        //  Template for ssml 
        //
        public static string ssmlTemplate = @"
                <speak version=""1.0"" xmlns=""https://www.w3.org/2001/10/synthesis"" xml:lang=""en-US"">
                    <voice name=""{1}"">
                        <prosody volume=""{2}"" {3} {4}>
                            {0}
                        </prosody>
                    </voice>
                </speak>
            ";


        // ────────────────────────────────────────────────────────────────────────
        //   :::    INITIALIZE AZURE 
        // ────────────────────────────────────────────────────────────────────────
        public static void Init()
        {
            if (CloudWS.Azure.key == "") return;

            AzureConfig          = SpeechConfig.FromSubscription(CloudWS.Azure.key, CloudWS.Azure.region);

            Synth                = new Microsoft.CognitiveServices.Speech.SpeechSynthesizer(AzureConfig);

            RoboVoice.AzureReady = true;

        }


        // ────────────────────────────────────────────────────────────────────────
        //   :::    SPEAK
        // ────────────────────────────────────────────────────────────────────────
        public static async void Speak(Voice voice)
        {

            string Pitch = "";
            string Rate = "";

            //
            //  Convert Pitch & Rate values
            //
            if (voice.Pitch != 5)
            {
                int pitchVal = voice.Pitch - 5;

                Pitch = "pitch=\"" + (pitchVal < 0 ? "-" : "+") + Math.Abs(pitchVal) + "st\"";
            }

            if (voice.Rate != 5)
            {
                double rateVal = ((voice.Rate - 5) * 0.1) + 1;
                Rate = "rate=\"" + rateVal + "\"";
            }

            //
            //  Build our ssml string from template
            //
            string ssml = String.Format(ssmlTemplate, voice.Speech, voice.Handle, voice.Volume, Pitch, Rate);

            Console.WriteLine(ssml);

            SpeechSynthesisResult result = await Synth.SpeakSsmlAsync(ssml);

        }


        // ────────────────────────────────────────────────────────────────────────
        //   :::    EXPORT
        // ────────────────────────────────────────────────────────────────────────
        public static AudioFile Export(Voice voice, string FileName = "")
        {

            //
            //  Validate and set our export file name/path
            //
            if (FileName == "") FileName = Helpers.GenerateFileName(voice) + ".mp3";

            string FilePath = Helpers.JoinPath(Config.ExportPath, FileName);

            ExportTask(voice, FilePath);

            AudioFile afile = Helpers.GetAudioFileInfo(FilePath);

            return afile;

        }

        public static async void ExportTask(Voice voice, string FilePath)
        {

            string Pitch = "";
            string Rate  = "";

            //
            //  Convert Pitch & Rate values
            //
            if( voice.Pitch != 5 )
            {
                int pitchVal = voice.Pitch - 5;
                Pitch = "pitch=\"" + (pitchVal < 0 ? "-" : "+") + Math.Abs(pitchVal) + "st\"";
            }

            if( voice.Rate != 5 )
            {
                double rateVal = ((voice.Rate - 5) * 0.1) + 1;
                Rate = "rate=\"" + rateVal + "\"";
            }

            //
            //  Build our ssml string from template
            //
            string ssml  = String.Format(ssmlTemplate, voice.Speech, voice.Handle, voice.Volume, Pitch, Rate);

            Console.WriteLine(ssml);

            //
            //  Create new Synthesizer object
            //
            SpeechConfig TempConfig = SpeechConfig.FromSubscription(CloudWS.Azure.key, CloudWS.Azure.region);

            TempConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Audio24Khz96KBitRateMonoMp3);

            var TempSynth                     = new SpeechSynthesizer(TempConfig);
            AudioConfig SaveFormat            = AudioConfig.FromWavFileOutput(FilePath);
            SpeechSynthesisResult SynthResult = await TempSynth.SpeakSsmlAsync(ssml);
            AudioDataStream stream            = AudioDataStream.FromResult(SynthResult);

            //
            //  Save the Stream Data to file
            //
            await stream.SaveToWaveFileAsync(FilePath);

        }

        
        
        // ────────────────────────────────────────────────────────────────────────
        //   :::    GET VOICE LIST
        // ────────────────────────────────────────────────────────────────────────
       


        public static List<Voice> GetVoiceList(bool ForceRefresh)
        {

            string CachedJson = Helpers.JoinPath(Config.RootPath, "Azure_" + CloudWS.Azure.region + ".json");

            string json = "";

            //
            //  Check if we already grabbed the voice list from Azure
            //
            if (!ForceRefresh && File.Exists(CachedJson))
            {
                json = File.ReadAllText(CachedJson);

            }
            else
            {

                //
                //  Download the voicelist from Azure
                //
                string FetchUri = String.Format(
                    "https://{0}.api.cognitive.microsoft.com/sts/v1.0/issueToken",
                    CloudWS.Azure.region
                );

                var http = new HttpPost();
                http.Headers.Add("Ocp-Apim-Subscription-Key", CloudWS.Azure.key);

                //
                //  If successful, we now have the Azure auth Token
                //
                CloudWS.Azure.token = http.Request(FetchUri);

                if( CloudWS.Azure.token == "" ) 
                {
                    Helpers.Alert("Could not retrieve the Azure Authentication Token. \n\nPlease check your credentials.", "Unable to connect to Azure");
                    return AzureVoice.VoiceList;
                }

                http = new HttpPost();
                http.Headers.Add("Authorization", "Bearer " + CloudWS.Azure.token);
                http.Method = "GET";

                FetchUri = String.Format(
                    "https://{0}.tts.speech.microsoft.com/cognitiveservices/voices/list",
                    CloudWS.Azure.region
                );

                json = http.Request(FetchUri);

                //
                //  Cache the Voice List to file
                //
                if (json.Length > 1) File.WriteAllText(CachedJson, json);
                else Helpers.Alert("Could not retrieve voice list from Azure.");

            }

            ConvertVoice(json);

            return AzureVoice.VoiceList;

        }

        public static bool ConvertVoice(string json){


            AzureVoice.VoiceList = new List<Voice>();

            List<VoiceConvert> JsonList = new List<VoiceConvert>();

            JsonList = JsonConvert.DeserializeObject<List<VoiceConvert>>(json);
            

            foreach (VoiceConvert row in JsonList)
            {
                Voice voice = new Voice()
                {
                    Id        = row.DisplayName,
                    Nickname  = row.DisplayName,
                    Handle    = row.ShortName,
                    Gender    = Voice.FromGender(row.Gender),
                    VoiceType = Voice.FromType(row.VoiceType),
                    Host      = Voice.EHost.Azure,
                    Locale    = row.Locale
                }; 

                AzureVoice.VoiceList.Add(voice);

            }

            return true;

        }

        public class VoiceConvert
        {
            public string Name            = "";
            public string DisplayName     = "";
            public string LocalName       = "";
            public string ShortName       = "";
            public string Gender          = "";
            public string Locale          = "";
            public string SampleRateHertz = "";
            public string VoiceType       = "";
            public string Status          = "";
            
        }

    }

}
