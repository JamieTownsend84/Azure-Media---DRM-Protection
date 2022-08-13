using VideoPlayer.Models;

namespace VideoPlayer.Repository.Interfaces
{
    public interface IAzureMediaServicesRepository
    {
        Task<StreamingLocator> GetStreamingLocator(Guid locatorId);
    }
}