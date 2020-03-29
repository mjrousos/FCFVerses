using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebApi.Data;
using WebApi.Data.Models;
using WebApi.Models;

namespace WebApi.Services
{
    public class VerseService : IVerseService
    {
        private VersesDbContext DbContext { get; }

        private LookupServiceResolver LookupResolver { get; }

        private ILogger<VerseService> Logger { get; }

        public VerseService(VersesDbContext dbContext, LookupServiceResolver lookupResolver, ILogger<VerseService> logger)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            LookupResolver = lookupResolver ?? throw new ArgumentNullException(nameof(lookupResolver));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Verse>> GetVersesAsync(IEnumerable<VerseReference> references, Translations translation)
        {
            /* TODO : Check SQL cache */

            var lookupService = LookupResolver(translation);
            var verses = await lookupService.LookupVersesAsync(references, translation);

            /* TODO : Get verses from service and cache them */

            return verses;
        }
    }
}
