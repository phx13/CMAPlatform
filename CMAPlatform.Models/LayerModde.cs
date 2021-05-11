using System.Collections.ObjectModel;
using Digihail.CCPSOE.Models.Group;

namespace CMAPlatform.Models
{
    /// <summary>
    ///     图层图片关系实体
    /// </summary>
    public class LayerModel : BaseCategory
    {
        private string m_LayerName;

        private string m_PicName;

        private bool m_SelectType;

        private ObservableCollection<LayerModel> m_SubLayerPicCollection = new ObservableCollection<LayerModel>();

        public string LayerName
        {
            get { return m_LayerName; }
            set
            {
                m_LayerName = value;
                RaisePropertyChanged(() => LayerName);
            }
        }

        public string PicName
        {
            get { return m_PicName; }
            set
            {
                m_PicName = value;
                RaisePropertyChanged(() => PicName);
            }
        }

        public bool SelectType
        {
            get { return m_SelectType; }
            set
            {
                m_SelectType = value;
                RaisePropertyChanged(() => SelectType);
            }
        }

        public ObservableCollection<LayerModel> SubLayerPicCollection
        {
            get { return m_SubLayerPicCollection; }
            set
            {
                m_SubLayerPicCollection = value;
                RaisePropertyChanged(() => SubLayerPicCollection);
            }
        }
    }
}