using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping.GraphData
{
    internal class LinkedJourney
    {
        [JsonProperty("self")] public LinkedJourneySelf Self { get; set; }
        [JsonProperty("offers")] public List<Offer> Offers { get; set; }
        [JsonProperty("__typename")] public string Typename { get; set; }
    }
}
