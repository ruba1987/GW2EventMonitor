using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{

    public class MapFloorEntryParser : IApiResponseParserAsync<MapFloorEntry>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MapFloorEntryParser()
        {
        }

        public MapFloorEntry Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var floorEntry = ParserHelper<MapFloorEntry>.Parse(json);
            foreach (var region in floorEntry.Regions)
            {
                region.Value.Id = region.Key;
                foreach (var map in region.Value.Maps)
                {
                    map.Value.Id = map.Key;
                    map.Value.RegionName = region.Value.Name;
                    map.Value.Id = region.Value.Id;
                    foreach (var poi in region.Value.Maps)
                    {
                        poi.Value.Id = poi.Key;
                    }
                }
            }
            return floorEntry;
        }

        /// <summary>
        /// Copies the readable and writable public property values from the source object to the target
        /// </summary>
        /// <remarks>The source and target objects must be of the same type.</remarks>
        /// <param name="target">The target object</param>
        /// <param name="source">The source object</param>
        /// <param name="ignoreProperties">An array of property names to ignore</param>
        public static void CopyPropertiesFrom(object target, object source, string[] ignoreProperties)
        {
            // Get and check the object types
            Type type = source.GetType();
            Type targetType = target.GetType();

            // Build a clean list of property names to ignore
            var ignoreList = new List<string>();
            foreach (string item in ignoreProperties)
            {
                if (!string.IsNullOrEmpty(item) && !ignoreList.Contains(item))
                {
                    ignoreList.Add(item);
                }
            }

            PropertyInfo[] targetProperties = targetType.GetProperties();
            // Copy the properties
            foreach (PropertyInfo property in type.GetProperties())
            {
                PropertyInfo targetProperty =
                    targetProperties.SingleOrDefault(p => p.Name == property.Name && p.PropertyType == property.PropertyType);
                if (property.CanWrite
                    && property.CanRead
                    && !ignoreList.Contains(property.Name)
                    && targetProperty != null)
                {
                    object val = property.GetValue(source, null);
                    targetProperty.SetValue(target, val, null);
                }
            }
        }

        public async Task<MapFloorEntry> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var floorEntry = await ParserHelper<MapFloorEntry>.ParseAsync(json).ConfigureAwait(false);
            foreach (var region in floorEntry.Regions)
            {
                region.Value.Id = region.Key;
                foreach (var map in region.Value.Maps)
                {
                    map.Value.Id = map.Key;
                    map.Value.RegionName = region.Value.Name;
                    map.Value.Id = region.Value.Id;
                    foreach (var poi in region.Value.Maps)
                    {
                        poi.Value.Id = poi.Key;
                    }
                }
            }
            return floorEntry;
        }
    }
}
