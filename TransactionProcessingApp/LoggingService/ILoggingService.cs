using System.Diagnostics.CodeAnalysis;
public interface ILoggingService
{
     [ExcludeFromCodeCoverage]
    void LogInfo(string message);
    void LogError(Exception ex, string message);
}
