using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

namespace CMAPlatform.Models
{
    /// <summary>
    ///     省份一本账详细数据
    /// </summary>
    public class percent : NotificationObject
    {
        private List<message> m_message;
        public string province { get; set; }
    }

    public class message
    {
        public string disasterName { get; set; }
        public bool exist { get; set; }
    }
}