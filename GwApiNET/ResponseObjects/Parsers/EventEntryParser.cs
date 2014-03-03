using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GwApiNET.ResponseObjects.Parsers
{

    public class EventEntryParser : IApiResponseParserAsync<EntryCollection<EventEntry>>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public EventEntryParser()
        {
        }

        /// <summary>
        /// Parse a json string containing an array of <seealso cref="EventEntry"/> 
        /// </summary>
        /// <param name="apiResponse">json string containing an array</param>
        /// <returns>Collection of EventEntry objects</returns>
        public EntryCollection<EventEntry> Parse(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            EntryCollection<EventEntry> events = new EntryCollection<EventEntry>();
            JObject jo = JsonConvert.DeserializeObject(json) as JObject;
            if (jo != null)
            {
                foreach (var property in jo["events"])
                {
                    try
                    {
                        events.Add(Parse(property));
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        GwApi.Logger.Error(e);
                    }
                }
            }
            return events;
        }

        public EventEntry Parse(JToken token)
        {
            return new EventEntry()
            {
                WorldId = (int)token["world_id"],
                MapId = (int)token["map_id"],
                EventId = new Guid(token["event_id"].ToString()),
                State = (EventState)Enum.Parse(typeof(EventState), token["state"].ToString()),
            };
        }

        public async Task<EntryCollection<EventEntry>> ParseAsync(object apiResponse)
        {
            string json = ParserResponseHelper.GetResponseString(apiResponse);
            var events = await ParserHelper<EntryDictionary<string, EntryCollection<EventEntry>>>.ParseAsync(json).ConfigureAwait(false);
            return events["events"];
        }
    }
}
