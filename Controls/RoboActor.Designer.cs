
namespace iYak.Controls
{
    partial class RoboActor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoboActor));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblDelete = new System.Windows.Forms.Label();
            this.lblEdit = new System.Windows.Forms.Label();
            this.pbActor = new System.Windows.Forms.PictureBox();
            this.lblNickname = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbActor)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // lblDelete
            // 
            this.lblDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDelete.BackColor = System.Drawing.Color.Indigo;
            this.lblDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDelete.Image = ((System.Drawing.Image)(resources.GetObject("lblDelete.Image")));
            this.lblDelete.Location = new System.Drawing.Point(60, 1);
            this.lblDelete.Margin = new System.Windows.Forms.Padding(0);
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(16, 16);
            this.lblDelete.TabIndex = 12;
            this.lblDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDelete.UseCompatibleTextRendering = true;
            this.lblDelete.UseMnemonic = false;
            // 
            // lblEdit
            // 
            this.lblEdit.BackColor = System.Drawing.Color.Transparent;
            this.lblEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblEdit.Image = ((System.Drawing.Image)(resources.GetObject("lblEdit.Image")));
            this.lblEdit.Location = new System.Drawing.Point(2, 18);
            this.lblEdit.Margin = new System.Windows.Forms.Padding(0);
            this.lblEdit.Name = "lblEdit";
            this.lblEdit.Size = new System.Drawing.Size(16, 16);
            this.lblEdit.TabIndex = 11;
            this.lblEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEdit.UseCompatibleTextRendering = true;
            this.lblEdit.UseMnemonic = false;
            // 
            // pbActor
            // 
            this.pbActor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbActor.Image = global::iYak.Properties.Resources.voice;
            this.pbActor.Location = new System.Drawing.Point(0, 16);
            this.pbActor.Margin = new System.Windows.Forms.Padding(0);
            this.pbActor.Name = "pbActor";
            this.pbActor.Size = new System.Drawing.Size(78, 68);
            this.pbActor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbActor.TabIndex = 9;
            this.pbActor.TabStop = false;
            // 
            // lblNickname
            // 
            this.lblNickname.BackColor = System.Drawing.Color.Indigo;
            this.lblNickname.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNickname.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblNickname.ForeColor = System.Drawing.Color.Linen;
            this.lblNickname.Location = new System.Drawing.Point(0, 0);
            this.lblNickname.Name = "lblNickname";
            this.lblNickname.Size = new System.Drawing.Size(78, 16);
            this.lblNickname.TabIndex = 10;
            this.lblNickname.Text = "nickname";
            this.lblNickname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RoboActor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblDelete);
            this.Controls.Add(this.lblEdit);
            this.Controls.Add(this.pbActor);
            this.Controls.Add(this.lblNickname);
            this.Name = "RoboActor";
            this.Size = new System.Drawing.Size(78, 84);
            ((System.ComponentModel.ISupportInitialize)(this.pbActor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblDelete;
        private System.Windows.Forms.Label lblEdit;
        public System.Windows.Forms.PictureBox pbActor;
        public System.Windows.Forms.Label lblNickname;
    }
}
