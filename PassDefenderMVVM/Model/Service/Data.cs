
namespace PassDefenderMVVM.Model.Service
{
    class Data
    {
        private string password;
        private bool cryptOnLostFocus;
        public string Info { get; set; }

        public string Login { get; set; }

        public Data(string info, string login, string password)
        {
            Info = info;
            Login = login;
            Password = password;
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (cryptOnLostFocus) // исключает двойную шифрацию пароля при старте программы
                {
                    MainPassword cryptoModel = new MainPassword();
                    password = new CryptoService().Encrypt(value, cryptoModel.PassPhrase, cryptoModel.SaltValue, cryptoModel.InitVector);
                }
                else
                {
                    password = value;
                    cryptOnLostFocus = true;
                }
            }
        }
    }
}
