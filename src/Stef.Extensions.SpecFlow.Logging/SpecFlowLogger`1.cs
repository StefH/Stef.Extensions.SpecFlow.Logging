using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow.Infrastructure;

namespace Stef.Extensions.SpecFlow.Logging;

public sealed class SpecFlowLogger<T>(ISpecFlowOutputHelper specFlowOutputHelper, LoggerExternalScopeProvider scopeProvider) : 
    SpecFlowLogger(specFlowOutputHelper, scopeProvider, typeof(T).FullName), ILogger<T>;