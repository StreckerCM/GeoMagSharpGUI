using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeoMagGUI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void buttonSelectModel_Click(object sender, EventArgs e)
        {
            var fDlg = new OpenFileDialog
            {
                Title = "Select a Model Data File",
                Filter = "COF | *.COF",
                InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false
            };

            if (fDlg.ShowDialog() != DialogResult.Cancel)
            {
                GeoMagSharp.GeoMag magCalc = new GeoMagSharp.GeoMag(fDlg.FileName);
            }
        }
    }
}
