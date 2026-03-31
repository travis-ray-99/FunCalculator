using Avalonia.Controls;
using Avalonia.Input;
using FunCalculator.ViewModels;

namespace FunCalculator;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new CalculatorViewModel();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (DataContext is not CalculatorViewModel vm) return;

        bool shift = e.KeyModifiers.HasFlag(KeyModifiers.Shift);
        bool handled = true;

        switch (e.Key)
        {
            // Shifted keys: Shift+5 = %, Shift+8 = ×, Shift+= = +
            // These must appear before the unshifted digit/equals cases
            // because C# evaluates cases top-to-bottom.
            case Key.D5 when shift:
                vm.PercentCommand.Execute(null); break;
            case Key.D8 when shift:
                vm.OperationCommand.Execute("×"); break;
            case Key.OemPlus when shift:
                vm.OperationCommand.Execute("+"); break;

            // Number keys (top row and numpad)
            case Key.D0 or Key.NumPad0:
                vm.DigitCommand.Execute("0"); break;
            case Key.D1 or Key.NumPad1:
                vm.DigitCommand.Execute("1"); break;
            case Key.D2 or Key.NumPad2:
                vm.DigitCommand.Execute("2"); break;
            case Key.D3 or Key.NumPad3:
                vm.DigitCommand.Execute("3"); break;
            case Key.D4 or Key.NumPad4:
                vm.DigitCommand.Execute("4"); break;
            case Key.D5 or Key.NumPad5:
                vm.DigitCommand.Execute("5"); break;
            case Key.D6 or Key.NumPad6:
                vm.DigitCommand.Execute("6"); break;
            case Key.D7 or Key.NumPad7:
                vm.DigitCommand.Execute("7"); break;
            case Key.D8 or Key.NumPad8:
                vm.DigitCommand.Execute("8"); break;
            case Key.D9 or Key.NumPad9:
                vm.DigitCommand.Execute("9"); break;

            // Numpad operators
            case Key.Add:
                vm.OperationCommand.Execute("+"); break;
            case Key.Subtract:
                vm.OperationCommand.Execute("−"); break;
            case Key.Multiply:
                vm.OperationCommand.Execute("×"); break;
            case Key.Divide:
                vm.OperationCommand.Execute("÷"); break;

            // Main keyboard operator keys
            case Key.OemMinus:
                vm.OperationCommand.Execute("−"); break;
            // = key (unshifted OemPlus on US keyboards)
            case Key.OemPlus:
                vm.EqualsCommand.Execute(null); break;

            // Slash key for divide
            case Key.Oem2:
                vm.OperationCommand.Execute("÷"); break;

            // Decimal point
            case Key.OemPeriod or Key.Decimal:
                vm.DecimalCommand.Execute(null); break;

            // Enter / Equals
            case Key.Enter:
                vm.EqualsCommand.Execute(null); break;

            // Clear / Backspace
            case Key.Escape or Key.Delete:
                vm.ClearCommand.Execute(null); break;
            case Key.Back:
                vm.BackspaceCommand.Execute(null); break;

            default:
                handled = false; break;
        }

        e.Handled = handled;
    }
}