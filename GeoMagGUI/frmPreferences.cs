﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeoMagGUI
{
    public partial class frmPreferences : Form
    {
        public bool UseDecimalDegrees
        {
            get
            {
                return comboBoxCoordianteFormat.SelectedItem.ToString().Equals(@"Decimal Degrees", StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool UseAltitude
        {
            get
            {
                return comboBoxElevationReference.SelectedItem.ToString().Equals(@"Altitude Above MSL", StringComparison.OrdinalIgnoreCase);
            }
        }

        public frmPreferences(FrmMain fmain)
        {
            InitializeComponent();

            Owner = fmain;

            LoadSettings();
        }

        private void LoadSettings()
        {
            var fmain = (FrmMain)Owner;

            comboBoxCoordianteFormat.SelectedItem = fmain._useDecimalDegrees
                                    ? comboBoxCoordianteFormat.Items[0]
                                    : comboBoxCoordianteFormat.Items[1];

            comboBoxElevationReference.SelectedItem = fmain._useAltitude
                        ? comboBoxElevationReference.Items[0]
                        : comboBoxElevationReference.Items[1];
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

    }
}
