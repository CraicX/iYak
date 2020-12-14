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
using iYak.Controls;



namespace iYak
{
    public partial class Main : Form
    {
        

        public Main()
        {
            InitializeComponent();

        }

        public void XMLToolStripMenuItem_Click(object sender, EventArgs e)
        {


            Config.frmSettings = new Settings();
            
            Config.frmSettings.cbAzure.Checked  = CloudWS.Azure.enabled;
            Config.frmSettings.AzureKey.Text    = CloudWS.Azure.key;
            Config.frmSettings.AzureRegion.Text = CloudWS.Azure.region;
            Application.DoEvents();
            Console.WriteLine("key:" + CloudWS.Azure.key);

            Config.frmSettings.ShowDialog();

        }

        private void Main_Load(object sender, EventArgs e)
        {

            Config.splasher = new ViewSplash();
            Config.splasher.Show();
            

            Config.LVoices     = VoiceSelect;
            Config.FAvatars    = AvatarsFlow;
            Config.FActors     = ActorsFlow;
            Config.FScripts    = FlowScript;
            Config.CurrentFace = pbFace;
            Config.mainRef     = this;

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

        public void SetTuningToVoice()
        {
            this.tbVolume.Value = Config.CurrentVoice.Volume;
            this.tbPitch.Value  = Config.CurrentVoice.Pitch;
            this.tbSpeed.Value  = Config.CurrentVoice.Rate;
        }

        public void SetVoiceToTuning()
        {
            Config.CurrentVoice.Volume = this.tbVolume.Value;
            Config.CurrentVoice.Pitch  = this.tbPitch.Value;
            Config.CurrentVoice.Rate   = this.tbSpeed.Value;
        }

        public void ResetTuning()
        {
            tbPitch.Value  = 5;
            tbSpeed.Value  = 5;
            tbVolume.Value = 100;
        }
        private void VoiceSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VoiceSelect.SelectedItems.Count < 1) return;

            Application.DoEvents();

            Config.CurrentVoice = new Voice();

            Config.CurrentVoice.SetVoice(VoiceSelect.Items[VoiceSelect.SelectedIndices[0]].Text);

            Config.CurrentVoice.Avatar = Config.CurrentFace.Tag.ToString();

            UpdateVoiceInfo(Config.CurrentVoice);

        }

        public void UpdateVoiceInfo( Voice speaker )
        {

            lblVoice.Text     = speaker.Id;
            lblGender.Text    = Voice.GetGender(speaker.Gender);
            lblService.Text   = Voice.GetHost(speaker.Host);
            lblType.Text      = Voice.GetType(speaker.VoiceType);
            tbNickname.Text   = speaker.Nickname;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            string SayText = SayBox.Text.Trim();
            Console.WriteLine(SayText);
            if (SayText == "") return;

            RoboVoice RVoice = new RoboVoice { voice = Config.CurrentVoice };

            RVoice.Say(SayText);





        }

        private void tbVolume_Scroll(object sender, EventArgs e)
        {
            Config.CurrentVoice.Volume = tbVolume.Value;
        }

        private void tbPitch_Scroll(object sender, EventArgs e)
        {
            Config.CurrentVoice.Pitch = tbPitch.Value;
        }

        private void tbSpeed_Scroll(object sender, EventArgs e)
        {
            Config.CurrentVoice.Rate = tbSpeed.Value;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        
            if (Config.CurrentVoice.Handle == null || Config.CurrentVoice.Handle == "") return;

            Utilities.AddSpeech(Config.CurrentVoice.Copy(), SayBox.Text);
            

        }


       
        private void tbNickname_Click(object sender, EventArgs e) 
        {
            if (tbNickname.Text == "nickname")
            {
                tbNickname.Text = "";

            } else {

                tbNickname.SelectAll();

            }

        }

        private void tbNickname_Changed(object sender, EventArgs e)
        {
            if (tbNickname.Text == "") return;

            Config.CurrentVoice.Nickname = tbNickname.Text;
        }

        private void btnAddActor_Click(object sender, EventArgs e)
        {
            if (Config.CurrentVoice.Handle == null || Config.CurrentVoice.Handle == "") return;

            Utilities.AddActor(Config.CurrentVoice);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            RoboActor actor = RoboActor.GetSelected();

            if (actor == null) return;

            Console.WriteLine(actor.ActorID);

            actor.voice = Config.CurrentVoice.Copy();
            actor.Speech = SayBox.Text;
            actor.RefreshActor();

        }
    }
}
