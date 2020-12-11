
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
            this.pbActor = new System.Windows.Forms.PictureBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbActor)).BeginInit();
            this.SuspendLayout();
            // 
            // pbActor
            // 
            this.pbActor.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbActor.Image = global::iYak.Properties.Resources.voice;
            this.pbActor.Location = new System.Drawing.Point(0, 16);
            this.pbActor.Margin = new System.Windows.Forms.Padding(0);
            this.pbActor.Name = "pbActor";
            this.pbActor.Size = new System.Drawing.Size(65, 64);
            this.pbActor.TabIndex = 0;
            this.pbActor.TabStop = false;
            this.pbActor.Click += new System.EventHandler(this.pbActor_Click);
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.lblName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(0, 81);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(65, 16);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Johnny";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblID
            // 
            this.lblID.BackColor = System.Drawing.Color.Indigo;
            this.lblID.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblID.ForeColor = System.Drawing.Color.Linen;
            this.lblID.Location = new System.Drawing.Point(0, 0);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(65, 16);
            this.lblID.TabIndex = 2;
            this.lblID.Text = "#1";
            this.lblID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RoboActor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.pbActor);
            this.Controls.Add(this.lblID);
            this.Name = "RoboActor";
            this.Size = new System.Drawing.Size(65, 97);
            ((System.ComponentModel.ISupportInitialize)(this.pbActor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbActor;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblID;
    }
}
