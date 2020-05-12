using PassDefenderMVVM.Model.Service;
using System;
using System.Collections.ObjectModel;
using System.Deployment.Application;
using System.Windows;

namespace PassDefenderMVVM.Model
{
    class DataModel
    {
        public ObservableCollection<Data> DataCollections { get; set; }

        public DataModel()
        {
            DataCollections = new ObservableCollection<Data>();
        }

        public void AddData(Data data)
        {
            DataCollections.Add(data);
        }

        public void LoadData(string path)
        {
            FileOperation fileOperation = new FileOperation();

            if (!fileOperation.CheckFileExists(path))
            {
                DataCollections.Add(new Data("", "", ""));

                try { fileOperation.CreateFile(path); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }

                try { fileOperation.SaveFile(path, DataCollections); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            }

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                try { path = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0]; }
                catch { path = Environment.CurrentDirectory + "\\keys.pdk"; }
            }

            DataCollections = new FileOperation().OpenFile(path);
        }

        public void DeleteRow(Data data)
        {
            DataCollections.Remove(data);
        }

        public void SaveData(string path)
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                try { path = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0]; }
                catch { path = Environment.CurrentDirectory + "\\keys.pdk"; }
            }

            try
            {
                new FileOperation().SaveFile(path, DataCollections);
                MessageBox.Show("Сохранено", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        public void UpdateRow(int index, Data data)
        {
            DataCollections.RemoveAt(index);
            DataCollections.Insert(index, data);
        }
    }
}
