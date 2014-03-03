using EventDataManager;
using GwApiNET;
using Persistance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GW2EventMonitor.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region fields
        private readonly Color _mutedColor = Colors.Black;
        private readonly Color _eventFailedColor = Colors.Red;
        private readonly Color _eventActiveColor = Colors.Orange;
        private readonly Color _eventSuccessColor = Colors.Green;
        private readonly Color _eventWarmupColor = Colors.Orange;
        private readonly Color _normalColor = Colors.DarkBlue;

        private SettingsManager _sm = new SettingsManager();
        private EventSettings _es;
        private BasicSettings _bs;

        private bool _isMuted = false;

        #endregion

        #region Props



        private bool _isNotiVisible;

        public bool IsNotiVisible
        {
            get { return _isNotiVisible; }
            set
            {
                _isNotiVisible = value;
                OnPropertyChanged("IsNotiVisible");
            }
        }

        private string _notification;

        public string Notification
        {
            get { return _notification; }
            set
            {
                _notification = value;
                OnPropertyChanged("Notification");
            }
        }


        private Color _fillColor;

        public Color FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
                OnPropertyChanged("FillColor");
            }
        }

        #endregion

        #region Commands
        private ICommand _settingsCommand;

        public ICommand SettingsCommand
        {
            get { return _settingsCommand; }
            set
            {
                _settingsCommand = value;
                OnPropertyChanged("SettingsCommand");
            }
        }

        private ICommand _viewTimersCommand;

        public ICommand ViewTimersCommand
        {
            get { return _viewTimersCommand; }
            set
            {
                _viewTimersCommand = value;
                OnPropertyChanged("ViewTimersCommand");
            }
        }

        private ICommand _muteCommand;

        public ICommand MuteCommand
        {
            get { return _muteCommand; }
            set
            {
                _muteCommand = value;
                OnPropertyChanged("MuteCommand");
            }
        }

        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get { return _closeCommand; }
            set
            {
                _closeCommand = value;
                OnPropertyChanged("CloseCommand");
            }
        }
        #endregion

        public MainViewModel()
        {
            _es = _sm.GetSettings(SettingType.Event) as EventSettings;
            _bs = _sm.GetSettings(SettingType.Baisc) as BasicSettings;

            IsNotiVisible = false;
            FillColor = _normalColor;
            Notification = String.Empty;
            SettingsCommand = new RelayCommand(SettingsExecute);
            ViewTimersCommand = new RelayCommand(ViewTimersExecute);
            MuteCommand = new RelayCommand(MuteExecute);
            CloseCommand = new RelayCommand(CloseExecute);

            //TODO this is garbage. It's a hack to get things working for now. This needs to be protected
            if (_bs.WorldID > 0)
                Task.Factory.StartNew(() => 
                    {
                        InitEvents();
                    });
            else
                MessageBox.Show("You must set a world! Go to the settings menu and select your world then restart the application", "Something Is Wrong!", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void InitEvents()
        {
            if (_es.WatchedEvents != null)
                foreach (KeyValuePair<Guid, String> entry in _es.WatchedEvents)
                {
                    EventMonitor.AddWatch(EventChangeState, entry.Value, entry.Key);
                }
        }

        private void EventChangeState(String eventName, EventState es)
        {
            if (!_isMuted)
            {
                Notification += String.Format("{0} - {1}\r\n", eventName, es.GetDescription());
                // TODO fix this up. I would like to have the event string that is displayed in the notification window
                // set to this color and have the "FillColor" set to something else. 
                // If too many events come in at one time this means nothing.
                FillColor = _eventActiveColor;
                if(!IsNotiVisible)
                    IsNotiVisible = true;
            }
        }

        private void MuteExecute()
        {
            if (_isMuted)
            {
                FillColor = _normalColor;
            }
            else
            {
                FillColor = _mutedColor;
            }

            _isMuted = !_isMuted;
        }

        private void CloseExecute()
        {
            Environment.Exit(0);
        }

        private void ViewTimersExecute()
        {
            EventsView e = new EventsView();
            e.Show();
        }

        private void SettingsExecute()
        {
            Settings s = new Settings();
            s.Show();
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
