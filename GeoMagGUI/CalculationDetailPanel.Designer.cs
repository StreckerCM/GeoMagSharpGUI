namespace GeoMagGUI
{
    partial class CalculationDetailPanel
    {
        private System.ComponentModel.IContainer components = null;

        // All control fields are declared here for Visual Studio Designer parsing.
        // The actual control configuration (sizing, text, child relationships)
        // is performed in BuildLayout() in CalculationDetailPanel.cs at runtime,
        // so the Designer can render this UserControl as an empty box without
        // choking on layout helper methods.

        // Header
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label labelHeaderDate;
        private System.Windows.Forms.Label labelHeaderRowIndex;

        // Field Values group (3-col: name | value | sigma)
        private System.Windows.Forms.GroupBox grpFieldValues;
        private System.Windows.Forms.TableLayoutPanel tblFieldValues;
        private System.Windows.Forms.Label labelDecl;
        private System.Windows.Forms.Label labelDeclValue;
        private System.Windows.Forms.Label labelDeclSigma;
        private System.Windows.Forms.Label labelIncl;
        private System.Windows.Forms.Label labelInclValue;
        private System.Windows.Forms.Label labelInclSigma;
        private System.Windows.Forms.Label labelH;
        private System.Windows.Forms.Label labelHValue;
        private System.Windows.Forms.Label labelHSigma;
        private System.Windows.Forms.Label labelF;
        private System.Windows.Forms.Label labelFValue;
        private System.Windows.Forms.Label labelFSigma;

        // Components group (3-col: name | value | sigma)
        private System.Windows.Forms.GroupBox grpComponents;
        private System.Windows.Forms.TableLayoutPanel tblComponents;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelXValue;
        private System.Windows.Forms.Label labelXSigma;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelYValue;
        private System.Windows.Forms.Label labelYSigma;
        private System.Windows.Forms.Label labelZ;
        private System.Windows.Forms.Label labelZValue;
        private System.Windows.Forms.Label labelZSigma;

        // Change/yr group (2-col: name | value)
        private System.Windows.Forms.GroupBox grpChange;
        private System.Windows.Forms.TableLayoutPanel tblChange;
        private System.Windows.Forms.Label labelChangeDeclName;
        private System.Windows.Forms.Label labelChangeDecl;
        private System.Windows.Forms.Label labelChangeInclName;
        private System.Windows.Forms.Label labelChangeIncl;
        private System.Windows.Forms.Label labelChangeFName;
        private System.Windows.Forms.Label labelChangeF;

        // Model group (2-col: name | value)
        private System.Windows.Forms.GroupBox grpModel;
        private System.Windows.Forms.TableLayoutPanel tblModel;
        private System.Windows.Forms.Label labelModelNameName;
        private System.Windows.Forms.Label labelModelName;
        private System.Windows.Forms.Label labelModelCategoryName;
        private System.Windows.Forms.Label labelModelCategory;
        private System.Windows.Forms.Label labelModelValidityName;
        private System.Windows.Forms.Label labelModelValidity;
        private System.Windows.Forms.Label labelModelEpochsName;
        private System.Windows.Forms.Label labelModelEpochs;
        private System.Windows.Forms.Label labelModelAltitudeName;
        private System.Windows.Forms.Label labelModelAltitude;
        private System.Windows.Forms.Label labelModelSigmaSourceName;
        private System.Windows.Forms.Label labelModelSigmaSource;

        // Chips (degree shown when descriptor.MaxDegree is known; coverage HDGM-only)
        private System.Windows.Forms.Label labelDegreeBadge;
        private System.Windows.Forms.Label labelCoverageBadge;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Visual Studio Designer-friendly stub. The actual control hierarchy
        /// is built in BuildLayout() in CalculationDetailPanel.cs at runtime.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Name = "CalculationDetailPanel";
            this.Size = new System.Drawing.Size(380, 668);
            this.ResumeLayout(false);
        }
    }
}
