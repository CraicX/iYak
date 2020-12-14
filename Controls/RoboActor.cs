using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

using iYak.Classes;
namespace iYak.Controls
{
    public partial class RoboActor : UserControl
    {
        static private int ControlCounter          = 1;
        static private StringFormat SpeechIDFormat = new StringFormat();



        public int ActorID      = 0;
        public string Speech    = "";
        public Voice voice      = new Voice();
        public ControlType Type = ControlType.Speech;
        public enum ControlType 
        {
            Speech,
            Actor
        }



        public RoboActor()
        {
            InitializeComponent();

            ControlCounter++;

            AddHandlers();

        }

        public RoboActor(int _ActorID, string _Speech, Voice _voice)
        {

            InitializeComponent();

            ActorID  = _ActorID;
            Speech   = _Speech;
            voice    = _voice;

            if (ActorID == 0) ActorID = ControlCounter;

            lblNickname.Text = voice.Nickname;

            ControlCounter++;

            AddHandlers();

        }
        
        

        public void SetAvatar(string _Avatar)
        {

            foreach (PictureBox pbox in Config.FAvatars.Controls)
            {
                Console.WriteLine(pbox.Tag + " -- " + _Avatar);

                if (pbox.Tag.ToString() == _Avatar) {
                    pbActor.Image = pbox.Image;
                    return;
                }
            }
        }

        public void AddHandlers()
        {
            RoboActor.SpeechIDFormat.Alignment = StringAlignment.Far;

            lblDelete.Visible = false;
            lblEdit.Visible   = false;

            foreach (Control control in this.Controls)
            {
                control.Click      += new EventHandler(RoboActor.MClick);
                control.MouseEnter += new EventHandler(RoboActor.MEnter);
                control.MouseLeave += new EventHandler(RoboActor.MLeave);
            }

            pbActor.Paint += new System.Windows.Forms.PaintEventHandler(this.PBActor_Paint);

        }

        static public void MClick(object sender, EventArgs e)
        {
            Control control  = (Control)sender;
            RoboActor parent = (RoboActor)control.Parent;
            
            Console.WriteLine(control.Name + ">click");
            
            switch(control.Name)
            {
                case "lblDelete":
                    parent.ConfirmDelete();
                    break;

                case "lblEdit":
                    parent.EditSpeech();
                    break;
            }

        }

        private void PBActor_Paint(object sender, PaintEventArgs e)
        {

            if (this.Type == ControlType.Actor) return;

            using (Font myFont = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                e.Graphics.DrawString("#"+ActorID, myFont, Brushes.Yellow, new Point(75, 60), RoboActor.SpeechIDFormat);
            }
        }

        static public void MEnter(object sender, EventArgs e)
        {
            Control control  = (Control)sender;
            RoboActor parent = (RoboActor)control.Parent;

            //  Dont show Edit option for Actors
            if (parent.Type == ControlType.Actor) {

                parent.lblDelete.Visible = true;
            
            } else {

                parent.lblDelete.Visible = true;
                parent.lblEdit.Visible   = true;

            }

            parent.timer1.Enabled    = false;
            
            Console.WriteLine(control.Name + ">enter");

        }

        static public void MLeave(object sender, EventArgs e)
        {
            Control control  = (Control)sender;
            RoboActor parent = (RoboActor)control.Parent;

            parent.timer1.Enabled = true;
            
            Console.WriteLine(control.Name + ">leave");

        }


        
        private void EditSpeech()
        {
            Config.CurrentVoice            = this.voice;
            Config.CurrentFace.Image       = this.pbActor.Image;
            Config.mainRef.tbNickname.Text = this.voice.Nickname;
            Config.mainRef.SayBox.Text     = this.Speech;

            Config.mainRef.SetTuningToVoice();
            Config.mainRef.UpdateVoiceInfo(Config.CurrentVoice);

        }


        private void ConfirmDelete() 
        {
            const string c_message = "Are you sure that you would like to delete this [[TYPE]]?";
            const string c_caption = "Delete [[TYPE]]";

            string message = c_message.Replace("[[TYPE]]", this.Type == ControlType.Actor ? "Actor" : "Speech");
            string caption = c_caption.Replace("[[TYPE]]", this.Type == ControlType.Actor ? "Actor" : "Speech");

            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                // cancel delete
                return;
            }
            this.Dispose();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled    = false;
            
            lblDelete.Visible = false;
            lblEdit.Visible   = false;
        }

        
    }
}
