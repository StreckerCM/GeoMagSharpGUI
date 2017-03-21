using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GeoMagSharp;
using System.IO;

namespace GeoMagGUI
{
    public partial class frmAddModel : Form
    {
        private MagneticModelSet Model;

        public frmAddModel()
        {
            InitializeComponent();

            var modelFile = AddFile();

            LoadModelData(modelFile);

        }

        private void LoadModelData(string modelFile)
        {
            Model = ModelReader.Read(modelFile);

            if(Model != null)
            {
                textBoxModelName.Text = Path.GetFileNameWithoutExtension(Model.FileName);

                dateTimePickerMin.Value = Model.MinDate.ToDateTime();

                dateTimePickerMax.Value = Model.MaxDate.ToDateTime();

                textBoxEarthRadius.Text = Model.EarthRadius.ToString();

                comboBoxEarthRadiusUnit.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(this, "", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private string AddFile()
        {
            var fDlg = new OpenFileDialog
            {
                Title = @"Select the main Model Data File",
                Filter = Properties.Resources.File_Type_All_Coeff_Files,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false
            };

            if (fDlg.ShowDialog() == DialogResult.Cancel) return string.Empty;

            dataGridViewFiles.Rows.Add();

            var dRow = dataGridViewFiles.Rows[dataGridViewFiles.Rows.Count - 1];

            dRow.Cells["ColumnFilePath"].Value = fDlg.FileName;

            dRow.Cells["ColumnFileName"].Value = Path.GetFileName(fDlg.FileName);

            return fDlg.FileName;
        }

        private void buttonAddFile_Click(object sender, EventArgs e)
        {
            var modelFile = AddFile();
        }
    }
}
