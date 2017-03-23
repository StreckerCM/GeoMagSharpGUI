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
        private MagneticModelSet _Model;

        public MagneticModelSet Model
        {
            get
            {
                return _Model;
            }
        }

        public frmAddModel()
        {
            InitializeComponent();

            var modelFile = AddFile();

            LoadModelData(modelFile);

        }

        private void LoadModelData(string modelFile)
        {
            _Model = ModelReader.Read(modelFile);

            if(_Model != null)
            {
                _Model.Name = Path.GetFileNameWithoutExtension(Model.FileNames.First());

                textBoxModelName.Text = _Model.Name;

                labelModelType.Text = Model.Type.ToString();

                labelModelNumberOfModels.Text = Model.NumberOfModels.ToString();

                labelModelDateMin.Text = Model.MinDate.ToDateTime().ToShortDateString();

                labelModelDateMax.Text = Model.MaxDate.ToDateTime().ToShortDateString();
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

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void textBoxModelName_Validated(object sender, EventArgs e)
        {
            Model.Name = textBoxModelName.Text;
        }
    }
}
