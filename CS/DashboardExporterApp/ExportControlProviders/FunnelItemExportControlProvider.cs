using System.Collections.Generic;
using System.Linq;
using DevExpress.DashboardCommon;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;

namespace DashboardExporterApp {
    public class FunnelItemExportControlProvider : ICustomExportControlProvider {
        readonly CustomDashboardItem dashboardItem;

        public FunnelItemExportControlProvider(CustomDashboardItem dashboardItem) {
            this.dashboardItem = dashboardItem;
        }
        XRControl ICustomExportControlProvider.GetPrintableControl(CustomItemData customItemData, 
            CustomItemExportInfo exportInfo) {
            XRChart chart = new XRChart();
            chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            chart.CustomDrawSeriesPoint += CustomDrawSeriesPoint;
            DashboardFlatDataSource flatData = customItemData.GetFlatData(new DashboardFlatDataSourceOptions() {
                AddColoringColumns = true });
            ConfigureSeries(chart, customItemData, flatData, exportInfo.DrillDownValues.Count);
            SetSelectionMode(chart);
            SetSelection(chart, exportInfo.Selection, flatData);
            return chart;
        }
        void ConfigureSeries(XRChart chart, CustomItemData customItemData,
            DashboardFlatDataSource flatData, int drillDownLevel) {
            chart.Series.Clear();
            Series series = new Series("A Funnel Series", ViewType.Funnel);
            IList<CustomItemBindingValue> values = customItemData.GetBindings("Value");
            IList<CustomItemBindingValue> arguments = customItemData.GetBindings("Arguments");
            if(values.Count > 0 && arguments.Count > 0) {
                series.DataSource = flatData;
                series.ValueDataMembers.AddRange(values[0].UniqueId);
                if(dashboardItem.InteractivityOptions.IsDrillDownEnabled)
                    series.ArgumentDataMember = arguments[drillDownLevel].UniqueId;
                else
                    series.ArgumentDataMember = arguments.Last().UniqueId;
                series.ColorDataMember = flatData.GetColoringColumn(values[0].UniqueId).Name;
            }
            ((FunnelSeriesLabel)series.Label).Position = FunnelSeriesLabelPosition.Center;
            chart.Series.Add(series);
        }
        void SetSelectionMode(XRChart chart) {
            chart.Chart.SeriesSelectionMode = SeriesSelectionMode.Point;
            switch(dashboardItem.InteractivityOptions.MasterFilterMode) {
                case DashboardItemMasterFilterMode.None:
                    chart.Chart.SelectionMode = ElementSelectionMode.None;
                    break;
                case DashboardItemMasterFilterMode.Single:
                    chart.Chart.SelectionMode = ElementSelectionMode.Single;
                    break;
                case DashboardItemMasterFilterMode.Multiple:
                    chart.Chart.SelectionMode = ElementSelectionMode.Extended;
                    break;
                default:
                    chart.Chart.SelectionMode = ElementSelectionMode.None;
                    break;
            }
        }
        void SetSelection(XRChart chart, CustomItemSelection selection, 
            DashboardFlatDataSource flatData) {
            foreach(DashboardFlatDataSourceRow row in selection.GetDashboardFlatDataSourceRows(flatData))
                chart.Chart.SelectedItems.Add(row);
        }
        void CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e) {
            e.LabelText = e.SeriesPoint.Argument + " - " + e.LabelText;
            e.LegendText = e.SeriesPoint.Argument;
        }
    }
}
