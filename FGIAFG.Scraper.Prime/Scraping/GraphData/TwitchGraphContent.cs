using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping.GraphData
{
    internal class TwitchGraphContent
    {
        [JsonProperty("data")] public Data Data { get; set; }
    }
}
