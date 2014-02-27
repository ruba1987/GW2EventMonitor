using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public class EventSettings : ISettings
    {
        public List<String> WatchedEvents { get; set; }

        internal EventSettings()
        {

        }
    }
}
