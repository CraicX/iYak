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
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Newtonsoft.Json;


namespace iYak.Classes
{
    class AzureVoice
    {

        public static SpeechConfig AzureConfig;
        public static SpeechSynthesizer Synth;

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
        public static bool Speak(Voice voice)
        {

            

            return true;

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
                Pitch = "pitch=\"" + (pitchVal < 0, "-", "+") + Math.Abs(pitchVal) + "st\"";
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
        public static List<Voice> GetVoiceList(bool ForceRefresh=false)
        {
            Voice TempVoice;

            List<Voice> VoiceList = new List<Voice>();

            GetVoiceListTask(ForceRefresh);

            return VoiceList;

        }


        public static async Task<string> GetVoiceListTask(bool ForceRefresh=false)
        {
            string CachedJson = Helpers.JoinPath(Config.RootPath, "Azure_" + CloudWS.Azure.region + ".json");
            string json = "";

            //
            //  Check if we already grabbed the voice list from Azure
            //
            if (!ForceRefresh && File.Exists(CachedJson))
            {
                json = File.ReadAllText(CachedJson);
                Console.WriteLine("Cached List");

            }
            else
            {

                //
                //  Download the voicelist from Azure
                //
                await Authenticate();

                string wsUri = "https://" + CloudWS.Azure.region + ".tts.speech.microsoft.com/cognitiveservices/voices/list";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + CloudWS.Azure.token);

                    UriBuilder uriBuilder = new UriBuilder(wsUri);
                    var result            = await client.GetAsync(uriBuilder.Uri.AbsoluteUri);
                    json                  = await result.Content.ReadAsStringAsync();

                    //
                    //  Cache the Voice List to file
                    //
                    if (json.Length > 1) File.WriteAllText(CachedJson, json);
                    else Console.WriteLine("Could not retrieve voice list from Azure.");

                }

                ConvertAzureJsonToVoice(json);

            }

            return CloudWS.Azure.token;

        }

        public static bool ConvertAzureJsonToVoice(string json){


            List<Voice> VoiceList = new List<Voice>();

            //Console.WriteLine(json);

            List<Object> JsonList = new List<Object>();

            JsonList = JsonConvert.DeserializeObject<List<Object>>(json);

            foreach (Object azvoice in JsonList)
            {
                Voice voice = new Voice()
                {
                    // Nickname = azvoice.
                };
            }

            return true;

        }


        // ────────────────────────────────────────────────────────────────────────
        //   :::    AUTHENTICATE AZURE SERVICES
        // ────────────────────────────────────────────────────────────────────────
        public static async Task<string> Authenticate()
        {
            string FetchTokenUri = String.Format(
                "https://{0}.api.cognitive.microsoft.com/sts/v1.0/issueToken",
                CloudWS.Azure.region
            );


            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", CloudWS.Azure.key);

                UriBuilder uriBuilder = new UriBuilder(FetchTokenUri);

                var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);

                CloudWS.Azure.token = await result.Content.ReadAsStringAsync();

                return CloudWS.Azure.token;

            }

        }


        public class Authentication
        {
            private static DateTime LastToken;
            private static string accessToken;
            private static string AccessUri;   
            private static string apiKey;

            //Access token expires every 10 minutes. Renew it every 9 minutes only.
            private const int RefreshTokenDuration = 9;

            

            public static void SetupTokens()
            {
                AccessUri   = String.Format("https://{0}.api.cognitive.microsoft.com/sts/v1.0/issueToken", CloudWS.Azure.region);
                apiKey      = CloudWS.Azure.key;
                accessToken = HttpPost(AccessUri, apiKey);
                LastToken   = DateTime.UtcNow;

            }

            public string GetAccessToken()
            {
                return accessToken;
            }


            private static string HttpPost(string accessUri, string apiKey)
            {

                Console.WriteLine("Trying: {0} {1}", accessUri, apiKey);
                // Prepare OAuth request
                WebRequest webRequest = WebRequest.Create(accessUri);
                webRequest.Method = "POST";
                webRequest.ContentLength = 0;
                webRequest.Headers["Ocp-Apim-Subscription-Key"] = apiKey;

                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (Stream stream = webResponse.GetResponseStream())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            byte[] waveBytes = null;
                            int count = 0;
                            do
                            {
                                byte[] buf = new byte[1024];
                                count = stream.Read(buf, 0, 1024);
                                ms.Write(buf, 0, count);
                            } while (stream.CanRead && count > 0);

                            waveBytes = ms.ToArray();

                            return Encoding.UTF8.GetString(waveBytes);
                        }
                    }
                }
            }
        }


        public class GenericEventArgs<T> : EventArgs
        {
            public GenericEventArgs(T eventData)
            {
                this.EventData = eventData;
            }

            public T EventData { get; private set; }
        }

       

        
    }
}
