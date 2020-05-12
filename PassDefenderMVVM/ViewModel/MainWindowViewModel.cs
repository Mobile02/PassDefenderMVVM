using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PassDefenderMVVM.Model;
using PassDefenderMVVM.Model.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PassDefenderMVVM.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        private DataModel dataModel;
        private ObservableCollection<Data> dataCollections;
        private Data selectedData;
        private DispatcherTimer dispatcherTimer;
        private string filePath;
        private int progress;
        private string labelInfo;
        private bool copying;
        private bool isEditing;

        private ICommand commandCopyLogin;
        private ICommand commandCopyPassword;
        private ICommand commandGenerationPassword;
        private ICommand commandDeleteRow;
        private ICommand commandAddRow;
        private ICommand commandSave;
        private ICommand commandLoaded;

        public MainWindowViewModel()
        {
            filePath = "keys.pdk";

            DataCollections = new ObservableCollection<Data>();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        private void SelectedData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsEditing = true;
        }

        public bool IsEditing
        {
            get { return isEditing; }
            set
            {
                isEditing = value;
                RaisePropertyChanged("IsEditing");
            }
        }

        public bool Copying
        {
            get { return copying; }
            set
            {
                copying = value;
                RaisePropertyChanged("Copying");
            }
        }

        public Data SelectedData
        {
            get { return selectedData; }
            set
            {
                selectedData = value;
                SelectedData.PropertyChanged -= SelectedData_PropertyChanged;
                SelectedData.PropertyChanged += SelectedData_PropertyChanged;
                RaisePropertyChanged("SelectedData");
            }
        }

        public ObservableCollection<Data> DataCollections
        {
            get { return dataCollections; }
            set
            {
                dataCollections = value;
                RaisePropertyChanged("DataCollections");
            }
        }

        public int ProgressBarValue
        {
            get { return progress; }
            set
            {
                progress = value;
                RaisePropertyChanged("ProgressBarValue");
            }
        }

        public string LabelInfo
        {
            get { return labelInfo; }
            set
            {
                labelInfo = value;
                RaisePropertyChanged("LabelInfo");
            }
        }

        public ICommand CommandCopyLogin
        {
            get
            {
                if (commandCopyLogin == null)
                {
                    return commandCopyLogin = new RelayCommand(execute: CopyLogin);
                }
                return commandCopyLogin;
            }
        }

        public ICommand CommandCopyPassword
        {
            get
            {
                if (commandCopyPassword == null)
                {
                    return commandCopyPassword = new RelayCommand(execute: CopyPassword);
                }
                return commandCopyPassword;
            }
        }

        public ICommand CommandGenerationPassword
        {
            get
            {
                if (commandGenerationPassword == null)
                {
                    return commandGenerationPassword = new RelayCommand(execute: GenerationPassword);
                }
                return commandGenerationPassword;
            }
        }

        public ICommand CommandDeleteRow
        {
            get
            {
                if (commandDeleteRow == null)
                {
                    return commandDeleteRow = new RelayCommand(execute: DeleteRow);
                }
                return commandDeleteRow;
            }
        }

        public ICommand CommandAddRow
        {
            get
            {
                if (commandAddRow == null)
                {
                    return commandAddRow = new RelayCommand(execute: AddRow);
                }
                return commandAddRow;
            }
        }

        public ICommand CommandSave
        {
            get
            {
                if (commandSave == null)
                {
                    return commandSave = new RelayCommand(execute: Save);
                }
                return commandSave;
            }
        }

        public ICommand CommandLoaded
        {
            get
            {
                if (commandLoaded == null)
                {
                    return commandLoaded = new RelayCommand(execute: Loaded);
                }
                return commandLoaded;
            }
        }

        private void Loaded()
        {
            new PasswordWindowViewModel().Check();

            dataModel = new DataModel();
            dataModel.LoadData(filePath);
            DataCollections = dataModel.DataCollections;
        }

        private void CopyLogin()
        {
            Clipboard.Clear();
            Clipboard.SetText(SelectedData.Login);
        }

        private void CopyPassword()
        {
            MainPassword mainPassword = new MainPassword();
            Clipboard.Clear();
            Clipboard.SetText(new CryptoService().Decrypt(SelectedData.Password, mainPassword.PassPhrase, mainPassword.SaltValue, mainPassword.InitVector));

            if (dispatcherTimer.IsEnabled)
            {
                ProgressBarValue = 1500;
                Copying = false;
            }
            else
            {
                dispatcherTimer.Start();
                Copying = true;
            }
        }

        private void GenerationPassword()
        {
            IsEditing = true;
            int index = DataCollections.IndexOf(SelectedData);
            SelectedData.Password = new Randomizer().RndString(16);
            dataModel.UpdateRow(index, SelectedData);
        }

        private void DeleteRow()
        {
            IsEditing = true;
            dataModel.DeleteRow(SelectedData);
        }

        private void AddRow()
        {
            IsEditing = true;
            Data data = new Data("", "", "");
            dataModel.AddData(data);
        }

        private void Save()
        {
            dataModel.SaveData(filePath);
            IsEditing = false;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            LabelInfo = "Буфер обмена будет очищен через " + $"{(1500 - ProgressBarValue) / 100}";
            if ((ProgressBarValue += 1) >= 1500)
            {
                dispatcherTimer.Stop();
                ProgressBarValue = 0;
                LabelInfo = "";

                Copying = false;
                Clipboard.Clear();
            }
        }
    }
}
