using System;
using System.ComponentModel;
using System.Windows.Input;
using Model;

namespace View2.ViewModel
{
    public class Main : INotifyPropertyChanged
    {
        private double value1;
        private double value2;
        private string selectedOperation = "+";
        private bool isAdd = true;
        private bool isSubtract;
        private bool isMultiply;
        private bool isDivide;
        private string result;

        public double Value1
        {
            get => value1;
            set { value1 = value; OnPropertyChanged(nameof(Value1)); }
        }

        public double Value2
        {
            get => value2;
            set { value2 = value; OnPropertyChanged(nameof(Value2)); }
        }

        public string SelectedOperation
        {
            get => selectedOperation;
            set { selectedOperation = value; OnPropertyChanged(nameof(SelectedOperation)); }
        }

        public bool IsAdd
        {
            get => isAdd;
            set
            {
                if (isAdd == value) return;
                isAdd = value;
                if (value)
                {
                    isSubtract = isMultiply = isDivide = false;
                    SelectedOperation = "+";
                    OnPropertyChanged(nameof(IsSubtract));
                    OnPropertyChanged(nameof(IsMultiply));
                    OnPropertyChanged(nameof(IsDivide));
                }
                OnPropertyChanged(nameof(IsAdd));
            }
        }

        public bool IsSubtract
        {
            get => isSubtract;
            set
            {
                if (isSubtract == value) return;
                isSubtract = value;
                if (value)
                {
                    isAdd = isMultiply = isDivide = false;
                    SelectedOperation = "-";
                    OnPropertyChanged(nameof(IsAdd));
                    OnPropertyChanged(nameof(IsMultiply));
                    OnPropertyChanged(nameof(IsDivide));
                }
                OnPropertyChanged(nameof(IsSubtract));
            }
        }

        public bool IsMultiply
        {
            get => isMultiply;
            set
            {
                if (isMultiply == value) return;
                isMultiply = value;
                if (value)
                {
                    isAdd = isSubtract = isDivide = false;
                    SelectedOperation = "*";
                    OnPropertyChanged(nameof(IsAdd));
                    OnPropertyChanged(nameof(IsSubtract));
                    OnPropertyChanged(nameof(IsDivide));
                }
                OnPropertyChanged(nameof(IsMultiply));
            }
        }

        public bool IsDivide
        {
            get => isDivide;
            set
            {
                if (isDivide == value) return;
                isDivide = value;
                if (value)
                {
                    isAdd = isSubtract = isMultiply = false;
                    SelectedOperation = "/";
                    OnPropertyChanged(nameof(IsAdd));
                    OnPropertyChanged(nameof(IsSubtract));
                    OnPropertyChanged(nameof(IsMultiply));
                }
                OnPropertyChanged(nameof(IsDivide));
            }
        }

        public string Result
        {
            get => result;
            private set { result = value; OnPropertyChanged(nameof(Result)); }
        }

        public ICommand CalculateCommand { get; }

        public Main()
        {
            CalculateCommand = new RelayCommand(_ => Calculate(), _ => true);
        }

        private void Calculate()
        {
            try
            {
                var calc = new Calculator(Value1, Value2);
                double res = SelectedOperation switch
                {
                    "-" => calc.substract(),
                    "*" => calc.multiply(),
                    "/" => calc.divide(),
                    _ => calc.add(),
                };

                Result = res.ToString();
            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // Minimal relay command implementation
    internal class RelayCommand : ICommand
    {
        private readonly Action<object?> execute;
        private readonly Predicate<object?>? canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => execute(parameter);

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
