using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.IdentityModel.Tokens;
using VideoPlayer.Models;
using VideoPlayer.Repository.Interfaces;
using VideoPlayer.Services.Interfaces;

namespace VideoPlayer.Services
{
    public class MediaService : IMediaService
    {
        private readonly IAzureMediaServicesRepository _azureMediaServicesRepository;
        private readonly AzureMediaServicesConfiguration _azureMediaServicesConfiguration;

        public MediaService(IAzureMediaServicesRepository azureMediaServicesRepository, AzureMediaServicesConfiguration azureMediaServicesConfiguration)
        {
            _azureMediaServicesRepository = azureMediaServicesRepository;
            _azureMediaServicesConfiguration = azureMediaServicesConfiguration;
        }

        public async Task<Models.StreamingLocator> GetStreamingLocator(Guid locatorId)
        {
            return await _azureMediaServicesRepository.GetStreamingLocator(locatorId);
        }

        public string GetToken(string streamingLocatorContentKey)
        {
            var tokenSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_azureMediaServicesConfiguration.PrimaryVerificationKey));
            var cred = new SigningCredentials(tokenSigningKey, SecurityAlgorithms.HmacSha256, SecurityAlgorithms.Sha256Digest);

            var token = new JwtSecurityToken(
                issuer: $"https://login.microsoftonline.com/{_azureMediaServicesConfiguration.AadTenantId}/v2.0",
                audience: _azureMediaServicesConfiguration.AudienceId,
                claims: new[]
                {
                    new Claim(ContentKeyPolicyTokenClaim.ContentKeyIdentifierClaim.ClaimType, streamingLocatorContentKey),
                },
                notBefore: DateTime.Now.AddMinutes(-5),
                expires: DateTime.Now.AddMinutes(240),
                signingCredentials: cred
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}