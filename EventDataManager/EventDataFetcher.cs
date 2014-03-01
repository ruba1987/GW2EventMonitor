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
            return GwApi.GetEventNames();
        }

        public async Task<EntryDictionary<Guid, EventNameEntry>> GetEventNamesAsync()
        {
            var x = await GwApi.GetEventNamesAsync();
            return x;
        }
    }
}
