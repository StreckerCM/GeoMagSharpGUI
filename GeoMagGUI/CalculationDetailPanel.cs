using System;
using System.Globalization;
using System.Windows.Forms;
using GeoMagSharp;

namespace GeoMagGUI
{
    /// <summary>
    /// Side panel that displays the full breakdown of a selected calculation row.
    /// Three-column layout for value rows: [name | value | sigma]. The sigma column
    /// gets dedicated horizontal space rather than being concatenated into the value.
    /// </summary>
    public partial class CalculationDetailPanel : UserControl
    {
        public CalculationDetailPanel()
        {
            InitializeComponent();
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

            // Per-point sigmas (HDGM populates these). Fall back to ISCWSA values
            // for D/I/F when per-point are absent. X/Y/Z/H per-component sigmas are
            // HDGM-only until GeoMagSharp#13 (WMM/WMMHR Level 2) lands.
            var unc = result.Uncertainty;
            double? sD = NonZero(unc?.SigmaD) ?? NonZero(unc?.Declination);
            double? sI = NonZero(unc?.SigmaI) ?? NonZero(unc?.DipAngle);
            double? sH = NonZero(unc?.SigmaH);
            double? sF = NonZero(unc?.SigmaF) ?? NonZero(unc?.TotalField);
            double? sX = NonZero(unc?.SigmaX);
            double? sY = NonZero(unc?.SigmaY);
            double? sZ = NonZero(unc?.SigmaZ);

            // Field values + sigmas (separate labels in dedicated columns)
            labelDeclValue.Text = FormatDegrees(result.Declination?.Value);
            labelDeclSigma.Text = FormatSigmaDegrees(sD);
            labelInclValue.Text = FormatDegrees(result.Inclination?.Value);
            labelInclSigma.Text = FormatSigmaDegrees(sI);
            labelHValue.Text    = FormatNanoTesla(result.HorizontalIntensity?.Value);
            labelHSigma.Text    = FormatSigmaNanoTesla(sH);
            labelFValue.Text    = FormatNanoTesla(result.TotalField?.Value);
            labelFSigma.Text    = FormatSigmaNanoTesla(sF);

            // Components
            labelXValue.Text = FormatNanoTesla(result.NorthComp?.Value);
            labelXSigma.Text = FormatSigmaNanoTesla(sX);
            labelYValue.Text = FormatNanoTesla(result.EastComp?.Value);
            labelYSigma.Text = FormatSigmaNanoTesla(sY);
            labelZValue.Text = FormatNanoTesla(result.VerticalComp?.Value);
            labelZSigma.Text = FormatSigmaNanoTesla(sZ);

            // Change per year
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

            // Model
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
                labelCoverageBadge.BackColor = System.Drawing.Color.FromArgb(230, 244, 234);
                labelCoverageBadge.ForeColor = System.Drawing.Color.FromArgb(19, 115, 51);
            }
            else
            {
                labelCoverageBadge.Text = "⚠ Satellite fallback";
                labelCoverageBadge.BackColor = System.Drawing.Color.FromArgb(254, 247, 224);
                labelCoverageBadge.ForeColor = System.Drawing.Color.FromArgb(176, 96, 0);
            }
            labelCoverageBadge.Visible = true;
        }

        // ─── Formatters ─────────────────────────────────────────────

        /// <summary>Treat exact-zero sigmas as "not provided" — saves the user from misreading "± 0.00°" as if HDGM had perfect knowledge.</summary>
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
