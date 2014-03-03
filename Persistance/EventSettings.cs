using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;

namespace Persistance
{
    public class EventSettings : ISettings
    {
        public Dictionary<Guid, String> WatchedEvents { get; set; }
        //public Dictionary<String, Guid> WatchedEventsByName { get; set; }
        internal EventSettings()
        {

        }

        public void RefreshData(object settingsInfo)
        {
            if (settingsInfo is IEnumerable<KeyValuePair<Guid, EventNameEntry>>)
            {
                IEnumerable<KeyValuePair<Guid, EventNameEntry>> si = settingsInfo as IEnumerable<KeyValuePair<Guid, EventNameEntry>>;
          //      WatchedEventsByName = si.ToDictionary(key => key.Value.Name, value => value.Key);
                WatchedEvents = si.ToDictionary(key => key.Key, value => value.Value.Name);
            }
        }
    }
}
