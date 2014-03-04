using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET;
using GwApiNET.ResponseObjects;
namespace MapDataManager
{
    public class MapDataFetcher
    {

        public MapDataFetcher()
        {

        }

        public String GetMapName(int mapID)
        {
            return GwApi.GetMapNames()[mapID].Name;
        }

        public async Task<String> GetMapNameAsync(int mapID)
        {
            EntryDictionary<int, MapNameEntry> retVal = await GwApi.GetMapNamesAsync();
            return retVal[mapID].Name;
        }

        public EntryDictionary<int, MapNameEntry> GetMapNames()
        {
            return GwApi.GetMapNames();
        }

        public async Task<EntryDictionary<int, MapNameEntry>> GetMapNamesAsync()
        {
            EntryDictionary<int, MapNameEntry> retVal = await GwApi.GetMapNamesAsync();
            return retVal;
        }

        
    }
}
