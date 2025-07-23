namespace Applications.Interfaces.Logging
{
    public interface IMyLogger
    {
        public void LogError(string message, Exception? ex = null, object? value = null);
    }
}
