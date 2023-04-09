using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping.GraphData
{
    internal class OfferSelf
    {
        [JsonProperty("claimStatus")] public string ClaimStatus { get; set; }
        [JsonProperty("__typename")] public string Typename { get; set; }
    }
}
