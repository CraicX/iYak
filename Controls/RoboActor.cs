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

        private static readonly Color bg_inactive = Color.Indigo;
        private static readonly Color bg_active   = Color.DodgerBlue;


        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::::: C O N S T R U C T O R S
        // ────────────────────────────────────────────────────────────────────────
        //
        public RoboActor()
        {
            InitializeComponent();

            ControlCounter++;

            AddHandlers();

        }

        public RoboActor(int _ActorID, string _Speech, Voice _voice)
        {

            InitializeComponent();

            this.ActorID  = _ActorID;
            this.Speech   = _Speech;
            this.voice    = _voice;

            if (this.ActorID == 0) this.ActorID = ControlCounter;


            ControlCounter++;

            this.AddHandlers();

            this.RefreshActor();

            

        }

        public RoboActor(Voice _voice, ControlType _controlType)
        {
            InitializeComponent();

            this.Type  = _controlType;
            this.voice = _voice;

            if (this.Type == ControlType.Speech) ControlCounter++;

            this.AddHandlers();

            this.RefreshActor();


            
        }
        
        public void RefreshActor()
        {
            this.lblNickname.Text = this.voice.Nickname;
            
            if (this.voice.Avatar != "") this.SetAvatar(this.voice.Avatar);

        }

        public void SetAvatar(string _Avatar)
        {

            foreach (PictureBox pbox in Config.FAvatars.Controls)
            {
                if (pbox.Tag.ToString() == _Avatar) {
                    pbActor.Image = pbox.Image;
                    return;
                }
            }
        }

        


        
        private void EditSpeech()
        {
            Config.CurrentVoice            = this.voice.Copy();
            Config.CurrentFace.Image       = this.pbActor.Image;
            Config.mainRef.tbNickname.Text = this.voice.Nickname;
            Config.mainRef.SayBox.Text     = this.Speech;

            Config.mainRef.SetTuningToVoice();
            Config.mainRef.UpdateVoiceInfo(Config.CurrentVoice);

            RoboActor.Activate(this);

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

        static public void Activate(RoboActor _robo)
        {
            foreach( RoboActor actor in Config.FScripts.Controls) {
                if (RoboActor.Equals(actor,_robo)) {
                    actor.lblNickname.BackColor = bg_active;
                } else {
                    actor.lblNickname.BackColor = bg_inactive;
                }
            }
        }
        
        static public RoboActor GetSelected()
        {
            foreach (RoboActor actor in Config.FScripts.Controls)
            {
                if (actor.lblNickname.BackColor == bg_active) return actor;
            }

            return null;

        }
        
        public void SetActor()
        {
            Config.CurrentVoice            = this.voice.Copy();
            Config.CurrentFace.Image       = this.pbActor.Image;
            Config.mainRef.tbNickname.Text = this.voice.Nickname;

            Config.mainRef.SetTuningToVoice();
            Config.mainRef.UpdateVoiceInfo(Config.CurrentVoice);
        }

        public void SaySpeech()
        {
            if (this.Speech == "") return;

            RoboVoice robo = new RoboVoice(this.voice);

            robo.Say(this.Speech);

        }

        //
        // ────────────────────────────────────────────────────────────────────────
        //   :::::: H A N D L E R S
        // ────────────────────────────────────────────────────────────────────────
        //
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


        /**
         * ---------------------------------------------------
         * MOUSE CLICK
         * ---------------------------------------------------
         */
        static public void MClick(object sender, EventArgs e)
        {
            Control control  = (Control)sender;
            RoboActor parent = (RoboActor)control.Parent;

            Console.WriteLine(control.Name + ">click");

            switch (control.Name)
            {
                case "lblDelete":
                    parent.ConfirmDelete();
                    break;

                case "lblEdit":
                    parent.EditSpeech();
                    break;

                case "pbActor":
                    if (parent.Type == ControlType.Actor) parent.SetActor();
                    else parent.SaySpeech();
                    break;
            }

        }

        
        
        /**
         * ---------------------------------------------------
         * MOUSE ENTER
         * ---------------------------------------------------
         */
        static public void MEnter(object sender, EventArgs e)
        {
            Control control  = (Control)sender;
            RoboActor parent = (RoboActor)control.Parent;

            //  Dont show Edit option for Actors
            if (parent.Type == ControlType.Actor)
            {

                parent.lblDelete.Visible = true;

            }
            else
            {

                parent.lblDelete.Visible = true;
                parent.lblEdit.Visible   = true;

            }

            parent.timer1.Enabled = false;

            Console.WriteLine(control.Name + ">enter");

        }


        /**
         * ---------------------------------------------------
         * MOUSE LEAVE
         * ---------------------------------------------------
         */
        static public void MLeave(object sender, EventArgs e)
        {
            Control control  = (Control)sender;
            RoboActor parent = (RoboActor)control.Parent;

            parent.timer1.Enabled = true;

            Console.WriteLine(control.Name + ">leave");

        }


        private void PBActor_Paint(object sender, PaintEventArgs e)
        {

            if (this.Type == ControlType.Actor) return;

            using (Font myFont = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                e.Graphics.DrawString("#" + ActorID, myFont, Brushes.Yellow, new Point(75, 60), RoboActor.SpeechIDFormat);
            }
        }


        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled    = false;
            
            lblDelete.Visible = false;
            lblEdit.Visible   = false;
        }

        
    }
}
