using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping.GraphData
{
    internal class Content
    {
        [JsonProperty("externalURL")] public Uri ExternalUrl { get; set; }
        [JsonProperty("publisher")] public string Publisher { get; set; }
        [JsonProperty("categories")] public List<string> Categories { get; set; }
        [JsonProperty("__typename")] public string Typename { get; set; }
    }
}
