using System.Collections.Generic;
using WebApi.Entities;
using WebApi.Models.Modules;

namespace WebApi.Interfaces
{
    public interface IModulePageService
    {
        IEnumerable<ModulePage> GetAll();
        FuseNavigationModel GetModulePagesByUserId(int userID);
    }
}
