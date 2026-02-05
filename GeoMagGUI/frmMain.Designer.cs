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
            this.numericUpDownStepSize = new System.Windows.Forms.NumericUpDown();
            this.labelStepSize = new System.Windows.Forms.Label();
            this.labelDateTo = new System.Windows.Forms.Label();
            this.labelDateFrom = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.comboBoxModels = new System.Windows.Forms.ComboBox();
            this.textBoxAltitude = new System.Windows.Forms.TextBox();
            this.label_Elevation = new System.Windows.Forms.Label();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.label85 = new System.Windows.Forms.Label();
            this.TextBoxLongDeg = new System.Windows.Forms.TextBox();
            this.TextBoxLatSec = new System.Windows.Forms.TextBox();
            this.TextBoxLongMin = new System.Windows.Forms.TextBox();
            this.TextBoxLatMin = new System.Windows.Forms.TextBox();
            this.TextBoxLongSec = new System.Windows.Forms.TextBox();
            this.TextBoxLatDeg = new System.Windows.Forms.TextBox();
            this.label80 = new System.Windows.Forms.Label();
            this.textBoxLatitudeDecimal = new System.Windows.Forms.TextBox();
            this.textBoxLongitudeDecimal = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemUseRangeOfDates = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonMyLocation = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxAltitudeUnits = new System.Windows.Forms.ComboBox();
            this.ComboBoxLongDir = new System.Windows.Forms.ComboBox();
            this.ComboBoxLatDir = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripButtonCancel = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStepSize)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.dateTimePicker1, 2);
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(373, 11);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(81, 20);
            this.dateTimePicker1.TabIndex = 1;
            this.dateTimePicker1.Validated += new System.EventHandler(this.dateTimePicker_Validated);
            // 
            // numericUpDownStepSize
            // 
            this.numericUpDownStepSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.numericUpDownStepSize, 2);
            this.numericUpDownStepSize.Location = new System.Drawing.Point(713, 11);
            this.numericUpDownStepSize.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownStepSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownStepSize.Name = "numericUpDownStepSize";
            this.numericUpDownStepSize.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownStepSize.TabIndex = 3;
            this.numericUpDownStepSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownStepSize.Visible = false;
            // 
            // labelStepSize
            // 
            this.labelStepSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStepSize.AutoSize = true;
            this.labelStepSize.Location = new System.Drawing.Point(650, 13);
            this.labelStepSize.Name = "labelStepSize";
            this.labelStepSize.Size = new System.Drawing.Size(57, 13);
            this.labelStepSize.TabIndex = 9;
            this.labelStepSize.Text = "Step Size:";
            this.labelStepSize.Visible = false;
            // 
            // labelDateTo
            // 
            this.labelDateTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDateTo.AutoSize = true;
            this.labelDateTo.Location = new System.Drawing.Point(460, 13);
            this.labelDateTo.Name = "labelDateTo";
            this.labelDateTo.Size = new System.Drawing.Size(28, 13);
            this.labelDateTo.TabIndex = 7;
            this.labelDateTo.Text = "To:";
            this.labelDateTo.Visible = false;
            // 
            // labelDateFrom
            // 
            this.labelDateFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDateFrom.AutoSize = true;
            this.labelDateFrom.Location = new System.Drawing.Point(309, 13);
            this.labelDateFrom.Name = "labelDateFrom";
            this.labelDateFrom.Size = new System.Drawing.Size(58, 13);
            this.labelDateFrom.TabIndex = 5;
            this.labelDateFrom.Text = "From:";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.dateTimePicker2, 2);
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(494, 11);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(82, 20);
            this.dateTimePicker2.TabIndex = 2;
            this.dateTimePicker2.Visible = false;
            this.dateTimePicker2.Validated += new System.EventHandler(this.dateTimePicker_Validated);
            // 
            // comboBoxModels
            // 
            this.comboBoxModels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxModels.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxModels.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxModels, 4);
            this.comboBoxModels.FormattingEnabled = true;
            this.comboBoxModels.Location = new System.Drawing.Point(71, 11);
            this.comboBoxModels.Name = "comboBoxModels";
            this.comboBoxModels.Size = new System.Drawing.Size(224, 21);
            this.comboBoxModels.TabIndex = 0;
            // 
            // textBoxAltitude
            // 
            this.textBoxAltitude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxAltitude, 2);
            this.textBoxAltitude.Location = new System.Drawing.Point(713, 43);
            this.textBoxAltitude.Name = "textBoxAltitude";
            this.textBoxAltitude.Size = new System.Drawing.Size(57, 20);
            this.textBoxAltitude.TabIndex = 15;
            this.textBoxAltitude.Tag = "";
            this.textBoxAltitude.Text = "0";
            this.textBoxAltitude.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxAltitude_Validating);
            this.textBoxAltitude.Validated += new System.EventHandler(this.textBoxAltitude_Validated);
            // 
            // label_Elevation
            // 
            this.label_Elevation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Elevation.AutoSize = true;
            this.label_Elevation.Location = new System.Drawing.Point(650, 45);
            this.label_Elevation.Name = "label_Elevation";
            this.label_Elevation.Size = new System.Drawing.Size(57, 13);
            this.label_Elevation.TabIndex = 16;
            this.label_Elevation.Text = "Altitude:";
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.buttonCalculate, 19);
            this.buttonCalculate.Location = new System.Drawing.Point(11, 99);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(808, 25);
            this.buttonCalculate.TabIndex = 18;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // label85
            // 
            this.label85.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label85.AutoSize = true;
            this.label85.Location = new System.Drawing.Point(309, 45);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(58, 13);
            this.label85.TabIndex = 14;
            this.label85.Text = "Longitude:";
            // 
            // TextBoxLongDeg
            // 
            this.TextBoxLongDeg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxLongDeg.Location = new System.Drawing.Point(373, 67);
            this.TextBoxLongDeg.Name = "TextBoxLongDeg";
            this.TextBoxLongDeg.Size = new System.Drawing.Size(43, 20);
            this.TextBoxLongDeg.TabIndex = 10;
            this.TextBoxLongDeg.Tag = "";
            this.TextBoxLongDeg.Text = "0";
            this.TextBoxLongDeg.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLongitude_Validating);
            this.TextBoxLongDeg.Validated += new System.EventHandler(this.TextBoxLongitude_Validated);
            // 
            // TextBoxLatSec
            // 
            this.TextBoxLatSec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxLatSec.Location = new System.Drawing.Point(165, 67);
            this.TextBoxLatSec.Name = "TextBoxLatSec";
            this.TextBoxLatSec.Size = new System.Drawing.Size(78, 20);
            this.TextBoxLatSec.TabIndex = 7;
            this.TextBoxLatSec.Tag = "";
            this.TextBoxLatSec.Text = "0.0000";
            this.TextBoxLatSec.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLatitude_Validating);
            this.TextBoxLatSec.Validated += new System.EventHandler(this.TextBoxLatitude_Validated);
            // 
            // TextBoxLongMin
            // 
            this.TextBoxLongMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxLongMin.Location = new System.Drawing.Point(422, 67);
            this.TextBoxLongMin.Name = "TextBoxLongMin";
            this.TextBoxLongMin.Size = new System.Drawing.Size(32, 20);
            this.TextBoxLongMin.TabIndex = 11;
            this.TextBoxLongMin.Tag = "";
            this.TextBoxLongMin.Text = "0";
            this.TextBoxLongMin.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLongitude_Validating);
            this.TextBoxLongMin.Validated += new System.EventHandler(this.TextBoxLongitude_Validated);
            // 
            // TextBoxLatMin
            // 
            this.TextBoxLatMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxLatMin.Location = new System.Drawing.Point(127, 67);
            this.TextBoxLatMin.Name = "TextBoxLatMin";
            this.TextBoxLatMin.Size = new System.Drawing.Size(32, 20);
            this.TextBoxLatMin.TabIndex = 6;
            this.TextBoxLatMin.Tag = "";
            this.TextBoxLatMin.Text = "0";
            this.TextBoxLatMin.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLatitude_Validating);
            this.TextBoxLatMin.Validated += new System.EventHandler(this.TextBoxLatitude_Validated);
            // 
            // TextBoxLongSec
            // 
            this.TextBoxLongSec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.TextBoxLongSec, 2);
            this.TextBoxLongSec.Location = new System.Drawing.Point(460, 67);
            this.TextBoxLongSec.Name = "TextBoxLongSec";
            this.TextBoxLongSec.Size = new System.Drawing.Size(64, 20);
            this.TextBoxLongSec.TabIndex = 12;
            this.TextBoxLongSec.Tag = "";
            this.TextBoxLongSec.Text = "0.0000";
            this.TextBoxLongSec.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLongitude_Validating);
            this.TextBoxLongSec.Validated += new System.EventHandler(this.TextBoxLongitude_Validated);
            // 
            // TextBoxLatDeg
            // 
            this.TextBoxLatDeg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxLatDeg.Location = new System.Drawing.Point(71, 67);
            this.TextBoxLatDeg.Name = "TextBoxLatDeg";
            this.TextBoxLatDeg.Size = new System.Drawing.Size(50, 20);
            this.TextBoxLatDeg.TabIndex = 5;
            this.TextBoxLatDeg.Tag = "";
            this.TextBoxLatDeg.Text = "0";
            this.TextBoxLatDeg.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLatitude_Validating);
            this.TextBoxLatDeg.Validated += new System.EventHandler(this.TextBoxLatitude_Validated);
            // 
            // label80
            // 
            this.label80.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label80.AutoSize = true;
            this.label80.Location = new System.Drawing.Point(11, 45);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(54, 13);
            this.label80.TabIndex = 12;
            this.label80.Text = "Latitude:";
            // 
            // textBoxLatitudeDecimal
            // 
            this.textBoxLatitudeDecimal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxLatitudeDecimal, 4);
            this.textBoxLatitudeDecimal.Location = new System.Drawing.Point(71, 43);
            this.textBoxLatitudeDecimal.Name = "textBoxLatitudeDecimal";
            this.textBoxLatitudeDecimal.Size = new System.Drawing.Size(224, 20);
            this.textBoxLatitudeDecimal.TabIndex = 4;
            this.textBoxLatitudeDecimal.Tag = "";
            this.textBoxLatitudeDecimal.Text = "0";
            this.textBoxLatitudeDecimal.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBoxLatitudeDecimal.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxLatitudeDecimal_Validating);
            this.textBoxLatitudeDecimal.Validated += new System.EventHandler(this.textBoxLatitudeDecimal_Validated);
            // 
            // textBoxLongitudeDecimal
            // 
            this.textBoxLongitudeDecimal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxLongitudeDecimal, 5);
            this.textBoxLongitudeDecimal.Location = new System.Drawing.Point(373, 43);
            this.textBoxLongitudeDecimal.Name = "textBoxLongitudeDecimal";
            this.textBoxLongitudeDecimal.Size = new System.Drawing.Size(203, 20);
            this.textBoxLongitudeDecimal.TabIndex = 9;
            this.textBoxLongitudeDecimal.Tag = "";
            this.textBoxLongitudeDecimal.Text = "0";
            this.textBoxLongitudeDecimal.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxLongitudeDecimal_Validating);
            this.textBoxLongitudeDecimal.Validated += new System.EventHandler(this.textBoxLongitudeDecimal_Validated);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(844, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addModelToolStripMenuItem,
            this.loadModelToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // addModelToolStripMenuItem
            // 
            this.addModelToolStripMenuItem.Name = "addModelToolStripMenuItem";
            this.addModelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.addModelToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addModelToolStripMenuItem.Text = "Add Model";
            this.addModelToolStripMenuItem.Click += new System.EventHandler(this.addModelToolStripMenuItem_Click);
            // 
            // loadModelToolStripMenuItem
            // 
            this.loadModelToolStripMenuItem.Name = "loadModelToolStripMenuItem";
            this.loadModelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.loadModelToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadModelToolStripMenuItem.Text = "Load Model";
            this.loadModelToolStripMenuItem.Click += new System.EventHandler(this.loadModelToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem,
            this.toolStripSeparator2,
            this.toolStripMenuItemUseRangeOfDates});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences...";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(203, 6);
            // 
            // toolStripMenuItemUseRangeOfDates
            // 
            this.toolStripMenuItemUseRangeOfDates.CheckOnClick = true;
            this.toolStripMenuItemUseRangeOfDates.Name = "toolStripMenuItemUseRangeOfDates";
            this.toolStripMenuItemUseRangeOfDates.Size = new System.Drawing.Size(206, 22);
            this.toolStripMenuItemUseRangeOfDates.Text = "Calculate For Date Range";
            this.toolStripMenuItemUseRangeOfDates.CheckedChanged += new System.EventHandler(this.toolStripMenuItemUseRangeOfDates_CheckedChanged);
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
            this.aboutGeoMagToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutGeoMagToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
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
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridViewResults, 19);
            this.dataGridViewResults.Location = new System.Drawing.Point(11, 139);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.ReadOnly = true;
            this.tableLayoutPanel1.SetRowSpan(this.dataGridViewResults, 2);
            this.dataGridViewResults.Size = new System.Drawing.Size(808, 117);
            this.dataGridViewResults.TabIndex = 19;
            // 
            // ColumnDate
            // 
            this.ColumnDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnDate.Frozen = true;
            this.ColumnDate.HeaderText = "Date";
            this.ColumnDate.Name = "ColumnDate";
            this.ColumnDate.ReadOnly = true;
            this.ColumnDate.Width = 55;
            // 
            // ColumnDeclination
            // 
            this.ColumnDeclination.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 21;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxLatitudeDecimal, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxModels, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label80, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.TextBoxLatDeg, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.TextBoxLatMin, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.TextBoxLatSec, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.label85, 7, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelDateFrom, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLongitudeDecimal, 8, 3);
            this.tableLayoutPanel1.Controls.Add(this.TextBoxLongDeg, 8, 4);
            this.tableLayoutPanel1.Controls.Add(this.TextBoxLongMin, 9, 4);
            this.tableLayoutPanel1.Controls.Add(this.TextBoxLongSec, 10, 4);
            this.tableLayoutPanel1.Controls.Add(this.dateTimePicker1, 8, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewResults, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.buttonMyLocation, 14, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonCalculate, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.labelDateTo, 10, 1);
            this.tableLayoutPanel1.Controls.Add(this.dateTimePicker2, 11, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelStepSize, 16, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_Elevation, 16, 3);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownStepSize, 17, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxAltitude, 17, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 19, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxAltitudeUnits, 19, 3);
            this.tableLayoutPanel1.Controls.Add(this.ComboBoxLongDir, 12, 4);
            this.tableLayoutPanel1.Controls.Add(this.ComboBoxLatDir, 5, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(844, 267);
            this.tableLayoutPanel1.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Model:";
            // 
            // buttonMyLocation
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonMyLocation, 2);
            this.buttonMyLocation.Image = ((System.Drawing.Image)(resources.GetObject("buttonMyLocation.Image")));
            this.buttonMyLocation.Location = new System.Drawing.Point(590, 43);
            this.buttonMyLocation.Name = "buttonMyLocation";
            this.tableLayoutPanel1.SetRowSpan(this.buttonMyLocation, 2);
            this.buttonMyLocation.Size = new System.Drawing.Size(26, 25);
            this.buttonMyLocation.TabIndex = 14;
            this.buttonMyLocation.UseVisualStyleBackColor = true;
            this.buttonMyLocation.Click += new System.EventHandler(this.buttonMyLocation_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(776, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Days";
            this.label4.Visible = false;
            // 
            // comboBoxAltitudeUnits
            // 
            this.comboBoxAltitudeUnits.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxAltitudeUnits.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxAltitudeUnits.FormattingEnabled = true;
            this.comboBoxAltitudeUnits.Items.AddRange(new object[] {
            "km",
            "m",
            "ft"});
            this.comboBoxAltitudeUnits.Location = new System.Drawing.Point(775, 42);
            this.comboBoxAltitudeUnits.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxAltitudeUnits.Name = "comboBoxAltitudeUnits";
            this.comboBoxAltitudeUnits.Size = new System.Drawing.Size(45, 21);
            this.comboBoxAltitudeUnits.TabIndex = 16;
            this.comboBoxAltitudeUnits.Validated += new System.EventHandler(this.comboBoxAltitudeUnits_Validated);
            // 
            // ComboBoxLongDir
            // 
            this.ComboBoxLongDir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxLongDir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxLongDir.FormattingEnabled = true;
            this.ComboBoxLongDir.Items.AddRange(new object[] {
            "E",
            "W"});
            this.ComboBoxLongDir.Location = new System.Drawing.Point(529, 66);
            this.ComboBoxLongDir.Margin = new System.Windows.Forms.Padding(2);
            this.ComboBoxLongDir.Name = "ComboBoxLongDir";
            this.ComboBoxLongDir.Size = new System.Drawing.Size(48, 21);
            this.ComboBoxLongDir.TabIndex = 13;
            this.ComboBoxLongDir.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLongitude_Validating);
            this.ComboBoxLongDir.Validated += new System.EventHandler(this.TextBoxLongitude_Validated);
            // 
            // ComboBoxLatDir
            // 
            this.ComboBoxLatDir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxLatDir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxLatDir.FormattingEnabled = true;
            this.ComboBoxLatDir.Items.AddRange(new object[] {
            "N",
            "S"});
            this.ComboBoxLatDir.Location = new System.Drawing.Point(248, 66);
            this.ComboBoxLatDir.Margin = new System.Windows.Forms.Padding(2);
            this.ComboBoxLatDir.Name = "ComboBoxLatDir";
            this.ComboBoxLatDir.Size = new System.Drawing.Size(48, 21);
            this.ComboBoxLatDir.TabIndex = 8;
            this.ComboBoxLatDir.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxLatitude_Validating);
            this.ComboBoxLatDir.Validated += new System.EventHandler(this.TextBoxLatitude_Validated);
            //
            // statusStrip1
            //
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripButtonCancel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 269);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(844, 22);
            this.statusStrip1.TabIndex = 24;
            this.statusStrip1.Text = "statusStrip1";
            //
            // toolStripStatusLabel1
            //
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(729, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Ready";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // toolStripProgressBar1
            //
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            //
            // toolStripButtonCancel
            //
            this.toolStripButtonCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonCancel.Name = "toolStripButtonCancel";
            this.toolStripButtonCancel.Size = new System.Drawing.Size(47, 20);
            this.toolStripButtonCancel.Text = "Cancel";
            this.toolStripButtonCancel.Visible = false;
            this.toolStripButtonCancel.Click += new System.EventHandler(this.toolStripButtonCancel_Click);
            //
            // FrmMain
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 291);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(804, 328);
            this.Name = "FrmMain";
            this.Text = "GeoMag #";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMain_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStepSize)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label labelDateTo;
        private System.Windows.Forms.Label labelDateFrom;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.ComboBox comboBoxModels;
        internal System.Windows.Forms.TextBox textBoxAltitude;
        private System.Windows.Forms.Label label_Elevation;
        private System.Windows.Forms.Button buttonCalculate;
        internal System.Windows.Forms.Label label85;
        internal System.Windows.Forms.TextBox TextBoxLongDeg;
        internal System.Windows.Forms.TextBox TextBoxLatSec;
        internal System.Windows.Forms.TextBox TextBoxLongMin;
        internal System.Windows.Forms.TextBox TextBoxLatMin;
        internal System.Windows.Forms.TextBox TextBoxLongSec;
        internal System.Windows.Forms.TextBox TextBoxLatDeg;
        internal System.Windows.Forms.Label label80;
        internal System.Windows.Forms.TextBox textBoxLatitudeDecimal;
        internal System.Windows.Forms.TextBox textBoxLongitudeDecimal;
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
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUseRangeOfDates;
        private System.Windows.Forms.Button buttonMyLocation;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxAltitudeUnits;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ComboBox ComboBoxLongDir;
        private System.Windows.Forms.ComboBox ComboBoxLatDir;
        private System.Windows.Forms.ToolStripMenuItem addModelToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDeclination;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInclination;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHorizontalIntensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNorthComp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEastComp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVerticalComp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTotalField;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripButton toolStripButtonCancel;
    }
}

