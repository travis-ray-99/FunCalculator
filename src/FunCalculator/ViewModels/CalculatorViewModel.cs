using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FunCalculator.Models;

namespace FunCalculator.ViewModels;

public sealed class CalculatorViewModel : INotifyPropertyChanged
{
    private readonly CalculatorEngine _engine = new();

    private string _display = "0";
    private string _expression = "";
    private bool _isNewEntry = true;
    private bool _hasDecimalPoint;

    // ── Bound properties ────────────────────────────────────────────

    public string Display
    {
        get => _display;
        private set => SetField(ref _display, value);
    }

    public string Expression
    {
        get => _expression;
        private set => SetField(ref _expression, value);
    }

    // ── Commands ────────────────────────────────────────────────────

    public ICommand DigitCommand { get; }
    public ICommand OperationCommand { get; }
    public ICommand EqualsCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand ClearEntryCommand { get; }
    public ICommand BackspaceCommand { get; }
    public ICommand NegateCommand { get; }
    public ICommand PercentCommand { get; }
    public ICommand DecimalCommand { get; }

    public CalculatorViewModel()
    {
        DigitCommand      = new RelayCommand(OnDigit);
        OperationCommand  = new RelayCommand(OnOperation);
        EqualsCommand     = new RelayCommand(_ => OnEquals());
        ClearCommand      = new RelayCommand(_ => OnClear());
        ClearEntryCommand = new RelayCommand(_ => OnClearEntry());
        BackspaceCommand  = new RelayCommand(_ => OnBackspace());
        NegateCommand     = new RelayCommand(_ => OnNegate());
        PercentCommand    = new RelayCommand(_ => OnPercent());
        DecimalCommand    = new RelayCommand(_ => OnDecimal());
    }

    // ── Command handlers ────────────────────────────────────────────

    private void OnDigit(object? parameter)
    {
        if (parameter is not string digit) return;

        if (_isNewEntry)
        {
            Display = digit;
            _isNewEntry = false;
            _hasDecimalPoint = false;
        }
        else
        {
            if (Display == "0" && digit != "0")
                Display = digit;
            else if (Display != "0")
                Display += digit;
        }

        _engine.SetCurrentValue(ParseDisplay());
    }

    private void OnOperation(object? parameter)
    {
        if (parameter is not string op) return;

        var operation = op switch
        {
            "+"  => CalculatorEngine.Operation.Add,
            "−"  => CalculatorEngine.Operation.Subtract,
            "×"  => CalculatorEngine.Operation.Multiply,
            "÷"  => CalculatorEngine.Operation.Divide,
            _    => throw new ArgumentException($"Unknown operation: {op}")
        };

        _engine.ApplyOperation(operation);
        Display = FormatNumber(_engine.CurrentValue);
        Expression = $"{FormatNumber(_engine.CurrentValue)} {op}";
        _isNewEntry = true;
    }

    private void OnEquals()
    {
        _engine.Evaluate();
        Display = FormatNumber(_engine.CurrentValue);
        Expression = "";
        _isNewEntry = true;
    }

    private void OnClear()
    {
        _engine.Clear();
        Display = "0";
        Expression = "";
        _isNewEntry = true;
        _hasDecimalPoint = false;
    }

    private void OnClearEntry()
    {
        Display = "0";
        _isNewEntry = true;
        _hasDecimalPoint = false;
        _engine.SetCurrentValue(0);
    }

    private void OnBackspace()
    {
        if (_isNewEntry) return;
        if (Display == "0") return;

        // Handle error/special displays
        if (Display.Contains("Oops") || Display == "∞")
        {
            OnClear();
            return;
        }

        if (Display.Length == 1 || (Display.Length == 2 && Display[0] == '-'))
        {
            Display = "0";
            _isNewEntry = true;
            _hasDecimalPoint = false;
        }
        else
        {
            if (Display[^1] == '.') _hasDecimalPoint = false;
            Display = Display[..^1];
        }

        _engine.SetCurrentValue(ParseDisplay());
    }

    private void OnNegate()
    {
        _engine.Negate();
        Display = FormatNumber(_engine.CurrentValue);
    }

    private void OnPercent()
    {
        _engine.Percent();
        Display = FormatNumber(_engine.CurrentValue);
    }

    private void OnDecimal()
    {
        if (_hasDecimalPoint) return;

        if (_isNewEntry)
        {
            Display = "0.";
            _isNewEntry = false;
        }
        else
        {
            Display += ".";
        }

        _hasDecimalPoint = true;
        _engine.SetCurrentValue(ParseDisplay());
    }

    // ── Helpers ─────────────────────────────────────────────────────

    private double ParseDisplay()
    {
        return double.TryParse(Display, NumberStyles.Float, CultureInfo.InvariantCulture, out var v) ? v : 0;
    }

    internal static string FormatNumber(double value)
    {
        if (double.IsNaN(value))      return "Oops! 🤯";
        if (double.IsInfinity(value)) return "∞";

        // Show integers without decimal point
        if (value == Math.Floor(value) && Math.Abs(value) < 1e15)
            return ((long)value).ToString(CultureInfo.InvariantCulture);

        return value.ToString("G10", CultureInfo.InvariantCulture);
    }

    // ── INotifyPropertyChanged ──────────────────────────────────────

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (Equals(field, value)) return;
        field = value;
        OnPropertyChanged(name);
    }
}
