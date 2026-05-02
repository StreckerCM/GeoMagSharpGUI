namespace GeoMagGUI
{
    partial class CalculationDetailPanel
    {
        private System.ComponentModel.IContainer components = null;

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
        private System.Windows.Forms.Label labelModelSigmaSourceName;
        private System.Windows.Forms.Label labelModelSigmaSource;

        // Coverage badge (HDGM only)
        private System.Windows.Forms.Label labelCoverageBadge;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.labelHeaderRowIndex = new System.Windows.Forms.Label();
            this.labelHeaderDate = new System.Windows.Forms.Label();

            this.grpFieldValues = new System.Windows.Forms.GroupBox();
            this.tblFieldValues = new System.Windows.Forms.TableLayoutPanel();
            this.labelDecl = new System.Windows.Forms.Label();
            this.labelDeclValue = new System.Windows.Forms.Label();
            this.labelDeclSigma = new System.Windows.Forms.Label();
            this.labelIncl = new System.Windows.Forms.Label();
            this.labelInclValue = new System.Windows.Forms.Label();
            this.labelInclSigma = new System.Windows.Forms.Label();
            this.labelH = new System.Windows.Forms.Label();
            this.labelHValue = new System.Windows.Forms.Label();
            this.labelHSigma = new System.Windows.Forms.Label();
            this.labelF = new System.Windows.Forms.Label();
            this.labelFValue = new System.Windows.Forms.Label();
            this.labelFSigma = new System.Windows.Forms.Label();

            this.grpComponents = new System.Windows.Forms.GroupBox();
            this.tblComponents = new System.Windows.Forms.TableLayoutPanel();
            this.labelX = new System.Windows.Forms.Label();
            this.labelXValue = new System.Windows.Forms.Label();
            this.labelXSigma = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelYValue = new System.Windows.Forms.Label();
            this.labelYSigma = new System.Windows.Forms.Label();
            this.labelZ = new System.Windows.Forms.Label();
            this.labelZValue = new System.Windows.Forms.Label();
            this.labelZSigma = new System.Windows.Forms.Label();

            this.grpChange = new System.Windows.Forms.GroupBox();
            this.tblChange = new System.Windows.Forms.TableLayoutPanel();
            this.labelChangeDeclName = new System.Windows.Forms.Label();
            this.labelChangeDecl = new System.Windows.Forms.Label();
            this.labelChangeInclName = new System.Windows.Forms.Label();
            this.labelChangeIncl = new System.Windows.Forms.Label();
            this.labelChangeFName = new System.Windows.Forms.Label();
            this.labelChangeF = new System.Windows.Forms.Label();

            this.grpModel = new System.Windows.Forms.GroupBox();
            this.tblModel = new System.Windows.Forms.TableLayoutPanel();
            this.labelModelNameName = new System.Windows.Forms.Label();
            this.labelModelName = new System.Windows.Forms.Label();
            this.labelModelCategoryName = new System.Windows.Forms.Label();
            this.labelModelCategory = new System.Windows.Forms.Label();
            this.labelModelValidityName = new System.Windows.Forms.Label();
            this.labelModelValidity = new System.Windows.Forms.Label();
            this.labelModelSigmaSourceName = new System.Windows.Forms.Label();
            this.labelModelSigmaSource = new System.Windows.Forms.Label();

            this.labelCoverageBadge = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            this.grpFieldValues.SuspendLayout();
            this.tblFieldValues.SuspendLayout();
            this.grpComponents.SuspendLayout();
            this.tblComponents.SuspendLayout();
            this.grpChange.SuspendLayout();
            this.tblChange.SuspendLayout();
            this.grpModel.SuspendLayout();
            this.tblModel.SuspendLayout();
            this.SuspendLayout();

            // ── Panel header ──
            this.pnlHeader.Controls.Add(this.labelHeaderRowIndex);
            this.pnlHeader.Controls.Add(this.labelHeaderDate);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(8);
            this.pnlHeader.Size = new System.Drawing.Size(380, 40);
            this.pnlHeader.TabIndex = 0;

            this.labelHeaderDate.AutoSize = true;
            this.labelHeaderDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelHeaderDate.Location = new System.Drawing.Point(8, 8);
            this.labelHeaderDate.Name = "labelHeaderDate";
            this.labelHeaderDate.Text = "(no calculation)";

            this.labelHeaderRowIndex.AutoSize = true;
            this.labelHeaderRowIndex.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelHeaderRowIndex.Location = new System.Drawing.Point(120, 12);
            this.labelHeaderRowIndex.Name = "labelHeaderRowIndex";
            this.labelHeaderRowIndex.Text = "";

            // ── Field Values group (3-column) ──
            ConfigureGroupBoxThreeCol(this.grpFieldValues, "Field Values", 60, this.tblFieldValues, 4);
            AddRowThreeCol(this.tblFieldValues, 0, this.labelDecl, "Decl.", this.labelDeclValue, this.labelDeclSigma);
            AddRowThreeCol(this.tblFieldValues, 1, this.labelIncl, "Incl.", this.labelInclValue, this.labelInclSigma);
            AddRowThreeCol(this.tblFieldValues, 2, this.labelH,    "H",     this.labelHValue,    this.labelHSigma);
            AddRowThreeCol(this.tblFieldValues, 3, this.labelF,    "F",     this.labelFValue,    this.labelFSigma);

            // ── Components group (3-column) ──
            ConfigureGroupBoxThreeCol(this.grpComponents, "Components (X N · Y E · Z Down)", 200, this.tblComponents, 3);
            AddRowThreeCol(this.tblComponents, 0, this.labelX, "X (North)",    this.labelXValue, this.labelXSigma);
            AddRowThreeCol(this.tblComponents, 1, this.labelY, "Y (East)",     this.labelYValue, this.labelYSigma);
            AddRowThreeCol(this.tblComponents, 2, this.labelZ, "Z (Vertical)", this.labelZValue, this.labelZSigma);

            // ── Change per year group (2-column) ──
            ConfigureGroupBoxTwoCol(this.grpChange, "Change per Year", 312, this.tblChange, 3);
            AddRowTwoCol(this.tblChange, 0, this.labelChangeDeclName, "Decl.", this.labelChangeDecl);
            AddRowTwoCol(this.tblChange, 1, this.labelChangeInclName, "Incl.", this.labelChangeIncl);
            AddRowTwoCol(this.tblChange, 2, this.labelChangeFName,    "F",     this.labelChangeF);

            // ── Model group (2-column) ──
            ConfigureGroupBoxTwoCol(this.grpModel, "Model", 408, this.tblModel, 4);
            AddRowTwoCol(this.tblModel, 0, this.labelModelNameName,        "Name",     this.labelModelName);
            AddRowTwoCol(this.tblModel, 1, this.labelModelCategoryName,    "Type",     this.labelModelCategory);
            AddRowTwoCol(this.tblModel, 2, this.labelModelValidityName,    "Validity", this.labelModelValidity);
            AddRowTwoCol(this.tblModel, 3, this.labelModelSigmaSourceName, "σ source", this.labelModelSigmaSource);

            // ── Coverage badge (HDGM only — populated by LoadCalculation) ──
            this.labelCoverageBadge.AutoSize = true;
            this.labelCoverageBadge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelCoverageBadge.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular);
            this.labelCoverageBadge.Location = new System.Drawing.Point(16, 540);
            this.labelCoverageBadge.Name = "labelCoverageBadge";
            this.labelCoverageBadge.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.labelCoverageBadge.Text = "✓ NSD covered";
            this.labelCoverageBadge.Visible = false;

            // ── UserControl root ──
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.labelCoverageBadge);
            this.Controls.Add(this.grpModel);
            this.Controls.Add(this.grpChange);
            this.Controls.Add(this.grpComponents);
            this.Controls.Add(this.grpFieldValues);
            this.Controls.Add(this.pnlHeader);
            this.Name = "CalculationDetailPanel";
            this.Size = new System.Drawing.Size(380, 580);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.grpFieldValues.ResumeLayout(false);
            this.tblFieldValues.ResumeLayout(false);
            this.tblFieldValues.PerformLayout();
            this.grpComponents.ResumeLayout(false);
            this.tblComponents.ResumeLayout(false);
            this.tblComponents.PerformLayout();
            this.grpChange.ResumeLayout(false);
            this.tblChange.ResumeLayout(false);
            this.tblChange.PerformLayout();
            this.grpModel.ResumeLayout(false);
            this.tblModel.ResumeLayout(false);
            this.tblModel.PerformLayout();
            this.ResumeLayout(false);
        }

        private void ConfigureGroupBoxTwoCol(System.Windows.Forms.GroupBox grp, string title, int top,
                                             System.Windows.Forms.TableLayoutPanel tbl, int rowCount)
        {
            grp.Controls.Add(tbl);
            grp.Location = new System.Drawing.Point(8, top);
            grp.Name = "grp" + title.Replace(" ", "");
            grp.Size = new System.Drawing.Size(364, rowCount * 24 + 32);
            grp.TabStop = false;
            grp.Text = title;

            tbl.ColumnCount = 2;
            tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tbl.Dock = System.Windows.Forms.DockStyle.Fill;
            tbl.Name = "tbl" + title.Replace(" ", "");
            tbl.RowCount = rowCount;
            for (int i = 0; i < rowCount; i++)
            {
                tbl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            }
            tbl.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);
        }

        private void ConfigureGroupBoxThreeCol(System.Windows.Forms.GroupBox grp, string title, int top,
                                               System.Windows.Forms.TableLayoutPanel tbl, int rowCount)
        {
            grp.Controls.Add(tbl);
            grp.Location = new System.Drawing.Point(8, top);
            grp.Name = "grp" + title.Replace(" ", "");
            grp.Size = new System.Drawing.Size(364, rowCount * 24 + 32);
            grp.TabStop = false;
            grp.Text = title;

            tbl.ColumnCount = 3;
            // [name 70px] [value 110px] [sigma fills rest ~170px]
            tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tbl.Dock = System.Windows.Forms.DockStyle.Fill;
            tbl.Name = "tbl" + title.Replace(" ", "");
            tbl.RowCount = rowCount;
            for (int i = 0; i < rowCount; i++)
            {
                tbl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            }
            tbl.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);
        }

        private void AddRowTwoCol(System.Windows.Forms.TableLayoutPanel tbl, int row,
                                  System.Windows.Forms.Label nameLabel, string nameText,
                                  System.Windows.Forms.Label valueLabel)
        {
            ConfigureNameLabel(nameLabel, nameText, "name" + row);
            ConfigureValueLabel(valueLabel, "value" + row);
            tbl.Controls.Add(nameLabel,  0, row);
            tbl.Controls.Add(valueLabel, 1, row);
        }

        private void AddRowThreeCol(System.Windows.Forms.TableLayoutPanel tbl, int row,
                                    System.Windows.Forms.Label nameLabel, string nameText,
                                    System.Windows.Forms.Label valueLabel,
                                    System.Windows.Forms.Label sigmaLabel)
        {
            ConfigureNameLabel(nameLabel, nameText, "name" + row);
            ConfigureValueLabel(valueLabel, "value" + row);
            ConfigureValueLabel(sigmaLabel, "sigma" + row);
            sigmaLabel.ForeColor = System.Drawing.Color.FromArgb(120, 80, 0);
            tbl.Controls.Add(nameLabel,  0, row);
            tbl.Controls.Add(valueLabel, 1, row);
            tbl.Controls.Add(sigmaLabel, 2, row);
        }

        private static void ConfigureNameLabel(System.Windows.Forms.Label lbl, string text, string name)
        {
            lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl.AutoSize = false;
            lbl.ForeColor = System.Drawing.SystemColors.GrayText;
            lbl.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            lbl.Name = name;
            lbl.Text = text;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        }

        private static void ConfigureValueLabel(System.Windows.Forms.Label lbl, string name)
        {
            lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl.AutoSize = false;
            lbl.Font = new System.Drawing.Font("Consolas", 9F);
            lbl.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            lbl.Name = name;
            lbl.Text = "";
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        }
    }
}
