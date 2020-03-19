using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebApi.Data.Models;
using WebApi.Models;
using WebApi.Models.BibleApi;

namespace WebApi.Services
{
    public class BibleApiVerseLookupService : IVerseLookupService
    {
        // HTML tags and characters included in Bible API responses that need stripped out
        private static Dictionary<string, string> NormalizationReplacements { get; } = new Dictionary<string, string>
        {
            { @"<span data-number.*?>\d+<\/span>", string.Empty }, // Included first to make sure that verse numbers are stripped out with their <span>
            { @"(<p.*?>|<\/p>|<span.*?>|<\/span>|¶\s*)", string.Empty }
        };

        private static Dictionary<Translations, string> TranslationIds { get; } = new Dictionary<Translations, string>
        {
            { Translations.KJV, "de4e12af7f28f599-02" },
            { Translations.ASV, "06125adad2d5898a-01" },
            { Translations.RSV, "40072c4a5aba4022-01" },
            { Translations.ESV, "f421fe261da7624f-01" }
        };

        private const int VerseLookupChunkSize = 50;
        internal const string BibleApiHttpClientName = "BibleApiHttpClient";

        private HttpClient ApiClient { get; }

        private ILogger<BibleApiVerseLookupService> Logger { get; }

        public BibleApiVerseLookupService(IHttpClientFactory clientFactory, ILogger<BibleApiVerseLookupService> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ApiClient = clientFactory?.CreateClient(BibleApiHttpClientName) ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<IEnumerable<Verse>> LookupVersesAsync(IEnumerable<VerseReference> references, Translations translation)
        {
            if (references is null)
            {
                Logger.LogWarning("Null references when calling LookupVersesAsync");
                throw new ArgumentNullException(nameof(references));
            }

            var verses = new List<Verse>();

            // Limit API requests to looking up 50 verses at a time
            var referencesArray = references.ToArray();
            var referenceSpan = new ReadOnlyMemory<string>(referencesArray.Select(r => $"\"{r}\"").ToArray());
            for (var i = 0; i < referenceSpan.Length; i += VerseLookupChunkSize)
            {
                var requestPath = BuildRequestPath(translation, referenceSpan.Length - i > VerseLookupChunkSize ? referenceSpan.Span.Slice(i, VerseLookupChunkSize) : referenceSpan.Span.Slice(i));

                Logger.LogInformation("Requesting verses from {Url}", requestPath);
                using var response = await ApiClient.GetAsync(requestPath);
                if (response.IsSuccessStatusCode)
                {
                    using var contentStream = await response.Content.ReadAsStreamAsync();
                    var index = 0;
                    var responseData = await JsonSerializer.DeserializeAsync<BibleApiResponse>(contentStream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    if (responseData?.Data?.Passages == null)
                    {
                        Logger.LogWarning("Failed to read response content");
                    }
                    else
                    {
                        Logger.LogInformation("Received {VerseCount} verses in response", responseData.Data.Passages.Length);
                        foreach (var verseText in responseData.Data.Passages.Select(p => NormalizeText(p.Content)))
                        {
                            if (verseText is null)
                            {
                                Logger.LogInformation("Skipping null verse content for verse {VerseReference}", referencesArray[(i * VerseLookupChunkSize) + index]);
                                index++;
                                continue;
                            }

                            var reference = referencesArray[i + index++];
                            verses.Add(new Verse(reference.Book, reference.Chapter, reference.Verse, translation, verseText));
                        }
                    }
                }
                else
                {
                    Logger.LogError("Verse request failed: {StatusCode} {ErrorMessage}", response.StatusCode, await response.Content.ReadAsStringAsync());
                }
            }

            Logger.LogInformation("Returning {VerseCount} retrieved verses", verses.Count);
            return verses;
        }

        private string? NormalizeText(string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return content;
            }

            foreach (var regex in NormalizationReplacements.Keys)
            {
                content = Regex.Replace(content, regex, NormalizationReplacements[regex]);
            }

            return content.Trim();
        }

        private Uri BuildRequestPath(Translations translation, ReadOnlySpan<string> query)
        {
            Logger.LogInformation("Creating URL to lookup {VerseCount} verses on scripture.api.bible", query.Length);
            var ret = new StringBuilder($"{TranslationIds[translation]}/search?query=");
            for (var i = 0; i < query.Length - 1; i++)
            {
                ret.Append($"{query[i]},");
            }

            ret.Append(query[query.Length - 1]);
            return new Uri(ret.ToString(), UriKind.Relative);
        }
    }
}
