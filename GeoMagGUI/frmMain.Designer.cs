namespace GeoMagGUI
{
    partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelDateTo = new System.Windows.Forms.Label();
            this.labelDateFrom = new System.Windows.Forms.Label();
            this.radioButtonDateRange = new System.Windows.Forms.RadioButton();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.radioButtonDateSingle = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBoxModels = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxAltitude = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxUnits = new System.Windows.Forms.ComboBox();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.label85 = new System.Windows.Forms.Label();
            this.ComboBoxLatDir = new System.Windows.Forms.ComboBox();
            this.TextBoxLongDeg = new System.Windows.Forms.TextBox();
            this.TextBoxLatSec = new System.Windows.Forms.TextBox();
            this.TextBoxLongMin = new System.Windows.Forms.TextBox();
            this.TextBoxLatMin = new System.Windows.Forms.TextBox();
            this.TextBoxLongSec = new System.Windows.Forms.TextBox();
            this.TextBoxLatDeg = new System.Windows.Forms.TextBox();
            this.ComboBoxLongDir = new System.Windows.Forms.ComboBox();
            this.label80 = new System.Windows.Forms.Label();
            this.textBoxLatitudeDecimal = new System.Windows.Forms.TextBox();
            this.textBoxLongitudeDecimal = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxGeodeticType = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutGeoMagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProviderCheck = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(141, 22);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(161, 20);
            this.dateTimePicker1.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelDateTo);
            this.groupBox2.Controls.Add(this.labelDateFrom);
            this.groupBox2.Controls.Add(this.radioButtonDateRange);
            this.groupBox2.Controls.Add(this.dateTimePicker2);
            this.groupBox2.Controls.Add(this.radioButtonDateSingle);
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Location = new System.Drawing.Point(238, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(315, 96);
            this.groupBox2.TabIndex = 94;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Date";
            // 
            // labelDateTo
            // 
            this.labelDateTo.AutoSize = true;
            this.labelDateTo.Location = new System.Drawing.Point(109, 63);
            this.labelDateTo.Name = "labelDateTo";
            this.labelDateTo.Size = new System.Drawing.Size(23, 13);
            this.labelDateTo.TabIndex = 98;
            this.labelDateTo.Text = "To:";
            this.labelDateTo.Visible = false;
            // 
            // labelDateFrom
            // 
            this.labelDateFrom.AutoSize = true;
            this.labelDateFrom.Location = new System.Drawing.Point(99, 26);
            this.labelDateFrom.Name = "labelDateFrom";
            this.labelDateFrom.Size = new System.Drawing.Size(33, 13);
            this.labelDateFrom.TabIndex = 99;
            this.labelDateFrom.Text = "From:";
            this.labelDateFrom.Visible = false;
            // 
            // radioButtonDateRange
            // 
            this.radioButtonDateRange.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonDateRange.AutoSize = true;
            this.radioButtonDateRange.Location = new System.Drawing.Point(12, 61);
            this.radioButtonDateRange.Name = "radioButtonDateRange";
            this.radioButtonDateRange.Size = new System.Drawing.Size(57, 17);
            this.radioButtonDateRange.TabIndex = 3;
            this.radioButtonDateRange.Text = "Range";
            this.radioButtonDateRange.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(141, 59);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(161, 20);
            this.dateTimePicker2.TabIndex = 5;
            this.dateTimePicker2.Visible = false;
            // 
            // radioButtonDateSingle
            // 
            this.radioButtonDateSingle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonDateSingle.AutoSize = true;
            this.radioButtonDateSingle.Checked = true;
            this.radioButtonDateSingle.Location = new System.Drawing.Point(12, 24);
            this.radioButtonDateSingle.Name = "radioButtonDateSingle";
            this.radioButtonDateSingle.Size = new System.Drawing.Size(54, 17);
            this.radioButtonDateSingle.TabIndex = 2;
            this.radioButtonDateSingle.TabStop = true;
            this.radioButtonDateSingle.Text = "Single";
            this.radioButtonDateSingle.UseVisualStyleBackColor = true;
            this.radioButtonDateSingle.CheckedChanged += new System.EventHandler(this.radioButtonDateSingle_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxModels);
            this.groupBox3.Location = new System.Drawing.Point(22, 46);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 96);
            this.groupBox3.TabIndex = 95;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Model";
            // 
            // comboBoxModels
            // 
            this.comboBoxModels.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxModels.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxModels.FormattingEnabled = true;
            this.comboBoxModels.Location = new System.Drawing.Point(6, 23);
            this.comboBoxModels.Name = "comboBoxModels";
            this.comboBoxModels.Size = new System.Drawing.Size(172, 21);
            this.comboBoxModels.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxAltitude);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.comboBoxUnits);
            this.groupBox4.Location = new System.Drawing.Point(563, 46);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(230, 96);
            this.groupBox4.TabIndex = 95;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Altitude (Above mean sea level)";
            // 
            // textBoxAltitude
            // 
            this.textBoxAltitude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAltitude.Location = new System.Drawing.Point(58, 59);
            this.textBoxAltitude.Name = "textBoxAltitude";
            this.textBoxAltitude.Size = new System.Drawing.Size(151, 20);
            this.textBoxAltitude.TabIndex = 7;
            this.textBoxAltitude.Tag = "LATITUDE";
            this.textBoxAltitude.Text = "0";
            this.textBoxAltitude.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxAltitude_Validating);
            this.textBoxAltitude.Validated += new System.EventHandler(this.textBoxAltitude_Validated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 96;
            this.label1.Text = "Altitude:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 97;
            this.label2.Text = "Unit:";
            // 
            // comboBoxUnits
            // 
            this.comboBoxUnits.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxUnits.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxUnits.FormattingEnabled = true;
            this.comboBoxUnits.Items.AddRange(new object[] {
            "Kilometers",
            "Meters",
            "Feet"});
            this.comboBoxUnits.Location = new System.Drawing.Point(58, 22);
            this.comboBoxUnits.Name = "comboBoxUnits";
            this.comboBoxUnits.Size = new System.Drawing.Size(151, 21);
            this.comboBoxUnits.TabIndex = 6;
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new System.Drawing.Point(22, 264);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(75, 23);
            this.buttonCalculate.TabIndex = 19;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // label85
            // 
            this.label85.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label85.AutoSize = true;
            this.label85.Location = new System.Drawing.Point(488, 34);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(57, 13);
            this.label85.TabIndex = 91;
            this.label85.Text = "Longitude:";
            // 
            // ComboBoxLatDir
            // 
            this.ComboBoxLatDir.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComboBoxLatDir.FormattingEnabled = true;
            this.ComboBoxLatDir.Items.AddRange(new object[] {
            "N",
            "S"});
            this.ComboBoxLatDir.Location = new System.Drawing.Point(425, 30);
            this.ComboBoxLatDir.Name = "ComboBoxLatDir";
            this.ComboBoxLatDir.Size = new System.Drawing.Size(47, 21);
            this.ComboBoxLatDir.TabIndex = 13;
            this.ComboBoxLatDir.Tag = "LATITUDE";
            this.ComboBoxLatDir.Text = "N";
            this.ComboBoxLatDir.Visible = false;
            this.ComboBoxLatDir.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLatitude_Validating);
            this.ComboBoxLatDir.Validated += new System.EventHandler(this.TextBoxLatitude_Validated);
            // 
            // TextBoxLongDeg
            // 
            this.TextBoxLongDeg.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxLongDeg.Location = new System.Drawing.Point(549, 30);
            this.TextBoxLongDeg.Name = "TextBoxLongDeg";
            this.TextBoxLongDeg.Size = new System.Drawing.Size(38, 20);
            this.TextBoxLongDeg.TabIndex = 15;
            this.TextBoxLongDeg.Tag = "LONGITUDE";
            this.TextBoxLongDeg.Text = "0";
            this.TextBoxLongDeg.Visible = false;
            this.TextBoxLongDeg.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLongitude_Validating);
            this.TextBoxLongDeg.Validated += new System.EventHandler(this.TextBoxLongitude_Validated);
            // 
            // TextBoxLatSec
            // 
            this.TextBoxLatSec.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxLatSec.Location = new System.Drawing.Point(357, 30);
            this.TextBoxLatSec.Name = "TextBoxLatSec";
            this.TextBoxLatSec.Size = new System.Drawing.Size(61, 20);
            this.TextBoxLatSec.TabIndex = 12;
            this.TextBoxLatSec.Tag = "LATITUDE";
            this.TextBoxLatSec.Text = "0";
            this.TextBoxLatSec.Visible = false;
            this.TextBoxLatSec.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLatitude_Validating);
            this.TextBoxLatSec.Validated += new System.EventHandler(this.TextBoxLatitude_Validated);
            // 
            // TextBoxLongMin
            // 
            this.TextBoxLongMin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxLongMin.Location = new System.Drawing.Point(592, 31);
            this.TextBoxLongMin.Name = "TextBoxLongMin";
            this.TextBoxLongMin.Size = new System.Drawing.Size(38, 20);
            this.TextBoxLongMin.TabIndex = 16;
            this.TextBoxLongMin.Tag = "LONGITUDE";
            this.TextBoxLongMin.Text = "0";
            this.TextBoxLongMin.Visible = false;
            this.TextBoxLongMin.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLongitude_Validating);
            this.TextBoxLongMin.Validated += new System.EventHandler(this.TextBoxLongitude_Validated);
            // 
            // TextBoxLatMin
            // 
            this.TextBoxLatMin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxLatMin.Location = new System.Drawing.Point(314, 30);
            this.TextBoxLatMin.Name = "TextBoxLatMin";
            this.TextBoxLatMin.Size = new System.Drawing.Size(38, 20);
            this.TextBoxLatMin.TabIndex = 11;
            this.TextBoxLatMin.Tag = "LATITUDE";
            this.TextBoxLatMin.Text = "0";
            this.TextBoxLatMin.Visible = false;
            this.TextBoxLatMin.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLatitude_Validating);
            this.TextBoxLatMin.Validated += new System.EventHandler(this.TextBoxLatitude_Validated);
            // 
            // TextBoxLongSec
            // 
            this.TextBoxLongSec.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxLongSec.Location = new System.Drawing.Point(635, 31);
            this.TextBoxLongSec.Name = "TextBoxLongSec";
            this.TextBoxLongSec.Size = new System.Drawing.Size(61, 20);
            this.TextBoxLongSec.TabIndex = 17;
            this.TextBoxLongSec.Tag = "LONGITUDE";
            this.TextBoxLongSec.Text = "0";
            this.TextBoxLongSec.Visible = false;
            this.TextBoxLongSec.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLongitude_Validating);
            this.TextBoxLongSec.Validated += new System.EventHandler(this.TextBoxLongitude_Validated);
            // 
            // TextBoxLatDeg
            // 
            this.TextBoxLatDeg.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxLatDeg.Location = new System.Drawing.Point(271, 30);
            this.TextBoxLatDeg.Name = "TextBoxLatDeg";
            this.TextBoxLatDeg.Size = new System.Drawing.Size(38, 20);
            this.TextBoxLatDeg.TabIndex = 10;
            this.TextBoxLatDeg.Tag = "LATITUDE";
            this.TextBoxLatDeg.Text = "0";
            this.TextBoxLatDeg.Visible = false;
            this.TextBoxLatDeg.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLatitude_Validating);
            this.TextBoxLatDeg.Validated += new System.EventHandler(this.TextBoxLatitude_Validated);
            // 
            // ComboBoxLongDir
            // 
            this.ComboBoxLongDir.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComboBoxLongDir.FormattingEnabled = true;
            this.ComboBoxLongDir.Items.AddRange(new object[] {
            "E",
            "W"});
            this.ComboBoxLongDir.Location = new System.Drawing.Point(703, 30);
            this.ComboBoxLongDir.Name = "ComboBoxLongDir";
            this.ComboBoxLongDir.Size = new System.Drawing.Size(47, 21);
            this.ComboBoxLongDir.TabIndex = 18;
            this.ComboBoxLongDir.Tag = "LONGITUDE";
            this.ComboBoxLongDir.Text = "W";
            this.ComboBoxLongDir.Visible = false;
            this.ComboBoxLongDir.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLongitude_Validating);
            this.ComboBoxLongDir.Validated += new System.EventHandler(this.TextBoxLongitude_Validated);
            // 
            // label80
            // 
            this.label80.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label80.AutoSize = true;
            this.label80.Location = new System.Drawing.Point(219, 34);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(48, 13);
            this.label80.TabIndex = 90;
            this.label80.Text = "Latitude:";
            // 
            // textBoxLatitudeDecimal
            // 
            this.textBoxLatitudeDecimal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxLatitudeDecimal.Location = new System.Drawing.Point(271, 30);
            this.textBoxLatitudeDecimal.Name = "textBoxLatitudeDecimal";
            this.textBoxLatitudeDecimal.Size = new System.Drawing.Size(201, 20);
            this.textBoxLatitudeDecimal.TabIndex = 9;
            this.textBoxLatitudeDecimal.Tag = "LATITUDE";
            this.textBoxLatitudeDecimal.Text = "0";
            this.textBoxLatitudeDecimal.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBoxLatitudeDecimal.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxLatitudeDecimal_Validating);
            this.textBoxLatitudeDecimal.Validated += new System.EventHandler(this.textBoxLatitudeDecimal_Validated);
            // 
            // textBoxLongitudeDecimal
            // 
            this.textBoxLongitudeDecimal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxLongitudeDecimal.Location = new System.Drawing.Point(549, 30);
            this.textBoxLongitudeDecimal.Name = "textBoxLongitudeDecimal";
            this.textBoxLongitudeDecimal.Size = new System.Drawing.Size(201, 20);
            this.textBoxLongitudeDecimal.TabIndex = 14;
            this.textBoxLongitudeDecimal.Tag = "LONGITUDE";
            this.textBoxLongitudeDecimal.Text = "0";
            this.textBoxLongitudeDecimal.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxLongitudeDecimal_Validating);
            this.textBoxLongitudeDecimal.Validated += new System.EventHandler(this.textBoxLongitudeDecimal_Validated);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxGeodeticType);
            this.groupBox1.Controls.Add(this.textBoxLongitudeDecimal);
            this.groupBox1.Controls.Add(this.textBoxLatitudeDecimal);
            this.groupBox1.Controls.Add(this.label80);
            this.groupBox1.Controls.Add(this.ComboBoxLongDir);
            this.groupBox1.Controls.Add(this.TextBoxLatDeg);
            this.groupBox1.Controls.Add(this.TextBoxLongSec);
            this.groupBox1.Controls.Add(this.TextBoxLatMin);
            this.groupBox1.Controls.Add(this.TextBoxLongMin);
            this.groupBox1.Controls.Add(this.TextBoxLatSec);
            this.groupBox1.Controls.Add(this.TextBoxLongDeg);
            this.groupBox1.Controls.Add(this.ComboBoxLatDir);
            this.groupBox1.Controls.Add(this.label85);
            this.groupBox1.Location = new System.Drawing.Point(22, 169);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(771, 78);
            this.groupBox1.TabIndex = 93;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Coordinates";
            // 
            // comboBoxGeodeticType
            // 
            this.comboBoxGeodeticType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxGeodeticType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxGeodeticType.FormattingEnabled = true;
            this.comboBoxGeodeticType.Items.AddRange(new object[] {
            "Decimal Degrees",
            "Degrees, Minutes, and Seconds"});
            this.comboBoxGeodeticType.Location = new System.Drawing.Point(6, 30);
            this.comboBoxGeodeticType.Name = "comboBoxGeodeticType";
            this.comboBoxGeodeticType.Size = new System.Drawing.Size(194, 21);
            this.comboBoxGeodeticType.TabIndex = 8;
            this.comboBoxGeodeticType.Text = "Decimal Degrees";
            this.comboBoxGeodeticType.SelectedValueChanged += new System.EventHandler(this.comboBoxGeodeticType_SelectedValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(812, 24);
            this.menuStrip1.TabIndex = 96;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadModelToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadModelToolStripMenuItem
            // 
            this.loadModelToolStripMenuItem.Name = "loadModelToolStripMenuItem";
            this.loadModelToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.loadModelToolStripMenuItem.Text = "Load Model";
            this.loadModelToolStripMenuItem.Click += new System.EventHandler(this.loadModelToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(134, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutGeoMagToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutGeoMagToolStripMenuItem
            // 
            this.aboutGeoMagToolStripMenuItem.Name = "aboutGeoMagToolStripMenuItem";
            this.aboutGeoMagToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutGeoMagToolStripMenuItem.Text = "About...";
            // 
            // errorProviderCheck
            // 
            this.errorProviderCheck.ContainerControl = this;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 480);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "GeoMag";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderCheck)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label labelDateTo;
        private System.Windows.Forms.Label labelDateFrom;
        internal System.Windows.Forms.RadioButton radioButtonDateRange;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        internal System.Windows.Forms.RadioButton radioButtonDateSingle;
        private System.Windows.Forms.ComboBox comboBoxModels;
        internal System.Windows.Forms.TextBox textBoxAltitude;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxUnits;
        private System.Windows.Forms.Button buttonCalculate;
        internal System.Windows.Forms.Label label85;
        internal System.Windows.Forms.ComboBox ComboBoxLatDir;
        internal System.Windows.Forms.TextBox TextBoxLongDeg;
        internal System.Windows.Forms.TextBox TextBoxLatSec;
        internal System.Windows.Forms.TextBox TextBoxLongMin;
        internal System.Windows.Forms.TextBox TextBoxLatMin;
        internal System.Windows.Forms.TextBox TextBoxLongSec;
        internal System.Windows.Forms.TextBox TextBoxLatDeg;
        internal System.Windows.Forms.ComboBox ComboBoxLongDir;
        internal System.Windows.Forms.Label label80;
        internal System.Windows.Forms.TextBox textBoxLatitudeDecimal;
        internal System.Windows.Forms.TextBox textBoxLongitudeDecimal;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxGeodeticType;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutGeoMagToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ErrorProvider errorProviderCheck;
    }
}

