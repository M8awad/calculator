using CompanyCalculator.Client.Helpers;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CompanyCalculator.Client
{
  
    public partial class MainWindow : Window
    {
        private MainCalculatorViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainCalculatorViewModel();
            DataContext = _viewModel;
        }

        // Window dragging functionality
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        // Window control buttons functionality
        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ?
                WindowState.Normal : WindowState.Maximized;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Create and show the history popup
            var historyPopup = new HistoryPopup(_viewModel.CalculationHistory);
            historyPopup.HistoryItemSelected += (s, args) => {
                if (args.SelectedItem != null)
                {
                    // Use the selected history item
                    _viewModel.UseHistoryItem(args.SelectedItem);
                }
            };

            historyPopup.Owner = this;
            historyPopup.ShowDialog();
        }
    

        //private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Calculation History Feature\nComing Soon!", "History",
        //        MessageBoxButton.OK, MessageBoxImage.Information);
        //}
    }
}
