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
            this.numericUpDownStepSize = new System.Windows.Forms.NumericUpDown();
            this.labelStepSize = new System.Windows.Forms.Label();
            this.labelDateTo = new System.Windows.Forms.Label();
            this.labelDateFrom = new System.Windows.Forms.Label();
            this.radioButtonDateRange = new System.Windows.Forms.RadioButton();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.radioButtonDateSingle = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBoxModels = new System.Windows.Forms.ComboBox();
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coordinateFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decimalDegreesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.degreesMinutesAndSecondsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutGeoMagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProviderCheck = new System.Windows.Forms.ErrorProvider(this.components);
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.ColumnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDeclination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInclination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnHorizontalIntensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNorthComp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEastComp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVerticalComp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTotalField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStepSize)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(141, 23);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(161, 20);
            this.dateTimePicker1.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.numericUpDownStepSize);
            this.groupBox2.Controls.Add(this.labelStepSize);
            this.groupBox2.Controls.Add(this.labelDateTo);
            this.groupBox2.Controls.Add(this.labelDateFrom);
            this.groupBox2.Controls.Add(this.radioButtonDateRange);
            this.groupBox2.Controls.Add(this.dateTimePicker2);
            this.groupBox2.Controls.Add(this.radioButtonDateSingle);
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Location = new System.Drawing.Point(238, 34);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(315, 130);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Date";
            // 
            // numericUpDownStepSize
            // 
            this.numericUpDownStepSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownStepSize.DecimalPlaces = 3;
            this.numericUpDownStepSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownStepSize.Location = new System.Drawing.Point(141, 93);
            this.numericUpDownStepSize.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownStepSize.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            this.numericUpDownStepSize.Name = "numericUpDownStepSize";
            this.numericUpDownStepSize.Size = new System.Drawing.Size(161, 20);
            this.numericUpDownStepSize.TabIndex = 10;
            this.numericUpDownStepSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numericUpDownStepSize.Visible = false;
            // 
            // labelStepSize
            // 
            this.labelStepSize.AutoSize = true;
            this.labelStepSize.Location = new System.Drawing.Point(77, 97);
            this.labelStepSize.Name = "labelStepSize";
            this.labelStepSize.Size = new System.Drawing.Size(55, 13);
            this.labelStepSize.TabIndex = 9;
            this.labelStepSize.Text = "Step Size:";
            this.labelStepSize.Visible = false;
            // 
            // labelDateTo
            // 
            this.labelDateTo.AutoSize = true;
            this.labelDateTo.Location = new System.Drawing.Point(109, 63);
            this.labelDateTo.Name = "labelDateTo";
            this.labelDateTo.Size = new System.Drawing.Size(23, 13);
            this.labelDateTo.TabIndex = 7;
            this.labelDateTo.Text = "To:";
            this.labelDateTo.Visible = false;
            // 
            // labelDateFrom
            // 
            this.labelDateFrom.AutoSize = true;
            this.labelDateFrom.Location = new System.Drawing.Point(99, 27);
            this.labelDateFrom.Name = "labelDateFrom";
            this.labelDateFrom.Size = new System.Drawing.Size(33, 13);
            this.labelDateFrom.TabIndex = 5;
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
            this.radioButtonDateRange.TabIndex = 4;
            this.radioButtonDateRange.Text = "Range";
            this.radioButtonDateRange.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(141, 59);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(161, 20);
            this.dateTimePicker2.TabIndex = 8;
            this.dateTimePicker2.Visible = false;
            // 
            // radioButtonDateSingle
            // 
            this.radioButtonDateSingle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonDateSingle.AutoSize = true;
            this.radioButtonDateSingle.Checked = true;
            this.radioButtonDateSingle.Location = new System.Drawing.Point(12, 25);
            this.radioButtonDateSingle.Name = "radioButtonDateSingle";
            this.radioButtonDateSingle.Size = new System.Drawing.Size(54, 17);
            this.radioButtonDateSingle.TabIndex = 3;
            this.radioButtonDateSingle.TabStop = true;
            this.radioButtonDateSingle.Text = "Single";
            this.radioButtonDateSingle.UseVisualStyleBackColor = true;
            this.radioButtonDateSingle.CheckedChanged += new System.EventHandler(this.radioButtonDateSingle_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxModels);
            this.groupBox3.Location = new System.Drawing.Point(25, 34);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 130);
            this.groupBox3.TabIndex = 0;
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
            this.comboBoxModels.TabIndex = 1;
            // 
            // textBoxAltitude
            // 
            this.textBoxAltitude.Location = new System.Drawing.Point(63, 93);
            this.textBoxAltitude.Name = "textBoxAltitude";
            this.textBoxAltitude.Size = new System.Drawing.Size(95, 20);
            this.textBoxAltitude.TabIndex = 17;
            this.textBoxAltitude.Tag = "LATITUDE";
            this.textBoxAltitude.Text = "0";
            this.textBoxAltitude.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxAltitude_Validating);
            this.textBoxAltitude.Validated += new System.EventHandler(this.textBoxAltitude_Validated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Elevation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Units:";
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
            this.comboBoxUnits.Location = new System.Drawing.Point(204, 93);
            this.comboBoxUnits.Name = "comboBoxUnits";
            this.comboBoxUnits.Size = new System.Drawing.Size(94, 21);
            this.comboBoxUnits.TabIndex = 19;
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCalculate.Location = new System.Drawing.Point(25, 175);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(845, 33);
            this.buttonCalculate.TabIndex = 20;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // label85
            // 
            this.label85.AutoSize = true;
            this.label85.Location = new System.Drawing.Point(1, 63);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(57, 13);
            this.label85.TabIndex = 14;
            this.label85.Text = "Longitude:";
            // 
            // ComboBoxLatDir
            // 
            this.ComboBoxLatDir.FormattingEnabled = true;
            this.ComboBoxLatDir.Items.AddRange(new object[] {
            "N",
            "S"});
            this.ComboBoxLatDir.Location = new System.Drawing.Point(216, 23);
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
            this.TextBoxLongDeg.Location = new System.Drawing.Point(62, 59);
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
            this.TextBoxLatSec.Location = new System.Drawing.Point(148, 23);
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
            this.TextBoxLongMin.Location = new System.Drawing.Point(105, 59);
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
            this.TextBoxLatMin.Location = new System.Drawing.Point(105, 23);
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
            this.TextBoxLongSec.Location = new System.Drawing.Point(148, 59);
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
            this.TextBoxLatDeg.Location = new System.Drawing.Point(62, 23);
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
            this.ComboBoxLongDir.FormattingEnabled = true;
            this.ComboBoxLongDir.Items.AddRange(new object[] {
            "E",
            "W"});
            this.ComboBoxLongDir.Location = new System.Drawing.Point(216, 59);
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
            this.label80.AutoSize = true;
            this.label80.Location = new System.Drawing.Point(10, 27);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(48, 13);
            this.label80.TabIndex = 12;
            this.label80.Text = "Latitude:";
            // 
            // textBoxLatitudeDecimal
            // 
            this.textBoxLatitudeDecimal.Location = new System.Drawing.Point(62, 23);
            this.textBoxLatitudeDecimal.Name = "textBoxLatitudeDecimal";
            this.textBoxLatitudeDecimal.Size = new System.Drawing.Size(236, 20);
            this.textBoxLatitudeDecimal.TabIndex = 13;
            this.textBoxLatitudeDecimal.Tag = "LATITUDE";
            this.textBoxLatitudeDecimal.Text = "0";
            this.textBoxLatitudeDecimal.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBoxLatitudeDecimal.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxLatitudeDecimal_Validating);
            this.textBoxLatitudeDecimal.Validated += new System.EventHandler(this.textBoxLatitudeDecimal_Validated);
            // 
            // textBoxLongitudeDecimal
            // 
            this.textBoxLongitudeDecimal.Location = new System.Drawing.Point(62, 59);
            this.textBoxLongitudeDecimal.Name = "textBoxLongitudeDecimal";
            this.textBoxLongitudeDecimal.Size = new System.Drawing.Size(236, 20);
            this.textBoxLongitudeDecimal.TabIndex = 15;
            this.textBoxLongitudeDecimal.Tag = "LONGITUDE";
            this.textBoxLongitudeDecimal.Text = "0";
            this.textBoxLongitudeDecimal.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxLongitudeDecimal_Validating);
            this.textBoxLongitudeDecimal.Validated += new System.EventHandler(this.textBoxLongitudeDecimal_Validated);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxAltitude);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxLongitudeDecimal);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxLatitudeDecimal);
            this.groupBox1.Controls.Add(this.comboBoxUnits);
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
            this.groupBox1.Location = new System.Drawing.Point(566, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 130);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Location";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(895, 24);
            this.menuStrip1.TabIndex = 22;
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
            this.loadModelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadModelToolStripMenuItem.Text = "Load Model";
            this.loadModelToolStripMenuItem.Click += new System.EventHandler(this.loadModelToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.coordinateFormatToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // coordinateFormatToolStripMenuItem
            // 
            this.coordinateFormatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decimalDegreesToolStripMenuItem,
            this.degreesMinutesAndSecondsToolStripMenuItem});
            this.coordinateFormatToolStripMenuItem.Name = "coordinateFormatToolStripMenuItem";
            this.coordinateFormatToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.coordinateFormatToolStripMenuItem.Text = "Coordinate Format";
            // 
            // decimalDegreesToolStripMenuItem
            // 
            this.decimalDegreesToolStripMenuItem.Checked = true;
            this.decimalDegreesToolStripMenuItem.CheckOnClick = true;
            this.decimalDegreesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.decimalDegreesToolStripMenuItem.Name = "decimalDegreesToolStripMenuItem";
            this.decimalDegreesToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.decimalDegreesToolStripMenuItem.Text = "Decimal Degrees";
            this.decimalDegreesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.decimalDegreesToolStripMenuItem_CheckedChanged);
            // 
            // degreesMinutesAndSecondsToolStripMenuItem
            // 
            this.degreesMinutesAndSecondsToolStripMenuItem.CheckOnClick = true;
            this.degreesMinutesAndSecondsToolStripMenuItem.Name = "degreesMinutesAndSecondsToolStripMenuItem";
            this.degreesMinutesAndSecondsToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.degreesMinutesAndSecondsToolStripMenuItem.Text = "Degrees, Minutes, and Seconds";
            this.degreesMinutesAndSecondsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.degreesMinutesAndSecondsToolStripMenuItem_CheckedChanged);
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
            this.aboutGeoMagToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutGeoMagToolStripMenuItem.Text = "About...";
            this.aboutGeoMagToolStripMenuItem.Click += new System.EventHandler(this.aboutGeoMagToolStripMenuItem_Click);
            // 
            // errorProviderCheck
            // 
            this.errorProviderCheck.ContainerControl = this;
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.AllowUserToAddRows = false;
            this.dataGridViewResults.AllowUserToDeleteRows = false;
            this.dataGridViewResults.AllowUserToOrderColumns = true;
            this.dataGridViewResults.AllowUserToResizeRows = false;
            this.dataGridViewResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDate,
            this.ColumnDeclination,
            this.ColumnInclination,
            this.ColumnHorizontalIntensity,
            this.ColumnNorthComp,
            this.ColumnEastComp,
            this.ColumnVerticalComp,
            this.ColumnTotalField});
            this.dataGridViewResults.Location = new System.Drawing.Point(25, 219);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(845, 162);
            this.dataGridViewResults.TabIndex = 21;
            // 
            // ColumnDate
            // 
            this.ColumnDate.Frozen = true;
            this.ColumnDate.HeaderText = "Date";
            this.ColumnDate.Name = "ColumnDate";
            this.ColumnDate.ReadOnly = true;
            // 
            // ColumnDeclination
            // 
            this.ColumnDeclination.HeaderText = "Declination (+E/-W)";
            this.ColumnDeclination.Name = "ColumnDeclination";
            this.ColumnDeclination.ReadOnly = true;
            // 
            // ColumnInclination
            // 
            this.ColumnInclination.HeaderText = "Inclination (+D/-U)";
            this.ColumnInclination.Name = "ColumnInclination";
            this.ColumnInclination.ReadOnly = true;
            // 
            // ColumnHorizontalIntensity
            // 
            this.ColumnHorizontalIntensity.HeaderText = "Horizontal Intensity";
            this.ColumnHorizontalIntensity.Name = "ColumnHorizontalIntensity";
            this.ColumnHorizontalIntensity.ReadOnly = true;
            // 
            // ColumnNorthComp
            // 
            this.ColumnNorthComp.HeaderText = "North Comp (+N/-S)";
            this.ColumnNorthComp.Name = "ColumnNorthComp";
            this.ColumnNorthComp.ReadOnly = true;
            // 
            // ColumnEastComp
            // 
            this.ColumnEastComp.HeaderText = "East Comp (+E/-W)";
            this.ColumnEastComp.Name = "ColumnEastComp";
            this.ColumnEastComp.ReadOnly = true;
            // 
            // ColumnVerticalComp
            // 
            this.ColumnVerticalComp.HeaderText = "Vertical Comp (+D/-U)";
            this.ColumnVerticalComp.Name = "ColumnVerticalComp";
            this.ColumnVerticalComp.ReadOnly = true;
            // 
            // ColumnTotalField
            // 
            this.ColumnTotalField.HeaderText = "Total Field";
            this.ColumnTotalField.Name = "ColumnTotalField";
            this.ColumnTotalField.ReadOnly = true;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 396);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.buttonCalculate);
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStepSize)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutGeoMagToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ErrorProvider errorProviderCheck;
        private System.Windows.Forms.NumericUpDown numericUpDownStepSize;
        internal System.Windows.Forms.Label labelStepSize;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coordinateFormatToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDeclination;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInclination;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHorizontalIntensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNorthComp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEastComp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVerticalComp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTotalField;
        private System.Windows.Forms.ToolStripMenuItem decimalDegreesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem degreesMinutesAndSecondsToolStripMenuItem;
    }
}

