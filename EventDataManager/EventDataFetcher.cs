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
        private BasicSettings _bs;
        private EventSettings _es;
        private SettingsManager _sm = new SettingsManager();
        public EventDataFetcher()
        {
            _bs = _sm.GetSettings(SettingType.Baisc) as BasicSettings;
            _es = _sm.GetSettings(SettingType.Event) as EventSettings;
        }

        public async Task<EventState> GetEventSate(String eventName, bool bypassCache = true)
        {
            EntryCollection<EventEntry> x = await GwApi.GetEventsAsync(_bs.WorldID, -1, _es.WatchedEvents.First(a => a.Value == eventName).Key, bypassCache);
            return x.First(a => a.EventId == _es.WatchedEvents.First(n => n.Value == eventName).Key).State;
        }

        public async Task<EventState> GetEventSate(Guid eventId, bool bypassCache = true)
        {
            EntryCollection<EventEntry> x = await GwApi.GetEventsAsync(_bs.WorldID, -1, eventId, bypassCache);
            return x.First(a => a.EventId == eventId).State;
        }

        public EntryDictionary<Guid, EventNameEntry> GetEventNames(int mapID)
        {
            EntryCollection<EventEntry> x = GwApi.GetEvents(_bs.WorldID, mapID, null);
            HashSet<Guid> z = new HashSet<Guid>();
            foreach (EventEntry p in x)
            {
                z.Add(p.EventId);
            }
            var y = GwApi.GetEventNames();
            EntryDictionary<Guid, EventNameEntry> ret = new EntryDictionary<Guid, EventNameEntry>();
            y.Where(j => z.Contains(j.Value.Id)).ToList().ForEach(k => ret.Add(k.Key, k.Value));
            return ret;
        }

        public async Task<EntryDictionary<Guid, EventNameEntry>> GetEventNamesAsync(int mapID)
        {
            EntryCollection<EventEntry> x = await GwApi.GetEventsAsync(_bs.WorldID, mapID, null);
            var y = await GwApi.GetEventNamesAsync();
            HashSet<Guid> z = new HashSet<Guid>();
            foreach (EventEntry p in x)
            {
                z.Add(p.EventId);
            }
            
            EntryDictionary<Guid, EventNameEntry> ret = new EntryDictionary<Guid, EventNameEntry>();
            y.Where(j => z.Contains(j.Value.Id)).ToList().ForEach(k => ret.Add(k.Key, k.Value));
            return ret;
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
