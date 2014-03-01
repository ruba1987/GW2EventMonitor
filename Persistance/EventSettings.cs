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
        public List<KeyValuePair<Guid, String>> WatchedEvents { get; set; }
        //public List<Guid> WatchedEventIds { get; set; }
        internal EventSettings()
        {

        }

        public void RefreshData(object settingsInfo)
        {
            if (settingsInfo is IEnumerable<KeyValuePair<Guid, EventNameEntry>>)
            {
                IEnumerable<KeyValuePair<Guid, EventNameEntry>> si = settingsInfo as IEnumerable<KeyValuePair<Guid, EventNameEntry>>;
                WatchedEvents = si.Select(x => new KeyValuePair<Guid, string>(x.Key, x.Value.Name)).ToList<KeyValuePair<Guid, String>>();
            }
        }
    }
}
