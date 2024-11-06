using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using Stef.Extensions.Reqnroll.Logging;

namespace ExampleCalculator.ReqnrollTests;

[Binding]
public class Startup
{
    private static IReqnrollOutputHelper? _outputHelper;

    protected Startup()
    {
    }

    [BeforeTestRun]
    public static void BeforeTestRun(ITestRunnerManager testRunnerManager)
    {
        if (_outputHelper != null)
        {
            return;
        }

        var runner = testRunnerManager.GetTestRunner();
        var context = runner.TestThreadContext;
        _outputHelper = context.TestThreadContainer.Resolve<IReqnrollOutputHelper>();
        testRunnerManager.ReleaseTestThreadContext(context);
    }

    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        return new ServiceCollection()
            .AddLogging(builder => builder
                .AddProvider(new ReqnrollLoggerProvider(_outputHelper!)))
            .AddTransient<Calculator>();
    }
}