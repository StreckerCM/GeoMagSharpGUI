using GeoMagGUI.Properties;
using GeoMagSharp;
using System;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeoMagGUI
{
    public partial class FrmMain : Form
    {
        private GeoCoordinateWatcher Watcher = null;

        private MagneticModelCollection Models;

        public bool _processingEvents;

        public Preferences ApplicationPreferences;

        private GeoMag _MagCalculator;

        private CancellationTokenSource _calculationCts;

        #region Getters & Setters

        public string ModelFolder
        {
            get
            {
                return Path.Combine(Application.StartupPath, Resources.Folder_Coeffient);
            }
        }

        public string ModelJson
        {
            get
            {
                return Path.Combine(Application.StartupPath, Resources.Folder_Coeffient, Resources.File_Name_Magnetic_Model_JSON);
            }
        }

        public string PreferencesJson
        {
            get
            {
                return Path.Combine(Application.StartupPath, Resources.File_Name_Application_Preferences_JSON);
            }
        }

        #endregion

        public FrmMain()
        {
            _processingEvents = true;

            InitializeComponent();

            ApplicationPreferences = Preferences.Load(PreferencesJson);

            // Create the watcher.
            Watcher = new GeoCoordinateWatcher();
            // Catch the StatusChanged event.

            Watcher.StatusChanged += Watcher_StatusChanged;

            comboBoxAltitudeUnits.SelectedItem = ApplicationPreferences.AltitudeUnits;

            ComboBoxLatDir.SelectedItem = ApplicationPreferences.LatitudeHemisphere;

            ComboBoxLongDir.SelectedItem = ApplicationPreferences.LongitudeHemisphere;

            Models = MagneticModelCollection.Load(ModelJson);

            if (Models == null) Models = new MagneticModelCollection();

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

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            // F5 - Calculate
            if (e.KeyCode == Keys.F5)
            {
                buttonCalculate_Click(sender, e);
                e.Handled = true;
            }
            // Ctrl+L - My Location (GPS)
            else if (e.Control && e.KeyCode == Keys.L)
            {
                buttonMyLocation_Click(sender, e);
                e.Handled = true;
            }
            // Escape - Cancel active operation (only when a calculation is running)
            else if (e.KeyCode == Keys.Escape && _calculationCts != null)
            {
                _calculationCts.Cancel();
                e.Handled = true;
            }
        }

        private void SetUIBusy(bool busy)
        {
            buttonCalculate.Enabled = !busy;
            addModelToolStripMenuItem.Enabled = !busy;
            loadModelToolStripMenuItem.Enabled = !busy;
            toolStripProgressBar1.Visible = busy;
            toolStripButtonCancel.Visible = busy;
            UseWaitCursor = busy;

            if (!busy)
            {
                toolStripProgressBar1.Value = 0;
            }
        }

        private void toolStripButtonCancel_Click(object sender, EventArgs e)
        {
            _calculationCts?.Cancel();
        }

        private async void buttonCalculate_Click(object sender, EventArgs e)
        {
            // Re-entrancy guard: ignore if already calculating
            if (_calculationCts != null) return;

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

            var selectedModel = Models.TList.Find(model => model.ID.Equals(comboBoxModels.SelectedValue));

            if (selectedModel != null)
            {
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

                var altUnit = Distance.FromString(comboBoxAltitudeUnits.SelectedItem.ToString());

                if (string.IsNullOrEmpty(textBoxLatitudeDecimal.Text) || !Helper.IsNumeric(textBoxLatitudeDecimal.Text))
                {
                    this.errorProviderCheck.SetError(textBoxLatitudeDecimal, @"Latitude must be a valid number");
                    return;
                }

                if (string.IsNullOrEmpty(textBoxLongitudeDecimal.Text) || !Helper.IsNumeric(textBoxLongitudeDecimal.Text))
                {
                    this.errorProviderCheck.SetError(textBoxLongitudeDecimal, @"Longitude must be a valid number");
                    return;
                }

                _calculationCts = new CancellationTokenSource();

                try
                {
                    SetUIBusy(true);
                    toolStripStatusLabel1.Text = "Calculating...";

                    var calcOptions = new CalculationOptions
                        {
                            Latitude = Convert.ToDouble(textBoxLatitudeDecimal.Text),
                            Longitude = Convert.ToDouble(textBoxLongitudeDecimal.Text),
                            StartDate = dateTimePicker1.Value,
                            StepInterval = Convert.ToDouble(numericUpDownStepSize.Value),
                        };

                    calcOptions.SetElevation(altitude, altUnit, ApplicationPreferences.UseAltitude);

                    dataGridViewResults.Rows.Clear();

                    _MagCalculator = new GeoMag();

                    _MagCalculator.LoadModel(selectedModel);

                    if (toolStripMenuItemUseRangeOfDates.Checked) calcOptions.EndDate = dateTimePicker2.Value;

                    var progress = new Progress<CalculationProgressInfo>(info =>
                    {
                        toolStripStatusLabel1.Text = info.StatusMessage;
                        toolStripProgressBar1.Value = Math.Min((int)info.PercentComplete, 100);
                    });

                    await _MagCalculator.MagneticCalculationsAsync(calcOptions, progress, _calculationCts.Token);

                    if (_MagCalculator.ResultsOfCalculation == null || !_MagCalculator.ResultsOfCalculation.Any())
                    {
                        MessageBox.Show(this, "No Calculations were returned for the given parameters", "No Calculation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    foreach (var mag in _MagCalculator.ResultsOfCalculation)
                    {
                        if (mag == null) continue;

                        dataGridViewResults.Rows.Add();

                        dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDate"].Value = mag.Date.ToShortDateString();

                        dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDeclination"].Value = string.Format("{0}°", (mag.EastComp == null ? double.NaN.ToString() : mag.Declination.Value.ToString("F4")));

                        dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnInclination"].Value = string.Format("{0}°", (mag.EastComp == null ? double.NaN.ToString() : mag.Inclination.Value.ToString("F4")));

                        dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnHorizontalIntensity"].Value = string.Format("{0} nT", (mag.EastComp == null ? double.NaN.ToString() : mag.HorizontalIntensity.Value.ToString("F2")));

                        dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnNorthComp"].Value = string.Format("{0} nT", (mag.EastComp == null ? double.NaN.ToString() : mag.NorthComp.Value.ToString("F2")));

                        dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnEastComp"].Value = string.Format("{0} nT", (mag.EastComp == null ? double.NaN.ToString() : mag.EastComp.Value.ToString("F2")));

                        dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnVerticalComp"].Value = string.Format("{0} nT", (mag.EastComp == null ? double.NaN.ToString() : mag.VerticalComp.Value.ToString("F2")));

                        dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnTotalField"].Value = string.Format("{0} nT", (mag.EastComp == null ? double.NaN.ToString() : mag.TotalField.Value.ToString("F2")));

                    }

                    dataGridViewResults.Rows.Add();

                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDate"].Value = @"Change per year";
                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDate"].Style.BackColor = System.Drawing.Color.LightBlue;

                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDeclination"].Value = string.Format("{0}°", _MagCalculator.ResultsOfCalculation.Last().Declination.ChangePerYear.ToString("F4"));
                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnDeclination"].Style.BackColor = System.Drawing.Color.LightBlue;

                    dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1].Cells["ColumnInclination"].Value = string.Format("{0}°", _MagCalculator.ResultsOfCalculation.Last().Inclination.ChangePerYear.ToString("F4"));
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
                    toolStripStatusLabel1.Text = "Calculation complete";
                }
                catch (OperationCanceledException)
                {
                    dataGridViewResults.Rows.Clear();
                    toolStripStatusLabel1.Text = "Calculation cancelled - Ready";
                    _MagCalculator = null;
                }
                catch (Exception ex)
                {
                    dataGridViewResults.Rows.Clear();
                    MessageBox.Show(ex.Message, "Error: Calculating Magnetics", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatusLabel1.Text = "Ready";
                    _MagCalculator = null;
                }
                finally
                {
                    SetUIBusy(false);
                    _calculationCts?.Dispose();
                    _calculationCts = null;
                }
            }
        }

        private void LoadModels(string selected = null)
        {
            Guid selectedIdx;

            Guid.TryParse(selected, out selectedIdx);

            comboBoxModels.DataSource = Models.GetDataTable.DefaultView;

            comboBoxModels.DisplayMember = "ModelName";

            comboBoxModels.ValueMember = "ID";

            if(selectedIdx != Guid.Empty) comboBoxModels.SelectedValue = selectedIdx;
        }

        private async void addModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_calculationCts != null) return;

            using (var fAddModel = new frmAddModel())
            {
                if (string.IsNullOrEmpty(fAddModel.SelectedFilePath))
                    return;

                _calculationCts = new CancellationTokenSource();
                try
                {
                    SetUIBusy(true);
                    toolStripStatusLabel1.Text = "Reading model file...";

                    var progress = new Progress<CalculationProgressInfo>(info =>
                    {
                        toolStripStatusLabel1.Text = info.StatusMessage;
                        toolStripProgressBar1.Value = Math.Min((int)info.PercentComplete, 100);
                    });

                    await fAddModel.LoadModelDataAsync(fAddModel.SelectedFilePath, progress, _calculationCts.Token);

                    SetUIBusy(false);
                    toolStripStatusLabel1.Text = "Ready";

                    if (fAddModel.ShowDialog(this) != DialogResult.OK)
                        return;

                    SetUIBusy(true);
                    toolStripStatusLabel1.Text = "Saving model...";

                    Models.AddOrReplace(fAddModel.Model);
                    await Models.SaveAsync(ModelJson, _calculationCts.Token);

                    LoadModels(fAddModel.Model?.ID.ToString());
                    toolStripStatusLabel1.Text = "Model added successfully";
                }
                catch (OperationCanceledException)
                {
                    toolStripStatusLabel1.Text = "Model loading cancelled - Ready";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error: Adding Model", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatusLabel1.Text = "Ready";
                }
                finally
                {
                    SetUIBusy(false);
                    _calculationCts?.Dispose();
                    _calculationCts = null;
                }
            }
        }

        private async void loadModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_calculationCts != null) return;

            var fDlg = new OpenFileDialog
            {
                Title = @"Select a Model Data File",
                Filter = Properties.Resources.File_Type_All_Coeff_Files,
                Multiselect = false
            };

            if (fDlg.ShowDialog() == DialogResult.Cancel) return;

            var copyToLocation = Path.Combine(ModelFolder, Path.GetFileName(fDlg.FileName));

            _calculationCts = new CancellationTokenSource();
            try
            {
                SetUIBusy(true);
                toolStripStatusLabel1.Text = "Copying model file...";

                File.Copy(fDlg.FileName, copyToLocation, overwrite: true);

                toolStripStatusLabel1.Text = "Reading model file...";
                var progress = new Progress<CalculationProgressInfo>(info =>
                {
                    toolStripStatusLabel1.Text = info.StatusMessage;
                    toolStripProgressBar1.Value = Math.Min((int)info.PercentComplete, 100);
                });

                var model = await ModelReader.ReadAsync(copyToLocation, progress, _calculationCts.Token);

                toolStripStatusLabel1.Text = "Saving model collection...";
                Models.AddOrReplace(model);
                await Models.SaveAsync(ModelJson, _calculationCts.Token);

                LoadModels(model.ID.ToString());
                toolStripStatusLabel1.Text = "Model loaded successfully";
            }
            catch (OperationCanceledException)
            {
                toolStripStatusLabel1.Text = "Model loading cancelled - Ready";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error: Loading Model", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel1.Text = "Ready";
            }
            finally
            {
                SetUIBusy(false);
                _calculationCts?.Dispose();
                _calculationCts = null;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBoxLatitudeDecimal_Validated(object sender, EventArgs e)
        {
            if (_processingEvents) return;

            this.errorProviderCheck.SetError(textBoxLatitudeDecimal, string.Empty);

            var latitude = new Latitude(Convert.ToDouble(textBoxLatitudeDecimal.Text));

            _processingEvents = true;
            TextBoxLatDeg.Text = latitude.Degrees.ToString("F0");
            TextBoxLatMin.Text = latitude.Minutes.ToString("F0");
            TextBoxLatSec.Text = latitude.Seconds.ToString("F4");
            ComboBoxLatDir.SelectedItem = latitude.Hemisphere.ToString();
            _processingEvents = false;
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
            if (_processingEvents) return;

            this.errorProviderCheck.SetError(textBoxLongitudeDecimal, string.Empty);

            var longitude = new Longitude(Convert.ToDouble(textBoxLongitudeDecimal.Text));

            _processingEvents = true;
            TextBoxLongDeg.Text = longitude.Degrees.ToString("F0");
            TextBoxLongMin.Text = longitude.Minutes.ToString("F0");
            TextBoxLongSec.Text = longitude.Seconds.ToString("F4");
            ComboBoxLongDir.SelectedItem = longitude.Hemisphere.ToString();
            _processingEvents = false;
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
            if (_processingEvents) return;

            this.errorProviderCheck.SetError(ComboBoxLongDir, string.Empty);

            var longitude = new Longitude(Convert.ToDouble(TextBoxLongDeg.Text), Convert.ToDouble(TextBoxLongMin.Text), Convert.ToDouble(TextBoxLongSec.Text), ComboBoxLongDir.SelectedItem.ToString());

            _processingEvents = true;
            textBoxLongitudeDecimal.Text = longitude.Decimal.ToString("F8");
            _processingEvents = false;

            ApplicationPreferences.LongitudeHemisphere = ComboBoxLongDir.SelectedItem.ToString();

            ApplicationPreferences.Save(PreferencesJson);
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
            if (_processingEvents) return;

            this.errorProviderCheck.SetError(ComboBoxLatDir, string.Empty);

            var latitude = new Latitude(Convert.ToDouble(TextBoxLatDeg.Text), Convert.ToDouble(TextBoxLatMin.Text), Convert.ToDouble(TextBoxLatSec.Text), ComboBoxLatDir.SelectedItem.ToString());

            _processingEvents = true;
            textBoxLatitudeDecimal.Text = latitude.Decimal.ToString("F8");
            _processingEvents = false;

            ApplicationPreferences.LatitudeHemisphere = ComboBoxLatDir.SelectedItem.ToString();

            ApplicationPreferences.Save(PreferencesJson);
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
            textBoxLongitudeDecimal.Enabled = ApplicationPreferences.UseDecimalDegrees;

            TextBoxLongDeg.Enabled = !ApplicationPreferences.UseDecimalDegrees;
            TextBoxLongMin.Enabled = !ApplicationPreferences.UseDecimalDegrees;
            TextBoxLongSec.Enabled = !ApplicationPreferences.UseDecimalDegrees;
            ComboBoxLongDir.Enabled = !ApplicationPreferences.UseDecimalDegrees;

            textBoxLatitudeDecimal.Enabled = ApplicationPreferences.UseDecimalDegrees;

            TextBoxLatDeg.Enabled = !ApplicationPreferences.UseDecimalDegrees;
            TextBoxLatMin.Enabled = !ApplicationPreferences.UseDecimalDegrees;
            TextBoxLatSec.Enabled = !ApplicationPreferences.UseDecimalDegrees;
            ComboBoxLatDir.Enabled = !ApplicationPreferences.UseDecimalDegrees;
        }

        private void SetElevationDisplay()
        {
            label_Elevation.Text = ApplicationPreferences.UseAltitude
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
                ApplicationPreferences.UseAltitude = fPref.UseAltitude;

                ApplicationPreferences.UseDecimalDegrees = fPref.UseDecimalDegrees;

                ApplicationPreferences.FieldUnit = fPref.FieldUnit;

                ApplicationPreferences.Save(PreferencesJson);
            }

            fPref.Close();

            SetCoordinateDisplay();

            SetElevationDisplay();
        }

        private bool _isSaving;

        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isSaving || _MagCalculator == null) return;

            var fileName = @"Results";

            var fldlg = new SaveFileDialog
            {
                Filter = Resources.File_Type_Text_Tab,
                Title = "Save Results",
                FileName = fileName

            };

            if (fldlg.ShowDialog() == DialogResult.OK)
            {
                _isSaving = true;
                try
                {
                    buttonCalculate.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    UseWaitCursor = true;
                    toolStripStatusLabel1.Text = "Saving results...";
                    await _MagCalculator.SaveResultsAsync(fldlg.FileName);
                    toolStripStatusLabel1.Text = "Results saved";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error: Saving Results", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatusLabel1.Text = "Error saving";
                }
                finally
                {
                    buttonCalculate.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                    UseWaitCursor = false;
                    _isSaving = false;
                }
            }
        }

        private void comboBoxAltitudeUnits_Validated(object sender, EventArgs e)
        {
            ApplicationPreferences.AltitudeUnits = comboBoxAltitudeUnits.SelectedItem.ToString();

            ApplicationPreferences.Save(PreferencesJson);
        }
    }
}
