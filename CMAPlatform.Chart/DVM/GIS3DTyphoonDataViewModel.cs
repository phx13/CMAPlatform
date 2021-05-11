using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels.GIS3D;

namespace CMAPlatform.Chart.DVM
{
    [Serializable]
    public class GIS3DPlaybackDataViewModel : GIS3DPointDataViewModel
    {
        private MeasureFieldCollection m_ColorFields = new MeasureFieldCollection();

        /// <summary>
        ///     构造函数
        /// </summary>
        // Token: 0x06000F21 RID: 3873 RVA: 0x00035ECB File Offset: 0x000340CB
        public GIS3DPlaybackDataViewModel()
        {
            m_CanSort = true;
            IsGetFirstDataImmediate = true;
        }

        /// <summary>
        ///     数据设置 - 颜色列
        /// </summary>
        [Synchronous]
        [PropertyDescription("颜色列", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.MeasureCollection, IsNecessary = false, RefreshChartData = true)]
        public MeasureFieldCollection ColorFields
        {
            get { return m_ColorFields; }
            set
            {
                m_ColorFields = value;
                RaisePropertyChanged("ColorFields");
            }
        }

        /// <summary>
        ///     获取所有用于查询分组的列
        /// </summary>
        /// <returns></returns>
        public override List<DataColumnModel> GetColumns()
        {
            var dataColumns = base.GetColumns();
            dataColumns.AddRange(ColorFields);
            return dataColumns;
        }
    }
}