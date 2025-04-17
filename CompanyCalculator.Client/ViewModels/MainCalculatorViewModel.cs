using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CompanyCalculator.Core.Models;

namespace CompanyCalculator.Client
{
    public class MainCalculatorViewModel : INotifyPropertyChanged
    {
        private string _displayText = "0";
        public string DisplayText
        {
            get => _displayText;
            set { _displayText = value; OnPropertyChanged(); }
        }

        // Internal state tracking
        private double _currentValue;
        private CalculationOperation? _currentOperation;
        private bool _resetDisplay;

        // Add this property to store calculation history
        public ObservableCollection<CalculationHistoryItem> CalculationHistory { get; } = new ObservableCollection<CalculationHistoryItem>();

        public ICommand NumberCommand { get; }
        public ICommand OperationCommand { get; }
        public ICommand CalculateCommand { get; }
        public ICommand DecimalCommand { get; }
        public ICommand ClearEntryCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand BackspaceCommand { get; }
        public ICommand ToggleSignCommand { get; }
        public ICommand KeyDownCommand { get; }

        // HttpClient to call the backend API
        private readonly HttpClient _httpClient;

        public MainCalculatorViewModel()
        {
            NumberCommand = new RelayCommand<string>(AppendNumber);
            OperationCommand = new RelayCommand<string>(SetOperation);
            CalculateCommand = new AsyncRelayCommand(CalculateResult); // No parameter needed
            DecimalCommand = new RelayCommand(AppendDecimal);

            // Wrap parameterless methods in lambdas to match Action<object>
            ClearEntryCommand = new RelayCommand(_ => ClearEntry());
            ClearCommand = new RelayCommand(_ => ClearAll());
            BackspaceCommand = new RelayCommand(_ => Backspace());
            ToggleSignCommand = new RelayCommand(_ => ToggleSign());
            KeyDownCommand = new RelayCommand<KeyEventArgs>(HandleKeyDown);

            // Bypass SSL validation for local development
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7058/")
            };
        }

        private void HandleKeyDown(KeyEventArgs e)
        {
            // Process numeric keys from the top row (D0-D9)...
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string number = (e.Key - Key.D0).ToString();
                NumberCommand.Execute(number);
            }
            // Process keys from the numeric keypad
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                string number = (e.Key - Key.NumPad0).ToString();
                NumberCommand.Execute(number);
            }
            // Decimal separator
            else if (e.Key == Key.OemPeriod || e.Key == Key.Decimal)
            {
                DecimalCommand.Execute(null);
            }
            // Operations (you may need to adjust the keys based on your keyboard layout)
            else if (e.Key == Key.OemPlus || e.Key == Key.Add)
            {
                OperationCommand.Execute("Add");
            }
            else if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                OperationCommand.Execute("Subtract");
            }
            // Enter key for calculating the result
            else if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                if (CalculateCommand.CanExecute(null))
                {
                    CalculateCommand.Execute(null);
                }
            }
            // You can extend to cover other keys (e.g., Backspace, Clear, ToggleSign) if desired.
        }

        private void AppendNumber(string number)
        {
            if (_resetDisplay || DisplayText == "0")
            {
                DisplayText = number;
                _resetDisplay = false;
            }
            else
            {
                DisplayText += number;
            }
        }

        private void SetOperation(string op)
        {
            if (double.TryParse(DisplayText, out double value))
            {
                _currentValue = value;
            }
            _currentOperation = Enum.TryParse<CalculationOperation>(op, out var operation)
                                ? operation
                                : (CalculationOperation?)null;
            _resetDisplay = true;
        }

        private void AppendDecimal(object parameter)
        {
            if (!DisplayText.Contains("."))
            {
                DisplayText += ".";
            }
        }

        // Modify your CalculateResult method to add items to history
        private async Task CalculateResult()
        {
            if (!_currentOperation.HasValue)
                return;

            if (!double.TryParse(DisplayText, out double secondValue))
                return;

            var request = new CalculationRequest
            {
                Operand1 = _currentValue,
                Operand2 = secondValue,
                Operation = _currentOperation.Value
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Calculator/calculate", request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<CalculationResult>();

                    if (result != null)
                    {
                        // Add to history
                        var historyItem = new CalculationHistoryItem
                        {
                            Operand1 = _currentValue,
                            Operand2 = secondValue,
                            Operation = _currentOperation.Value,
                            Result = result.Result,
                            Timestamp = DateTime.Now
                        };
                        CalculationHistory.Insert(0, historyItem); // Add to beginning of list

                        DisplayText = result.Result.ToString();
                    }
                    else
                    {
                        DisplayText = "Error";
                    }
                }
                else
                {
                    DisplayText = "Error";
                }
            }
            catch (Exception)
            {
                DisplayText = "Error";
            }

            _resetDisplay = true;
            _currentOperation = null;
        }

        // Add this method to use a history item
        public void UseHistoryItem(CalculationHistoryItem historyItem)
        {
            DisplayText = historyItem.Result.ToString();
            _currentValue = historyItem.Result;
            _resetDisplay = true;
        }

        // ClearEntry: Resets the current display without affecting stored state.
        private void ClearEntry()
        {
            DisplayText = "0";
            _resetDisplay = true;
        }

        // ClearAll: Completely resets the calculator's state.
        private void ClearAll()
        {
            DisplayText = "0";
            _currentValue = 0;
            _currentOperation = null;
            _resetDisplay = false;
        }

        // Backspace: Removes the last digit from the display.
        private void Backspace()
        {
            if (DisplayText.Length > 1)
            {
                DisplayText = DisplayText.Substring(0, DisplayText.Length - 1);
            }
            else
            {
                DisplayText = "0";
            }
        }

        // ToggleSign: Changes the sign of the current displayed number.
        private void ToggleSign()
        {
            if (double.TryParse(DisplayText, out double value))
            {
                value = -value;
                DisplayText = value.ToString();
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

     

    }
}
