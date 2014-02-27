using EventDataManager;
using Persistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GW2EventMonitor.ViewModels
{
    public class EventsViewModel : INotifyPropertyChanged
    {
        #region Fields
        private SettingsManager _sm = new SettingsManager();
        private EventSettings _es;
        private BasicSettings _bs;
        private EventDataFetcher _em = new EventDataFetcher();
        #endregion

        #region Props
        private List<String> _events;

        public List<String> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                OnPropertyChanged("Events");
            }
        }


        private ObservableCollection<String> _watchedEvents;

        public ObservableCollection<String> WatchedEvents
        {
            get { return _watchedEvents; }
            set
            {
                _watchedEvents = value;
                OnPropertyChanged("WatchedEvents");
            }
        }

        private String _selectedEventName;

        public String SelectedEventName
        {
            get { return _selectedEventName; }
            set
            {
                _selectedEventName = value;
                OnPropertyChanged("SelectedEventName");
            }
        }

        #endregion

        #region Commands
        private ICommand _saveCommand;

        public ICommand SaveCommand
        {
            get { return _saveCommand; }
            set
            {
                _saveCommand = value;
                OnPropertyChanged("SaveCommand");
            }
        }

        private ICommand _addCommand;

        public ICommand AddCommand
        {
            get { return _addCommand; }
            set
            {
                _addCommand = value;
                OnPropertyChanged("AddCommand");
            }
        }
        #endregion

        public EventsViewModel()
        {
            _es = _sm.GetSettings(SettingType.Event) as EventSettings;
            _bs = _sm.GetSettings(SettingType.Baisc) as BasicSettings;
            SaveCommand = new RelayCommand<Window>(SaveExecute);
            AddCommand = new RelayCommand(AddExecute);
            WatchedEvents = new ObservableCollection<string>();
            if(_es.WatchedEvents != null)
                _es.WatchedEvents.ForEach(x => WatchedEvents.Add(x));
            Events = _em.GetEvents("");
        }

        private void AddExecute()
        {
            WatchedEvents.Add(SelectedEventName);
        }

        private void SaveExecute(Window w)
        {
            _es.WatchedEvents = WatchedEvents.ToList<String>();
            _sm.Save(_es);
            if (w != null)
                w.Close();
        }

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
