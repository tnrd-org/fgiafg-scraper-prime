using System.Net;
using System.Net.Http.Headers;

namespace FGIAFG.Scraper.Prime;

internal static class Extensions
{
    public static IServiceCollection AddCustomHttpClient(this IServiceCollection collection)
    {
        collection.AddHttpClient("twitch",
            (provider, client) =>
            {
                client.BaseAddress = new Uri("https://gaming.amazon.com");
                client.DefaultRequestHeaders.Accept.TryParseAdd("*/*");
                client.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("gzip");
                client.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("deflate");
                client.DefaultRequestHeaders.UserAgent.Add(
                    new ProductInfoHeaderValue("FreeGameIsAFreeGame", "1.0.0.0"));
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        });

        return collection;
    }
}
