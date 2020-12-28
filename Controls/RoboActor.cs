//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     RoboActor.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  Custom control for displaying Avatar and storing Voice options
//
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private static readonly StringFormat SpeechIDFormat = new StringFormat();



        public int ActorID      = 0;
        public string Speech    = "";
        public Voice voice      = new Voice();
        public ControlType Type = ControlType.Speech;
        public string tipText;
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

        public void SetAvatar(string _Avatar="")
        {
            if (_Avatar == "") _Avatar = this.voice.Avatar;

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
            
            if(this.Type == ControlType.Actor) {
                Datax.DeleteActor(this.voice.Uid, Config.CurrentPlaylist.Uid);
            } else {
                Datax.DeleteSpeech(this.voice.Uid, Config.CurrentPlaylist.Uid);
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

            this.voice.Speech = this.Speech;

            RoboVoice.Speak(this.voice);


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
                string[] tipWords        = parent.voice.Speech.Split(' ');
                int curLength            = 0;
                parent.tipText           = "";
                foreach (string tipWord in tipWords)
                {
                    parent.tipText += tipWord + ' ';

                    curLength += tipWord.Length;
                    if (curLength > 30)
                    {
                        parent.tipText += Environment.NewLine;
                        curLength = 0;
                    }
                }
                Config.mainRef.tip.SetToolTip(parent.pbActor, parent.tipText);

            }

            parent.timer1.Enabled = false;


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


    //
    // ────────────────────────────────────────────────────────────────────────
    //   :::::: T O O L  T I P
    // ────────────────────────────────────────────────────────────────────────
    //
    class CustomToolTip : ToolTip
    {
        public CustomToolTip()
        {
            this.OwnerDraw = true;
            //this.Popup    += new PopupEventHandler(this.OnPopup);
            //this.Draw     += new DrawToolTipEventHandler(this.OnDraw);
            this.IsBalloon = true;
            
            
        }

        private void OnPopup(object sender, PopupEventArgs e) // use this event to set the size of the tool tip
        {
            //e.ToolTipSize = new Size(250, 100);
        }

        private void OnDraw(object sender, DrawToolTipEventArgs e) // use this event to customise the tool tip
        {
            //Graphics g = e.Graphics;

            //LinearGradientBrush b = new LinearGradientBrush(e.Bounds,
            //    Color.GreenYellow, Color.MintCream, 45f);

            //g.FillRectangle(b, e.Bounds);

            //g.DrawRectangle(new Pen(Brushes.Red, 1), new Rectangle(e.Bounds.X, e.Bounds.Y,
            //    e.Bounds.Width - 1, e.Bounds.Height - 1));

            //g.DrawString(
            //    e.ToolTipText, 
            //    new Font("Open Sans", 12.0F, FontStyle.Bold, GraphicsUnit.Pixel), 
            //    Brushes.Silver,
            //    new PointF(e.Bounds.X + 6, e.Bounds.Y + 6)
            //); // shadow layer
            
            //g.DrawString(
            //    e.ToolTipText, 
            //    new Font("Open Sans", 12.0F, FontStyle.Bold, GraphicsUnit.Pixel), 
            //    Brushes.Black,
            //    new PointF(e.Bounds.X + 5, e.Bounds.Y + 5)
            //); // top layer

            //b.Dispose();
        }
    }
}
