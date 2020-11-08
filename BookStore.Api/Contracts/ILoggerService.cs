namespace BookStore.Api.Contracts
{
    public interface ILoggerService
    {
        void LogInfo(string mesagge);

        void LogWarn(string mesagge);

        void LogDebug(string mesagge);

        void LogError(string mesagge);
    }
}
