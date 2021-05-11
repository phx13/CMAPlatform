using System;
using System.Collections.Generic;
using CMAPlatform.Models.Enum;
using Digihail.AVE.Launcher.Infrastructure.Communiction;

namespace CMAPlatform.Models.MessageModel
{
    /// <summary>
    ///     gce图层控制消息
    /// </summary>
    public class CMAGceLayerControlMessage : CompositeCommunictionMessage<GceLayer>
    {
    }

    [Serializable]
    public class GceLayer
    {
        public string LayerName { get; set; }
        public bool LayerVisiable { get; set; }
    }
}