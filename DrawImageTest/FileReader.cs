using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Digihail.AVE.Controls.Utils;

namespace DrawImageTest
{
    public class FileReader
    {
        public List<TyphoonDataModel> TyphoonData()
        {
            List<TyphoonDataModel> typhoonDataModels = new List<TyphoonDataModel>();

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页/台风.json");
            var root = JsonHelper.DeserializeFromFile<TyphoonPathModel>(filePath);
            if (root != null)
            {
                typhoonDataModels = root.data.typhoon_live.ToList();
            }

            return typhoonDataModels;
        }
    }
}