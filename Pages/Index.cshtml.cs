using VideoPlayer.Models;
using VideoPlayer.Services.Interfaces;

namespace VideoPlayer.Pages
{
    public class IndexModel : HomeModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediaService _mediaService;

        public IndexModel(ILogger<IndexModel> logger, IMediaService mediaService)
        {
            _logger = logger;
            _mediaService = mediaService;
        }

        public async Task OnGet()
        {
            var locatorId = Guid.Parse("2d6c2a0e-5af6-454a-8f3e-5f0f8247170e");
            var streamingLocator = await _mediaService.GetStreamingLocator(locatorId);
            var token = _mediaService.GetToken(streamingLocator.ContentKey);

            Token = token;
            LocatorId = locatorId.ToString();
            VideoUrl = streamingLocator.StreamingUrl;
        }
    }
}