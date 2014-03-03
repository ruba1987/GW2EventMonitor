using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{
    internal static class ParserResponseHelper
    {
        public static string DefaultInfoLogFormat = "Parsing {0}: {1}";
        public static string DefaultInfoLogFormatAsync = "Parsing {0}: {1} - TaskID {2}";
        public static string GetResponseString(object apiResponse)
        {
            IRestResponse response = apiResponse as IRestResponse;
            if (response == null) return "{}";
            return response.Content;
        }

        public static byte[] GetResponseRaw(object apiResponse)
        {
            IRestResponse response = apiResponse as IRestResponse;
            if (response == null) return new byte[0];
            return response.RawBytes;
        }
    }
    internal static class ParserHelper<T> where T : class
    {
        public static T Parse(string jsonString) 
        {
            JsonSerializer serializer = new JsonSerializer();
            using (TextReader reader = new StringReader(jsonString))
            {
                using (JsonReader jreader = new JsonTextReader(reader))
                {
                    try
                    {
                        serializer.Converters.Add(new StringEnumConverter());
                        var obj = serializer.Deserialize<T>(jreader);
                        return obj;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        GwApi.Logger.Error(e);
                    }
                    return null;
                }
            }
        }

        public static async Task<T> ParseAsync(string jsonString)
        {
            try
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());
                return await JsonConvert.DeserializeObjectAsync<T>(jsonString, settings).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                GwApi.Logger.Error(e);
            }
            return null;
        }

    }

    public class BoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((bool)value) ? 1 : 0);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString() == "1";
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}
