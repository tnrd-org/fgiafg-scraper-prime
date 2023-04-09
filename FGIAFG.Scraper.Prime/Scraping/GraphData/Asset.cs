using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping.GraphData
{
    internal class Asset
    {
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("purpose")] public string Purpose { get; set; }
        [JsonProperty("location")] public string Location { get; set; }
        [JsonProperty("location2x")] public Uri Location2X { get; set; }
        [JsonProperty("__typename")] public string Typename { get; set; }
    }
}
