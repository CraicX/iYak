//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     AWSVoice.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  For supporting Amazon's Polly
//
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime.CredentialManagement;
using Newtonsoft.Json;
using Amazon.Runtime;

namespace iYak.Classes
{
    class AWSVoice
    {

        public static List<Voice> VoiceList = new List<Voice>();

        // ────────────────────────────────────────────────────────────────────────
        //   :::    INITIALIZE AZURE 
        // ────────────────────────────────────────────────────────────────────────
        public static void Init()
        {
            if (CloudWS.AWS.accessKey == "") return;

            var options = new CredentialProfileOptions
            {
                AccessKey = CloudWS.AWS.accessKey,
                SecretKey = CloudWS.AWS.secretKey
            };
            var profile = new Amazon.Runtime.CredentialManagement.CredentialProfile("basic_profile", options)
            {
                Region = RegionEndpoint.GetBySystemName(CloudWS.AWS.region)
            };
            
            var netSDKFile = new NetSDKCredentialsFile();

            netSDKFile.RegisterProfile(profile);

            var chain = new CredentialProfileStoreChain();
            AWSCredentials awsCredentials;
            if (chain.TryGetAWSCredentials("basic_profile", out awsCredentials))
            {
                // use awsCredentials
            }

            // AzureConfig = SpeechConfig.FromSubscription(CloudWS.Azure.key, CloudWS.Azure.region);

            // Synth = new Microsoft.CognitiveServices.Speech.SpeechSynthesizer(AzureConfig);

            RoboVoice.AWSReady = true;

        }



        // ────────────────────────────────────────────────────────────────────────
        //   :::    SPEAK
        // ────────────────────────────────────────────────────────────────────────
        public static void Speak(Voice voice)
        {

            VoiceExport voiceExport = new VoiceExport();
            Console.WriteLine("AWS Speak");

            var client = new AmazonPollyClient(RegionEndpoint.GetBySystemName(CloudWS.AWS.region));

            string filePath = Helpers.JoinPath(voiceExport.FilePath, "speech.mp3");

            var synthesizeSpeechRequest = new SynthesizeSpeechRequest()
            {
                OutputFormat = OutputFormat.Mp3,
                VoiceId      = VoiceId.FindValue(voice.Nickname),
                Text         = voice.Speech
            };

                using (var outputStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    var synthesizeSpeechResponse = client.SynthesizeSpeech(synthesizeSpeechRequest);
                    byte[] buffer = new byte[2 * 1024];
                    int readBytes;

                    var inputStream = synthesizeSpeechResponse.AudioStream;
                    while ((readBytes = inputStream.Read(buffer, 0, 2 * 1024)) > 0)
                        outputStream.Write(buffer, 0, readBytes);
                }

            AudioTools.PlayAudio(filePath);


        }


        // ────────────────────────────────────────────────────────────────────────
        //   :::    EXPORT
        // ────────────────────────────────────────────────────────────────────────
        public static void Export(Voice voice, VoiceExport voiceExport, Action callback = null)
        {

            //
            //  set our export file name/path
            //
            string FilePath = VoiceExport.GetFilePath(voiceExport);

            var client = new AmazonPollyClient(RegionEndpoint.GetBySystemName(CloudWS.AWS.region));

            string filePath = Helpers.JoinPath(voiceExport.FilePath, voiceExport.FileName);

            var synthesizeSpeechRequest = new SynthesizeSpeechRequest()
            {
                VoiceId = VoiceId.FindValue(voice.Nickname),
                Text = voice.Speech
            };
            synthesizeSpeechRequest.OutputFormat = (voiceExport.OutputFormat == VoiceExport.OutputFormats.wav)
                                                    ? OutputFormat.Mp3
                                                    : OutputFormat.Mp3;

            using (var outputStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                var synthesizeSpeechResponse = client.SynthesizeSpeech(synthesizeSpeechRequest);
                byte[] buffer = new byte[2 * 1024];
                int readBytes;

                var inputStream = synthesizeSpeechResponse.AudioStream;
                while ((readBytes = inputStream.Read(buffer, 0, 2 * 1024)) > 0)
                    outputStream.Write(buffer, 0, readBytes);
            }

            AudioTools.PlayAudio(FilePath);





                //azConfig.SetSpeechSynthesisOutputFormat(outputFormat);

                //var azSynth = new SpeechSynthesizer(azConfig, null);

                //var result = await azSynth.SpeakSsmlAsync(ssml);

                //var stream = AudioDataStream.FromResult(result);

                //await stream.SaveToWaveFileAsync(FilePath);

                callback?.Invoke();



        }


        public static List<Voice> GetVoiceList(bool ForceRefresh)
        {

            string CachedJson = Helpers.JoinPath(Config.RootPath, "AWS_" + CloudWS.AWS.region + ".json");

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

                json = "";


                var client = new AmazonPollyClient(RegionEndpoint.GetBySystemName(CloudWS.AWS.region));
                var allVoicesRequest = new DescribeVoicesRequest();

                    String nextToken;
                    do
                    {
                        var allVoicesResponse = client.DescribeVoices(allVoicesRequest);
                        nextToken = allVoicesResponse.NextToken;
                        allVoicesRequest.NextToken = nextToken;

                        AWSVoice.VoiceList = new List<Voice>();


                        foreach (var voice in allVoicesResponse.Voices)
                        {
                            VoiceConvert voiceConvert = new VoiceConvert()
                            {
                                Name = voice.Name,
                                DisplayName = voice.Name,
                                ShortName = voice.Name,
                                Gender = voice.Gender,
                                Locale = voice.LanguageName
                            };

                            Voice ivoice = new Voice()
                            {
                                Id = voiceConvert.DisplayName,
                                Nickname = voiceConvert.DisplayName,
                                Handle = voiceConvert.ShortName,
                                Gender = Voice.FromGender(voiceConvert.Gender),
                                VoiceType = Voice.EVoiceType.Standard,
                                Host = Voice.EHost.AWS,
                                Locale = voiceConvert.Locale
                            };

                            AWSVoice.VoiceList.Add(ivoice);

                        }

                    } while (nextToken != null);

            }
                


            return AWSVoice.VoiceList;

        }


        // ────────────────────────────────────────────────────────────────────────
        //   :::    CONVERT VOICE
        // ────────────────────────────────────────────────────────────────────────
        


        
        public class VoiceConvert
        {
            public string Name = "";
            public string DisplayName = "";
            public string LocalName = "";
            public string ShortName = "";
            public string Gender = "";
            public string Locale = "";
            public string SampleRateHertz = "";
            public string VoiceType = "";
            public string Status = "";

        }
    }
}
