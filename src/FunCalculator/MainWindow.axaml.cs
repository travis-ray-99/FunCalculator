using Avalonia.Controls;
using FunCalculator.ViewModels;

namespace FunCalculator;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new CalculatorViewModel();
    }
}