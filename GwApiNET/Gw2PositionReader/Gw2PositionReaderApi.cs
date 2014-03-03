using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwApiNET.Gw2PositionReader
{
    /// <summary>
    /// Position information service API.
    /// Takes advantage of mumble link information to provide real time player information such as the current world, map, and position of the character.
    /// <remarks>Much of this API requires a character to be logged into GW2 on the same computer running this library.  Mumble link information is obtained via memory from the GW2 client.</remarks>
    /// </summary>
    public static class Gw2PositionReaderApi
    {
        /// <summary>
        /// Retrieves the players data.
        /// <remarks>This requires that the GW2 client to be running and logged into a world on the same computer running this library.</remarks>
        /// </summary>
        /// <returns></returns>
        public static Player GetPlayerDataInstance()
        {
            Player p = new Player();
            return p;
        }
    }
}
