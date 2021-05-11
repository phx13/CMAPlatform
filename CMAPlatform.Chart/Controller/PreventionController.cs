using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CMAPlatform.Chart.DVM;
using CMAPlatform.Models;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.DataAdapter.DataAdapters;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.Controller
{
    public class PreventionController : ChartControllerBase
    {
        private PreventionDataViewModel m_DataViewModel;

        public ObservableCollection<PreventionModel> m_PreventionData = new ObservableCollection<PreventionModel>();


        public Dictionary<string, string[]> NameDictionary = new Dictionary<string, string[]>();

        public PreventionController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            DataViewModel = dvm as PreventionDataViewModel;


            if (DataViewModel != null && DataViewModel.DataSourceModel != null)
            {
                var dicSource = DataViewModel.DataSourceModel.DeepCopy();
                dicSource.TableName = "dicprovince";

                var dictable = AdapterManager.GetAllData(dicSource);

                GetProvinceDic(dictable);
            }
        }

        /// <summary>
        ///     DVM
        /// </summary>
        public PreventionDataViewModel DataViewModel
        {
            get { return m_DataViewModel; }
            set
            {
                m_DataViewModel = value;
                OnPropertyChanged("DataViewModel");
            }
        }

        /// <summary>
        ///     数据集合
        /// </summary>
        public ObservableCollection<PreventionModel> PreventionData
        {
            get { return m_PreventionData; }
            set
            {
                m_PreventionData = value;
                OnPropertyChanged("PreventionData");
            }
        }

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        public override void ReceiveData(AdapterDataTable adt)
        {
            if (adt == null)
            {
                return;
            }
            PreventionData = new ObservableCollection<PreventionModel>();
            var rows = adt.Rows;
            foreach (var row in rows)
            {
                var preventionModel = new PreventionModel();
                preventionModel.ProvinceName = row[DataViewModel.ProvinceName.AsName].ToString();

                if (NameDictionary.ContainsKey(preventionModel.ProvinceName))
                {
                    preventionModel.ShortName = NameDictionary[preventionModel.ProvinceName][1];
                    preventionModel.id = int.Parse(NameDictionary[preventionModel.ProvinceName][0]);
                }
                else
                {
                    preventionModel.ShortName = preventionModel.ProvinceName;
                }
                preventionModel.Value = Convert.ToDouble(row[DataViewModel.ValueColumnModel.AsName]);
                preventionModel.Time = Convert.ToDateTime(row[DataViewModel.DataTimeColumn.AsName]);
                PreventionData.Add(preventionModel);
            }
            var s = from t in PreventionData orderby t.id ascending select t;
            PreventionData = new ObservableCollection<PreventionModel>(s.ToList());
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        /// <summary>
        ///     获取省份简称字典
        /// </summary>
        /// <param name="table"></param>
        private void GetProvinceDic(AdapterDataTable table)
        {
            NameDictionary.Clear();

            if (table != null)
            {
                foreach (var dataRow in table.Rows)
                {
                    var name = dataRow["Name"].ToString();
                    var shortName = dataRow["ShortName"].ToString();
                    var Id = dataRow["Id"].ToString();

                    var strarr = new string[2];
                    strarr[0] = Id;
                    strarr[1] = shortName;
                    NameDictionary.Add(name, strarr);
                }
            }
        }
    }
}