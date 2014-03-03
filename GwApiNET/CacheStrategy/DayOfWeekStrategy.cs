using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.ResponseObjects;

namespace GwApiNET.CacheStrategy
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(DayOfWeekStrategy))]
    public class DayOfWeekStrategy : ICacheStrategy
    {
        [DataMember]
        public DayOfWeek DayOfWeek { get; set; }

        protected DateTime GetLastDayOfWeekTime(DateTime now)
        {
            DateTime then = now.Subtract(TimeSpan.FromDays(now.DayOfWeek - DayOfWeek));
            then = then.Subtract(now.TimeOfDay);
            return then;
        }

        public DayOfWeekStrategy() : this(DayOfWeek.Tuesday)
        {}
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DayOfWeekStrategy(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
        }

        public bool Expired(ResponseObject responseObject)
        {
            return Expired(responseObject, responseObject.Age, DateTime.Now);
        }

        public bool Expired(ResponseObject responseObject, TimeSpan age, DateTime now)
        {
            return age >= TimeSpan.FromDays(7) ||
                (now.DayOfWeek > DayOfWeek &&
                    GetLastDayOfWeekTime(now) > responseObject.LastUpdated) ||
                    (DayOfWeek == now.DayOfWeek && age > TimeSpan.FromDays(1));
        }
    }
}
