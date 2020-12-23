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

        public static void Init()
        {
            if (CloudWS.Azure.key == "") return;

            AzureConfig          = SpeechConfig.FromSubscription(CloudWS.Azure.key, CloudWS.Azure.region);

            Synth                = new Microsoft.CognitiveServices.Speech.SpeechSynthesizer(AzureConfig);

            RoboVoice.AzureReady = true;

        }

        public static AudioFile Export(Voice voice, string FileName = "")
        {

            //
            //  Validate and set our export file name/path
            //
            if (FileName == "") FileName = RoboVoice.GenerateFileName(voice) + ".mp3";

            string FilePath = Helpers.JoinPath(Config.ExportPath, FileName);

            ExportTask(voice, FilePath);

            AudioFile afile = Helpers.GetAudioFileInfo(FilePath);

            return afile;

        }

        public static async void ExportTask(Voice voice, string FilePath)
        {

            //
            //  Template for ssml 
            //
            string ssmlTemplate = @"
                <speak version=""1.0"" xmlns=""https://www.w3.org/2001/10/synthesis"" xml:lang=""en-US"">
                    <voice name=""{1}"">
                        <prosody volume=""{2}"" {3} {4}>
                            {0}
                        </prosody>
                    </voice>
                </speak>
            ";

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

            var TempSynth = new SpeechSynthesizer(TempConfig);

            AudioConfig SaveFormat = AudioConfig.FromWavFileOutput(FilePath);

            SpeechSynthesisResult SynthResult = await TempSynth.SpeakSsmlAsync(ssml);

            AudioDataStream stream = AudioDataStream.FromResult(SynthResult);

            await stream.SaveToWaveFileAsync(FilePath);

        }

        public static List<Voice> GetVoiceList()
        {
            Voice TempVoice;

            List<Voice> VoiceList = new List<Voice>();



            return VoiceList;

        }


        static public async Task<string> AppendVoiceListAzure()
        {
            string CachedJson = Helpers.JoinPath(Config.RootPath, "Azure_" + CloudWS.Azure.region + ".json");
            string json = "";

            //
            //  Check if we already grabbed the voice list from Azure
            //
            if (File.Exists(CachedJson))
            {
                json = File.ReadAllText(CachedJson);
                Console.WriteLine("Cached List");

            }
            else
            {

                //
                //  Download the voicelist from Azure
                //
                Authentication.SetupTokens();

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
                    File.WriteAllText(CachedJson, json);

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

        public enum Gender
        {
            Female,
            Male
        }

        public enum AudioOutputFormat
        {
            Raw8Khz8BitMonoMULaw,
            Raw16Khz16BitMonoPcm,
            Riff8Khz8BitMonoMULaw,
            Riff16Khz16BitMonoPcm,
            Ssml16Khz16BitMonoSilk,
            Raw16Khz16BitMonoTrueSilk,
            Ssml16Khz16BitMonoTts,
            Audio16Khz128KBitRateMonoMp3,
            Audio16Khz64KBitRateMonoMp3,
            Audio16Khz32KBitRateMonoMp3,
            Audio16Khz16KbpsMonoSiren,
            Riff16Khz16KbpsMonoSiren,
            Raw24Khz16BitMonoTrueSilk,
            Raw24Khz16BitMonoPcm,
            Riff24Khz16BitMonoPcm,
            Audio24Khz48KBitRateMonoMp3,
            Audio24Khz96KBitRateMonoMp3,
            Audio24Khz160KBitRateMonoMp3
        }

        public class Synthesize
        {
            private string GenerateSsml(string locale, string gender, string name, string text)
            {
                var ssmlDoc = new XDocument(
                                  new XElement("speak",
                                      new XAttribute("version", "1.0"),
                                      new XAttribute(XNamespace.Xml + "lang", locale),
                                      new XElement("voice",
                                          new XAttribute(XNamespace.Xml + "lang", locale),
                                          new XAttribute(XNamespace.Xml + "gender", gender),
                                          new XAttribute("name", name),
                                          text)));
                return ssmlDoc.ToString();
            }

            private HttpClient client;
            private HttpClientHandler handler;

            public Synthesize()
            {
                var cookieContainer = new CookieContainer();
                handler = new HttpClientHandler() { CookieContainer = new CookieContainer(), UseProxy = false };
                client = new HttpClient(handler);
            }

            ~Synthesize()
            {
                client.Dispose();
                handler.Dispose();
            }

            public event EventHandler<GenericEventArgs<Stream>> OnAudioAvailable;

            public event EventHandler<GenericEventArgs<Exception>> OnError;

            public Task Speak(CancellationToken cancellationToken, InputOptions inputOptions)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var header in inputOptions.Headers)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                var genderValue = "";
                switch (inputOptions.VoiceType)
                {
                    case Gender.Male:
                        genderValue = "Male";
                        break;

                    case Gender.Female:
                    default:
                        genderValue = "Female";
                        break;
                }

                var request = new HttpRequestMessage(HttpMethod.Post, inputOptions.RequestUri)
                {
                    Content = new StringContent(GenerateSsml(inputOptions.Locale, genderValue, inputOptions.VoiceName, inputOptions.Text))
                };

                var httpTask = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                Console.WriteLine("Response status code: [{0}]", httpTask.Result.StatusCode);

                var saveTask = httpTask.ContinueWith(
                    async (responseMessage, token) =>
                    {
                        try
                        {
                            if (responseMessage.IsCompleted && responseMessage.Result != null && responseMessage.Result.IsSuccessStatusCode)
                            {
                                var httpStream = await responseMessage.Result.Content.ReadAsStreamAsync().ConfigureAwait(false);
                                this.AudioAvailable(new GenericEventArgs<Stream>(httpStream));
                            }
                            else
                            {
                                this.Error(new GenericEventArgs<Exception>(new Exception(String.Format("Service returned {0}", responseMessage.Result.StatusCode))));
                            }
                        }
                        catch (Exception e)
                        {
                            this.Error(new GenericEventArgs<Exception>(e.GetBaseException()));
                        }
                        finally
                        {
                            responseMessage.Dispose();
                            request.Dispose();
                        }
                    },
                    TaskContinuationOptions.AttachedToParent,
                    cancellationToken);

                return saveTask;
            }

            private void AudioAvailable(GenericEventArgs<Stream> e)
            {
                EventHandler<GenericEventArgs<Stream>> handler = this.OnAudioAvailable;
                if (handler != null)
                {
                    handler(this, e);
                }
            }

            private void Error(GenericEventArgs<Exception> e)
            {
                EventHandler<GenericEventArgs<Exception>> handler = this.OnError;
                if (handler != null)
                {
                    handler(this, e);
                }
            }

            public class InputOptions
            {
                public InputOptions()
                {
                    this.Locale = "en-us";
                    this.VoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)";
                    // Default to Riff16Khz16BitMonoPcm output format.
                    this.OutputFormat = AudioOutputFormat.Riff16Khz16BitMonoPcm;
                }

                public Uri RequestUri { get; set; }

                public AudioOutputFormat OutputFormat { get; set; }

                public IEnumerable<KeyValuePair<string, string>> Headers
                {
                    get
                    {
                        List<KeyValuePair<string, string>> toReturn = new List<KeyValuePair<string, string>>();
                        toReturn.Add(new KeyValuePair<string, string>("Content-Type", "application/ssml+xml"));

                        string outputFormat;

                        switch (this.OutputFormat)
                        {
                            case AudioOutputFormat.Raw16Khz16BitMonoPcm:
                                outputFormat = "raw-16khz-16bit-mono-pcm";
                                break;

                            case AudioOutputFormat.Raw8Khz8BitMonoMULaw:
                                outputFormat = "raw-8khz-8bit-mono-mulaw";
                                break;

                            case AudioOutputFormat.Riff16Khz16BitMonoPcm:
                                outputFormat = "riff-16khz-16bit-mono-pcm";
                                break;

                            case AudioOutputFormat.Riff8Khz8BitMonoMULaw:
                                outputFormat = "riff-8khz-8bit-mono-mulaw";
                                break;

                            case AudioOutputFormat.Ssml16Khz16BitMonoSilk:
                                outputFormat = "ssml-16khz-16bit-mono-silk";
                                break;

                            case AudioOutputFormat.Raw16Khz16BitMonoTrueSilk:
                                outputFormat = "raw-16khz-16bit-mono-truesilk";
                                break;

                            case AudioOutputFormat.Ssml16Khz16BitMonoTts:
                                outputFormat = "ssml-16khz-16bit-mono-tts";
                                break;

                            case AudioOutputFormat.Audio16Khz128KBitRateMonoMp3:
                                outputFormat = "audio-16khz-128kbitrate-mono-mp3";
                                break;

                            case AudioOutputFormat.Audio16Khz64KBitRateMonoMp3:
                                outputFormat = "audio-16khz-64kbitrate-mono-mp3";
                                break;

                            case AudioOutputFormat.Audio16Khz32KBitRateMonoMp3:
                                outputFormat = "audio-16khz-32kbitrate-mono-mp3";
                                break;

                            case AudioOutputFormat.Audio16Khz16KbpsMonoSiren:
                                outputFormat = "audio-16khz-16kbps-mono-siren";
                                break;

                            case AudioOutputFormat.Riff16Khz16KbpsMonoSiren:
                                outputFormat = "riff-16khz-16kbps-mono-siren";
                                break;
                            case AudioOutputFormat.Raw24Khz16BitMonoPcm:
                                outputFormat = "raw-24khz-16bit-mono-pcm";
                                break;
                            case AudioOutputFormat.Riff24Khz16BitMonoPcm:
                                outputFormat = "riff-24khz-16bit-mono-pcm";
                                break;
                            case AudioOutputFormat.Audio24Khz48KBitRateMonoMp3:
                                outputFormat = "audio-24khz-48kbitrate-mono-mp3";
                                break;
                            case AudioOutputFormat.Audio24Khz96KBitRateMonoMp3:
                                outputFormat = "audio-24khz-96kbitrate-mono-mp3";
                                break;
                            case AudioOutputFormat.Audio24Khz160KBitRateMonoMp3:
                                outputFormat = "audio-24khz-160kbitrate-mono-mp3";
                                break;
                            default:
                                outputFormat = "riff-16khz-16bit-mono-pcm";
                                break;
                        }

                        toReturn.Add(new KeyValuePair<string, string>("X-Microsoft-OutputFormat", outputFormat));
                        // authorization Header
                        toReturn.Add(new KeyValuePair<string, string>("Authorization", this.AuthorizationToken));
                        // Refer to the doc
                        toReturn.Add(new KeyValuePair<string, string>("X-Search-AppId", "07D3234E49CE426DAA29772419F436CA"));
                        // Refer to the doc
                        toReturn.Add(new KeyValuePair<string, string>("X-Search-ClientID", "1ECFAE91408841A480F00935DC390960"));
                        // The software originating the request
                        toReturn.Add(new KeyValuePair<string, string>("User-Agent", "TTSClient"));

                        return toReturn;
                    }
                    set
                    {
                        Headers = value;
                    }
                }

                public String Locale { get; set; }

                public Gender VoiceType { get; set; }

                public string VoiceName { get; set; }

                public string AuthorizationToken { get; set; }

                public string Text { get; set; }

            }
        }
    }
}
