using System.Collections.Generic;
using WebApi.Entities;

namespace WebApi.Interfaces
{
    public interface IUserRoleService
    {
        IEnumerable<UserRole> GetAll();
    }
}
