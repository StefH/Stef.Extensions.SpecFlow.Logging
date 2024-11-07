using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow.Infrastructure;

namespace Stef.Extensions.SpecFlow.Logging;

public sealed class SpecFlowLoggerProvider : ILoggerProvider
{
    private readonly ISpecFlowOutputHelper _outputHelper;
    private readonly SpecFlowLoggerOptions _options;
    private readonly LoggerExternalScopeProvider _scopeProvider = new();

    public SpecFlowLoggerProvider(ISpecFlowOutputHelper outputHelper)
        : this(outputHelper, options: null)
    {
    }

    public SpecFlowLoggerProvider(ISpecFlowOutputHelper outputHelper, bool appendScope)
        : this(outputHelper, new SpecFlowLoggerOptions { IncludeScopes = appendScope })
    {
    }

    public SpecFlowLoggerProvider(ISpecFlowOutputHelper outputHelper, SpecFlowLoggerOptions? options)
    {
        _outputHelper = outputHelper;
        _options = options ?? new SpecFlowLoggerOptions();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new SpecFlowLogger(_outputHelper, _scopeProvider, categoryName, _options);
    }

    public void Dispose()
    {
    }
}