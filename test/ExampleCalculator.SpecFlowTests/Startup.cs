using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidToken.SpecFlow.DependencyInjection;
using Stef.Extensions.SpecFlow.Logging;
using TechTalk.SpecFlow.Infrastructure;

namespace ExampleCalculator.SpecFlowTests;

[Binding]
public class Startup
{
    private static ISpecFlowOutputHelper _outputHelper = null!;

    protected Startup()
    {
    }

    [BeforeTestRun]
    public static void BeforeTestRun(ISpecFlowOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        return new ServiceCollection()
            .AddLogging(builder => builder
                .AddProvider(new SpecFlowLoggerProvider(_outputHelper)))
            .AddTransient<Calculator>();
    }
}