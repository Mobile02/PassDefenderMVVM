/// <summary>
/// Шифрация и дешифрация основного пароля (вводимого при старте программы).
/// Пароль в результате хранится в зашифрованном виде в Properties.Settings.Default.savePassword
/// при обращении к нему дешифруется в нормальный вид
/// </summary>
namespace PassDefenderMVVM.Model.Service
{
    class MainPassword
    {
        private string passPhrase;

        public string PassPhrase
        {
            get { return passPhrase; }
            set
            {
                passPhrase = new CryptoService().Decrypt(value, "Dfe4%^GteE4@sw!21dssdfE", SaltValue, InitVector);
            }
        }

        public string SaltValue { get; set; }
        public string InitVector { get; set; }

        public MainPassword()
        {
            SaltValue = "DF544D%%#ssdsf#@";
            InitVector = "qhdE4Fhy$?d><hRw";
            PassPhrase = Properties.Settings.Default.savePassword;
        }

        public string EncryptPassPhrase(string passPhrase)
        {
            return new CryptoService().Encrypt(passPhrase, "Dfe4%^GteE4@sw!21dssdfE", SaltValue, InitVector);
        }
    }
}
