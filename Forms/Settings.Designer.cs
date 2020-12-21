
namespace iYak
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSave1 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tb_export = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Pb_aws = new System.Windows.Forms.PictureBox();
            this.gbAWS = new System.Windows.Forms.GroupBox();
            this.AWSRegion = new System.Windows.Forms.TextBox();
            this.AWSKey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbAWS = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Pb_gcloud = new System.Windows.Forms.PictureBox();
            this.gbGCloud = new System.Windows.Forms.GroupBox();
            this.GCloudRegion = new System.Windows.Forms.TextBox();
            this.GCloudKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbGCloud = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Pb_azure = new System.Windows.Forms.PictureBox();
            this.gbAzure = new System.Windows.Forms.GroupBox();
            this.AzureRegion = new System.Windows.Forms.TextBox();
            this.AzureKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbAzure = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnSave3 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.TTSLV = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRegion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGender = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fbrowse1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lblRestart = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_aws)).BeginInit();
            this.gbAWS.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_gcloud)).BeginInit();
            this.gbGCloud.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_azure)).BeginInit();
            this.gbAzure.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(540, 411);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSave1);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(532, 382);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSave1
            // 
            this.btnSave1.Location = new System.Drawing.Point(443, 342);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new System.Drawing.Size(79, 34);
            this.btnSave1.TabIndex = 3;
            this.btnSave1.Text = "OK";
            this.btnSave1.UseVisualStyleBackColor = true;
            this.btnSave1.Click += new System.EventHandler(this.BtnSave1_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.button1);
            this.groupBox5.Controls.Add(this.tb_export);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(10, 158);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(512, 95);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Set up Paths";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(79, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(192, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Save exported sound files to this folder:";
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.Location = new System.Drawing.Point(6, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 57);
            this.button1.TabIndex = 1;
            this.button1.Text = "Browse";
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_export
            // 
            this.tb_export.Location = new System.Drawing.Point(80, 48);
            this.tb_export.Name = "tb_export";
            this.tb_export.Size = new System.Drawing.Size(415, 20);
            this.tb_export.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.textBox5);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(8, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(515, 136);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "New Speeches";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(247, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Start new speeches with the following prefilled text:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(6, 39);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox5.Size = new System.Drawing.Size(503, 56);
            this.textBox5.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.btnSave);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(532, 382);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cloud Setup";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.Pb_aws);
            this.groupBox4.Controls.Add(this.gbAWS);
            this.groupBox4.Controls.Add(this.cbAWS);
            this.groupBox4.Location = new System.Drawing.Point(8, 228);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(516, 102);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Amazon AWS";
            // 
            // Pb_aws
            // 
            this.Pb_aws.Image = ((System.Drawing.Image)(resources.GetObject("Pb_aws.Image")));
            this.Pb_aws.Location = new System.Drawing.Point(6, 43);
            this.Pb_aws.Name = "Pb_aws";
            this.Pb_aws.Size = new System.Drawing.Size(86, 45);
            this.Pb_aws.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pb_aws.TabIndex = 3;
            this.Pb_aws.TabStop = false;
            // 
            // gbAWS
            // 
            this.gbAWS.Controls.Add(this.AWSRegion);
            this.gbAWS.Controls.Add(this.AWSKey);
            this.gbAWS.Controls.Add(this.label5);
            this.gbAWS.Controls.Add(this.label6);
            this.gbAWS.Enabled = false;
            this.gbAWS.Location = new System.Drawing.Point(100, 14);
            this.gbAWS.Name = "gbAWS";
            this.gbAWS.Size = new System.Drawing.Size(402, 74);
            this.gbAWS.TabIndex = 2;
            this.gbAWS.TabStop = false;
            // 
            // AWSRegion
            // 
            this.AWSRegion.Location = new System.Drawing.Point(63, 42);
            this.AWSRegion.Name = "AWSRegion";
            this.AWSRegion.Size = new System.Drawing.Size(168, 20);
            this.AWSRegion.TabIndex = 3;
            // 
            // AWSKey
            // 
            this.AWSKey.Location = new System.Drawing.Point(63, 15);
            this.AWSKey.Name = "AWSKey";
            this.AWSKey.Size = new System.Drawing.Size(313, 20);
            this.AWSKey.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Region";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "API Key";
            // 
            // cbAWS
            // 
            this.cbAWS.AutoSize = true;
            this.cbAWS.Location = new System.Drawing.Point(18, 19);
            this.cbAWS.Name = "cbAWS";
            this.cbAWS.Size = new System.Drawing.Size(59, 17);
            this.cbAWS.TabIndex = 1;
            this.cbAWS.Text = "Enable";
            this.cbAWS.UseVisualStyleBackColor = true;
            this.cbAWS.CheckedChanged += new System.EventHandler(this.cbAWS_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(445, 342);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(79, 34);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "OK";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Pb_gcloud);
            this.groupBox2.Controls.Add(this.gbGCloud);
            this.groupBox2.Controls.Add(this.cbGCloud);
            this.groupBox2.Location = new System.Drawing.Point(8, 123);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(516, 102);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "GCloud Services";
            // 
            // Pb_gcloud
            // 
            this.Pb_gcloud.Image = ((System.Drawing.Image)(resources.GetObject("Pb_gcloud.Image")));
            this.Pb_gcloud.Location = new System.Drawing.Point(6, 44);
            this.Pb_gcloud.Name = "Pb_gcloud";
            this.Pb_gcloud.Size = new System.Drawing.Size(86, 45);
            this.Pb_gcloud.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pb_gcloud.TabIndex = 3;
            this.Pb_gcloud.TabStop = false;
            // 
            // gbGCloud
            // 
            this.gbGCloud.Controls.Add(this.GCloudRegion);
            this.gbGCloud.Controls.Add(this.GCloudKey);
            this.gbGCloud.Controls.Add(this.label3);
            this.gbGCloud.Controls.Add(this.label4);
            this.gbGCloud.Enabled = false;
            this.gbGCloud.Location = new System.Drawing.Point(100, 15);
            this.gbGCloud.Name = "gbGCloud";
            this.gbGCloud.Size = new System.Drawing.Size(402, 74);
            this.gbGCloud.TabIndex = 2;
            this.gbGCloud.TabStop = false;
            // 
            // GCloudRegion
            // 
            this.GCloudRegion.Location = new System.Drawing.Point(63, 42);
            this.GCloudRegion.Name = "GCloudRegion";
            this.GCloudRegion.Size = new System.Drawing.Size(168, 20);
            this.GCloudRegion.TabIndex = 3;
            // 
            // GCloudKey
            // 
            this.GCloudKey.Location = new System.Drawing.Point(63, 15);
            this.GCloudKey.Name = "GCloudKey";
            this.GCloudKey.Size = new System.Drawing.Size(313, 20);
            this.GCloudKey.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Region";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "API Key";
            // 
            // cbGCloud
            // 
            this.cbGCloud.AutoSize = true;
            this.cbGCloud.Location = new System.Drawing.Point(18, 19);
            this.cbGCloud.Name = "cbGCloud";
            this.cbGCloud.Size = new System.Drawing.Size(59, 17);
            this.cbGCloud.TabIndex = 1;
            this.cbGCloud.Text = "Enable";
            this.cbGCloud.UseVisualStyleBackColor = true;
            this.cbGCloud.CheckedChanged += new System.EventHandler(this.cbGCloud_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Pb_azure);
            this.groupBox1.Controls.Add(this.gbAzure);
            this.groupBox1.Controls.Add(this.cbAzure);
            this.groupBox1.Location = new System.Drawing.Point(8, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(516, 102);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Azure Services";
            // 
            // Pb_azure
            // 
            this.Pb_azure.Enabled = false;
            this.Pb_azure.Image = ((System.Drawing.Image)(resources.GetObject("Pb_azure.Image")));
            this.Pb_azure.Location = new System.Drawing.Point(6, 43);
            this.Pb_azure.Name = "Pb_azure";
            this.Pb_azure.Size = new System.Drawing.Size(86, 45);
            this.Pb_azure.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pb_azure.TabIndex = 2;
            this.Pb_azure.TabStop = false;
            // 
            // gbAzure
            // 
            this.gbAzure.Controls.Add(this.AzureRegion);
            this.gbAzure.Controls.Add(this.AzureKey);
            this.gbAzure.Controls.Add(this.label2);
            this.gbAzure.Controls.Add(this.label1);
            this.gbAzure.Enabled = false;
            this.gbAzure.Location = new System.Drawing.Point(100, 14);
            this.gbAzure.Name = "gbAzure";
            this.gbAzure.Size = new System.Drawing.Size(402, 74);
            this.gbAzure.TabIndex = 1;
            this.gbAzure.TabStop = false;
            // 
            // AzureRegion
            // 
            this.AzureRegion.Location = new System.Drawing.Point(63, 42);
            this.AzureRegion.Name = "AzureRegion";
            this.AzureRegion.Size = new System.Drawing.Size(168, 20);
            this.AzureRegion.TabIndex = 3;
            // 
            // AzureKey
            // 
            this.AzureKey.Location = new System.Drawing.Point(63, 15);
            this.AzureKey.Name = "AzureKey";
            this.AzureKey.Size = new System.Drawing.Size(313, 20);
            this.AzureKey.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Region";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "API Key";
            // 
            // cbAzure
            // 
            this.cbAzure.AutoSize = true;
            this.cbAzure.Location = new System.Drawing.Point(18, 22);
            this.cbAzure.Name = "cbAzure";
            this.cbAzure.Size = new System.Drawing.Size(59, 17);
            this.cbAzure.TabIndex = 0;
            this.cbAzure.Text = "Enable";
            this.cbAzure.UseVisualStyleBackColor = true;
            this.cbAzure.CheckedChanged += new System.EventHandler(this.cbAzure_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lblRestart);
            this.tabPage3.Controls.Add(this.btnSave3);
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(532, 382);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Local Voices";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnSave3
            // 
            this.btnSave3.Location = new System.Drawing.Point(445, 342);
            this.btnSave3.Name = "btnSave3";
            this.btnSave3.Size = new System.Drawing.Size(79, 34);
            this.btnSave3.TabIndex = 3;
            this.btnSave3.Text = "OK";
            this.btnSave3.UseVisualStyleBackColor = true;
            this.btnSave3.Click += new System.EventHandler(this.btnSave3_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.TTSLV);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(6, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(522, 327);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Microsoft TTS";
            // 
            // TTSLV
            // 
            this.TTSLV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TTSLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colPath,
            this.colRegion,
            this.colGender,
            this.colStatus});
            this.TTSLV.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TTSLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TTSLV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TTSLV.FullRowSelect = true;
            this.TTSLV.GridLines = true;
            this.TTSLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.TTSLV.HideSelection = false;
            this.TTSLV.LabelWrap = false;
            this.TTSLV.Location = new System.Drawing.Point(3, 16);
            this.TTSLV.MultiSelect = false;
            this.TTSLV.Name = "TTSLV";
            this.TTSLV.ShowGroups = false;
            this.TTSLV.Size = new System.Drawing.Size(516, 308);
            this.TTSLV.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.TTSLV.TabIndex = 0;
            this.TTSLV.UseCompatibleStateImageBehavior = false;
            this.TTSLV.View = System.Windows.Forms.View.Details;
            this.TTSLV.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TTSLV_DoubleClicked);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 162;
            // 
            // colPath
            // 
            this.colPath.Text = "Path";
            this.colPath.Width = 154;
            // 
            // colRegion
            // 
            this.colRegion.Text = "Region";
            // 
            // colGender
            // 
            this.colGender.Text = "Gender";
            this.colGender.Width = 58;
            // 
            // colStatus
            // 
            this.colStatus.Text = "-";
            this.colStatus.Width = 54;
            // 
            // lblRestart
            // 
            this.lblRestart.AutoSize = true;
            this.lblRestart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRestart.ForeColor = System.Drawing.Color.Crimson;
            this.lblRestart.Location = new System.Drawing.Point(95, 353);
            this.lblRestart.Name = "lblRestart";
            this.lblRestart.Size = new System.Drawing.Size(270, 13);
            this.lblRestart.TabIndex = 4;
            this.lblRestart.Text = "Restart application for changes to take effect!";
            this.lblRestart.Visible = false;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 411);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_aws)).EndInit();
            this.gbAWS.ResumeLayout(false);
            this.gbAWS.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_gcloud)).EndInit();
            this.gbGCloud.ResumeLayout(false);
            this.gbGCloud.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_azure)).EndInit();
            this.gbAzure.ResumeLayout(false);
            this.gbAzure.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbAzure;
        public System.Windows.Forms.TextBox AzureRegion;
        public System.Windows.Forms.TextBox AzureKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.CheckBox cbAzure;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox gbAWS;
        public System.Windows.Forms.TextBox AWSRegion;
        public System.Windows.Forms.TextBox AWSKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.CheckBox cbAWS;
        private System.Windows.Forms.GroupBox gbGCloud;
        public System.Windows.Forms.TextBox GCloudRegion;
        public System.Windows.Forms.TextBox GCloudKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.CheckBox cbGCloud;
        private System.Windows.Forms.PictureBox Pb_aws;
        private System.Windows.Forms.PictureBox Pb_gcloud;
        private System.Windows.Forms.PictureBox Pb_azure;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox tb_export;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.FolderBrowserDialog fbrowse1;
        private System.Windows.Forms.Button btnSave1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ListView TTSLV;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.ColumnHeader colRegion;
        private System.Windows.Forms.ColumnHeader colGender;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.Button btnSave3;
        private System.Windows.Forms.Label lblRestart;
    }
}