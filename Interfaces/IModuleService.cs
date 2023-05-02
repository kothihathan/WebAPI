using System.Collections.Generic;
using WebApi.Entities;
using WebApi.Models.Modules;

namespace WebApi.Interfaces
{
    public interface IModuleService
    {
        IEnumerable<Module> GetAll();
        FuseNavigationModel GetModulesByUserId(int userID);
    }
}
