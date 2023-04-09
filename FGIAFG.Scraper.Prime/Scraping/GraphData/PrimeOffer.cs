using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping.GraphData
{
    internal class PrimeOffer
    {
        [JsonProperty("id")] public Guid Id { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("assets")] public List<Asset> Assets { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("deliveryMethod")] public string DeliveryMethod { get; set; }
        [JsonProperty("priority")] public long Priority { get; set; }
        [JsonProperty("tags")] public List<Tag> Tags { get; set; }
        [JsonProperty("content")] public Content Content { get; set; }
        [JsonProperty("startTime")] public DateTimeOffset StartTime { get; set; }
        [JsonProperty("endTime")] public DateTimeOffset EndTime { get; set; }
        [JsonProperty("self")] public PrimeOfferSelf Self { get; set; }
        [JsonProperty("linkedJourney")] public LinkedJourney LinkedJourney { get; set; }
        [JsonProperty("__typename")] public string Typename { get; set; }
    }
}
