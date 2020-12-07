
namespace iYak
{
    partial class ViewSplash
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewSplash));
            this.pBox = new System.Windows.Forms.PictureBox();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pBox
            // 
            this.pBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pBox.BackgroundImage")));
            this.pBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pBox.Location = new System.Drawing.Point(16, 9);
            this.pBox.Margin = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.pBox.Name = "pBox";
            this.pBox.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.pBox.Size = new System.Drawing.Size(407, 453);
            this.pBox.TabIndex = 0;
            this.pBox.TabStop = false;
            // 
            // pBar
            // 
            this.pBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBar.Location = new System.Drawing.Point(0, 465);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(423, 23);
            this.pBar.TabIndex = 1;
            this.pBar.Value = 10;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblStatus.Location = new System.Drawing.Point(16, 449);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(407, 13);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Initializing App";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ViewSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 488);
            this.ControlBox = false;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pBar);
            this.Controls.Add(this.pBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ViewSplash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ViewSplash";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBox;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.Label lblStatus;
    }
}