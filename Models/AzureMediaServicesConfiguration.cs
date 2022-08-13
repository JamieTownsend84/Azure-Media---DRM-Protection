namespace VideoPlayer.Models
{
    public class AzureMediaServicesConfiguration
    {
        public string AadClientId { get; set; }
        public string AadSecret { get; set; }
        public string AadTenantDomain { get; set; }
        public string AadTenantId { get; set; }
        public string AudienceId { get; set; }
        public string AccountName { get; set; }
        public string ResourceGroup { get; set; }
        public string SubscriptionId { get; set; }
        public string ArmAadAudience { get; set; }
        public string ArmEndpoint { get; set; }
        public string PrimaryVerificationKey { get; set; }
    }
}