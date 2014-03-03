using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.Gw2Stats.ResponseObjects;
using GwApiNET.ResponseObjects;

namespace GwApiNET.Gw2Stats
{

    [Serializable]
    public class StatusCodeDescriptions : EntryDictionary<string, string>
    {
        public void AddRange(KeyValuePair<string, string>[] pairs)
        {
            foreach (var pair in pairs)
            {
                Add(pair);
            }
        }

        public void AddRange(IDictionary<string, string> dictionary)
        {
            KeyValuePair<string, string>[] pairs = dictionary.ToArray();
            AddRange(pairs);
        }
        public string this[ApiStatus status]
        {

            get { return this[status.ToString().Replace("_", " ")]; }
            set { this[status.ToString().Replace("_", " ")] = value; }
        }
    }
}
