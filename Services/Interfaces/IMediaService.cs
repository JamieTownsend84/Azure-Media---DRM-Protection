using VideoPlayer.Models;

namespace VideoPlayer.Services.Interfaces
{
    public interface IMediaService
    {
        Task<StreamingLocator> GetStreamingLocator(Guid locatorId);
        string GetToken(string streamingLocatorContentKey);
    }
}