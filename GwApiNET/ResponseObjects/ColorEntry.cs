using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GwApiNET.ResponseObjects
{
    [Serializable]
    //[JsonConverter(typeof(JsonColorEntryConverter))]
    public class ColorEntry : ResponseObject
    {
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonConverter(typeof(JsonColorConverter))]
        [JsonProperty("base_rgb")]
        public Color BaseRGB { get; set; }

        [JsonProperty("cloth")]
        public ColorItemEntry Cloth { get; set; }

        [JsonProperty("leather")]
        public ColorItemEntry Leather { get; set; }

        [JsonProperty("metal")]
        public ColorItemEntry Metal { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ColorEntry()
        {

        }

        [Serializable]
        //[JsonConverter(typeof(JsonColorItemEntryConverter))]
        public class ColorItemEntry : ResponseObject
        {
            public ColorItemType Type { get; set; }

            [JsonProperty("brightness")]
            public int Brightness { get; set; }

            [JsonProperty("contrast")]
            public double Contrast { get; set; }

            [JsonProperty("hue")]
            public int Hue { get; set; }

            [JsonProperty("saturation")]
            public double Saturation { get; set; }

            [JsonProperty("lightness")]
            public double Lightness { get; set; }

            [JsonProperty("rgb")]
            [JsonConverter(typeof(JsonColorConverter))]
            public Color RGB { get; set; }

        }
    }

    public class JsonColorEntryConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            dynamic obj = JObject.Load(reader);

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonColorConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);
            Color? color = null;
            switch (array.Count)
            {
                case 3:
                    color = Color.FromArgb((int)array[0], (int)array[1], (int)array[2]);
                    break;
                case 4:
                    color = Color.FromArgb((int)array[0], (int)array[1], (int)array[2], (int)array[3]);
                    break;
            }
            return color;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Color).IsAssignableFrom(objectType);
        }
 
    }
}
