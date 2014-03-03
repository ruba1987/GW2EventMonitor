using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;
using Slf;

namespace GwApiNET
{
    /// <summary>
    /// Represents a coordinate in GW2.  Can be used as pixel coordinates or world/player coordinates.
    /// </summary>
    [Tested(TestedAttribute.TestStatus.Untested)]
    public struct Gw2Point
    {
        public Gw2Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// X Position
        /// </summary>
        public double X;
        /// <summary>
        /// Y Position
        /// </summary>
        public double Y;

        #region Equals Override
        /// <summary>
        /// Determines of given Gw2Point is equal to this point in values.
        /// </summary>
        /// <param name="other">point to compare to.</param>
        /// <returns>true if the X positions and Y positions are equal.  Due to 
        /// the X and Y values being double, an algorithm is used to determine equality.
        /// Currently the values would be considered equal at extremly high accuracy.</returns>
        public bool Equals(Gw2Point other)
        {
            return other == this;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyFieldInGetHashCode
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
                // ReSharper restore NonReadonlyFieldInGetHashCode
            }
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("[{0},{1}]", X, Y);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Gw2Point && (Gw2Point) obj == this;
        }

        #endregion Equals Override

        #region Operator Override
        /// <summary>
        /// Determines if two Gw2Points are equal.
        /// </summary>
        /// <param name="left">left hand object</param>
        /// <param name="right">right hand object</param>
        /// <returns>true if the X positions and Y positions are equal.  Due to 
        /// the X and Y values being double, an algorithm is used to determine equality.
        /// Currently the values would be considered equal at extremly high accuracy.</returns>
        public static bool operator ==(Gw2Point left, Gw2Point right)
        {
            return (left.X.HasMinimalDifference(right.X, 10) && left.Y.HasMinimalDifference(right.Y, 10));
        }

        /// <summary>
        /// Determines if two Gw2Points are not equal.
        /// </summary>
        /// <param name="left">left hand object</param>
        /// <param name="right">right hand object</param>
        /// <returns>true if the X positions and Y positions are not equal.  Due to 
        /// the X and Y values being double, an algorithm is used to determine equality.
        /// Currently the values would be considered equal at extremly high accuracy.</returns>
        public static bool operator !=(Gw2Point left, Gw2Point right)
        {
            return !(left == right);
        }
        /// <summary>
        /// Provides multiplication operator to the point using a scaler.
        /// </summary>
        /// <param name="point">value to multiply</param>
        /// <param name="scaler">scaler to multiply the X and Y value by.</param>
        /// <returns></returns>
        public static Gw2Point operator *(Gw2Point point, double scaler)
        {
            return new Gw2Point {X = point.X*scaler, Y = point.Y*scaler};
        }
        /// <summary>
        /// Provides multiplication operator to the point using a scaler.
        /// </summary>
        /// <param name="point">value to multiply</param>
        /// <param name="scaler">scaler to multiply the X and Y value by.</param>
        /// <returns></returns>
        public static Gw2Point operator *(double scaler, Gw2Point point)
        {
            return point*scaler;
        }
        /// <summary>
        /// Provides division operator to the point using a scaler.
        /// </summary>
        /// <param name="point">value to divide</param>
        /// <param name="scaler">scaler to divide the X and Y value by.</param>
        /// <returns></returns>
        public static Gw2Point operator /(Gw2Point point, double scaler)
        {
            return new Gw2Point {X = point.X/scaler, Y = point.Y/scaler};
        }

        /// <summary>
        /// Provides subtraction operator to the point using a scaler.
        /// </summary>
        /// <param name="left">left hand value</param>
        /// <param name="right">right hand value</param>
        /// <returns></returns>
        public static Gw2Point operator -(Gw2Point left, Gw2Point right)
        {
            return new Gw2Point {X = left.X - right.X, Y = left.Y - right.Y};
        }

        /// <summary>
        /// Provides addition operator to the point using a scaler.
        /// </summary>
        /// <param name="left">left hand value</param>
        /// <param name="right">right hand value</param>
        /// <returns></returns>
        public static Gw2Point operator +(Gw2Point left, Gw2Point right)
        {
            return new Gw2Point {X = left.X + right.X, Y = left.Y + right.Y};
        }

        #endregion Operator Override


    }

    [Tested("GwMapsHelperTest", "Full testing not completed", TestedAttribute.TestStatus.Untested)]
    public static class GwMapsHelper
    {
        private static EntryDictionary<int, MapEntry> _maps;
        private static EntryDictionary<int, MapEntry> Maps { get { return _maps ?? (_maps = GwApi.GetMap()); } }
        private static EntryDictionary<int, ContinentEntry> _continents;
        private static EntryDictionary<int, ContinentEntry> Continents { get{return _continents ?? (_continents = GwApi.GetContinents());} }
        private static ILogger _logger;
        private static string LoggerName
        {
            get { return Constants.LoggerNames[0]; }
        }


        public static ILogger Logger
        {
            get { return _logger; }
        }

        static GwMapsHelper()
        {
            _logger = LoggerService.GetLogger(LoggerName);
        }
        private static MapEntryBase GetMap(int mapId)
        {
            if (Maps.ContainsKey(mapId))
                return Maps[mapId];
            else return Maps[50]; // Return Lions Gate if map isn't available;
        }

        private static MapEntryBase GetMap(string mapName, bool ignoreCase = true)
        {
            return ignoreCase
                       ? Maps.Single(m => m.Value.MapName.ToLower() == mapName.ToLower()).Value
                       : Maps.Single(m => m.Value.MapName == mapName).Value;
        }

        [Tested("PixelToWorldPosTest")]
        public static Gw2Point PixelToWorldPos(int mapId, Gw2Point pixel, int zoom)
        {
            return PixelToWorldPos(GetMap(mapId), pixel, zoom);
        }

        [Tested("PixelToWorldPosTest")]
        public static Gw2Point PixelToWorldPos(string mapName, Gw2Point pixel, int zoom)
        {
            return PixelToWorldPos(GetMap(mapName), pixel, zoom);
        }

        [Tested("PixelToWorldPosTest")]
        public static Gw2Point PixelToWorldPos(MapEntryBase map, Gw2Point pixel, int zoom)
        {
            try
            {
                var continent = GetContinent(map);
                int zoomFromMax = continent.MaxZoom - zoom;
                int projection = 1 << zoomFromMax;
                pixel *= projection;
                var Xwn = (pixel.X - map.ContinentRectangle[0][0]) * Constants.PixelToWorldPosRatio + map.MapRectangle[0][0];
                var Ywn = map.MapRectangle[1][1] - (pixel.Y - map.ContinentRectangle[0][1]) * Constants.PixelToWorldPosRatio;
                return new Gw2Point { X = Xwn, Y = Ywn };
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return new Gw2Point( 0, 0 );
        }

        [Tested("WorldPosToPixelTest")]
        public static Gw2Point WorldPosToPixel(int mapId, Gw2Point worldPosition, int zoom)
        {
            return WorldPosToPixel(GetMap(mapId), worldPosition, zoom);
        }

        [Tested("WorldPosToPixelTest")]
        public static Gw2Point WorldPosToPixel(string mapName, Gw2Point worldPosition, int zoom)
        {
            return WorldPosToPixel(GetMap(mapName), worldPosition, zoom);
        }

        [Tested("WorldPosToPixelTest")]
        public static Gw2Point WorldPosToPixel(MapEntryBase map, Gw2Point worldPosition, int zoom)
        {
            try
            {
                var continent = GetContinent(map);
                int zoomFromMax = continent.MaxZoom - zoom;
                int projection = 1 << zoomFromMax;
                var Xpn = map.ContinentRectangle[0][0] + (worldPosition.X - map.MapRectangle[0][0]) * Constants.WorldPosToPixelRatio;
                var Ypn = map.ContinentRectangle[0][1] + (map.MapRectangle[1][1] - worldPosition.Y) * Constants.WorldPosToPixelRatio;
                return new Gw2Point { X = Xpn, Y = Ypn } / projection;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return new Gw2Point(0, 0);
        }

        public static Gw2Point PixelToTile(Gw2Point pixel)
        {
            // ReSharper disable PossibleLossOfFraction
            var point = new Gw2Point { X = (int)pixel.X / Constants.TileSize, Y = (int)pixel.Y / Constants.TileSize };
            return point;
            // ReSharper restore PossibleLossOfFraction
        }

        [Tested("WorldPosToTilePixelTest")]
        public static Gw2Point WorldPosToTilePixel(int mapId, Gw2Point worldPosition, int zoom)
        {
            return WorldPosToTilePixel(GetMap(mapId), worldPosition, zoom);
        }

        [Tested("WorldPosToTilePixelTest")]
        public static Gw2Point WorldPosToTilePixel(string mapName, Gw2Point worldPosition, int zoom)
        {
            return WorldPosToTilePixel(GetMap(mapName), worldPosition, zoom);
        }

        [Tested("WorldPosToTilePixelTest")]
        public static Gw2Point WorldPosToTilePixel(MapEntryBase map, Gw2Point worldPosition, int zoom)
        {
            Gw2Point pixelAtZoom = WorldPosToPixel(map, worldPosition, zoom);
            Gw2Point tile = WorldPosToTile(map, worldPosition, zoom);
            Gw2Point tilePixel = pixelAtZoom - (tile*Constants.TileSize);
            return tilePixel;
        }

        [Tested("WorldPosToTileTest")]
        public static Gw2Point WorldPosToTile(int mapId, Gw2Point worldPosition, int zoom)
        {
            return WorldPosToTile(GetMap(mapId), worldPosition, zoom);
        }

        [Tested("WorldPosToTileTest")]
        public static Gw2Point WorldPosToTile(string mapName, Gw2Point worldPosition, int zoom)
        {
            return WorldPosToTile(GetMap(mapName), worldPosition, zoom);
        }

        [Tested("WorldPosToTileTest")]
        public static Gw2Point WorldPosToTile(MapEntryBase map, Gw2Point worldPosition, int zoom)
        {
            var point = PixelToTile(WorldPosToPixel(map, worldPosition, zoom));
            return point;
        }

        public static string GetTileUrlFromPixel(int continentId, int floor, int zoomLevel, Gw2Point pixelCoord)
        {
            return GetTileUrl(continentId, floor, zoomLevel, PixelToTile(pixelCoord));
        }

        public static string GetTileUrlFromPixel(int continentId, int floor, int zoomLevel, Gw2Point pixelCoord, string errorTileUrl)
        {
            return GetTileUrl(continentId, floor, zoomLevel, PixelToTile(pixelCoord), errorTileUrl);
        }

        [Tested("GetTileUrlFromWorldPosTest1")]
        public static string GetTileUrlFromWorldPos(string mapName, int floor, int zoomLevel, Gw2Point worldPosition)
        {
            MapEntryBase map = GetMap(mapName);
            return GetTileUrlFromWorldPos(map, floor, zoomLevel, worldPosition);
        }

        [Tested("GetTileUrlFromWorldPosTest1")]
        public static string GetTileUrlFromWorldPos(string mapName, int floor, int zoomLevel, Gw2Point worldPosition, string errorTileUrl)
        {
            MapEntryBase map = GetMap(mapName);
            return GetTileUrlFromWorldPos(map, floor, zoomLevel, worldPosition, errorTileUrl);
        }

        [Tested("GetTileUrlFromWorldPosTest2")]
        public static string GetTileUrlFromWorldPos(int mapId, int floor, int zoomLevel, Gw2Point worldPosition)
        {
            MapEntryBase map = GetMap(mapId);
            return GetTileUrlFromWorldPos(map, floor, zoomLevel, worldPosition);
        }

        [Tested("GetTileUrlFromWorldPosTest2")]
        public static string GetTileUrlFromWorldPos(int mapId, int floor, int zoomLevel, Gw2Point worldPosition, string errorTileUrl)
        {
            MapEntryBase map = GetMap(mapId);
            return GetTileUrlFromWorldPos(map, floor, zoomLevel, worldPosition, errorTileUrl);
        }

        [Tested("GetTileUrlFromWorldPosTest1")]
        public static string GetTileUrlFromWorldPos(MapEntryBase map, int floor, int zoomLevel, Gw2Point worldPosition)
        {
            MapEntry mapEntry = map is MapEntry ? (map as MapEntry) : GwApi.GetMap()[map.Id];
            int continentId = mapEntry.ContinentId;
            return GetTileUrl(continentId, floor, zoomLevel, WorldPosToTile(map, worldPosition, zoomLevel));
        }

        [Tested("GetTileUrlFromWorldPosTest2")]
        public static string GetTileUrlFromWorldPos(MapEntryBase map, int floor, int zoomLevel, Gw2Point worldPosition, string errorTileUrl)
        {
            MapEntry mapEntry = map is MapEntry ? (map as MapEntry) : GwApi.GetMap()[map.Id];
            int continentId = mapEntry.ContinentId;
            return GetTileUrl(continentId, floor, zoomLevel, WorldPosToTile(map, worldPosition, zoomLevel), errorTileUrl);
        }

        [Tested("GetTileUrlFromWorldPosTest1")]
        public static string GetTileUrl(int continentId, int floor, int zoomLevel, Gw2Point tileCoord)
        {
            return GetTileUrl(continentId, floor, zoomLevel, tileCoord, Constants.ContinentErrorTileUrl[continentId]);
        }

        [Tested("GetTileUrlFromWorldPosTest2")]
        public static string GetTileUrl(int continentId, int floor, int zoomLevel, Gw2Point tileCoord, string errorTileUrl)
        {
            var continents = Continents;
            var continent = continents[continentId];
            if (continent.Floors.Contains(floor) == false)
                throw new ArgumentException(string.Format("Floor value doesn't exist for the given Continent {0}",
                                                          continent.Name, "floor"));
            zoomLevel = zoomLevel.EnsureWithin(continent.MinZoom, continent.MaxZoom);
            var numTilesAtZoom = 1 << zoomLevel;
            return tileCoord.X.IsBetweenInclusive(0, numTilesAtZoom) &&
                   tileCoord.Y.IsBetweenInclusive(0, numTilesAtZoom)
                       ? string.Format(Constants.MapTileUrlFormat, continentId, floor, zoomLevel,
                                       ((int)tileCoord.X), ((int)tileCoord.Y))
                       : errorTileUrl;
        }

        /// <summary>
        /// Returns the Continent for the given map
        /// </summary>
        /// <param name="map">map entry</param>
        /// <returns>Continent of the map supplied</returns>
        public static ContinentEntry GetContinent(MapEntryBase map)
        {
            if (map is MapEntry) return  Continents[(map as MapEntry).ContinentId];
            return Continents[Maps[map.Id].ContinentId];
        }

        /// <summary>
        /// Returns the Continent for the given mapId
        /// </summary>
        /// <param name="mapId">map entry</param>
        /// <returns>Continent of the mapId supplied</returns>
        public static ContinentEntry GetContinent(int mapId)
        {
            return GetContinent(GetMap(mapId));
        }
    }
}
