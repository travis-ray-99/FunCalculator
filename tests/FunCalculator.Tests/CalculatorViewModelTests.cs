using FunCalculator.ViewModels;

namespace FunCalculator.Tests;

public class CalculatorViewModelTests
{
    private readonly CalculatorViewModel _vm = new();

    [Fact]
    public void Initial_DisplayIsZero()
    {
        Assert.Equal("0", _vm.Display);
        Assert.Equal("", _vm.Expression);
    }

    [Fact]
    public void DigitCommand_UpdatesDisplay()
    {
        _vm.DigitCommand.Execute("5");
        Assert.Equal("5", _vm.Display);
    }

    [Fact]
    public void MultipleDigits_BuildNumber()
    {
        _vm.DigitCommand.Execute("1");
        _vm.DigitCommand.Execute("2");
        _vm.DigitCommand.Execute("3");
        Assert.Equal("123", _vm.Display);
    }

    [Fact]
    public void LeadingZero_IsReplaced()
    {
        _vm.DigitCommand.Execute("0");
        _vm.DigitCommand.Execute("5");
        Assert.Equal("5", _vm.Display);
    }

    [Fact]
    public void DecimalCommand_AddsDecimalPoint()
    {
        _vm.DigitCommand.Execute("3");
        _vm.DecimalCommand.Execute(null);
        _vm.DigitCommand.Execute("1");
        _vm.DigitCommand.Execute("4");
        Assert.Equal("3.14", _vm.Display);
    }

    [Fact]
    public void DecimalCommand_OnNewEntry_ShowsZeroDot()
    {
        _vm.DecimalCommand.Execute(null);
        Assert.Equal("0.", _vm.Display);
    }

    [Fact]
    public void DecimalCommand_MultiplePresses_OnlyOneDecimal()
    {
        _vm.DigitCommand.Execute("1");
        _vm.DecimalCommand.Execute(null);
        _vm.DecimalCommand.Execute(null);
        _vm.DigitCommand.Execute("5");
        Assert.Equal("1.5", _vm.Display);
    }

    [Fact]
    public void SimpleAddition()
    {
        _vm.DigitCommand.Execute("3");
        _vm.OperationCommand.Execute("+");
        _vm.DigitCommand.Execute("4");
        _vm.EqualsCommand.Execute(null);

        Assert.Equal("7", _vm.Display);
    }

    [Fact]
    public void SimpleSubtraction()
    {
        _vm.DigitCommand.Execute("9");
        _vm.OperationCommand.Execute("−");
        _vm.DigitCommand.Execute("4");
        _vm.EqualsCommand.Execute(null);

        Assert.Equal("5", _vm.Display);
    }

    [Fact]
    public void DivideByZero_ShowsOops()
    {
        _vm.DigitCommand.Execute("5");
        _vm.OperationCommand.Execute("÷");
        _vm.DigitCommand.Execute("0");
        _vm.EqualsCommand.Execute(null);

        Assert.Contains("Oops", _vm.Display);
    }

    [Fact]
    public void ClearCommand_ResetsDisplay()
    {
        _vm.DigitCommand.Execute("9");
        _vm.DigitCommand.Execute("9");
        _vm.ClearCommand.Execute(null);

        Assert.Equal("0", _vm.Display);
        Assert.Equal("", _vm.Expression);
    }

    [Fact]
    public void ClearEntryCommand_ResetsDisplayButKeepsPendingOperation()
    {
        _vm.DigitCommand.Execute("5");
        _vm.OperationCommand.Execute("+");
        _vm.DigitCommand.Execute("3");
        _vm.ClearEntryCommand.Execute(null);

        Assert.Equal("0", _vm.Display);
        // Expression should still be set from the operation
        Assert.Equal("5 +", _vm.Expression);

        // Continue with a new number and evaluate
        _vm.DigitCommand.Execute("7");
        _vm.EqualsCommand.Execute(null);
        Assert.Equal("12", _vm.Display);
    }

    [Fact]
    public void BackspaceCommand_RemovesLastDigit()
    {
        _vm.DigitCommand.Execute("1");
        _vm.DigitCommand.Execute("2");
        _vm.DigitCommand.Execute("3");
        _vm.BackspaceCommand.Execute(null);

        Assert.Equal("12", _vm.Display);
    }

    [Fact]
    public void BackspaceCommand_SingleDigit_ResetsToZero()
    {
        _vm.DigitCommand.Execute("5");
        _vm.BackspaceCommand.Execute(null);

        Assert.Equal("0", _vm.Display);
    }

    [Fact]
    public void BackspaceCommand_RemovesDecimalPoint()
    {
        _vm.DigitCommand.Execute("3");
        _vm.DecimalCommand.Execute(null);
        _vm.BackspaceCommand.Execute(null);

        Assert.Equal("3", _vm.Display);

        // Should be able to add decimal again after removing it
        _vm.DecimalCommand.Execute(null);
        _vm.DigitCommand.Execute("5");
        Assert.Equal("3.5", _vm.Display);
    }

    [Fact]
    public void BackspaceCommand_OnNewEntry_DoesNothing()
    {
        // After equals, display is in "new entry" state
        _vm.DigitCommand.Execute("5");
        _vm.OperationCommand.Execute("+");
        _vm.DigitCommand.Execute("3");
        _vm.EqualsCommand.Execute(null);

        _vm.BackspaceCommand.Execute(null);
        Assert.Equal("8", _vm.Display);
    }

    [Fact]
    public void NegateCommand_FlipsSign()
    {
        _vm.DigitCommand.Execute("5");
        _vm.NegateCommand.Execute(null);

        Assert.Equal("-5", _vm.Display);
    }

    [Fact]
    public void PercentCommand_DividesByHundred()
    {
        _vm.DigitCommand.Execute("5");
        _vm.DigitCommand.Execute("0");
        _vm.PercentCommand.Execute(null);

        Assert.Equal("0.5", _vm.Display);
    }

    [Fact]
    public void OperationCommand_SetsExpression()
    {
        _vm.DigitCommand.Execute("7");
        _vm.OperationCommand.Execute("+");

        Assert.Equal("7 +", _vm.Expression);
    }

    [Fact]
    public void EqualsCommand_ClearsExpression()
    {
        _vm.DigitCommand.Execute("2");
        _vm.OperationCommand.Execute("+");
        _vm.DigitCommand.Execute("3");
        _vm.EqualsCommand.Execute(null);

        Assert.Equal("", _vm.Expression);
    }

    [Theory]
    [InlineData(double.NaN, "Oops! 🤯")]
    [InlineData(double.PositiveInfinity, "∞")]
    [InlineData(42.0, "42")]
    [InlineData(3.14, "3.14")]
    [InlineData(-7.0, "-7")]
    public void FormatNumber_ReturnsExpectedStrings(double input, string expected)
    {
        Assert.Equal(expected, CalculatorViewModel.FormatNumber(input));
    }
}
