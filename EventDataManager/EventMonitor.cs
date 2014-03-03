using GwApiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventDataManager
{
    public class EventMonitor
    {
        private static Dictionary<Guid, EventMonitor> _monitor = new Dictionary<Guid, EventMonitor>();
        private static List<Timer> _activeTimers = new List<Timer>();

        private EventDataFetcher _edf = new EventDataFetcher();
        private Action<String, EventState> _callback;
        private EventState _currState;
        private string _eventName;
        //TODO uncomment before deploy. This is for testing
        //private int _pollInterval = 180000;
        private int _pollInterval = 30000;
        private Guid _eventID;

        private EventMonitor(Action<String, EventState> callback, String eventName, Guid eventID)
        {
            _callback = callback;
            _eventName = eventName;
            _eventID = eventID;
        }

        private EventMonitor(Action<String, EventState> callback, String eventName, Guid eventID, int pollInterval)
            : this(callback, eventName, eventID)
        {
            _pollInterval = pollInterval;
        }

        public static void AddWatch(Action<String, EventState> callback, String eventName, Guid eventID)
        {
            EventMonitor e = new EventMonitor(callback, eventName, eventID);
            _monitor.Add(eventID, e);
            ActivateTimer(e);
        }

        public static void AddWatch(Action<String, EventState> callback, String eventName, Guid eventID, int pollInterval)
        {
            EventMonitor e = new EventMonitor(callback, eventName, eventID, pollInterval);
            _monitor.Add(eventID, e);
            ActivateTimer(e);
        }

        private static async void ActivateTimer(EventMonitor e)
        {
            e._currState = await e._edf.GetEventSate(e._eventName);
            _activeTimers.Add(new Timer(async (_) =>
            {
                EventState es = await e._edf.GetEventSate(e._eventName);
                if (es != e._currState)
                {
                    e._currState = es;
                    e._callback.Invoke(e._eventName, e._currState);
                }
            }, null, 0, e._pollInterval));
        }
    }
}
