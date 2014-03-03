using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Entry in recipe_details.json
    /// </summary>
    [Serializable]
    public class RecipeDetailsEntry : ResponseObject
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("recipe_id")]
        public int RecipeId { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty("type")]
        public RecipeType RecipeType { get; set; }
        /// <summary>
        /// Output result Item ID
        /// </summary>
        [JsonProperty("output_item_id")]
        public int OutputItemId { get; set; }
        /// <summary>
        /// Number of output items
        /// </summary>
        [JsonProperty("output_item_count")]
        public int OutputCount { get; set; }
        /// <summary>
        /// Minimum rating needed to craft the item.
        /// </summary>
        [JsonProperty("min_rating")]
        public int MinRating { get; set; }
        /// <summary>
        /// Time to craft the item in milliseconds
        /// </summary>
        [JsonProperty("time_to_craft_ms")]
        public int TimeToCraftMsec { get; set; }
        /// <summary>
        /// Crafting Discipline
        /// </summary>
        [JsonProperty("disciplines")]
        public IList<DisciplineType> Diciplines { get; set; }
        /// <summary>
        /// Recipe flags
        /// </summary>
        [JsonProperty("flags")]
        public string[] Flags { get; set; }
        /// <summary>
        /// Input ingredients needed to make the recipe.
        /// </summary>
        [JsonProperty("ingredients")]
        public EntryCollection<RecipeIngredient> Ingredients { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RecipeDetailsEntry()
        {
        }

        private const string _defaultPropertyFormat = "{0}: {1}\n";

        public override string ToString()
        {
            var sb = new StringBuilder();
            ItemDetailsEntry outputItem = GwApi.GetItemDetails(OutputItemId);
            sb.AppendFormat("{0}: {1} x{2}\n", "Name", outputItem.Name, OutputCount);
            sb.AppendFormat(_defaultPropertyFormat, "Type", RecipeType);
            sb.AppendFormat(_defaultPropertyFormat, "Skill Needed", MinRating);
            sb.AppendFormat(_defaultPropertyFormat, "Craft Time(s)", TimeToCraftMsec / 1000.0);
            sb.AppendLine("Diciplines:");
            foreach (var dicipline in Diciplines)
                sb.AppendFormat("  {0}", dicipline);
            sb.AppendLine("Flags: " + string.Join(", ", Flags));
            sb.AppendLine("Ingredients:");
            foreach (var ingredient in Ingredients)
                sb.AppendLine("  " + ingredient);
            return sb.ToString();
        }
    }
}
