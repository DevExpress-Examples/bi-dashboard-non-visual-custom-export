Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports DevExpress.DashboardCommon

Namespace DashboardExporterApp
	Friend Class Program
		Shared Sub Main(ByVal args() As String)
			If args.Length < 1 OrElse Not Directory.Exists(args(0)) Then
				Console.WriteLine("Path to the output folder are required")
				Return
			End If
			Dim outputFolder As String = args(0)
			Dim outputFilePath As String = Path.Combine(outputFolder, "Dashboard.pdf")
			Dim exporter As New DashboardExporter()
			AddHandler exporter.CustomItemExportControlCreating, AddressOf Exporter_CustomItemExportControlCreating
			Dim dashboardState As New DashboardState()
			Dim listBoxState As New DashboardItemState("listBoxDashboardItem1") With {
				.MasterFilterValues = New List(Of Object()) From {
					New String() { "England" },
					New String() { "France" }
				}
			}
			Dim funnelState As New DashboardItemState("funnelDashboardItem1") With {
				.MasterFilterValues = New List(Of Object()) From {
					New String() { "Final" },
					New String() { "Winner" }
				}
			}
			dashboardState.Items.Add(listBoxState)
			dashboardState.Items.Add(funnelState)
			Try
				exporter.ExportToPdf("Dashboards\Dashboard.xml", outputFilePath, state:= dashboardState, dashboardSize:= New Size(800, 500))
                		Console.WriteLine("Success!")
			Catch e As Exception
				Console.WriteLine(e.Message)
			End Try
		End Sub
		Private Shared Sub Exporter_CustomItemExportControlCreating(ByVal sender As Object, ByVal e As CustomItemExportControlCreatingEventArgs)
			If e.CustomItemType = "FunnelItem" Then
				e.ExportControlProvider = New FunnelItemExportControlProvider(e.DashboardItem)
			End If
		End Sub
	End Class
End Namespace
