using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GeoMagGUI
{
    public partial class FrmMain : Form
    {
        public readonly string ModelFolder;

        public DataTable DtModels;

        public FrmMain()
        {
            InitializeComponent();

            ModelFolder = string.Format("{0}\\coefficient\\", Application.StartupPath);

            DtModels = new DataTable();

            DtModels.Columns.Add(new DataColumn("ID", typeof(Int32)));

            DtModels.Columns.Add(new DataColumn("ModelName", typeof(string)));

            DtModels.Columns.Add(new DataColumn("FileName", typeof(string)));

            LoadModels();
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

                var magCalc = new GeoMagSharp.GeoMag(modelFile);
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
            }
            else
            {
                labelDateFrom.Visible = true;
                labelDateTo.Visible = true;
                dateTimePicker2.Visible = true; 
            }
        }

        private void comboBoxGeodeticType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxGeodeticType.SelectedItem == null) return;

            var selectedText = comboBoxGeodeticType.SelectedItem.ToString();

            if (selectedText.Equals("Decimal Degrees", StringComparison.OrdinalIgnoreCase))
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
            else if (selectedText.Equals("Degrees, Minutes, and Seconds", StringComparison.OrdinalIgnoreCase))
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
    }
}
