using System.Windows;

namespace PassDefenderMVVM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWindow1.Left = Properties.Settings.Default.Left;
            MainWindow1.Top = Properties.Settings.Default.Top;
            MainWindow1.Width = Properties.Settings.Default.Width;
            MainWindow1.Height = Properties.Settings.Default.Height;
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Left = MainWindow1.Left;
            Properties.Settings.Default.Top = MainWindow1.Top;
            Properties.Settings.Default.Width = MainWindow1.Width;
            Properties.Settings.Default.Height = MainWindow1.Height;
            Properties.Settings.Default.Save();
        }
    }
}
