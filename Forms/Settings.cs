//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     Settings.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  The Settings/Options Form
//
//
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
using System.Drawing.Imaging;
using System.IO;

namespace iYak
{
    public partial class Settings : Form
    {

        static public Settings MyForm;

        Image OrigAzure  = null;
        Image OrigAWS    = null;
        Image OrigGCloud = null;
        public Settings()
        {
            InitializeComponent();

            cbAzure.Checked   = CloudWS.Azure.enabled;
            AzureKey.Text     = CloudWS.Azure.key;
            AzureRegion.Text  = CloudWS.Azure.region;

            cbGCloud.Checked  = CloudWS.GCloud.enabled;
            GCloudKey.Text    = CloudWS.GCloud.key;
            GCloudRegion.Text = CloudWS.GCloud.region;

            cbAWS.Checked     = CloudWS.AWS.enabled;
            AWSKey.Text       = CloudWS.AWS.key;
            AWSRegion.Text    = CloudWS.AWS.region;

            Settings.MyForm = this;

        }

        private void cbAzure_CheckedChanged(object sender, EventArgs e)
        {
            gbAzure.Enabled = cbAzure.Checked;
            if( this.Visible )  Pb_azure.Image = SetAlpha((Bitmap)OrigAzure, cbAzure.Checked ? 200:50 );
        }
        private void cbGCloud_CheckedChanged(object sender, EventArgs e)
        {
            gbGCloud.Enabled = cbGCloud.Checked;
            if( this.Visible )  Pb_gcloud.Image = SetAlpha((Bitmap)OrigGCloud, cbGCloud.Checked ? 200:50 );

        }

        private void cbAWS_CheckedChanged(object sender, EventArgs e)
        {
            gbAWS.Enabled = cbAWS.Checked;
            if( this.Visible )  Pb_aws.Image = SetAlpha((Bitmap)OrigAWS, cbAWS.Checked ? 200:50 );
        }
        public void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            
            
        }

        static Bitmap SetAlpha(Bitmap bmpIn, int alpha)
        {
            Bitmap bmpOut = new Bitmap(bmpIn.Width, bmpIn.Height);
            float a       = alpha / 255f;
            Rectangle r   = new Rectangle(0, 0, bmpIn.Width, bmpIn.Height);

            float[][] matrixItems = {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, a, 0},
                new float[] {0, 0, 0, 0, 1}};

            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

            ImageAttributes imageAtt = new ImageAttributes();
            imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(bmpOut))
                g.DrawImage(bmpIn, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);

            return bmpOut;
        }



        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    L O A D   F O R M
        // ────────────────────────────────────────────────────────────────────────
        //
        // Runs every time user opens the settings
        //
        private void Settings_Load(object sender, EventArgs e)
        {
            if (OrigAzure  == null) OrigAzure   = (Bitmap)Pb_azure.Image.Clone();
            if (OrigAWS    == null) OrigAWS     = (Bitmap)Pb_aws.Image.Clone();
            if (OrigGCloud == null) OrigGCloud  = (Bitmap)Pb_gcloud.Image.Clone();

            if (this.Visible) 
            {
                Pb_azure.Image  = SetAlpha((Bitmap)OrigAzure,  CloudWS.Azure.enabled  ? 200:50);
                Pb_aws.Image    = SetAlpha((Bitmap)OrigAWS,    CloudWS.AWS.enabled    ? 200:50);
                Pb_gcloud.Image = SetAlpha((Bitmap)OrigGCloud, CloudWS.GCloud.enabled ? 200:50);
            }


            ShowLocalVoices();

            lblRestart.Visible   = false;
        
            TBDefaultSpeech.Text = Config.DefaultText;
            TBExport.Text        = Config.ExportPath;






        }

        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    S H O W   L O C A L   V O I C E S
        // ────────────────────────────────────────────────────────────────────────
        //
        // 
        //
        public void ShowLocalVoices() 
        {
            TTSLV.Items.Clear();

            VoiceImport.GetAvailableVoices();


            if (VoiceImport.ListTTS.Count <= 0) return;

            foreach( VoiceImport.VoiceSynth synth in VoiceImport.ListTTS ) {

                ListViewItem lvi = new ListViewItem(synth.Name);
                lvi.UseItemStyleForSubItems = false;

                lvi.SubItems.Add(synth.TokenPath);
                lvi.SubItems.Add(synth.Region);
                lvi.SubItems.Add(synth.Gender);

                if (synth.Installed)
                {

                    lvi.SubItems.Add("ok");

                }
                else
                {

                    ListViewItem.ListViewSubItem iSublink = new ListViewItem.ListViewSubItem()
                    {
                        Text      = "install",
                        Font      = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Pixel, ((byte)(0))),
                        ForeColor = Color.Blue,
                    };
                    
                    lvi.SubItems.Add(iSublink);
                }

                TTSLV.Items.Add(lvi);
            }

        }
        

        private void BtnSave1_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = fbrowse1.ShowDialog();
            if (result == DialogResult.OK)
            {
                TBExport.Text = fbrowse1.SelectedPath;
                
            }
        }



        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::    S A V E   S E T T I N G S
        // ────────────────────────────────────────────────────────────────────────
        //
        // Saves the settings for all tabs
        //
        private void SaveSettings() {
            CloudWS.Azure.enabled  = cbAzure.Checked;
            CloudWS.Azure.key      = AzureKey.Text;
            CloudWS.Azure.region   = AzureRegion.Text;
            CloudWS.GCloud.enabled = cbGCloud.Checked;
            CloudWS.GCloud.key     = GCloudKey.Text;
            CloudWS.GCloud.region  = GCloudRegion.Text;
            CloudWS.AWS.enabled    = cbAWS.Checked;
            CloudWS.AWS.key        = AWSKey.Text;
            CloudWS.AWS.region     = AWSRegion.Text;


            Application.DoEvents();
            Utilities.SaveCloudCreds();


            Config.DefaultText = TBDefaultSpeech.Text.Trim();

            

            if (Main.GetForm().SayBox.Text == "") Main.GetForm().SayBox.Text = Config.DefaultText.Trim();

            //  Settings.ini
            if(!Directory.Exists(TBExport.Text.Trim())) {

                try
                {
                    Directory.CreateDirectory(TBExport.Text.Trim());

                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    TBExport.Text = Config.ExportPath;
                }

            }

            Utilities.SaveSettings();

            Config.frmSettings.Hide();
        }

        
        private void TTSLV_DoubleClicked(object sender, EventArgs e)
        {

            if (TTSLV.SelectedItems.Count <= 0) return;

            foreach( ListViewItem SelItem in TTSLV.SelectedItems )
            {
                Console.WriteLine(SelItem.SubItems[2].Text);
                if (SelItem.SubItems[4].Text != "install") continue;

                var result = MessageBox.Show("Install TTS Voice: " + SelItem.Text, "Install this voice?",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

                // If the no button was pressed ...
                if (result == DialogResult.No) continue;


                VoiceImport.InstallTTS(SelItem.Text);

                lblRestart.Visible = true;


            }
        }
        
    }

}
