using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping.GraphData
{
    internal class LinkedJourneySelf
    {
        [JsonProperty("enrollmentStatus")] public string EnrollmentStatus { get; set; }
        [JsonProperty("__typename")] public string Typename { get; set; }
    }
}
