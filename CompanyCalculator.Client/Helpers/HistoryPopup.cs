using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CompanyCalculator.Core.Models;

namespace CompanyCalculator.Client.Helpers
{
    // History popup window class
    public class HistoryPopup : Window
    {
        private ListBox _historyListBox;

        public event EventHandler<HistoryItemSelectedEventArgs> HistoryItemSelected;

        public HistoryPopup(ObservableCollection<CalculationHistoryItem> history)
        {
            Title = "Calculation History";
            Width = 300;
            Height = 400;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var grid = new Grid();

            // Create the history list
            _historyListBox = new ListBox
            {
                ItemsSource = history,
                Margin = new Thickness(10)
            };

            // Create a data template for the history items
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("DisplayText"));
            var dataTemplate = new DataTemplate();
            dataTemplate.VisualTree = factory;
            _historyListBox.ItemTemplate = dataTemplate;

            _historyListBox.MouseDoubleClick += HistoryListBox_MouseDoubleClick;

            // Create buttons
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10)
            };

            var useButton = new Button
            {
                Content = "Use",
                Width = 80,
                Height = 30,
                Margin = new Thickness(5)
            };
            useButton.Click += UseButton_Click;

            var clearButton = new Button
            {
                Content = "Clear History",
                Width = 100,
                Height = 30,
                Margin = new Thickness(5)
            };
            clearButton.Click += (s, e) => {
                history.Clear();
            };

            buttonPanel.Children.Add(useButton);
            buttonPanel.Children.Add(clearButton);

            // Set up the layout
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            Grid.SetRow(_historyListBox, 0);
            Grid.SetRow(buttonPanel, 1);

            grid.Children.Add(_historyListBox);
            grid.Children.Add(buttonPanel);

            Content = grid;
        }

        private void UseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_historyListBox.SelectedItem is CalculationHistoryItem selectedItem)
            {
                HistoryItemSelected?.Invoke(this, new HistoryItemSelectedEventArgs(selectedItem));
                Close();
            }
        }

        private void HistoryListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_historyListBox.SelectedItem is CalculationHistoryItem selectedItem)
            {
                HistoryItemSelected?.Invoke(this, new HistoryItemSelectedEventArgs(selectedItem));
                Close();
            }
        }
    }

    public class HistoryItemSelectedEventArgs : EventArgs
    {
        public CalculationHistoryItem SelectedItem { get; }

        public HistoryItemSelectedEventArgs(CalculationHistoryItem selectedItem)
        {
            SelectedItem = selectedItem;
        }
    }

   
    
}
