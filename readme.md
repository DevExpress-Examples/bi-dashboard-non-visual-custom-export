<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/420023909/21.2.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1038672)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# BI Dashboard - Non-Visual Custom Export

This example shows how to use the [DashboardExporter](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardExporter) component to export a dashboard with a [custom Funnel item](https://docs.devexpress.com/Dashboard/403031/winforms-dashboard/winforms-designer/create-a-custom-item).

<!-- default file list -->
## Files to look at

* [FunnelItemExportControlProvider.cs](./CS/DashboardExporterApp/ExportControlProviders/FunnelItemExportControlProvider.cs) (VB: [FunnelItemExportControlProvider.vb](./VB/DashboardExporterApp/ExportControlProviders/FunnelItemExportControlProvider.vb))
* [Program.cs](./CS/DashboardExporterApp/Program.cs) (VB: [Program.vb](./VB/DashboardExporterApp/Program.vb))

<!-- default file list end -->

## Overview

The following API used in this example:

### FunnelItemExportControlProvider

 [CS](./CS/DashboardExporterApp/ExportControlProviders/FunnelItemExportControlProvider.cs)/[VB](./VB/DashboardExporterApp/ExportControlProviders/FunnelItemExportControlProvider.vb)

The `FunnelItemExportControlProvider` class implements the [ICustomExportControlProvider](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.ICustomExportControlProvider) interface. The `FunnelItemExportControlProvider` is used to configure the printable control and export options before you export a dashboard. 

The [ICustomExportControlProvider.GetPrintableControl](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.ICustomExportControlProvider.GetPrintableControl(DevExpress.DashboardCommon.CustomItemData-DevExpress.DashboardCommon.CustomItemExportInfo)) method specifies the printable `XRChart` control that is used to export a custom Funnel. The method gets [CustomItemExportInfo](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.CustomItemExportInfo) and [CustomItemData](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.CustomItemData) objects as parameters. 

The `CustomItemExportInfo` object contains the custom Funnel's export settings as [ExportMode](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.CustomItemExportInfo.ExportMode) and master filter state.

The `ChartControl.SelectionMode` property is updated according to the actual master filter mode.
The [SetSelection](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWin.CustomControlProviderBase.SetSelection(DevExpress.DashboardCommon.CustomItemSelection)) method updates a custom control according to the current master filter selection. The method is called each time a master filter selection changes.

The `ConigureSeries` method is used to bind a custom Funnel chart's series to data and configure them. The [CustomItemData.GetBindings](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.CustomItemData.GetBindings(System.String)) method gets a CustomItemBindingValue collection. Each object in this collection contains information about data items stored in custom item metadata. The object’s UniqueId property value can be used as a data member when you bind a custom control to data. 

### Program

[CS](./CS/DashboardExporterApp/Program.cs)/[VB](./VB/DashboardExporterApp/Program.vb)

The [CustomItemExportControlCreating](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardExporter.CustomItemExportControlCreating) event fires for the Funnel item and the [ExportControlProvider](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.CustomItemExportControlCreatingEventArgs.ExportControlProvider) method returns the configured XRChart control to be printed in the resulting ducument.

The [DashboardExporter.ExportToPdf](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardExporter.ExportDashboardItemToPdf(Dashboard--String--String--Nullable-Size---DashboardState--DashboardPdfExportOptions)) method exports a dashboard in a Pdf file.

## Documentation

- [Create an Interactive Data-Aware Item for the WinForms Dashboard](https://docs.devexpress.com/Dashboard/403032/winforms-dashboard/winforms-designer/ui-elements-and-customization/create-a-custom-item/create-an-interactive-data-aware-item)
- [ICustomExportControlProvider](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.ICustomExportControlProvider)
- [DashboardExporter](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardExporter)
- [CustomItemExportControlCreating](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardExporter.CustomItemExportControlCreating)

## More examples 

[BI Dashboard - Non-Visual Export Component](https://github.com/DevExpress-Examples/bi-dashboard-non-visual-exporter)

