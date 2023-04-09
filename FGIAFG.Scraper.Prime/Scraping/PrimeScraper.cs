using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using FGIAFG.Scraper.Prime.Scraping.GraphData;
using FluentResults;
using Newtonsoft.Json;

namespace FGIAFG.Scraper.Prime.Scraping;

internal class PrimeScraper
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<PrimeScraper> logger;

    public PrimeScraper(IHttpClientFactory httpClientFactory, ILogger<PrimeScraper> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
    }

    public async Task<Result<IEnumerable<FreeGame>>> Scrape(CancellationToken cancellationToken)
    {
        Result<string> csrfTokenResult = await GetCsrfToken(cancellationToken);
        if (csrfTokenResult.IsFailed)
            return csrfTokenResult.ToResult();

        Result<TwitchGraphContent> twitchGraphContentResult =
            await GetTwitchGraphContent(csrfTokenResult.Value, cancellationToken);
        if (twitchGraphContentResult.IsFailed)
            return twitchGraphContentResult.ToResult();

        List<FreeGame> freeGames = new();
        List<PrimeOffer> primeOffers = GetPrimeOffers(twitchGraphContentResult.Value);

        foreach (PrimeOffer primeOffer in primeOffers)
        {
            FreeGame freeGame = new FreeGame(primeOffer.Title,
                GetImageUrl(primeOffer),
                primeOffer.Content.ExternalUrl?.ToString() ?? "https://gaming.amazon.com/home",
                primeOffer.StartTime.UtcDateTime,
                primeOffer.EndTime.UtcDateTime);

            freeGames.Add(freeGame);
        }

        return Result.Ok((IEnumerable<FreeGame>)freeGames);
    }

    private async Task<Result<string>> GetCsrfToken(CancellationToken cancellationToken)
    {
        HttpClient httpClient = httpClientFactory.CreateClient("twitch");
        HttpResponseMessage response = await httpClient.GetAsync("tp/loot", cancellationToken);

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            return Result.Fail(new ExceptionalError("Unable to get CSRF token", e));
        }

        string content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!Regex.IsMatch(content, "(<input).+(csrf-key).+(/>)"))
        {
            return Result.Fail("Unable to find CSRF token in response");
        }

        Match match = Regex.Match(content, "(<input).+(csrf-key).+(/>)");

        int lastIndex = match.Value.LastIndexOf("'", StringComparison.Ordinal);
        int previousIndex = match.Value.LastIndexOf("'", lastIndex - 1, StringComparison.Ordinal);

        return Result.Ok(match.Value.Substring(previousIndex + 1, lastIndex - previousIndex - 1));
    }

    private async Task<Result<TwitchGraphContent>> GetTwitchGraphContent(
        string csrfToken,
        CancellationToken cancellationToken
    )
    {
        const string graphQuery =
            "{\"operationName\":\"Prime_OfferList_Offers\",\"variables\":{},\"query\":\"query Prime_OfferList_Offers($dateOverride: Time) {\\n  primeOffers(dateOverride: $dateOverride) {\\n    ...Offer\\n    __typename\\n  }\\n}\\n\\nfragment Offer on PrimeOffer {\\n  id\\n  title\\n  assets {\\n    type\\n    purpose\\n    location\\n    location2x\\n    __typename\\n  }\\n  description\\n  deliveryMethod\\n  priority\\n  tags {\\n    type\\n    tag\\n    __typename\\n  }\\n  content {\\n    externalURL\\n    publisher\\n    categories\\n    __typename\\n  }\\n  startTime\\n  endTime\\n  self {\\n    hasEntitlement\\n    claimData\\n    claimInstructions\\n    __typename\\n  }\\n  linkedJourney {\\n    ...Journey\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment Journey on Journey {\\n  self {\\n    enrollmentStatus\\n    __typename\\n  }\\n  offers {\\n    self {\\n      claimStatus\\n      __typename\\n    }\\n    __typename\\n  }\\n  __typename\\n}\\n\",\"extensions\":{}}";

        HttpClient httpClient = httpClientFactory.CreateClient("twitch");

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "graphql");
        request.Headers.Add("csrf-token", csrfToken);
        request.Headers.Accept.TryParseAdd("application/json");
        request.Content = new StringContent(graphQuery);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            return Result.Fail(new ExceptionalError(e));
        }

        string json = await response.Content.ReadAsStringAsync(cancellationToken);
        TwitchGraphContent twitchGraphContent = null;

        try
        {
            twitchGraphContent = JsonConvert.DeserializeObject<TwitchGraphContent>(json);
        }
        catch (JsonSerializationException e)
        {
            return Result.Fail(new ExceptionalError(e));
        }

        return Result.Ok(twitchGraphContent);
    }

    private List<PrimeOffer> GetPrimeOffers(TwitchGraphContent twitchGraphContent)
    {
        if (twitchGraphContent.Data?.PrimeOffers == null)
            return new List<PrimeOffer>();

        return twitchGraphContent.Data.PrimeOffers
            .Where(x => x.Tags.Any(y => y.TagTag == "FGWP"))
            .ToList();
    }

    private string GetImageUrl(PrimeOffer offer)
    {
        Asset? result = offer.Assets.FirstOrDefault(x => x.Purpose == "DETAIL");
        if (result != null)
            return result.Location2X.ToString();

        result = offer.Assets.FirstOrDefault(x => x.Purpose == "THUMBNAIL");
        if (result != null)
            return result.Location2X.ToString();

        result = offer.Assets.FirstOrDefault(x => x.Purpose == "ICON");
        if (result != null)
            return result.Location2X.ToString();

        return
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/ce/Twitch_logo_2019.svg/512px-Twitch_logo_2019.svg.png";
    }
}
