using Microsoft.Azure.Management.Media;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using VideoPlayer.Models;
using VideoPlayer.Repository.Interfaces;

namespace VideoPlayer.Repository
{
    public class AzureMediaServicesRepository : IAzureMediaServicesRepository
    {
        const string DefaultStreamingEndpointName = "default";
        private readonly AzureMediaServicesConfiguration _amsConfiguration;

        public AzureMediaServicesRepository(AzureMediaServicesConfiguration amsConfiguration)
        {
            _amsConfiguration = amsConfiguration;
        }

        public async Task<StreamingLocator> GetStreamingLocator(Guid locatorId)
        {
            var client = await CreateClient();
            var allLocators = await client.StreamingLocators.ListAsync(_amsConfiguration.ResourceGroup, _amsConfiguration.AccountName);
            var locator = allLocators.FirstOrDefault(x => x.StreamingLocatorId == locatorId);

            if (locator == null)
            {
                return new StreamingLocator();
            }

            var contentKeys = await client.StreamingLocators.ListContentKeysAsync(_amsConfiguration.ResourceGroup,
                _amsConfiguration.AccountName, locator.Name);
            var contentKey = contentKeys.ContentKeys.FirstOrDefault()?.Value;
            var streamingUrls = new List<string>();
            var streamingEndpoint = await client.StreamingEndpoints.GetAsync(_amsConfiguration.ResourceGroup, _amsConfiguration.AccountName, DefaultStreamingEndpointName);
            var paths = await client.StreamingLocators.ListPathsAsync(_amsConfiguration.ResourceGroup, _amsConfiguration.AccountName, locator.Name);

            foreach (var path in paths.StreamingPaths)
            {
                var uriBuilder = new UriBuilder
                {
                    Scheme = "https",
                    Host = streamingEndpoint.HostName,
                    Path = path.Paths[0]
                };

                streamingUrls.Add(uriBuilder.ToString());
            }

            return new StreamingLocator()
            {
                Id = locatorId.ToString(),
                AssetName = locator.AssetName,
                StreamingUrl = streamingUrls.First(),
                ContentKey = contentKey ?? string.Empty
            };
        }

        private async Task<AzureMediaServicesClient> CreateClient()
        {
            var credentials = await GetCredentialsAsync();

            return new AzureMediaServicesClient(new Uri(_amsConfiguration.ArmEndpoint), credentials)
            {
                SubscriptionId = _amsConfiguration.SubscriptionId,
            };
        }

        private async Task<ServiceClientCredentials> GetCredentialsAsync()
        {
            var clientCredential = new ClientCredential(_amsConfiguration.AadClientId, _amsConfiguration.AadSecret);

            return await ApplicationTokenProvider.LoginSilentAsync(_amsConfiguration.AadTenantDomain, clientCredential, ActiveDirectoryServiceSettings.Azure);
        }
    }
}