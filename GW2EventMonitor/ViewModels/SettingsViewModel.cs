using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldDataManager;
using Persistance;
using System.Windows;

namespace GW2EventMonitor.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        #region Fields
        private WorldManager worldFetch = new WorldManager();
        private SettingsManager _sm = new SettingsManager();
        private Persistance.BasicSettings _settings;
        #endregion

        #region props
        private string[] _worlds;

        public string[] Worlds
        {
            get { return _worlds; }
            set
            {
                _worlds = value;
                OnPropertyChanged("Worlds");
            }
        }

        public string CurrWorldName
        {
            get { return _settings.CurrentWorld; }
            set
            {
                _settings.CurrentWorld = value;
                OnPropertyChanged("CurrWorldName");
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

        #endregion

        public SettingsViewModel()
        {
            Worlds = worldFetch.GetWorldNames();
            SaveCommand = new RelayCommand<Window>(SaveExecute);
            _settings = _sm.GetSettings(SettingType.Baisc) as BasicSettings;
        }

        private void SaveExecute(Window w)
        {
            _sm.Save(_settings);
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
