using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping.GraphData
{
    internal class Data
    {
        [JsonProperty("primeOffers")] public List<PrimeOffer> PrimeOffers { get; set; }
    }
}
