using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using Microsoft.CognitiveServices.Speech;


namespace iYak.Classes
{
    class RoboVoice
    {

        static private Boolean isImported  = false;
        static private Boolean AzureReady  = false;
        static private Boolean AzureEnabled = false;
        static private Boolean GCloudReady = false;
        static private String AzureKey     = "";
        static private String AzureRegion  = "";

        static private SpeechConfig AzureConfig;



        public RoboVoice() 
        {
            if (RoboVoice.isImported == false) {

                RoboVoice.InitAzure();

                RoboVoice.isImported = true;

            }
            
        }


        static void InitAzure() 
        {
            if (CloudWS.Azure.key == "") return;

            // SpeechConf = SpeechConfig.FromSubscription(CloudKey, CloudRegion)

            RoboVoice.AzureConfig = SpeechConfig.FromSubscription(CloudWS.Azure.key, CloudWS.Azure.region);

            RoboVoice.AzureReady = true;

            
        }
    }
}
