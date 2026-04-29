using System;
using System.IO;
using System.Windows.Forms;

using GeoMagSharp;

namespace GeoMagGUI
{
    public partial class frmAddModel : Form
    {
        private ModelDescriptor _Model;

        public ModelDescriptor Model
        {
            get { return _Model; }
        }

        /// <summary>
        /// Gets the file path selected by the user in the open file dialog.
        /// Empty string if user cancelled.
        /// </summary>
        public string SelectedFilePath { get; private set; }

        public frmAddModel()
        {
            InitializeComponent();

            SelectedFilePath = AddFile();

            if (!string.IsNullOrEmpty(SelectedFilePath))
            {
                LoadModelData(SelectedFilePath);
            }
        }

        private void LoadModelData(string modelFile)
        {
            _Model = ModelDiscovery.DescribeFile(modelFile);

            DisplayModelData();
        }

        private void DisplayModelData()
        {
            if (_Model == null)
            {
                MessageBox.Show(this, "Failed to identify model from the selected file.",
                    "Model Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBoxModelName.Text = _Model.DisplayName;

            labelModelType.Text = _Model.DetectedType.ToString();

            labelModelNumberOfModels.Text = string.Empty;

            labelModelDateMin.Text = _Model.MinDate.HasValue
                ? _Model.MinDate.Value.ToDateTime().ToShortDateString()
                : "—";

            labelModelDateMax.Text = _Model.MaxDate.HasValue
                ? _Model.MaxDate.Value.ToDateTime().ToShortDateString()
                : "—";
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
            AddFile();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Hide();
        }
    }
}
