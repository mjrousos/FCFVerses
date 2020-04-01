using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Data.Models;
using WebApi.Models;
using WebApi.ViewModels;

namespace WebApi.Services
{
    public interface IGroupService
    {
        Task<bool> AddPassageAsync(int groupId, PassageReference passageReference);

        Task<PassageGroup> GetPassagesAsync(int groupId, Translations translation, bool admin);

        Task<bool> RemovePassageAsync(int groupId, int passageId);

        Task<IEnumerable<GroupViewModel>> GetAllPublicGroupsAsync();

        Task<IEnumerable<GroupViewModel>> GetGroupsAsync(string userId);
    }
}
