using System.Windows;
using System.Windows.Input;

namespace CompanyCalculator.Client.Helpers
{
    public static class KeyDownBehavior
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(KeyDownBehavior),
                new PropertyMetadata(null, OnCommandChanged));

        public static void SetCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(CommandProperty);
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                // Remove previous handler, if any.
                element.KeyDown -= Element_KeyDown;

                if (e.NewValue != null)
                {
                    element.KeyDown += Element_KeyDown;
                }
            }
        }

        private static void Element_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is UIElement element)
            {
                var command = GetCommand(element);
                if (command != null && command.CanExecute(e))
                {
                    command.Execute(e);
                }
            }
        }
    }
}
