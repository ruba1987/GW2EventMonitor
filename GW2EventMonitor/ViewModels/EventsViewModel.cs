using EventDataManager;
using Persistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GwApiNET.ResponseObjects;

namespace GW2EventMonitor.ViewModels
{
    public class EventsViewModel : NotificationObject
    {
        #region Fields
        private readonly SettingsManager _sm = new SettingsManager();
        private readonly EventSettings _es;
        private readonly BasicSettings _bs;
        private readonly EventDataFetcher _em = new EventDataFetcher();
        private EntryDictionary<Guid, EventNameEntry> _eventData;
        #endregion

        #region Props
        private List<String> _events;

        public List<String> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                RaisePropertyChanged(() => Events);
            }
        }


        private ObservableCollection<String> _watchedEvents;

        public ObservableCollection<String> WatchedEvents
        {
            get { return _watchedEvents; }
            set
            {
                _watchedEvents = value;
                RaisePropertyChanged(() => WatchedEvents);
            }
        }

        private String _selectedEventName;

        public String SelectedEventName
        {
            get { return _selectedEventName; }
            set
            {
                _selectedEventName = value;
                RaisePropertyChanged(() => SelectedEventName);
            }
        }

        private String _loadingMsg;

        public String LoadingMsg
        {
            get { return _loadingMsg; }
            set { 
                _loadingMsg = value;
                RaisePropertyChanged("LoadingMsg");
            }
        }


        #endregion

        #region Commands

        private RelayCommand<Window> _saveCommand;
        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand<Window>(SaveExecute)); }
        }

        private RelayCommand _addCommand;
        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(AddExecute)); }
        }
        #endregion

        public EventsViewModel()
        {
            _es = _sm.GetSettings(SettingType.Event) as EventSettings;
            _bs = _sm.GetSettings(SettingType.Baisc) as BasicSettings;
            WatchedEvents = new ObservableCollection<string>();
            if(_es != null && _es.WatchedEvents != null)
                _es.WatchedEvents.Values.ToList().ForEach(x => WatchedEvents.Add(x));
            LoadAsync();
        }

        private async void LoadAsync()
        {
            LoadingMsg = "Loading Data";
            _eventData = await _em.GetEventNamesAsync();
            Events = _eventData.Select(y => y.Value.Name).ToList();
            Events.Sort();
            //NOTE: build add just for testing
            //Events.ForEach(c => WatchedEvents.Add(c));
            LoadingMsg = "Load Complete";
        }

        private void AddExecute()
        {
            WatchedEvents.Add(SelectedEventName);
        }

        private void SaveExecute(Window w)
        {
            //_es.WatchedEvents = WatchedEvents.ToList<String>();
            _es.RefreshData(_eventData.Where(x => WatchedEvents.Contains(x.Value.Name)));
            _sm.Save(_es);
            if (w != null)
                w.Close();
        }
    }
}
