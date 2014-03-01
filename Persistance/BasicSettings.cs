using GwApiNET.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public class BasicSettings : ISettings
    {
        public String CurrentWorld { get; set; }
        public int WorldID { get; set; }
        internal BasicSettings()
        {

        }

        public void RefreshData(object settingsInfo)
        {
            if (settingsInfo is WorldNameEntry)
            {
                WorldNameEntry wne = settingsInfo as WorldNameEntry;
                WorldID = wne.Id;
            }
        }
    }

}
