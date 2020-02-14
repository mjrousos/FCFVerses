using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebApi.Data.Models;
using WebApi.Models;

namespace WebApi.Services
{
    public class BibleApiVerseLookupService : IVerseLookupService
    {
        internal const string BibleApiHttpClientName = "BibleApiHttpClient";

        private HttpClient ApiClient { get; set; }

        private ILogger<BibleApiVerseLookupService> Logger { get; }

        public BibleApiVerseLookupService(IHttpClientFactory clientFactory, ILogger<BibleApiVerseLookupService> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ApiClient = clientFactory?.CreateClient(BibleApiHttpClientName) ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public Task<IEnumerable<Verse>> LookupVersesAsync(IEnumerable<VerseReference> references, Translations translation)
        {
            // Query API

            throw new NotImplementedException();
        }
    }
}
