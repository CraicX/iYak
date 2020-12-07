using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iYak
{
    public partial class ViewSplash : Form
    {
        public ViewSplash()
        {
            InitializeComponent();
        }

        public void ShowStatus( string msg, int currentPos, int maxPos ) 
        {

            lblStatus.Text = msg;
            pBar.Maximum   = maxPos;
            pBar.Value     = currentPos;
            Application.DoEvents();

        }
    }
}
