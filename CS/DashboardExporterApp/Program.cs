using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DevExpress.DashboardCommon;

namespace DashboardExporterApp {
    class Program {
        static void Main(string[] args) {
            if(args.Length < 1 || !Directory.Exists(args[0])) {
                Console.WriteLine("Path to the output folder are required");
                return;
            }
            string outputFolder = args[0];
            string outputFilePath = Path.Combine(outputFolder, "Dashboard.pdf");
            DashboardExporter exporter = new DashboardExporter();
            exporter.CustomItemExportControlCreating += Exporter_CustomItemExportControlCreating;
            DashboardState dashboardState = new DashboardState();
            DashboardItemState listBoxState = new DashboardItemState("listBoxDashboardItem1") {
                MasterFilterValues = new List<object[]> { new[] { "England" }, new[] { "France" } }
            };
            DashboardItemState funnelState = new DashboardItemState("funnelDashboardItem1") {
                MasterFilterValues = new List<object[]> { new[] { "Final" }, new[] { "Winner" } }
            };
            dashboardState.Items.Add(listBoxState);
            dashboardState.Items.Add(funnelState);
            try {
                exporter.ExportToPdf(@"Dashboards\Dashboard.xml", outputFilePath, state: dashboardState, 
                    dashboardSize: new Size(800, 500));
                Console.WriteLine("Success!");
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        }
        static void Exporter_CustomItemExportControlCreating(object sender, CustomItemExportControlCreatingEventArgs e) {
            if(e.CustomItemType == "FunnelItem")
                e.ExportControlProvider = new FunnelItemExportControlProvider(e.DashboardItem);
        }
    }
}
