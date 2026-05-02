using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using GeoMagSharp;

namespace GeoMagGUI
{
    /// <summary>
    /// Side panel that displays the full breakdown of a selected calculation row.
    /// Three-column layout for value rows: [name | value | sigma]. The sigma
    /// column gets dedicated horizontal space rather than being concatenated
    /// into the value cell.
    /// </summary>
    /// <remarks>
    /// The control hierarchy is built in <see cref="BuildLayout"/> at runtime
    /// rather than in Designer.cs, so Visual Studio's WinForms Designer can open
    /// the file without choking on layout helper methods. The Designer will show
    /// an empty user control preview but the actual rendering at runtime is
    /// fully configured.
    /// </remarks>
    public partial class CalculationDetailPanel : UserControl
    {
        public CalculationDetailPanel()
        {
            InitializeComponent();   // Designer stub — sets size/scaling only
            BuildLayout();           // creates and configures all child controls
            Clear();
        }

        public void Clear()
        {
            labelHeaderDate.Text = "(no calculation)";
            labelHeaderRowIndex.Text = string.Empty;

            labelDeclValue.Text = "—";  labelDeclSigma.Text = "";
            labelInclValue.Text = "—";  labelInclSigma.Text = "";
            labelHValue.Text    = "—";  labelHSigma.Text    = "";
            labelFValue.Text    = "—";  labelFSigma.Text    = "";

            labelXValue.Text = "—";  labelXSigma.Text = "";
            labelYValue.Text = "—";  labelYSigma.Text = "";
            labelZValue.Text = "—";  labelZSigma.Text = "";

            labelChangeDecl.Text = "—";
            labelChangeIncl.Text = "—";
            labelChangeF.Text = "—";

            labelModelName.Text = "—";
            labelModelCategory.Text = "—";
            labelModelValidity.Text = "—";
            labelModelSigmaSource.Text = "—";

            labelCoverageBadge.Visible = false;
        }

        public void LoadCalculation(MagneticCalculations result,
                                    int rowIndex,
                                    int totalRows,
                                    MagneticCalculations changePerYearLast,
                                    ModelDescriptor descriptor)
        {
            if (result == null)
            {
                Clear();
                return;
            }

            labelHeaderDate.Text = result.Date.ToShortDateString();
            labelHeaderRowIndex.Text = totalRows > 1
                ? string.Format(CultureInfo.CurrentCulture, "row {0} of {1}", rowIndex + 1, totalRows)
                : string.Empty;

            var unc = result.Uncertainty;
            double? sD = NonZero(unc?.SigmaD) ?? NonZero(unc?.Declination);
            double? sI = NonZero(unc?.SigmaI) ?? NonZero(unc?.DipAngle);
            double? sH = NonZero(unc?.SigmaH);
            double? sF = NonZero(unc?.SigmaF) ?? NonZero(unc?.TotalField);
            double? sX = NonZero(unc?.SigmaX);
            double? sY = NonZero(unc?.SigmaY);
            double? sZ = NonZero(unc?.SigmaZ);

            labelDeclValue.Text = FormatDegrees(result.Declination?.Value);
            labelDeclSigma.Text = FormatSigmaDegrees(sD);
            labelInclValue.Text = FormatDegrees(result.Inclination?.Value);
            labelInclSigma.Text = FormatSigmaDegrees(sI);
            labelHValue.Text    = FormatNanoTesla(result.HorizontalIntensity?.Value);
            labelHSigma.Text    = FormatSigmaNanoTesla(sH);
            labelFValue.Text    = FormatNanoTesla(result.TotalField?.Value);
            labelFSigma.Text    = FormatSigmaNanoTesla(sF);

            labelXValue.Text = FormatNanoTesla(result.NorthComp?.Value);
            labelXSigma.Text = FormatSigmaNanoTesla(sX);
            labelYValue.Text = FormatNanoTesla(result.EastComp?.Value);
            labelYSigma.Text = FormatSigmaNanoTesla(sY);
            labelZValue.Text = FormatNanoTesla(result.VerticalComp?.Value);
            labelZSigma.Text = FormatSigmaNanoTesla(sZ);

            if (changePerYearLast != null)
            {
                labelChangeDecl.Text = FormatChangeDegrees(changePerYearLast.Declination?.ChangePerYear);
                labelChangeIncl.Text = FormatChangeDegrees(changePerYearLast.Inclination?.ChangePerYear);
                labelChangeF.Text    = FormatChangeNanoTesla(changePerYearLast.TotalField?.ChangePerYear);
            }
            else
            {
                labelChangeDecl.Text = "—";
                labelChangeIncl.Text = "—";
                labelChangeF.Text = "—";
            }

            if (descriptor != null)
            {
                labelModelName.Text = string.IsNullOrEmpty(descriptor.DisplayName) ? "—" : descriptor.DisplayName;
                labelModelCategory.Text = descriptor.DetectedType.ToString();
                labelModelValidity.Text = FormatValidityRange(descriptor.MinDate, descriptor.MaxDate);
            }
            else
            {
                labelModelName.Text = "—";
                labelModelCategory.Text = "—";
                labelModelValidity.Text = "—";
            }

            labelModelSigmaSource.Text = (unc != null && unc.SigmaD.HasValue)
                ? "NOAA DLL (per-point)"
                : (unc != null ? "ISCWSA (default)" : "—");

            UpdateCoverageBadge(unc);
        }

        // ─── Layout construction ────────────────────────────────────────

        private void BuildLayout()
        {
            SuspendLayout();

            // Header panel
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Padding = new Padding(8),
                Size = new Size(380, 40),
                Name = "pnlHeader"
            };
            labelHeaderDate = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(8, 8),
                Name = "labelHeaderDate",
                Text = "(no calculation)"
            };
            labelHeaderRowIndex = new Label
            {
                AutoSize = true,
                ForeColor = SystemColors.GrayText,
                Location = new Point(140, 12),
                Name = "labelHeaderRowIndex",
                Text = ""
            };
            pnlHeader.Controls.Add(labelHeaderDate);
            pnlHeader.Controls.Add(labelHeaderRowIndex);

            // Field Values group (3-col)
            grpFieldValues = MakeGroupBox("Field Values", 60, 4);
            tblFieldValues = MakeTableLayoutPanel(threeColumns: true, rowCount: 4);
            grpFieldValues.Controls.Add(tblFieldValues);
            labelDecl       = MakeNameLabel("Decl.");
            labelDeclValue  = MakeValueLabel();
            labelDeclSigma  = MakeSigmaLabel();
            AddRow3(tblFieldValues, 0, labelDecl, labelDeclValue, labelDeclSigma);
            labelIncl       = MakeNameLabel("Incl.");
            labelInclValue  = MakeValueLabel();
            labelInclSigma  = MakeSigmaLabel();
            AddRow3(tblFieldValues, 1, labelIncl, labelInclValue, labelInclSigma);
            labelH       = MakeNameLabel("H");
            labelHValue  = MakeValueLabel();
            labelHSigma  = MakeSigmaLabel();
            AddRow3(tblFieldValues, 2, labelH, labelHValue, labelHSigma);
            labelF       = MakeNameLabel("F");
            labelFValue  = MakeValueLabel();
            labelFSigma  = MakeSigmaLabel();
            AddRow3(tblFieldValues, 3, labelF, labelFValue, labelFSigma);

            // Components group (3-col)
            grpComponents = MakeGroupBox("Components (X N · Y E · Z Down)", 200, 3);
            tblComponents = MakeTableLayoutPanel(threeColumns: true, rowCount: 3);
            grpComponents.Controls.Add(tblComponents);
            labelX       = MakeNameLabel("X (North)");
            labelXValue  = MakeValueLabel();
            labelXSigma  = MakeSigmaLabel();
            AddRow3(tblComponents, 0, labelX, labelXValue, labelXSigma);
            labelY       = MakeNameLabel("Y (East)");
            labelYValue  = MakeValueLabel();
            labelYSigma  = MakeSigmaLabel();
            AddRow3(tblComponents, 1, labelY, labelYValue, labelYSigma);
            labelZ       = MakeNameLabel("Z (Vertical)");
            labelZValue  = MakeValueLabel();
            labelZSigma  = MakeSigmaLabel();
            AddRow3(tblComponents, 2, labelZ, labelZValue, labelZSigma);

            // Change per Year group (2-col)
            grpChange = MakeGroupBox("Change per Year", 312, 3);
            tblChange = MakeTableLayoutPanel(threeColumns: false, rowCount: 3);
            grpChange.Controls.Add(tblChange);
            labelChangeDeclName = MakeNameLabel("Decl.");
            labelChangeDecl     = MakeValueLabel();
            AddRow2(tblChange, 0, labelChangeDeclName, labelChangeDecl);
            labelChangeInclName = MakeNameLabel("Incl.");
            labelChangeIncl     = MakeValueLabel();
            AddRow2(tblChange, 1, labelChangeInclName, labelChangeIncl);
            labelChangeFName    = MakeNameLabel("F");
            labelChangeF        = MakeValueLabel();
            AddRow2(tblChange, 2, labelChangeFName, labelChangeF);

            // Model group (2-col)
            grpModel = MakeGroupBox("Model", 408, 4);
            tblModel = MakeTableLayoutPanel(threeColumns: false, rowCount: 4);
            grpModel.Controls.Add(tblModel);
            labelModelNameName        = MakeNameLabel("Name");
            labelModelName            = MakeValueLabel();
            AddRow2(tblModel, 0, labelModelNameName, labelModelName);
            labelModelCategoryName    = MakeNameLabel("Type");
            labelModelCategory        = MakeValueLabel();
            AddRow2(tblModel, 1, labelModelCategoryName, labelModelCategory);
            labelModelValidityName    = MakeNameLabel("Validity");
            labelModelValidity        = MakeValueLabel();
            AddRow2(tblModel, 2, labelModelValidityName, labelModelValidity);
            labelModelSigmaSourceName = MakeNameLabel("σ source");
            labelModelSigmaSource     = MakeValueLabel();
            AddRow2(tblModel, 3, labelModelSigmaSourceName, labelModelSigmaSource);

            // Coverage badge (populated by LoadCalculation)
            labelCoverageBadge = new Label
            {
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                Location = new Point(16, 540),
                Name = "labelCoverageBadge",
                Padding = new Padding(8, 3, 8, 3),
                Text = "✓ NSD covered",
                Visible = false
            };

            // Add everything to the user control (order: bottom-most last)
            Controls.Add(labelCoverageBadge);
            Controls.Add(grpModel);
            Controls.Add(grpChange);
            Controls.Add(grpComponents);
            Controls.Add(grpFieldValues);
            Controls.Add(pnlHeader);

            ResumeLayout(true);
        }

        private static GroupBox MakeGroupBox(string title, int top, int rowCount)
        {
            return new GroupBox
            {
                Location = new Point(8, top),
                Name = "grp_" + title.Replace(" ", "_"),
                Size = new Size(364, rowCount * 24 + 32),
                TabStop = false,
                Text = title
            };
        }

        private static TableLayoutPanel MakeTableLayoutPanel(bool threeColumns, int rowCount)
        {
            var tbl = new TableLayoutPanel
            {
                ColumnCount = threeColumns ? 3 : 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(6, 2, 6, 2),
                RowCount = rowCount
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            if (threeColumns)
            {
                tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
                tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            }
            else
            {
                tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            }
            for (int i = 0; i < rowCount; i++)
            {
                tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            }
            return tbl;
        }

        private static Label MakeNameLabel(string text)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                ForeColor = SystemColors.GrayText,
                Margin = new Padding(3, 0, 3, 0),
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private static Label MakeValueLabel()
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font("Consolas", 9F),
                Margin = new Padding(3, 0, 3, 0),
                Text = "",
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private static Label MakeSigmaLabel()
        {
            var lbl = MakeValueLabel();
            lbl.ForeColor = Color.FromArgb(120, 80, 0);
            return lbl;
        }

        private static void AddRow2(TableLayoutPanel tbl, int row, Label nameLabel, Label valueLabel)
        {
            tbl.Controls.Add(nameLabel,  0, row);
            tbl.Controls.Add(valueLabel, 1, row);
        }

        private static void AddRow3(TableLayoutPanel tbl, int row, Label nameLabel, Label valueLabel, Label sigmaLabel)
        {
            tbl.Controls.Add(nameLabel,  0, row);
            tbl.Controls.Add(valueLabel, 1, row);
            tbl.Controls.Add(sigmaLabel, 2, row);
        }

        // ─── Coverage badge ─────────────────────────────────────────────

        private void UpdateCoverageBadge(GeomagneticUncertainty unc)
        {
            if (unc == null || !unc.HighResolutionCoverage.HasValue)
            {
                labelCoverageBadge.Visible = false;
                return;
            }

            if (unc.HighResolutionCoverage.Value)
            {
                labelCoverageBadge.Text = "✓ NSD covered";
                labelCoverageBadge.BackColor = Color.FromArgb(230, 244, 234);
                labelCoverageBadge.ForeColor = Color.FromArgb(19, 115, 51);
            }
            else
            {
                labelCoverageBadge.Text = "⚠ Satellite fallback";
                labelCoverageBadge.BackColor = Color.FromArgb(254, 247, 224);
                labelCoverageBadge.ForeColor = Color.FromArgb(176, 96, 0);
            }
            labelCoverageBadge.Visible = true;
        }

        // ─── Formatters ─────────────────────────────────────────────────

        private static double? NonZero(double? v) => (v.HasValue && v.Value != 0.0) ? v : null;

        private static string FormatDegrees(double? v)
        {
            return v.HasValue ? v.Value.ToString("F4", CultureInfo.CurrentCulture) + "°" : "—";
        }

        private static string FormatNanoTesla(double? v)
        {
            return v.HasValue ? v.Value.ToString("N0", CultureInfo.CurrentCulture) + " nT" : "";
        }

        private static string FormatSigmaDegrees(double? v)
        {
            return v.HasValue ? "± " + v.Value.ToString("F2", CultureInfo.CurrentCulture) + "°" : "";
        }

        private static string FormatSigmaNanoTesla(double? v)
        {
            return v.HasValue ? "± " + v.Value.ToString("N0", CultureInfo.CurrentCulture) + " nT" : "";
        }

        private static string FormatChangeDegrees(double? v)
        {
            if (!v.HasValue) return "—";
            string sign = v.Value >= 0 ? "+" : "";
            return sign + v.Value.ToString("F4", CultureInfo.CurrentCulture) + "°/yr";
        }

        private static string FormatChangeNanoTesla(double? v)
        {
            if (!v.HasValue) return "—";
            string sign = v.Value >= 0 ? "+" : "";
            return sign + v.Value.ToString("F2", CultureInfo.CurrentCulture) + " nT/yr";
        }

        private static string FormatValidityRange(double? minDate, double? maxDate)
        {
            if (!minDate.HasValue && !maxDate.HasValue) return "—";
            string lo = minDate.HasValue ? ((int)minDate.Value).ToString(CultureInfo.CurrentCulture) : "?";
            string hi = maxDate.HasValue ? ((int)maxDate.Value).ToString(CultureInfo.CurrentCulture) : "?";
            return lo + " – " + hi;
        }
    }
}
