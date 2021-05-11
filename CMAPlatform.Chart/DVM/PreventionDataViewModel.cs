using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CMAPlatform.Chart.DVM
{
    [Serializable]
    public class PreventionDataViewModel : ChartDataViewModel
    {
        /// <summary>
        ///     获取列方法
        /// </summary>
        /// <returns></returns>
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            if (ValueColumnModel != null)
            {
                columns.Add(ValueColumnModel);
            }
            if (ProvinceName != null)
            {
                columns.Add(ProvinceName);
            }
            return columns;
        }

        #region 字段

        private MeasureColumnModel m_ValueColumnModel;

        /// <summary>
        ///     省份占比值
        /// </summary>
        [Synchronous]
        [PropertyDescription("省份占比列",
            Category = DescriptionEnum.数据设置,
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true)]
        public MeasureColumnModel ValueColumnModel
        {
            get { return m_ValueColumnModel; }
            set
            {
                m_ValueColumnModel = value;
                RaisePropertyChanged(() => ValueColumnModel);
            }
        }

        private DimensionColumnModel m_ProVinceName;

        [PropertyDescription("省份名称列",
            Category = DescriptionEnum.数据设置,
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true)]
        public DimensionColumnModel ProvinceName
        {
            get { return m_ProVinceName; }
            set
            {
                m_ProVinceName = value;
                RaisePropertyChanged(() => ProvinceName);
            }
        }

        // DimensionColumnModel

        #endregion
    }
}