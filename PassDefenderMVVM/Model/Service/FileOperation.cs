using System.Collections.ObjectModel;
using System.IO;

namespace PassDefenderMVVM.Model.Service
{
    class FileOperation
    {
        public void SaveFile(string path, ObservableCollection<Data> dataCollections)
        {
            StreamWriter streamWriter = new StreamWriter(path);
            CryptoService cryptoService = new CryptoService();
            MainPassword mainPassword = new MainPassword();

            for (int i = 0; i < dataCollections.Count; i++)
            {
                streamWriter.WriteLine(cryptoService.Encrypt(dataCollections[i].Info, mainPassword.PassPhrase, mainPassword.SaltValue, mainPassword.InitVector));
                streamWriter.WriteLine(cryptoService.Encrypt(dataCollections[i].Login, mainPassword.PassPhrase, mainPassword.SaltValue, mainPassword.InitVector));
                streamWriter.WriteLine(cryptoService.Encrypt(dataCollections[i].Password, mainPassword.PassPhrase, mainPassword.SaltValue, mainPassword.InitVector));
            }
            streamWriter.Close();
        }

        public ObservableCollection<Data> OpenFile(string path)
        {
            StreamReader streamReader = new StreamReader(path);
            ObservableCollection<Data> dataCollections = new ObservableCollection<Data>();

            while (!streamReader.EndOfStream)
            {
                CryptoService crypto = new CryptoService();
                MainPassword mainPassword = new MainPassword();
                Data data = new Data
                    (
                    crypto.Decrypt(streamReader.ReadLine(), mainPassword.PassPhrase, mainPassword.SaltValue, mainPassword.InitVector),
                    crypto.Decrypt(streamReader.ReadLine(), mainPassword.PassPhrase, mainPassword.SaltValue, mainPassword.InitVector),
                    crypto.Decrypt(streamReader.ReadLine(), mainPassword.PassPhrase, mainPassword.SaltValue, mainPassword.InitVector)
                    );
                dataCollections.Add(data);
            }
            streamReader.Close();
            return dataCollections;
        }

        public bool CheckFileExists(string path)
        {
            if (File.Exists(path))
                return true;
            else
                return false;
        }

        public void CreateFile(string path)
        {
            File.Create(path).Close();
        }
    }
}
