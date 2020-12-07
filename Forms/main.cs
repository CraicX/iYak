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

        private void XMLToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Settings frmSettings = new Settings();
            frmSettings.ShowDialog();


        }

        private void Main_Load(object sender, EventArgs e)
        {

            Config.splasher = new ViewSplash();
            Config.splasher.Show();

            Utilities.StartUp();




        }
    }
}
