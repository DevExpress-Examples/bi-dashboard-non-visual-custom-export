Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.DashboardCommon
Imports DevExpress.XtraCharts
Imports DevExpress.XtraReports.UI

Namespace DashboardExporterApp
	Public Class FunnelItemExportControlProvider
		Implements ICustomExportControlProvider

		Private ReadOnly dashboardItem As CustomDashboardItem

		Public Sub New(ByVal dashboardItem As CustomDashboardItem)
			Me.dashboardItem = dashboardItem
		End Sub
		Private Function ICustomExportControlProvider_GetPrintableControl(ByVal customItemData As CustomItemData, ByVal exportInfo As CustomItemExportInfo) As XRControl Implements ICustomExportControlProvider.GetPrintableControl
			Dim chart As New XRChart()
			chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True
			AddHandler chart.CustomDrawSeriesPoint, AddressOf CustomDrawSeriesPoint
			Dim flatData As DashboardFlatDataSource = customItemData.GetFlatData(New DashboardFlatDataSourceOptions() With {.AddColoringColumns = True})
			ConfigureSeries(chart, customItemData, flatData, exportInfo.DrillDownValues.Count)
			SetSelectionMode(chart)
			SetSelection(chart, exportInfo.Selection, flatData)
			Return chart
		End Function
		Private Sub ConfigureSeries(ByVal chart As XRChart, ByVal customItemData As CustomItemData, ByVal flatData As DashboardFlatDataSource, ByVal drillDownLevel As Integer)
			chart.Series.Clear()
			Dim series As New Series("A Funnel Series", ViewType.Funnel)
			Dim values As IList(Of CustomItemBindingValue) = customItemData.GetBindings("Value")
			Dim arguments As IList(Of CustomItemBindingValue) = customItemData.GetBindings("Arguments")
			If values.Count > 0 AndAlso arguments.Count > 0 Then
				series.DataSource = flatData
				series.ValueDataMembers.AddRange(values(0).UniqueId)
				If dashboardItem.InteractivityOptions.IsDrillDownEnabled Then
					series.ArgumentDataMember = arguments(drillDownLevel).UniqueId
				Else
					series.ArgumentDataMember = arguments.Last().UniqueId
				End If
				series.ColorDataMember = flatData.GetColoringColumn(values(0).UniqueId).Name
			End If
			CType(series.Label, FunnelSeriesLabel).Position = FunnelSeriesLabelPosition.Center
			chart.Series.Add(series)
		End Sub
		Private Sub SetSelectionMode(ByVal chart As XRChart)
			chart.Chart.SeriesSelectionMode = SeriesSelectionMode.Point
			Select Case dashboardItem.InteractivityOptions.MasterFilterMode
				Case DashboardItemMasterFilterMode.None
					chart.Chart.SelectionMode = ElementSelectionMode.None
				Case DashboardItemMasterFilterMode.Single
					chart.Chart.SelectionMode = ElementSelectionMode.Single
				Case DashboardItemMasterFilterMode.Multiple
					chart.Chart.SelectionMode = ElementSelectionMode.Extended
				Case Else
					chart.Chart.SelectionMode = ElementSelectionMode.None
			End Select
		End Sub
		Private Sub SetSelection(ByVal chart As XRChart, ByVal selection As CustomItemSelection, ByVal flatData As DashboardFlatDataSource)
			For Each row As DashboardFlatDataSourceRow In selection.GetDashboardFlatDataSourceRows(flatData)
				chart.Chart.SelectedItems.Add(row)
			Next row
		End Sub
		Private Sub CustomDrawSeriesPoint(ByVal sender As Object, ByVal e As CustomDrawSeriesPointEventArgs)
			e.LabelText = e.SeriesPoint.Argument & " - " & e.LabelText
			e.LegendText = e.SeriesPoint.Argument
		End Sub
	End Class
End Namespace
