using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using GeoMagSharp;

// Diagnostic tooltips on value labels (#61): hovering reveals the full
// formatted string even when the cell visually clips it.

namespace GeoMagGUI
{
    /// <summary>
    /// Side panel that displays the full breakdown of a selected calculation row:
    /// field values with per-component sigma, components, change/yr, and model metadata.
    /// Phase 1 + 2 of issue #61 (results grid redesign).
    /// </summary>
    public partial class CalculationDetailPanel : UserControl
    {
        private readonly ToolTip _valueTooltip = new ToolTip();

        public CalculationDetailPanel()
        {
            InitializeComponent();
            Clear();
        }

        private void SetValueWithTooltip(Label lbl, string text)
        {
            lbl.Text = text;
            _valueTooltip.SetToolTip(lbl, text);
        }

        /// <summary>
        /// Reset all labels to "—" placeholders. Called when the grid is empty
        /// or when no row is selected.
        /// </summary>
        public void Clear()
        {
            labelHeaderDate.Text = "(no calculation)";
            labelHeaderRowIndex.Text = string.Empty;

            labelDeclValue.Text = "—";
            labelInclValue.Text = "—";
            labelHValue.Text = "—";
            labelFValue.Text = "—";

            labelXValue.Text = "—";
            labelYValue.Text = "—";
            labelZValue.Text = "—";

            labelChangeDecl.Text = "—";
            labelChangeIncl.Text = "—";
            labelChangeF.Text = "—";

            labelModelName.Text = "—";
            labelModelCategory.Text = "—";
            labelModelValidity.Text = "—";
            labelModelSigmaSource.Text = "—";

            labelCoverageBadge.Visible = false;
        }

        /// <summary>
        /// Populate the panel with a single row's calculation result.
        /// </summary>
        /// <param name="result">The calculation for the selected date.</param>
        /// <param name="rowIndex">Zero-based row index in the grid.</param>
        /// <param name="totalRows">Total rows currently in the grid.</param>
        /// <param name="changePerYearLast">The last result's secular variation values.</param>
        /// <param name="descriptor">The discovered model descriptor (may be null for legacy paths).</param>
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

            // Read per-point sigmas (HDGM populates these). Fall back to ISCWSA values
            // for D/I/F when per-point are absent. X/Y/Z/H per-component sigmas are HDGM-only
            // until GeoMagSharp#13 (WMM/WMMHR Level 2) lands.
            var unc = result.Uncertainty;
            double? sD = unc?.SigmaD ?? unc?.Declination;
            double? sI = unc?.SigmaI ?? unc?.DipAngle;
            double? sH = unc?.SigmaH;
            double? sF = unc?.SigmaF ?? unc?.TotalField;
            double? sX = unc?.SigmaX;
            double? sY = unc?.SigmaY;
            double? sZ = unc?.SigmaZ;

            // Field values (with sigma inline)
            SetValueWithTooltip(labelDeclValue, FormatDegreesWithSigma(result.Declination?.Value, sD));
            SetValueWithTooltip(labelInclValue, FormatDegreesWithSigma(result.Inclination?.Value, sI));
            SetValueWithTooltip(labelHValue,    FormatNanoTeslaWithSigma(result.HorizontalIntensity?.Value, sH));
            SetValueWithTooltip(labelFValue,    FormatNanoTeslaWithSigma(result.TotalField?.Value, sF));

            // Components
            SetValueWithTooltip(labelXValue, FormatNanoTeslaWithSigma(result.NorthComp?.Value, sX));
            SetValueWithTooltip(labelYValue, FormatNanoTeslaWithSigma(result.EastComp?.Value, sY));
            SetValueWithTooltip(labelZValue, FormatNanoTeslaWithSigma(result.VerticalComp?.Value, sZ));

            // Change per year (use last row's values; secular variation is reported per-model
            // not per-row, and the existing app convention is to show the latest date's values)
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

            // Coverage badge — HDGM only, color based on NSD coverage flag
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

        // ─── Formatters ─────────────────────────────────────────────

        private static string FormatDegreesWithSigma(double? v, double? sigma)
        {
            if (!v.HasValue) return "—";
            string main = v.Value.ToString("F4", CultureInfo.CurrentCulture) + "°";
            if (sigma.HasValue)
                return main + "  ± " + sigma.Value.ToString("F2", CultureInfo.CurrentCulture) + "°";
            return main;
        }

        private static string FormatNanoTeslaWithSigma(double? v, double? sigma)
        {
            if (!v.HasValue) return "—";
            string main = v.Value.ToString("N0", CultureInfo.CurrentCulture) + " nT";
            if (sigma.HasValue)
                return main + "  ± " + sigma.Value.ToString("N0", CultureInfo.CurrentCulture) + " nT";
            return main;
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
