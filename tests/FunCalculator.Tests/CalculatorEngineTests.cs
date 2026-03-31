using FunCalculator.Models;

namespace FunCalculator.Tests;

public class CalculatorEngineTests
{
    private readonly CalculatorEngine _engine = new();

    // ── Basic arithmetic ────────────────────────────────────────────

    [Fact]
    public void Add_TwoNumbers_ReturnsSum()
    {
        _engine.SetCurrentValue(3);
        _engine.ApplyOperation(CalculatorEngine.Operation.Add);
        _engine.SetCurrentValue(4);
        _engine.Evaluate();

        Assert.Equal(7, _engine.CurrentValue);
    }

    [Fact]
    public void Subtract_TwoNumbers_ReturnsDifference()
    {
        _engine.SetCurrentValue(10);
        _engine.ApplyOperation(CalculatorEngine.Operation.Subtract);
        _engine.SetCurrentValue(3);
        _engine.Evaluate();

        Assert.Equal(7, _engine.CurrentValue);
    }

    [Fact]
    public void Multiply_TwoNumbers_ReturnsProduct()
    {
        _engine.SetCurrentValue(6);
        _engine.ApplyOperation(CalculatorEngine.Operation.Multiply);
        _engine.SetCurrentValue(7);
        _engine.Evaluate();

        Assert.Equal(42, _engine.CurrentValue);
    }

    [Fact]
    public void Divide_TwoNumbers_ReturnsQuotient()
    {
        _engine.SetCurrentValue(20);
        _engine.ApplyOperation(CalculatorEngine.Operation.Divide);
        _engine.SetCurrentValue(4);
        _engine.Evaluate();

        Assert.Equal(5, _engine.CurrentValue);
    }

    // ── Edge cases ──────────────────────────────────────────────────

    [Fact]
    public void Divide_ByZero_ReturnsNaN()
    {
        _engine.SetCurrentValue(10);
        _engine.ApplyOperation(CalculatorEngine.Operation.Divide);
        _engine.SetCurrentValue(0);
        _engine.Evaluate();

        Assert.True(double.IsNaN(_engine.CurrentValue));
    }

    [Fact]
    public void Evaluate_WithNoPendingOperation_DoesNothing()
    {
        _engine.SetCurrentValue(42);
        _engine.Evaluate();

        Assert.Equal(42, _engine.CurrentValue);
    }

    // ── Chained operations ──────────────────────────────────────────

    [Fact]
    public void ChainedOperations_EvaluatesPendingBeforeNewOp()
    {
        // 2 + 3 × → should compute 2+3=5 first, then prepare ×
        _engine.SetCurrentValue(2);
        _engine.ApplyOperation(CalculatorEngine.Operation.Add);
        _engine.SetCurrentValue(3);
        _engine.ApplyOperation(CalculatorEngine.Operation.Multiply);

        Assert.Equal(5, _engine.CurrentValue);

        _engine.SetCurrentValue(4);
        _engine.Evaluate();

        Assert.Equal(20, _engine.CurrentValue); // 5 × 4
    }

    // ── Clear ───────────────────────────────────────────────────────

    [Fact]
    public void Clear_ResetsEverything()
    {
        _engine.SetCurrentValue(99);
        _engine.ApplyOperation(CalculatorEngine.Operation.Add);
        _engine.SetCurrentValue(1);
        _engine.Clear();

        Assert.Equal(0, _engine.CurrentValue);

        // Evaluate after clear should be a no-op
        _engine.Evaluate();
        Assert.Equal(0, _engine.CurrentValue);
    }

    // ── Negate & Percent ────────────────────────────────────────────

    [Fact]
    public void Negate_FlipsSign()
    {
        _engine.SetCurrentValue(5);
        _engine.Negate();
        Assert.Equal(-5, _engine.CurrentValue);

        _engine.Negate();
        Assert.Equal(5, _engine.CurrentValue);
    }

    [Fact]
    public void Negate_Zero_StaysZero()
    {
        _engine.SetCurrentValue(0);
        _engine.Negate();
        Assert.Equal(0, _engine.CurrentValue);
    }

    [Fact]
    public void Percent_DividesByHundred()
    {
        _engine.SetCurrentValue(50);
        _engine.Percent();
        Assert.Equal(0.5, _engine.CurrentValue);
    }

    // ── Decimal handling ────────────────────────────────────────────

    [Fact]
    public void SetCurrentValue_Decimal_WorksCorrectly()
    {
        _engine.SetCurrentValue(3.14);
        Assert.Equal(3.14, _engine.CurrentValue, precision: 10);
    }

    [Fact]
    public void Add_DecimalNumbers_ReturnsCorrectSum()
    {
        _engine.SetCurrentValue(1.5);
        _engine.ApplyOperation(CalculatorEngine.Operation.Add);
        _engine.SetCurrentValue(2.3);
        _engine.Evaluate();

        Assert.Equal(3.8, _engine.CurrentValue, precision: 10);
    }

    // ── Large numbers ───────────────────────────────────────────────

    [Fact]
    public void Multiply_LargeNumbers_ReturnsCorrectProduct()
    {
        _engine.SetCurrentValue(1_000_000);
        _engine.ApplyOperation(CalculatorEngine.Operation.Multiply);
        _engine.SetCurrentValue(1_000_000);
        _engine.Evaluate();

        Assert.Equal(1_000_000_000_000, _engine.CurrentValue);
    }
}
