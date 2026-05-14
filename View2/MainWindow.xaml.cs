using System.Windows;
using Model;
using View.ViewModel;

namespace View2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(new BallService());
        }
    }
}