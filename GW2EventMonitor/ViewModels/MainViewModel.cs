using EventDataManager;
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
        private readonly Color _alertColor = Colors.Red;
        private readonly Color _normalColor = Colors.DarkBlue;

        private bool _isMuted = false;

        private EventDataFetcher eventFetcher = new EventDataFetcher();
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
            IsNotiVisible = false;
            FillColor = _normalColor;

            SettingsCommand = new RelayCommand(SettingsExecute);
            ViewTimersCommand = new RelayCommand(ViewTimersExecute);
            MuteCommand = new RelayCommand(MuteExecute);
            CloseCommand = new RelayCommand(CloseExecute);

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
