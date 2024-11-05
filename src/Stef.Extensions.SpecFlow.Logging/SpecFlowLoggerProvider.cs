using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow.Infrastructure;

namespace Stef.Extensions.SpecFlow.Logging;

public sealed class SpecFlowLoggerProvider : ILoggerProvider
{
    private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
    private readonly SpecFlowLoggerOptions _options;
    private readonly LoggerExternalScopeProvider _scopeProvider = new();

    public SpecFlowLoggerProvider(ISpecFlowOutputHelper specFlowOutputHelper)
        : this(specFlowOutputHelper, options: null)
    {
    }

    public SpecFlowLoggerProvider(ISpecFlowOutputHelper specFlowOutputHelper, bool appendScope)
        : this(specFlowOutputHelper, new SpecFlowLoggerOptions { IncludeScopes = appendScope })
    {
    }

    public SpecFlowLoggerProvider(ISpecFlowOutputHelper specFlowOutputHelper, SpecFlowLoggerOptions? options)
    {
        _specFlowOutputHelper = specFlowOutputHelper;
        _options = options ?? new SpecFlowLoggerOptions();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new SpecFlowLogger(_specFlowOutputHelper, _scopeProvider, categoryName, _options);
    }

    public void Dispose()
    {
    }
}