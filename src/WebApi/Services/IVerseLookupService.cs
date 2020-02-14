using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Data.Models;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IVerseLookupService
    {
        Task<IEnumerable<Verse>> LookupVersesAsync(IEnumerable<VerseReference> references, Translations translation);
    }

    public delegate IVerseLookupService LookupServiceResolver(Translations translation);
}
