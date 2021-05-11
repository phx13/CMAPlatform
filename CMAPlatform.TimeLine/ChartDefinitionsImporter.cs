using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CMAPlatform.TimeLine
{
    [Export(typeof (IChartDefinitionsImporter))]
    public class ChartDefinitionsImporter : IChartDefinitionsImporter
    {
        public List<ChartDefinition> GetChartDefinitions()
        {
            var chartDefinitions = new List<ChartDefinition>();

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{CFCBF526-8604-4AAC-8CE7-44D93D0EBD66}"),
                Category = "事件页图表",
                ChartType = "泳道图",
                DisplayName = "泳道图",
                DataViewModelType = typeof (TimelineChartDataViewModel),
                ChartViewType = typeof (MainChartView),
                ChartControllerType = typeof (TimeLineChartController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });

            chartDefinitions.Add(new ChartDefinition
            {
                Id = new Guid("{FCE295A3-1705-4B7F-8354-5B4D22550EA4}"),
                Category = "事件页图表",
                ChartType = "泳道图_3D",
                DisplayName = "泳道图_3D",
                DataViewModelType = typeof (GIS3DTimelineChartDataViewModel),
                ChartViewType = typeof (OnGis3DChartView),
                ChartControllerType = typeof (TimeLineChartController),
                SmallIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png",
                BigIcon = "pack://application:,,,/DAD3.Charts;component/Images/ChartIcons/DynamicDigitDashboard.png"
            });


            return chartDefinitions;
        }
    }
}