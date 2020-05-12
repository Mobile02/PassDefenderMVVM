using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PassDefenderMVVM.Model.Service;
using PassDefenderMVVM.View;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace PassDefenderMVVM.ViewModel
{
    class PasswordWindowViewModel : ViewModelBase
    {
        private Window ownerWindow;
        private PasswordWindow passwordWindow;
        private ICommand commandButtonEnter;
        private ICommand commandButtonExit;

        private string password = "";
        private string buttonContent;

        public PasswordWindowViewModel()
        {
            ownerWindow = Application.Current.MainWindow;
            if (Properties.Settings.Default.savePassword == "")
                ButtonContent = "Сохранить";
            else
                ButtonContent = "Вход";
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        public string ButtonContent
        {
            get
            { return buttonContent; }
            set
            {
                buttonContent = value;
                RaisePropertyChanged("ButtonContent");
            }
        }

        public void Check()
        {
            passwordWindow = new PasswordWindow();
            ownerWindow.Effect = new BlurEffect();

            passwordWindow.Owner = ownerWindow;
            passwordWindow.ShowDialog();
        }


        public ICommand CommandButtonEnter
        {
            get
            {
                if (commandButtonEnter == null)
                {
                    return commandButtonEnter = new RelayCommand(execute: ButtonEnter);
                }
                return commandButtonEnter;
            }
        }

        public ICommand CommandButtonExit
        {
            get
            {
                if (commandButtonExit == null)
                {
                    return commandButtonExit = new RelayCommand(execute: ButtonExit);
                }
                return commandButtonExit;
            }
        }

        public void ButtonEnter()
        {
            MainPassword mainPassword = new MainPassword();

            Properties.Settings.Default.savePassword = mainPassword.EncryptPassPhrase(Password);
            Properties.Settings.Default.Save();

            ownerWindow.Effect = null;
        }

        public void ButtonExit()
        {
            Application.Current.Shutdown();
        }
    }
}
