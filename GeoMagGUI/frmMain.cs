using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Device.Location;
using GeoMagGUI.Properties;
using GeoMagSharp;

namespace GeoMagGUI
{
    public partial class FrmMain : Form
    {
        public readonly string ModelFolder;

        private GeoCoordinateWatcher Watcher = null;

        public DataTable DtModels;

        public bool _processingEvents;

        public bool _useDecimalDegrees;

        public bool _useAltitude;

        private GeoMag _MagCalculator;

        public FrmMain()
        {
            _processingEvents = true;

            InitializeComponent();

            _useDecimalDegrees = true;

            _useAltitude = true;

            // Create the watcher.
            Watcher = new GeoCoordinateWatcher();
            // Catch the StatusChanged event.
            Watcher.StatusChanged += Watcher_StatusChanged;

            ModelFolder = string.Format("{0}\\coefficient\\", Application.StartupPath);

            comboBoxAltitudeUnits.SelectedItem = @"ft";

            ComboBoxLatDir.SelectedItem = @"N";

            ComboBoxLongDir.SelectedItem = @"W";

            DtModels = new DataTable();

            DtModels.Columns.Add(new DataColumn("ID", typeof(Int32)));

            DtModels.Columns.Add(new DataColumn("ModelName", typeof(string)));

            DtModels.Columns.Add(new DataColumn("FileName", typeof(string)));

            LoadModels();

            SetCoordinateDisplay();

            SetElevationDisplay();

            _processingEvents = false;
        }

        // The watcher's status has change. See if it is ready.
        private void Watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                if (Watcher.Position.Location.IsUnknown)
                {
                    textBoxAltitude.Text = "0";

                    textBoxLatitudeDecimal.Text = "0.0";

                    textBoxLongitudeDecimal.Text = "0.0";
                }
                else
                {
                    textBoxAltitude.Text = Watcher.Position.Location.Altitude.ToString();
                    
                    textBoxLatitudeDecimal.Text = Watcher.Position.Location.Latitude.ToString();

                    textBoxLongitudeDecimal.Text = Watcher.Position.Location.Longitude.ToString();
                    
                }

                textBoxLatitudeDecimal_Validated(textBoxLatitudeDecimal, new EventArgs());
                textBoxLongitudeDecimal_Validated(textBoxLongitudeDecimal, new EventArgs());
            }
        }



        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            _MagCalculator = null;
            saveToolStripMenuItem.Enabled = false;

            this.errorProviderCheck.SetError(comboBoxModels, string.Empty);
            this.errorProviderCheck.SetError(comboBoxAltitudeUnits, string.Empty);
            this.errorProviderCheck.SetError(textBoxAltitude, string.Empty);
            this.errorProviderCheck.SetError(textBoxLatitudeDecimal, string.Empty);
            this.errorProviderCheck.SetError(textBoxLongitudeDecimal, string.Empty);

            if (comboBoxModels.SelectedValue == null)
            {
                this.errorProviderCheck.SetError(comboBoxModels, @"No Model selected");
                return;
            }

            var dRow = DtModels.Select(string.Format("ID = {0}", comboBoxModels.SelectedValue));

            if (dRow != null && dRow.Any())
            {
                if (DBNull.Value.Equals(dRow.First()["FileName"]))
                {
                    this.errorProviderCheck.SetError(comboBoxModels, @"No file name was found for the model you selected");
                    return;
                }

                string modelFile = dRow.First()["FileName"].ToString();

                if (!File.Exists(modelFile))
                {
                    this.errorProviderCheck.SetError(comboBoxModels, string.Format("The model file {0} could not be found", Path.GetFileName(modelFile)));
                    return;
                }

                if (comboBoxAltitudeUnits.SelectedItem == null)
                {
                    this.errorProviderCheck.SetError(comboBoxAltitudeUnits, @"No Units have been selected");
                    return;
                }

                if (string.IsNullOrEmpty(textBoxAltitude.Text) || !Helper.IsNumeric(textBoxAltitude.Text))
                {
                    this.errorProviderCheck.SetError(textBoxAltitude, @"Altitude must be a valid number");
                    return;
                }

                double altitude = Convert.ToDouble(textBoxAltitude.Text);

                var altUnit = GeoMagDistance.FromString(comboBoxAltitudeUnits.SelectedItem.ToString());

                //GeoMagDistance altUnit = GeoMagDistance.kilometer;

                //switch (comboBoxAltitudeUnits.SelectedItem.ToString().ToLower())
                //{
                //    case "ft":
                //        altUnit = GeoMagDistance.Unit.foot;
                //        break;

                //    case "m":
                //        altUnit = GeoMagDistance.meter;
                //        break;

                //    case "mi":
                //        altUnit = GeoMagDistance.mile;
                //        break;

                //    default:
                //        altUnit = GeoMagDistance.kilometer;
                //        break;
                //}

                //if (comboBoxUnits.SelectedItem.ToString().Equals("ft", StringComparison.OrdinalIgnoreCase))
                //{
                //    altitude *= Constants.FeetToKilometer;
                //}
                //else if (comboBoxUnits.SelectedItem.ToString().Equals("m", StringComparison.OrdinalIgnoreCase))
                //{
                //    altitude *= Constants.MeterToKilometer;
                //}

                if (string.IsNullOrEmpty(textBoxLatitudeDecimal.Text) || !Helper.IsNumeric(textBoxLatitudeDecimal.Text))
                {
                    this.errorProviderCheck.SetError(textBoxLatitudeDecimal, @"Latitude must be a valid number");
                    return;
                }

                //double latitude = Convert.ToDouble(textBoxLatitudeDecimal.Text);

                if (string.IsNullOrEmpty(textBoxLongitudeDecimal.Text) || !Helper.IsNumeric(textBoxLongitudeDecimal.Text))
                {
                    this.errorProviderCheck.SetError(textBoxLongitudeDecimal, @"Longitude must be a valid number");
                    //MessageBox.Show(string.Format("Longitude must be a valid number.{0}Please correct and try again.", Environment.NewLine), @"Error: Longitude Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                Cursor = Cursors.WaitCursor;

                var calcOptions = new GeoMagOptions
                    {
                        Latitude = Convert.ToDouble(textBoxLatitudeDecimal.Text),
                        Longitude = Convert.ToDouble(textBoxLongitudeDecimal.Text),
                        StartDate = dateTimePicker1.Value,
                        StepInterval = Convert.ToDouble(numericUpDownStepSize.Value),
                    };

                calcOptions.SetElevation(altitude, altUnit, _useAltitude);

                dataGridViewResults.Rows.Clear();

                try
                {

                    _MagCalculator = new GeoMag();

                    _MagCalculator.LoadModel(modelFile);

                    if (toolStripMenuItemUseRangeOfDates.Checked) calcOptions.EndDate = dateTimePicker2.Value;

                    _MagCalculator.MagneticCalculations(calcOptions);
                     
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error: Calculating Magnetics", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _MagCalculator = null;
                }

                if (_MagCalculator == null)
                {
                    Cursor = Cursors.Default;
                    return;
                }

                if (_MagCalculator.ResultsOfCalculation == null || !_MagCalculator.ResultsOfCalculation.Any())
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show(this, "No Calculations were returned for the given parameters", "No Calculation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (var mag in _MagCalculator.ResultsOfCalculation)
                {
                    if (mag == null) continue;

                    dataGridViewResults.Rows.Add();

                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDate"].Value = mag.Date.ToShortDateString();

                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDeclination"].Value = string.Format("{0}°", (mag.EastComp == null ? double.NaN.ToString() : mag.Declination.Value.ToString("F3")));

                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnInclination"].Value = string.Format("{0}°", (mag.EastComp == null ? double.NaN.ToString() : mag.Inclination.Value.ToString("F3")));

                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnHorizontalIntensity"].Value = string.Format("{0} nT", (mag.EastComp == null ? double.NaN.ToString() : mag.HorizontalIntensity.Value.ToString("F2")));

                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnNorthComp"].Value = string.Format("{0} nT", (mag.EastComp == null ? double.NaN.ToString() : mag.NorthComp.Value.ToString("F2")));

                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnEastComp"].Value = string.Format("{0} nT", (mag.EastComp == null ? double.NaN.ToString() : mag.EastComp.Value.ToString("F2")));

                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnVerticalComp"].Value = string.Format("{0} nT", (mag.EastComp == null ? double.NaN.ToString() : mag.VerticalComp.Value.ToString("F2")));

                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnTotalField"].Value = string.Format("{0} nT", (mag.EastComp == null ? double.NaN.ToString() : mag.TotalField.Value.ToString("F2")));

                }

                dataGridViewResults.Rows.Add();
                
                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDate"].Value = @"Change per year";
                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDate"].Style.BackColor = System.Drawing.Color.LightBlue;

                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDeclination"].Value = string.Format("{0}°", _MagCalculator.ResultsOfCalculation.Last().Declination.ChangePerYear.ToString("F3"));
                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDeclination"].Style.BackColor = System.Drawing.Color.LightBlue;

                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnInclination"].Value = string.Format("{0}°", _MagCalculator.ResultsOfCalculation.Last().Inclination.ChangePerYear.ToString("F3"));
                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnInclination"].Style.BackColor = System.Drawing.Color.LightBlue;

                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnHorizontalIntensity"].Value = string.Format("{0} nT", _MagCalculator.ResultsOfCalculation.Last().HorizontalIntensity.ChangePerYear.ToString("F2"));
                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnHorizontalIntensity"].Style.BackColor = System.Drawing.Color.LightBlue;

                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnNorthComp"].Value = string.Format("{0} nT", _MagCalculator.ResultsOfCalculation.Last().NorthComp.ChangePerYear.ToString("F2"));
                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnNorthComp"].Style.BackColor = System.Drawing.Color.LightBlue;

                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnEastComp"].Value = string.Format("{0} nT", _MagCalculator.ResultsOfCalculation.Last().EastComp.ChangePerYear.ToString("F2"));
                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnEastComp"].Style.BackColor = System.Drawing.Color.LightBlue;

                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnVerticalComp"].Value = string.Format("{0} nT", _MagCalculator.ResultsOfCalculation.Last().VerticalComp.ChangePerYear.ToString("F2"));
                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnVerticalComp"].Style.BackColor = System.Drawing.Color.LightBlue;

                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnTotalField"].Value = string.Format("{0} nT", _MagCalculator.ResultsOfCalculation.Last().TotalField.ChangePerYear.ToString("F2"));
                dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnTotalField"].Style.BackColor = System.Drawing.Color.LightBlue;

                saveToolStripMenuItem.Enabled = true;

                Cursor = Cursors.Default;
            }
        }

        private void LoadModels(string selected = null)
        {
            DtModels.Rows.Clear();

            Int32 fileIdx = 1;

            Int32 selectedIdx = -1;

            foreach (var lfile in Directory.GetFiles(ModelFolder))
            {
                var extension = Path.GetExtension(lfile);

                if (extension == null) continue;

                if (!extension.Equals(".cof", StringComparison.OrdinalIgnoreCase) &&
                    !extension.Equals(".dat", StringComparison.OrdinalIgnoreCase)) continue;

                var fRow = DtModels.NewRow();

                fRow["ID"] = fileIdx;

                fRow["ModelName"] = Path.GetFileNameWithoutExtension(lfile);

                fRow["FileName"] = lfile;

                DtModels.Rows.Add(fRow);

                if (!string.IsNullOrEmpty(selected) && lfile.Equals(selected, StringComparison.OrdinalIgnoreCase))
                    selectedIdx = fileIdx;

                fileIdx++;
            }

            comboBoxModels.DataSource = DtModels.Copy().DefaultView;

            comboBoxModels.DisplayMember = "ModelName";

            comboBoxModels.ValueMember = "ID";

            comboBoxModels.SelectedValue = selectedIdx;
        }

        private void loadModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fDlg = new OpenFileDialog
            {
                Title = @"Select a Model Data File",
                Filter = Properties.Resources.File_Type_All_Coeff_Files,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false
            };

            if (fDlg.ShowDialog() != DialogResult.Cancel)
            {
                var copyToLocation = string.Format("{0}{1}", ModelFolder, Path.GetFileName(fDlg.FileName));

                File.Copy(fDlg.FileName, copyToLocation);

                LoadModels(copyToLocation);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBoxLatitudeDecimal_Validated(object sender, EventArgs e)
        {
            this.errorProviderCheck.SetError(textBoxLatitudeDecimal, string.Empty);

            var latitude = new Latitude(Convert.ToDouble(textBoxLatitudeDecimal.Text));

            TextBoxLatDeg.Text = latitude.Degrees.ToString("F0");
            TextBoxLatMin.Text = latitude.Minutes.ToString("F0");
            TextBoxLatSec.Text = latitude.Seconds.ToString("F4");
            ComboBoxLatDir.SelectedItem = latitude.Hemisphere.ToString();
        }

        private void textBoxLatitudeDecimal_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxLatitudeDecimal.Text))
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(textBoxLatitudeDecimal, "This value is required");

                return;
            }

            if(!Helper.IsNumeric(textBoxLatitudeDecimal.Text))
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(textBoxLatitudeDecimal, "Entered Value is not numeric");

                return;
            }

            if (Convert.ToDouble(textBoxLatitudeDecimal.Text) < -90 || Convert.ToDouble(textBoxLatitudeDecimal.Text) > 90)
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(textBoxLatitudeDecimal, "Decimal Latitude is between -90 and 90");

                return;
            }

        }

        private void textBoxLongitudeDecimal_Validated(object sender, EventArgs e)
        {
            this.errorProviderCheck.SetError(textBoxLongitudeDecimal, string.Empty);

            var longitude = new Longitude(Convert.ToDouble(textBoxLongitudeDecimal.Text));

            TextBoxLongDeg.Text = longitude.Degrees.ToString("F0");
            TextBoxLongMin.Text = longitude.Minutes.ToString("F0");
            TextBoxLongSec.Text = longitude.Seconds.ToString("F4");
            ComboBoxLongDir.SelectedItem = longitude.Hemisphere.ToString();
        }

        private void textBoxLongitudeDecimal_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxLongitudeDecimal.Text))
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(textBoxLongitudeDecimal, "This value is required");

                return;
            }

            if (!Helper.IsNumeric(textBoxLongitudeDecimal.Text))
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(textBoxLongitudeDecimal, "Entered Value is not numeric");

                return;
            }

            if (Convert.ToDouble(textBoxLongitudeDecimal.Text) < -180 || Convert.ToDouble(textBoxLongitudeDecimal.Text) > 180)
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(textBoxLongitudeDecimal, "Decimal Longitude is between -180 and 180");

                return;
            }
        }

        private void textBoxAltitude_Validated(object sender, EventArgs e)
        {
            this.errorProviderCheck.SetError(textBoxAltitude, string.Empty);
        }

        private void textBoxAltitude_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxAltitude.Text))
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(textBoxAltitude, "This value is required");

                return;
            }

            if (!Helper.IsNumeric(textBoxAltitude.Text))
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(textBoxAltitude, "Entered Value is not numeric");

                return;
            }
        }

        private void TextBoxLongitude_Validated(object sender, EventArgs e)
        {
            this.errorProviderCheck.SetError(ComboBoxLongDir, string.Empty);

            var longitude = new Longitude(Convert.ToDouble(TextBoxLongDeg.Text), Convert.ToDouble(TextBoxLongMin.Text), Convert.ToDouble(TextBoxLongSec.Text), ComboBoxLongDir.SelectedItem.ToString());

            textBoxLongitudeDecimal.Text = longitude.Decimal.ToString("F8");
        }

        private void TextBoxLongitude_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLongDeg.Text) || string.IsNullOrEmpty(TextBoxLongMin.Text) || string.IsNullOrEmpty(TextBoxLongSec.Text))
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(ComboBoxLongDir, "This value is required");

                return;
            }

            if (!Helper.IsNumeric(TextBoxLongDeg.Text) || !Helper.IsNumeric(TextBoxLongMin.Text) || !Helper.IsNumeric(TextBoxLongSec.Text))
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(ComboBoxLongDir, "Entered Value is not numeric");

                return;
            }

            if (ComboBoxLongDir.SelectedItem == null)
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(ComboBoxLongDir, "Missing Hemisphere");

                return;
            }

            if (Convert.ToDouble(TextBoxLongDeg.Text) < -180 || Convert.ToDouble(TextBoxLongDeg.Text) > 180)
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(ComboBoxLongDir, "Longitude Degree is between -180 and 180");

                return;
            }
        }

        private void TextBoxLatitude_Validated(object sender, EventArgs e)
        {
            this.errorProviderCheck.SetError(ComboBoxLatDir, string.Empty);

            var latitude = new Longitude(Convert.ToDouble(TextBoxLatDeg.Text), Convert.ToDouble(TextBoxLatMin.Text), Convert.ToDouble(TextBoxLatSec.Text), ComboBoxLatDir.SelectedItem.ToString());

            textBoxLatitudeDecimal.Text = latitude.Decimal.ToString("F8");
        }

        private void TextBoxLatitude_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLatDeg.Text) || string.IsNullOrEmpty(TextBoxLatMin.Text) || string.IsNullOrEmpty(TextBoxLatSec.Text))
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(ComboBoxLatDir, "This value is required");

                return;
            }

            if (!Helper.IsNumeric(TextBoxLatDeg.Text) || !Helper.IsNumeric(TextBoxLatMin.Text) || !Helper.IsNumeric(TextBoxLatSec.Text))
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(ComboBoxLatDir, "Entered Value is not numeric");

                return;
            }

            if (ComboBoxLatDir.SelectedItem == null)
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(ComboBoxLatDir, "Missing Hemisphere");

                return;
            }

            if (Convert.ToDouble(TextBoxLatDeg.Text) < -90 || Convert.ToDouble(TextBoxLatDeg.Text) > 90)
            {
                e.Cancel = true;

                this.errorProviderCheck.SetError(ComboBoxLatDir, "Latitude Degree is between -90 and 90");

                return;
            }
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            var txtBox = (TextBox)sender;

            txtBox.SelectionStart = 0;

            txtBox.SelectionLength = txtBox.Text.Length;
        }

        private void aboutGeoMagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutBox = new AboutBoxGeoMag();

            aboutBox.ShowDialog();
        }

        private void SetCoordinateDisplay()
        {
            textBoxLongitudeDecimal.Enabled = _useDecimalDegrees;

            TextBoxLongDeg.Enabled = !_useDecimalDegrees;
            TextBoxLongMin.Enabled = !_useDecimalDegrees;
            TextBoxLongSec.Enabled = !_useDecimalDegrees;
            ComboBoxLongDir.Enabled = !_useDecimalDegrees;

            textBoxLatitudeDecimal.Enabled = _useDecimalDegrees;

            TextBoxLatDeg.Enabled = !_useDecimalDegrees;
            TextBoxLatMin.Enabled = !_useDecimalDegrees;
            TextBoxLatSec.Enabled = !_useDecimalDegrees;
            ComboBoxLatDir.Enabled = !_useDecimalDegrees;
        }

        private void SetElevationDisplay()
        {
            label_Elevation.Text = _useAltitude
                                    ? @"Altitude:"
                                    : @"Depth:";
        }

        private void toolStripMenuItemUseRangeOfDates_CheckedChanged(object sender, EventArgs e)
        {
            if (!toolStripMenuItemUseRangeOfDates.Checked)
            {
                labelDateFrom.Visible = false;
                labelDateTo.Visible = false;
                dateTimePicker2.Visible = false;
                labelStepSize.Visible = false;
                numericUpDownStepSize.Visible = false;
            }
            else
            {
                labelDateFrom.Visible = true;
                labelDateTo.Visible = true;
                dateTimePicker2.Visible = true;
                labelStepSize.Visible = true;
                numericUpDownStepSize.Visible = true;
            }

        }

        private void buttonMyLocation_Click(object sender, EventArgs e)
        {
            // Start the watcher.
            Watcher.Start();
        }

        private void dateTimePicker_Validated(object sender, EventArgs e)
        {
            if (_processingEvents) return;

            TimeSpan timespan = (dateTimePicker2.Value - dateTimePicker1.Value);

            numericUpDownStepSize.Minimum = 1;
            numericUpDownStepSize.Maximum = Convert.ToDecimal(timespan.Days);
            numericUpDownStepSize.Value = Convert.ToDecimal(timespan.Days);
            numericUpDownStepSize.Increment = Convert.ToDecimal(Math.Round(timespan.Days / 4D, 0));

            if (numericUpDownStepSize.Increment < 1) numericUpDownStepSize.Increment = 1M;
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fPref = new frmPreferences(this);

            var res = fPref.ShowDialog(this);

            if (!res.Equals(DialogResult.Cancel))
            {
                _useAltitude = fPref.UseAltitude;

                _useDecimalDegrees = fPref.UseDecimalDegrees;
            }

            fPref.Close();

            SetCoordinateDisplay();

            SetElevationDisplay();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileName = @"Results";

            var fldlg = new SaveFileDialog
            {
                Filter = Resources.File_Type_Text_Tab,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = "Save Results",
                FileName = fileName

            };

            if (fldlg.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;

                _MagCalculator.SaveResults(fldlg.FileName);

                Cursor = Cursors.Default;
            }
        }

    }
}
