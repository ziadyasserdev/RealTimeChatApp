namespace RealTimeChatApp.Api.Jobs
{
    public interface IStoryExpirationJob
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
