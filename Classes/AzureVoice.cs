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

            string ssml = BuildSSML(voice);

            await Synth.SpeakSsmlAsync(ssml);

        }


        // ────────────────────────────────────────────────────────────────────────
        //   :::    EXPORT
        // ────────────────────────────────────────────────────────────────────────
        public static async void Export(Voice voice, VoiceExport voiceExport, Action callback=null)
        {

            //
            //  set our export file name/path
            //
            string FilePath = VoiceExport.GetFilePath(voiceExport);

            string ssml     = BuildSSML(voice);

            var azConfig    = SpeechConfig.FromSubscription(CloudWS.Azure.key, CloudWS.Azure.region);
            
            SpeechSynthesisOutputFormat outputFormat = SpeechSynthesisOutputFormat.Riff24Khz16BitMonoPcm;

            if(voiceExport.OutputFormat == VoiceExport.OutputFormats.wav) 
            {
                switch(voiceExport.Khz) {
                    case VoiceExport.KhzOptions.khz_16:
                        outputFormat = SpeechSynthesisOutputFormat.Riff16Khz16BitMonoPcm;
                        break;

                    case VoiceExport.KhzOptions.khz_24:
                        outputFormat = SpeechSynthesisOutputFormat.Riff24Khz16BitMonoPcm;
                        break;
                }
            }
            else if(voiceExport.OutputFormat == VoiceExport.OutputFormats.mp3)
            {
                switch(voiceExport.Khz) {
                    case VoiceExport.KhzOptions.khz_16:
                        outputFormat = SpeechSynthesisOutputFormat.Audio16Khz64KBitRateMonoMp3;
                        break;

                    case VoiceExport.KhzOptions.khz_24:
                        outputFormat = SpeechSynthesisOutputFormat.Audio24Khz96KBitRateMonoMp3;
                        break;
                }
            }

            azConfig.SetSpeechSynthesisOutputFormat(outputFormat);

            var azSynth = new SpeechSynthesizer(azConfig, null);

            var result  = await azSynth.SpeakSsmlAsync(ssml);

            var stream  = AudioDataStream.FromResult(result);

            await stream.SaveToWaveFileAsync(FilePath);

            callback?.Invoke();



        }


        // ────────────────────────────────────────────────────────────────────────
        //   :::    SSML 
        // ────────────────────────────────────────────────────────────────────────
        public static string BuildSSML(Voice voice)
        {
            string Pitch = "";
            string Rate  = "";

            //
            //  Convert Pitch & Rate values
            //
            if (voice.Pitch != 5)
            {
                int pitchVal = voice.Pitch - 5;
                Pitch        = "pitch=\"" + (pitchVal < 0 ? "-" : "+") + Math.Abs(pitchVal) + "st\"";
            }

            if (voice.Rate != 5)
            {
                double rateVal = ((voice.Rate - 5) * 0.1) + 1;
                Rate           = "rate=\"" + rateVal + "\"";
            }

            //
            //  Build our ssml string from template
            //
            string ssml = String.Format(ssmlTemplate, voice.Speech, voice.Handle, voice.Volume, Pitch, Rate);

            return ssml;
        }

        
        
        
        // ────────────────────────────────────────────────────────────────────────
        //   :::    GET VOICE LIST
        // ────────────────────────────────────────────────────────────────────────
       


        public static List<Voice> GetVoiceList(bool ForceRefresh)
        {

            string CachedJson = Helpers.JoinPath(Config.RootPath, "Azure_" + CloudWS.Azure.region + ".json");

            string json;

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


        // ────────────────────────────────────────────────────────────────────────
        //   :::    CONVERT VOICE
        // ────────────────────────────────────────────────────────────────────────
        public static bool ConvertVoice(string json){


            AzureVoice.VoiceList = new List<Voice>();

            List<VoiceConvert> JsonList;

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
