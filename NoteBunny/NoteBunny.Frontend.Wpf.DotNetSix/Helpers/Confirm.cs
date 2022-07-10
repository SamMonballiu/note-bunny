using System.Windows;

namespace NoteBunny.Frontend.Wpf.DotNetSix.Helpers;

internal class Helpers
{
    public static bool Confirm(string question, string? title = null, MessageBoxResult defaultChoice = MessageBoxResult.No)
    {
        return MessageBox.Show(question, title ?? string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question, defaultChoice) == MessageBoxResult.Yes;
    }
}
