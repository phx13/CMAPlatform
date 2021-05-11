using Digihail.CCPSOE.Models.Group;

namespace CMAPlatform.Models
{
    /// <summary>
    ///     图层图片关系实体
    /// </summary>
    public class SubLayerPicModdel : BaseCategory
    {
        private string m_PicName;
        private string m_SubLayerName;

        public string SubLayerName
        {
            get { return m_SubLayerName; }
            set
            {
                m_SubLayerName = value;
                RaisePropertyChanged(() => SubLayerName);
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
    }
}