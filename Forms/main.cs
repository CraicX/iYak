using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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


            Config.frmSettings.Location = new Point(Main.GetForm().Left + Main.GetForm().Width + Config.frmSettings.Width, this.Top);
            Config.frmSettings.ShowDialog(this);

        }

        private void Main_Load(object sender, EventArgs e)
        {

            Config.splasher = new ViewSplash();
            Config.splasher.Show();
            

            Config.LVoices     = VoiceSelect;
            Config.LExport     = ListExport;
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

            if (Config.CurrentFace.Tag != null)
            {

                Config.CurrentVoice.Avatar = Config.CurrentFace.Tag.ToString();

            }

            UpdateVoiceInfo(Config.CurrentVoice);

        }

        public void RefreshFilters() {
            Config.VFilter.female = this.VCGirls.Checked;
            Config.VFilter.male   = this.VCBoys.Checked;
            Config.VFilter.aws    = this.VCAWS.Checked;
            Config.VFilter.gcloud = this.VCGCloud.Checked;
            Config.VFilter.azure  = this.VCAzure.Checked;
            Config.VFilter.local  = this.VCLocal.Checked;
        }

        private void VCFilter_Click(object sender, EventArgs e)
        {
            Utilities.FillVoiceList(Config.Voices);

        }
        public void UpdateVoiceInfo( Voice speaker )
        {

            lblVoice.Text     = speaker.Id;
            lblGender.Text    = Voice.GetGender(speaker.Gender);
            lblService.Text   = Voice.GetHost(speaker.Host);
            lblType.Text      = Voice.GetType(speaker.VoiceType);
            tbNickname.Text   = speaker.Nickname;
        }

        private void BtnRead_Click(object sender, EventArgs e)
        {
            string SayText = SayBox.Text.Trim();
            Console.WriteLine(SayText);
            if (SayText == "") return;

            RoboVoice RVoice = new RoboVoice { voice = Config.CurrentVoice };

            RVoice.Say(SayText);





        }

        private void TbVolume_Scroll(object sender, EventArgs e)
        {
            Config.CurrentVoice.Volume = tbVolume.Value;
        }

        private void TbPitch_Scroll(object sender, EventArgs e)
        {
            Config.CurrentVoice.Pitch = tbPitch.Value;
        }

        private void TbSpeed_Scroll(object sender, EventArgs e)
        {
            Config.CurrentVoice.Rate = tbSpeed.Value;
        }


        private void BtnSave_Click(object sender, EventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {

        
            if (Config.CurrentVoice.Handle == null || Config.CurrentVoice.Handle == "") return;

            Utilities.AddSpeech(Config.CurrentVoice.Copy(), SayBox.Text);
            

        }


       
        private void TbNickname_Click(object sender, EventArgs e) 
        {
            if (tbNickname.Text == "nickname")
            {
                tbNickname.Text = "";

            } else {

                tbNickname.SelectAll();

            }

        }

        private void TbNickname_Changed(object sender, EventArgs e)
        {
            if (tbNickname.Text == "") return;

            Config.CurrentVoice.Nickname = tbNickname.Text;
        }

        private void BtnAddActor_Click(object sender, EventArgs e)
        {
            if (Config.CurrentVoice.Handle == null || Config.CurrentVoice.Handle == "") return;

            Utilities.AddActor(Config.CurrentVoice, true);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            RoboActor actor = RoboActor.GetSelected();

            if (actor == null) return;

            Console.WriteLine(actor.ActorID);

            actor.voice = Config.CurrentVoice.Copy();

            actor.Speech = actor.voice.Speech = SayBox.Text;
            actor.RefreshActor();

            Datax.AddSpeech(actor.voice, actor.voice.Speech, Config.CurrentPlaylist.Uid);

        }

        private void refreshListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.Voices.Clear();
            Config.Voices = RoboVoice.GetVoiceList();
           

            Utilities.FillVoiceList(Config.Voices);
        }

        static public Main GetForm() {
            return Config.mainRef;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string SayText = SayBox.Text.Trim();

            if (SayText == "") return;

            //Voice ExpVoice            = Config.CurrentVoice.Copy();
            Config.CurrentVoice.Speech  = SayText;
            Config.CurrentVoice.Volume  = tbVolume.Value;
            Config.CurrentVoice.Rate    = tbSpeed.Value;

            AudioFile fileOut           = RoboVoice.ExportSpeech(Config.CurrentVoice);

            SavedStatus.Text            = "Saved!";

            Utilities.FillExported();
            
        }

        private void ListExport_DoubleClicked(object sender, EventArgs e)
        {
            if (ListExport.SelectedItems.Count < 1) return;

            Application.DoEvents();

            Process.Start("explorer.exe", "/select," + ListExport.Items[ListExport.SelectedIndices[0]].Tag);

        }
    }


}
