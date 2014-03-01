using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET;
using GwApiNET.ResponseObjects;
using Persistance;

namespace EventDataManager
{
    public class EventDataFetcher
    {
        private BasicSettings _be;
        private SettingsManager _se = new SettingsManager();
        public EventDataFetcher()
        {
            _be = _se.GetSettings(SettingType.Baisc) as BasicSettings;
        }

        public EntryDictionary<Guid, EventNameEntry> GetEventNames()
        {
            return GwApi.GetEventNames(_be.WorldID);
        }

        public async void GetEventNamesAsync()
        {
            //var t = GwApi.GetEventsAsync()
        }
    }
}
