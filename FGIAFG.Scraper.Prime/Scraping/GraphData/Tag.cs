using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping.GraphData
{
    internal class Tag
    {
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("tag")] public string TagTag { get; set; }
        [JsonProperty("__typename")] public string Typename { get; set; }
    }
}
