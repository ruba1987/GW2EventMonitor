﻿using System;
using System.Linq;
using System.Windows.Input;
using WorldDataManager;
using Persistance;
using System.Windows;
using GwApiNET.ResponseObjects;

namespace GW2EventMonitor.ViewModels
{
    public class SettingsViewModel : NotificationObject
    {
        #region Fields
        private readonly WorldManager _worldFetch = new WorldManager();
        private readonly SettingsManager _sm = new SettingsManager();
        private readonly BasicSettings _settings;
        private EntryDictionary<int, WorldNameEntry> _worldData;
        #endregion

        #region props
        private String _infoText;

        public String InfoText
        {
            get { return _infoText; }
            set
            {
                _infoText = value;
                RaisePropertyChanged(() => InfoText);
            }
        }


        private string[] _worlds;

        public string[] Worlds
        {
            get { return _worlds; }
            set
            {
                _worlds = value;
                RaisePropertyChanged(() => Worlds);
            }
        }

        public string CurrWorldName
        {
            get { return _settings.CurrentWorld; }
            set
            {
                _settings.CurrentWorld = value;
                RaisePropertyChanged(() => CurrWorldName);
            }
        }

        #endregion

        #region Commands
        private RelayCommand<Window> _saveCommand;
        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand<Window>(SaveExecute)); }
        }

        #endregion

        public SettingsViewModel()
        {
            Worlds = new[] {"Loading Data"};
            LoadAsyncData();
            _settings = _sm.GetSettings(SettingType.Baisc) as BasicSettings;
        }

        private async void LoadAsyncData()
        {
            InfoText = "Loading Data";
            _worldData = await _worldFetch.GetWorldNamesAsync();
            var worlds = _worldData.Values.Select(x => x.Name).ToList();
            worlds.Sort();
            Worlds = worlds.ToArray<String>();
            InfoText = "Load Complete";
        }

        private void SaveExecute(Window w)
        {
            _settings.RefreshData(_worldData.Values.First(x => x.Name == CurrWorldName));
            _sm.Save(_settings);
            if (w != null)
                w.Close();
        }
    }
}
