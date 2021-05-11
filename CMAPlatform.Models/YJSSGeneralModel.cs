using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

namespace CMAPlatform.Models
{
    /// <summary>
    ///     全国预警设施及在线率
    /// </summary>
    public class YJSSGeneralModel : NotificationObject
    {
        private List<General> m_result;

        public List<General> Result
        {
            get { return m_result; }
            set
            {
                m_result = value;
                RaisePropertyChanged(() => Result);
            }
        }
    }

    public class General
    {
        public int count { get; set; }

        public double zxl { get; set; }

        public int type { get; set; }
    }
}