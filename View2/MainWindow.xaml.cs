using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System;

namespace View2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Set DataContext to the ViewModel so the View is connected to the Model through the ViewModel
            this.DataContext = new View2.ViewModel.Main();
        }

        // Allow digits and single decimal separator (dot or comma)
        private static readonly Regex _numberRegex = new Regex(@"^[0-9]*[.,]?[0-9]*$");

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox tb)
            {
                var selectionStart = tb.SelectionStart;
                var selectionLength = tb.SelectionLength;
                var text = tb.Text;
                // Simulate new text after input
                var newText = text.Remove(selectionStart, selectionLength).Insert(selectionStart, e.Text);
                e.Handled = !_numberRegex.IsMatch(newText);
            }
        }

        private void Number_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (e.DataObject.GetDataPresent(DataFormats.Text))
                {
                    var pasteText = e.DataObject.GetData(DataFormats.Text) as string ?? string.Empty;
                    var selectionStart = tb.SelectionStart;
                    var selectionLength = tb.SelectionLength;
                    var text = tb.Text;
                    var newText = text.Remove(selectionStart, selectionLength).Insert(selectionStart, pasteText);
                    if (!_numberRegex.IsMatch(newText))
                    {
                        e.CancelCommand();
                    }
                }
                else
                {
                    e.CancelCommand();
                }
            }
        }
    }
}