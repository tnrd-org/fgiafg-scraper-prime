using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping.GraphData
{
    internal class PrimeOfferSelf
    {
        [JsonProperty("hasEntitlement")] public bool HasEntitlement { get; set; }
        [JsonProperty("claimData")] public object ClaimData { get; set; }
        [JsonProperty("claimInstructions")] public string ClaimInstructions { get; set; }
        [JsonProperty("__typename")] public string Typename { get; set; }
    }
}
