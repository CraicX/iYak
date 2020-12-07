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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            cbAzure.Checked = CloudWS.Azure.enabled;
            AzureKey.Text = CloudWS.Azure.key;
            AzureRegion.Text = CloudWS.Azure.region;

        }

        private void cbAzure_CheckedChanged(object sender, EventArgs e)
        {
            gbAzure.Enabled = cbAzure.Checked;
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            CloudWS.Azure.enabled = cbAzure.Checked;
            CloudWS.Azure.key = AzureKey.Text;
            CloudWS.Azure.region = AzureRegion.Text;
            Application.DoEvents();
            Utilities.SaveCloudCreds();

            Config.frmSettings.Hide();
            
            
        }
    }
}
