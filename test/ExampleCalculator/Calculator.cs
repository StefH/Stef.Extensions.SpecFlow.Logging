using Microsoft.Extensions.Logging;

namespace ExampleCalculator;

public class Calculator
{
    private readonly ILogger<Calculator> _logger;

    public int FirstNumber { get; set; }
    public int SecondNumber { get; set; }

    public Calculator(ILogger<Calculator> logger)
    {
        _logger = logger;
    }

    public int Add()
    {
        _logger.LogInformation("Adding {FirstNumber} and {SecondNumber}", FirstNumber, SecondNumber);
        return FirstNumber + SecondNumber;
    }
}