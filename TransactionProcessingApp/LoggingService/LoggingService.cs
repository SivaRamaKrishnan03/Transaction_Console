using NLog;
using System.Diagnostics.CodeAnalysis;
 [ExcludeFromCodeCoverage]
public class LoggingService : ILoggingService
{
    
     
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
 [ExcludeFromCodeCoverage]
    public void LogInfo(string message)
    {
        Logger.Info(message);
    }
 [ExcludeFromCodeCoverage]
    public void LogError(Exception ex, string message)
    {
        Logger.Error(ex, message);
    }
}
