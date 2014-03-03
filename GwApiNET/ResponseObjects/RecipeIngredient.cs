using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GwApiNET.ResponseObjects
{
    /// <summary>
    /// Recipe Ingredient for recipe_details.json
    /// </summary>
    [Serializable]
    public class RecipeIngredient : ResponseObject
    {
        /// <summary>
        /// Item ID of input item.
        /// </summary>
        [JsonProperty("item_id")]
        public int ItemId { get; set; }
        /// <summary>
        /// Number of items required.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RecipeIngredient()
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var item = GwApi.GetItemDetails(ItemId);
            sb.AppendFormat("{0} x{1}", item.Name, Count);
            return sb.ToString();
        }
    }
}
