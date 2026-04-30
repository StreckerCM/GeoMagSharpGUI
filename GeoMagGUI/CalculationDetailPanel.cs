using System;
using System.Globalization;
using System.Windows.Forms;
using GeoMagSharp;

namespace GeoMagGUI
{
    /// <summary>
    /// Side panel that displays the full breakdown of a selected calculation row:
    /// field values, components, change/yr, and model metadata.
    /// Phase 1 of issue #61 (results grid redesign).
    /// </summary>
    public partial class CalculationDetailPanel : UserControl
    {
        public CalculationDetailPanel()
        {
            InitializeComponent();
            Clear();
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
        }

        /// <summary>
        /// Populate the panel with a single row's calculation result.
        /// </summary>
        /// <param name="result">The calculation for the selected date.</param>
        /// <param name="rowIndex">Zero-based row index in the grid.</param>
        /// <param name="totalRows">Total rows currently in the grid.</param>
        /// <param name="changePerYearLast">The last result's secular variation values (same for all rows in a calc).</param>
        /// <param name="modelName">Display name of the loaded model.</param>
        /// <param name="modelType">Detected type of the loaded model.</param>
        public void LoadCalculation(MagneticCalculations result,
                                    int rowIndex,
                                    int totalRows,
                                    MagneticCalculations changePerYearLast,
                                    string modelName,
                                    knownModels modelType)
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

            // Field values
            labelDeclValue.Text = FormatDegrees(result.Declination?.Value);
            labelInclValue.Text = FormatDegrees(result.Inclination?.Value);
            labelHValue.Text    = FormatNanoTesla(result.HorizontalIntensity?.Value);
            labelFValue.Text    = FormatNanoTesla(result.TotalField?.Value);

            // Components
            labelXValue.Text = FormatNanoTesla(result.NorthComp?.Value);
            labelYValue.Text = FormatNanoTesla(result.EastComp?.Value);
            labelZValue.Text = FormatNanoTesla(result.VerticalComp?.Value);

            // Change per year (taken from changePerYearLast, since secular variation is per-model not per-row)
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
            labelModelName.Text = string.IsNullOrEmpty(modelName) ? "—" : modelName;
            labelModelCategory.Text = modelType.ToString();
            labelModelValidity.Text = "—"; // Phase 2: pull from descriptor.MinDate/MaxDate
            labelModelSigmaSource.Text = (result.Uncertainty != null && result.Uncertainty.SigmaD.HasValue)
                ? "per-point (model)"
                : "ISCWSA (default)";
        }

        // ─── Formatters ─────────────────────────────────────────────

        private static string FormatDegrees(double? v)
        {
            return v.HasValue ? v.Value.ToString("F4", CultureInfo.CurrentCulture) + "°" : "—";
        }

        private static string FormatNanoTesla(double? v)
        {
            return v.HasValue ? v.Value.ToString("N0", CultureInfo.CurrentCulture) + " nT" : "—";
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
    }
}
