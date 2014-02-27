using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwSharp;

namespace WorldDataManager
{
    public class WorldManager
    {
        GwWatcher watcher = new GwWatcher();

        public GwWorld CurrentWorld { get; set; }

        public WorldManager()
        {
        }

        public String[] GetWorldNames()
        {
            return watcher.Worlds.Select(x => x.Name).ToArray();
        }

        public void SetWorld(String worldName)
        {
            if (worldName == null)
                throw new ArgumentNullException("World name can't be null");
            GwWorld tmp = watcher.Worlds.First(x => x.Name == worldName);
            if (tmp == null)
                throw new Exception(String.Format("World {0} not found", worldName));
            CurrentWorld = tmp;

        }
    }
}
