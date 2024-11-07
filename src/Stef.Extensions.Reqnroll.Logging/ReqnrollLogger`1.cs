using Microsoft.Extensions.Logging;
using Reqnroll;

namespace Stef.Extensions.Reqnroll.Logging;

public sealed class ReqnrollLogger<T>(IReqnrollOutputHelper outputHelper, LoggerExternalScopeProvider scopeProvider) :
    ReqnrollLogger(outputHelper, scopeProvider, typeof(T).FullName), ILogger<T>;