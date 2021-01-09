using System;
using Digihail.AVE.Launcher.Infrastructure.Communiction;

namespace CMAPlatform.Models.MessageModel
{
    /// <summary>
    ///     图层控制/列表控件交互消息
    /// </summary>
    public class CMAMainPageSelectedTabMessage : CompositeCommunictionMessage<CMAMainPageSelectedTabData>
    {
    }

    [Serializable]
    public class CMAMainPageSelectedTabData
    {
        public Guid InstanceGuid { get; set; }

        public string TabName { get; set; }
    }
}