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
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();


            StartApp();


        }
        
        public void StartApp() {

            this.Close();


            Application.Run(new Main());



        }


    }


}
