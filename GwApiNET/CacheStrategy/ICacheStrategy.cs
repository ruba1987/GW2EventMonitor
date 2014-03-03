using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GwApiNET.ResponseObjects;

namespace GwApiNET.CacheStrategy
{
    [XmlInclude(typeof(BuildVersionCacheStrategy))]
    [XmlInclude(typeof(AgeCacheStrategy))]
    [XmlInclude(typeof(DayOfWeekStrategy))]
    public interface ICacheStrategy
    {
        bool Expired(ResponseObject responseObject);
    }
}
