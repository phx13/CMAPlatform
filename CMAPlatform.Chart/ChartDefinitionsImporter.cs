using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CMAPlatform.Chart.Controller;
using CMAPlatform.Chart.DVM;
using CMAPlatform.Chart.View;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.GIS3D.Controls;
using Digihail.DAD3.Models.Charts;

namespace CMAPlatform.Chart
{
    [Export(typeof (IChartDefinitionsImporter))]
    public class ChartDefinitionsImporter : IChartDefinitionsImporter
    {
        public List<ChartDefinition> GetChartDefinitions()
        {
            var chartDefinitions = new List<ChartDefinition>();

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{0B463F8A-4574-49EC-A4C7-2D9FD2B80DF2}"),
                Category = "自定义控件图表",
                ChartType = "自定义图表",
                DisplayName = "省份一本账",
                DataViewModelType = typeof (PreventionDataViewModel),
                ChartViewType = typeof (Prevention),
                ChartControllerType = typeof (PreventionController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });
            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{4E6D0DB4-0F9F-4A5D-813A-956EB378FF0F}"),
                Category = "自定义控件图表",
                ChartType = "自定义事件图表",
                DisplayName = "突发事件列表",
                DataViewModelType = typeof (EventListDataViewModel),
                ChartViewType = typeof (EventListView),
                ChartControllerType = typeof (EventListController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });
            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{B3DB9A79-D74B-4A8D-BC18-E0D1AE423C9C}"),
                Category = "自定义控件图表",
                ChartType = "自定义时间图表",
                DisplayName = "计时入库",
                DataViewModelType = typeof (TimerAutomaticDataViewModel),
                ChartViewType = typeof (TimerAutomaticView),
                ChartControllerType = typeof (TimerAutomaticController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });


            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{4E6D0DB4-0F9F-4A5D-813A-956EB378FF0F}"),
                Category = "自定义控件图表",
                ChartType = "自定义图例图表",
                DisplayName = "三维图层控制",
                DataViewModelType = typeof (LayerControlDataViewModel),
                ChartViewType = typeof (LayerControl),
                ChartControllerType = typeof (LayerControlController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{1BB0508A-2AF1-4AB7-8968-684C4BD2410E}"),
                Category = "自定义控件图表",
                ChartType = "自定义图例图表1",
                DisplayName = "事件页图层控制",
                DataViewModelType = typeof (NewLayerControlDataViewModel),
                ChartViewType = typeof (NewLayerControl),
                ChartControllerType = typeof (NewLayerControlController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{8F6D5C8C-D91A-4967-8BD1-5BFF9AF687D8}"),
                Category = "自定义控件图表",
                ChartType = "gis3d.riskSituation",
                DisplayName = "风险态势（落区）",
                DataViewModelType = typeof (GIS3DRiskSituationDataViewModel),
                ChartViewType = typeof (GIS3DComplexView),
                ChartControllerType = typeof (GIS3DRiskSituationController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });
            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{64FD82B1-17E7-4B27-A6FD-65021383AE0D}"),
                Category = "自定义控件图表",
                ChartType = "gis3d.riskSituation1",
                DisplayName = "事件页风险态势（落区）",
                DataViewModelType = typeof (GIS3DEventPageRiskSituationDataViewModel),
                ChartViewType = typeof (GIS3DComplexView),
                ChartControllerType = typeof (GIS3DEventPageRiskSituationController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });


            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{3249CA70-EE74-401B-8C93-DEEAE570901E}"),
                Category = "自定义控件图表",
                ChartType = "gis3d.riskSituationLabel",
                DisplayName = "风险态势标签（落区标签）",
                DataViewModelType = typeof (GIS3DRiskSituationLabelDataViewModel),
                ChartViewType = typeof (RiskSituationLabelView),
                ChartControllerType = typeof (GIS3DRiskSituationLabelController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{72A9FC55-61B5-438C-88DF-05A9631C72AC}"),
                Category = "自定义控件图表",
                ChartType = "gis3d.riskSituationLabel1",
                DisplayName = "事件页风险态势标签（落区标签）",
                DataViewModelType = typeof (GIS3DEventPageRiskSituationLabelDataViewModel),
                ChartViewType = typeof (EventPageRiskSituationLabelView),
                ChartControllerType = typeof (GIS3DEventPageRiskSituationLabelController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{64EC46B9-CDE5-498F-9187-48C23FA74101}"),
                Category = "自定义控件图表",
                ChartType = "Custom.Bubble",
                DisplayName = "自定义气泡图",
                DataViewModelType = typeof (BubbleViewModel),
                ChartViewType = typeof (BubbleChart),
                ChartControllerType = typeof (BubbleController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{81CF3834-3A78-412F-A070-5C92A6D57547}"),
                Category = "自定义控件图表",
                ChartType = "Custom.EventTitle",
                DisplayName = "事件详情标题",
                DataViewModelType = typeof (EventInfoTitleDataViewModel),
                ChartViewType = typeof (EventInfoTitle),
                ChartControllerType = typeof (EventInfoTitleController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{0679A565-797E-4F73-B89C-ECEED09F695B}"),
                Category = "自定义控件图表",
                ChartType = "Custom.EventDetails",
                DisplayName = "事件页详情",
                DataViewModelType = typeof (EventInfoDetailsDataViewModel),
                ChartViewType = typeof (EventInfoDetails),
                ChartControllerType = typeof (EventInfoDetailsController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{51DFA909-E193-4692-898E-7D69C76E4007}"),
                Category = "自定义控件图表",
                ChartType = "Custom.EventInfoShowList",
                DisplayName = "事件页轮播列表",
                DataViewModelType = typeof (EventInfoShowListDataViewModel),
                ChartViewType = typeof (EventInfoShowList),
                ChartControllerType = typeof (EventInfoShowListController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{D69D64D3-8234-4762-A829-178B9807C116}"),
                Category = "自定义控件图表",
                ChartType = "Custom.RankingControl",
                DisplayName = "排名控件",
                DataViewModelType = typeof (RankingViewModel),
                ChartViewType = typeof (RankingView),
                ChartControllerType = typeof (RankingController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{689E4BF7-00C2-41C5-B1A9-4A2C20D4A29E}"),
                Category = "自定义控件图表",
                ChartType = "Custom.TyphoonControl",
                DisplayName = "台风详情按钮",
                DataViewModelType = typeof (EventPageTyphoonDetailViewModel),
                ChartViewType = typeof (EventPageTyphoonDetailButton),
                ChartControllerType = typeof (EventPageTyphoonDetailController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{092C5885-546E-4398-8B7D-38B4E6375316}"),
                Category = "自定义控件图表",
                ChartType = "Custom.TyphoonEventTitle",
                DisplayName = "台风事件详情标题",
                DataViewModelType = typeof (TyphoonTitleDataViewModel),
                ChartViewType = typeof (TyphoonTitle),
                ChartControllerType = typeof (TyphoonTitleController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = Guid.NewGuid(),
                Category = "自定义控件图表",
                ChartType = "gis3d.Typhoon",
                DisplayName = "台风风圈",
                DataViewModelType = typeof (GIS3DPlaybackDataViewModel),
                ChartViewType = typeof (GIS3DComplexView),
                ChartControllerType = typeof (GIS3DPlaybackController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{FCB1DFC0-5A69-4946-8696-2830363E73CC}"),
                Category = "自定义控件图表",
                ChartType = "Custom.TyphoonEventDetail",
                DisplayName = "台风事件详细信息",
                DataViewModelType = typeof (TyphoonDetailViewModel),
                ChartViewType = typeof (TyphoonDetailControl),
                ChartControllerType = typeof (TyphoonDetailController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            return chartDefinitions;
        }
    }
}