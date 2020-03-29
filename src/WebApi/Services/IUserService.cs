using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Data.Models;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<GroupRole>> GetGroupRolesAsync(string userId, Roles minimumRole);

        Task<Translations> GetPreferredTranslationAsync(string userId);

        Task<bool> IsGlobalAdmin(string userId);

        Task<bool> UserIsInGroup(string userId, int groupId, Roles minimumRole);
    }
}
