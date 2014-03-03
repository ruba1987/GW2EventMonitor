using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{

    public class ColorEntryParser : IApiResponseParserAsync<EntryDictionary<int,ColorEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ColorEntryParser()
        {
        }

        public EntryDictionary<int,ColorEntry> Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var temp = ParserHelper<Dictionary<string, EntryDictionary<int, ColorEntry>>>.Parse(json);
            EntryDictionary<int, ColorEntry> colors = temp["colors"];
            foreach (var pair in colors)
            {
                pair.Value.Id = pair.Key;
                pair.Value.Cloth.Type = ColorItemType.Cloth;
                pair.Value.Leather.Type = ColorItemType.Leather;
                pair.Value.Metal.Type = ColorItemType.Metal;
            }
            if (colors == null)
                GwApi.Logger.Error("Failed parsing colors");
            else GwApi.Logger.Info("Parsed {0} Colors", colors.Values.Count);
            return colors;
        }

        private ColorEntry.ColorItemEntry Parse(ColorItemType type, JObject jobject)
        {
            ColorEntry.ColorItemEntry item = new ColorEntry.ColorItemEntry();
            item.Type = type;
            item.Hue = (int) jobject["hue"];
            item.Saturation = (double)jobject["saturation"];
            item.Lightness = (double)jobject["lightness"];
            item.Contrast = (double)jobject["contrast"];
            item.Brightness = (int)jobject["brightness"];
            item.RGB = Color.FromArgb((int)jobject["rgb"][0], (int)jobject["rgb"][1],
                                           (int)jobject["rgb"][2]);
            return item;
        }

        public Task<EntryDictionary<int, ColorEntry>> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            return Task.Run(async () =>
                {
                    var temp = await ParserHelper<Dictionary<string, EntryDictionary<int, ColorEntry>>>.ParseAsync(json).ConfigureAwait(false);
                    EntryDictionary<int, ColorEntry> colors = temp["colors"];
                    foreach (var pair in colors)
                    {
                        pair.Value.Id = pair.Key;
                        pair.Value.Cloth.Type = ColorItemType.Cloth;
                        pair.Value.Leather.Type = ColorItemType.Leather;
                        pair.Value.Metal.Type = ColorItemType.Metal;
                    }
                    if (colors == null)
                        GwApi.Logger.Error("Failed parsing colors - {0}", Thread.CurrentContext.ContextID);
                    else GwApi.Logger.Info("Parsed {0} Colors - {1}", colors.Values.Count, Thread.CurrentContext.ContextID);
                    return colors;
                });
        }
    }
}
