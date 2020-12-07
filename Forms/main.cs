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

            Utilities.StartUp();




        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
