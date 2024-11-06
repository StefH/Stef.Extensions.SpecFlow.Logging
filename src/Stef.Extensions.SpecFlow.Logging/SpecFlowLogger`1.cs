using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow.Infrastructure;

namespace Stef.Extensions.SpecFlow.Logging;

public sealed class SpecFlowLogger<T>(ISpecFlowOutputHelper outputHelper, LoggerExternalScopeProvider scopeProvider) : 
    SpecFlowLogger(outputHelper, scopeProvider, typeof(T).FullName), ILogger<T>;