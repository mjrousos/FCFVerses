using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ViewModels;

namespace WebApi.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupViewModel>> GetAllPublicGroups();

        Task<IEnumerable<GroupViewModel>> GetGroups(string userId);
    }
}
