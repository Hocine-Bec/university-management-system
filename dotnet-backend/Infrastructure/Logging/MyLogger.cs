using Applications.Interfaces.Logging;

namespace Infrastructure.Logging
{
    public class MyLogger : IMyLogger
    {
        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "ErrorLogs.txt");

        public void LogError(string message, Exception? ex = null, object? value = null)
        {
            string date = DateTime.Now.ToString("yyyy/MM/dd - HH:mm:ss");
            string logEntry = $"- {{{date}}} [ERROR] {message}";

            if(value != null)
            {
                logEntry += $"- Value: {value}";
            }

            if (ex != null)
            {
                logEntry += $"\n\t - Exception: {ex.Message}\n\n";
            }

            File.AppendAllText(FilePath, logEntry + Environment.NewLine);
        }
    }
}
