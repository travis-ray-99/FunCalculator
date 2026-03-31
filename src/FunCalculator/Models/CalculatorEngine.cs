using System;

namespace FunCalculator.Models;

/// <summary>
/// Pure arithmetic engine for the calculator — no UI concerns.
/// </summary>
public sealed class CalculatorEngine
{
    public enum Operation { Add, Subtract, Multiply, Divide }

    public double CurrentValue { get; private set; }

    private double? _storedValue;
    private Operation? _pendingOperation;

    public void SetCurrentValue(double value)
    {
        CurrentValue = value;
    }

    public void ApplyOperation(Operation operation)
    {
        if (_pendingOperation.HasValue && _storedValue.HasValue)
        {
            Evaluate();
        }

        _storedValue = CurrentValue;
        _pendingOperation = operation;
    }

    public void Evaluate()
    {
        if (!_pendingOperation.HasValue || !_storedValue.HasValue)
            return;

        CurrentValue = _pendingOperation.Value switch
        {
            Operation.Add      => _storedValue.Value + CurrentValue,
            Operation.Subtract => _storedValue.Value - CurrentValue,
            Operation.Multiply => _storedValue.Value * CurrentValue,
            Operation.Divide when CurrentValue != 0 => _storedValue.Value / CurrentValue,
            Operation.Divide   => double.NaN,
            _ => CurrentValue
        };

        _storedValue = null;
        _pendingOperation = null;
    }

    public void Clear()
    {
        CurrentValue = 0;
        _storedValue = null;
        _pendingOperation = null;
    }

    public void Negate()
    {
        CurrentValue = -CurrentValue;
    }

    public void Percent()
    {
        CurrentValue /= 100.0;
    }
}
