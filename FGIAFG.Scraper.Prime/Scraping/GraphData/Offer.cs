using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping.GraphData
{
    internal class Offer
    {
        [JsonProperty("self")] public OfferSelf Self { get; set; }
        [JsonProperty("__typename")] public string Typename { get; set; }
    }
}
