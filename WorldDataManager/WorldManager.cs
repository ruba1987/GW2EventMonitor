using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET;
using GwApiNET.ResponseObjects;

namespace WorldDataManager
{
    public class WorldManager
    {
        private string _worldName = string.Empty;
        public WorldManager(String worldName)
        {
            SetWorld(worldName);
        }

        public WorldManager()
        {

        }

        public EntryDictionary<int, WorldNameEntry> GetWorldNames()
        {
            return GwApi.GetWorldNames();
        }

        public async Task<EntryDictionary<int, WorldNameEntry>> GetWorldNamesAsync()
        {
             EntryDictionary<int, WorldNameEntry> x = await GwApi.GetWorldNamesAsync();
             return x;
        }

        public void SetWorld(String worldName)
        {
            if (worldName == null)
                throw new ArgumentNullException("World name can't be null");
            string tmp = GetWorldNames().Values.Select(x => x.Name).FirstOrDefault(x => x == worldName);
            if (String.IsNullOrEmpty(tmp))
                throw new Exception(String.Format("World {0} not found", worldName));
            _worldName = tmp;

        }
    }
}
