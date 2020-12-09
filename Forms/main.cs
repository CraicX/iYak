using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iYak.Classes;



namespace iYak
{
    public partial class Main : Form
    {
        

        static public Voice CurrentVoice = new Voice();
        public Main()
        {
            InitializeComponent();





            //Splasher frmSplash = new Splasher();

            //frmSplash.ShowDialog();
        }

        public void XMLToolStripMenuItem_Click(object sender, EventArgs e)
        {


            Config.frmSettings = new Settings();
            
            Config.frmSettings.cbAzure.Checked = CloudWS.Azure.enabled;
            Config.frmSettings.AzureKey.Text = CloudWS.Azure.key;
            Config.frmSettings.AzureRegion.Text = CloudWS.Azure.region;
            Application.DoEvents();
            Console.WriteLine("key:" + CloudWS.Azure.key);

            Config.frmSettings.ShowDialog();

        }

        private void Main_Load(object sender, EventArgs e)
        {

            Config.splasher = new ViewSplash();
            Config.splasher.Show();
            

            Config.LVoices = VoiceSelect;
            Config.FAvatars = AvatarsFlow;

            Utilities.StartUp();

            if (Config.Avatars.Count >= 1) {

                ListAvatars();
            }





        }

        private void ListAvatars() 
        {

            

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void VoiceSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VoiceSelect.SelectedItems.Count < 1) return;

            Application.DoEvents();

            Main.CurrentVoice = new Voice();

            Main.CurrentVoice.SetVoice(VoiceSelect.Items[VoiceSelect.SelectedIndices[0]].Text);

            UpdateVoiceInfo(Main.CurrentVoice);
            


            //Voice1.SetVoice(CurrentVoice.Id);
            //VoiceName.Text = CurrentVoice.Id;

        }

        private void UpdateVoiceInfo( Voice speaker )
        {

            lblVoice.Text     = speaker.Id;
            lblGender.Text    = Voice.GetGender(speaker.Gender);
            lblService.Text   = Voice.GetHost(speaker.Host);
            lblType.Text = Voice.GetType(speaker.VoiceType);
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            string SayText = SayBox.Text.Trim();
            Console.WriteLine(SayText);
            if (SayText == "") return;

            RoboVoice RVoice = new RoboVoice();

            RVoice.voice = Main.CurrentVoice;

            RVoice.Say(SayText);





        }

        private void tbVolume_Scroll(object sender, EventArgs e)
        {
            Main.CurrentVoice.Volume = tbVolume.Value;
        }

        private void tbPitch_Scroll(object sender, EventArgs e)
        {
            Main.CurrentVoice.Pitch = tbPitch.Value;
        }

        private void tbSpeed_Scroll(object sender, EventArgs e)
        {
            Main.CurrentVoice.Rate = tbSpeed.Value;
        }
    }
}
