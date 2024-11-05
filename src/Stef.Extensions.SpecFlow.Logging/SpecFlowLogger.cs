using System.Text;
using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow.Infrastructure;

namespace Stef.Extensions.SpecFlow.Logging;

public class SpecFlowLogger : ILogger
{
    private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
    private readonly string? _categoryName;
    private readonly SpecFlowLoggerOptions _options;
    private readonly LoggerExternalScopeProvider _scopeProvider;

    public static ILogger CreateLogger(ISpecFlowOutputHelper testOutputHelper) => new SpecFlowLogger(testOutputHelper, new LoggerExternalScopeProvider(), string.Empty);
    public static ILogger<T> CreateLogger<T>(ISpecFlowOutputHelper testOutputHelper) => new SpecFlowLogger<T>(testOutputHelper, new LoggerExternalScopeProvider());

    public SpecFlowLogger(ISpecFlowOutputHelper specFlowOutputHelper, LoggerExternalScopeProvider scopeProvider, string? categoryName)
        : this(specFlowOutputHelper, scopeProvider, categoryName, appendScope: true)
    {
    }

    public SpecFlowLogger(ISpecFlowOutputHelper specFlowOutputHelper, LoggerExternalScopeProvider scopeProvider, string? categoryName, bool appendScope)
        : this(specFlowOutputHelper, scopeProvider, categoryName, options: new SpecFlowLoggerOptions { IncludeScopes = appendScope })
    {
    }

    public SpecFlowLogger(ISpecFlowOutputHelper specFlowOutputHelper, LoggerExternalScopeProvider scopeProvider, string? categoryName, SpecFlowLoggerOptions? options)
    {
        _specFlowOutputHelper = specFlowOutputHelper;
        _scopeProvider = scopeProvider;
        _categoryName = categoryName;
        _options = options ?? new();
    }

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _scopeProvider.Push(state);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var sb = new StringBuilder();

        if (_options.TimestampFormat is not null)
        {
            var now = _options.UseUtcTimestamp ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
            var timestamp = now.ToString(_options.TimestampFormat);
            sb.Append(timestamp).Append(' ');
        }

        if (_options.IncludeLogLevel)
        {
            sb.Append(GetLogLevelString(logLevel)).Append(' ');
        }

        if (_options.IncludeCategory)
        {
            sb.Append('[').Append(_categoryName).Append("] ");
        }

        sb.Append(formatter(state, exception));

        if (exception is not null)
        {
            sb.Append('\n').Append(exception);
        }

        // Append scopes
        if (_options.IncludeScopes)
        {
            _scopeProvider.ForEachScope((scope, st) =>
            {
                st.Append("\n => ");
                st.Append(scope);
            }, sb);
        }

        try
        {
            _specFlowOutputHelper.WriteLine(sb.ToString());
        }
        catch
        {
            // This can happen when the test is not active
        }
    }

    private static string GetLogLevelString(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => nameof(LogLevel.Trace),
            LogLevel.Debug => nameof(LogLevel.Debug),
            LogLevel.Information => nameof(LogLevel.Information),
            LogLevel.Warning => nameof(LogLevel.Warning),
            LogLevel.Error => nameof(LogLevel.Error),
            LogLevel.Critical => nameof(LogLevel.Critical),
            LogLevel.None => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
        };
    }
}