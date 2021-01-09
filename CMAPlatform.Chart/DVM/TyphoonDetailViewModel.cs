using System;
using System.Collections.Generic;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CMAPlatform.Chart.DVM
{
    /// <summary>
    ///     台风详细控件DVM
    /// </summary>
    [Serializable]
    public class TyphoonDetailViewModel : ChartDataViewModel
    {
        public override List<DataColumnModel> GetColumns()
        {
            var dataColumn = new List<DataColumnModel>();

            return dataColumn;
        }
    }
}