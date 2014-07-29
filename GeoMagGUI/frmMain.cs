using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GeoMagGUI.Properties;
using GeoMagSharp;

namespace GeoMagGUI
{
    public partial class FrmMain : Form
    {
        public readonly string ModelFolder;

        public DataTable DtModels;

        public bool _processingEvents;

        public FrmMain()
        {
            _processingEvents = true;

            InitializeComponent();

            ModelFolder = string.Format("{0}\\coefficient\\", Application.StartupPath);

            DtModels = new DataTable();

            DtModels.Columns.Add(new DataColumn("ID", typeof(Int32)));

            DtModels.Columns.Add(new DataColumn("ModelName", typeof(string)));

            DtModels.Columns.Add(new DataColumn("FileName", typeof(string)));

            LoadModels();

            _processingEvents = false;
        }



        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (comboBoxModels.SelectedValue == null)
            {
                MessageBox.Show(string.Format("No Model has been selected.{0}Please choose model and try again.", Environment.NewLine), @"Error: No Model", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var dRow = DtModels.Select(string.Format("ID = {0}", comboBoxModels.SelectedValue));

            if (dRow != null && dRow.Any())
            {
                if (DBNull.Value.Equals(dRow.First()["FileName"]))
                {
                    MessageBox.Show(@"No file name was found for the model you selected.", @"Error: No Model File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string modelFile = dRow.First()["FileName"].ToString();

                if (!File.Exists(modelFile))
                {
                    MessageBox.Show(string.Format("The model file {0} could not be found", Path.GetFileName(modelFile)), @"Error: Missing Model File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (comboBoxUnits.SelectedItem == null)
                {
                    MessageBox.Show(string.Format("No Units have been selected.{0}Please choose a Units and try again.", Environment.NewLine), @"Error: No Units", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (string.IsNullOrEmpty(textBoxAltitude.Text) || !Helper.IsNumeric(textBoxAltitude.Text))
                {
                    MessageBox.Show(string.Format("Altitude must be a valid number.{0}Please correct and try again.", Environment.NewLine), @"Error: Altitude Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                double altitude = Convert.ToDouble(textBoxAltitude.Text);

                if (comboBoxUnits.SelectedItem.ToString().Equals("Feet", StringComparison.OrdinalIgnoreCase))
                {
                    altitude *= Constants.FeetToKilometer;
                }
                else if (comboBoxUnits.SelectedItem.ToString().Equals("Meters", StringComparison.OrdinalIgnoreCase))
                {
                    altitude *= Constants.MeterToKilometer;
                }

                if (string.IsNullOrEmpty(textBoxLatitudeDecimal.Text) || !Helper.IsNumeric(textBoxLatitudeDecimal.Text))
                {
                    MessageBox.Show(string.Format("Latitude must be a valid number.{0}Please correct and try again.", Environment.NewLine), @"Error: Latitude Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                double latitude = Convert.ToDouble(textBoxLatitudeDecimal.Text);

                if (string.IsNullOrEmpty(textBoxLongitudeDecimal.Text) || !Helper.IsNumeric(textBoxLongitudeDecimal.Text))
                {
                    MessageBox.Show(string.Format("Longitude must be a valid number.{0}Please correct and try again.", Environment.NewLine), @"Error: Longitude Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                Cursor = Cursors.WaitCursor;

                double longitude = Convert.ToDouble(textBoxLongitudeDecimal.Text);

                double stepInterval = Convert.ToDouble(numericUpDownStepSize.Value);

                dataGridViewResults.Rows.Clear();

                var magCalc = new GeoMagSharp.GeoMag(modelFile);

                if (radioButtonDateSingle.Checked)
                {
                    magCalc.MagneticCalculations(dateTimePicker1.Value, dateTimePicker1.Value, latitude, longitude, altitude);
                }
                else
                {
                    magCalc.MagneticCalculations(dateTimePicker1.Value, dateTimePicker2.Value, latitude, longitude, altitude, stepInterval);
                }

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

                if (extension == null || !extension.Equals(".cof", StringComparison.OrdinalIgnoreCase)) continue;

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

        private void radioButtonDateSingle_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDateSingle.Checked)
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

        private void loadModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fDlg = new OpenFileDialog
            {
                Title = @"Select a Model Data File",
                Filter = Properties.Resources.File_Type_COF,
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

        private void decimalDegreesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_processingEvents) return;

            _processingEvents = true;

            degreesMinutesAndSecondsToolStripMenuItem.Checked = !decimalDegreesToolStripMenuItem.Checked;

            SetCoordinateDisplay();

            _processingEvents = false;
        }

        private void degreesMinutesAndSecondsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_processingEvents) return;

            _processingEvents = true;

            decimalDegreesToolStripMenuItem.Checked = !degreesMinutesAndSecondsToolStripMenuItem.Checked;

            SetCoordinateDisplay();

            _processingEvents = false;
        }

        private void SetCoordinateDisplay()
        {
            //if (comboBoxGeodeticType.SelectedItem == null) return;

            //var selectedText = comboBoxGeodeticType.SelectedItem.ToString();

            if (decimalDegreesToolStripMenuItem.Checked)
            {
                textBoxLongitudeDecimal.Visible = true;

                TextBoxLongDeg.Visible = false;
                TextBoxLongMin.Visible = false;
                TextBoxLongSec.Visible = false;
                ComboBoxLongDir.Visible = false;

                textBoxLatitudeDecimal.Visible = true;

                TextBoxLatDeg.Visible = false;
                TextBoxLatMin.Visible = false;
                TextBoxLatSec.Visible = false;
                ComboBoxLatDir.Visible = false;

            }
            else if (degreesMinutesAndSecondsToolStripMenuItem.Checked)
            {
                textBoxLongitudeDecimal.Visible = false;

                TextBoxLongDeg.Visible = true;
                TextBoxLongMin.Visible = true;
                TextBoxLongSec.Visible = true;
                ComboBoxLongDir.Visible = true;

                textBoxLatitudeDecimal.Visible = false;

                TextBoxLatDeg.Visible = true;
                TextBoxLatMin.Visible = true;
                TextBoxLatSec.Visible = true;
                ComboBoxLatDir.Visible = true;
            }
        }

    }
}
