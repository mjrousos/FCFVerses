using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Data.Models;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IVerseService
    {
        Task<IEnumerable<Verse>> GetVersesAsync(IEnumerable<VerseReference> references, Translations translation);
    }
}
