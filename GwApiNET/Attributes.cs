using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwApiNET
{
    internal class LanguageAttribute : Attribute
    {
        public string Value { get; set; }
        public LanguageAttribute(string value)
        {
            Value = value;
        }
    }
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
    public class ResponseCollectionAttribute : Attribute
    {
        public CollectionType Type { get; set; }
        public ResponseCollectionAttribute(CollectionType type = CollectionType.IEnumerable)
        {
            Type = type;
        }

        public enum CollectionType
        {
            /// <summary>
            /// Designates an IDictionary Collection
            /// </summary>
            IDictionary,
            /// <summary>
            /// Designates an IList Collection
            /// </summary>
            IList,
            /// <summary>
            /// Designates an IEnumerable.
            /// This should be used if no other enum values matches
            /// </summary>
            IEnumerable,
        }
    }
}
