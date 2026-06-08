using System.Windows;
using Model;
using View.ViewModel;
using Diagnostics;

namespace View2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(new BallService(), new Logger());
        }
    }
}