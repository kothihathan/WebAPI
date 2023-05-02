using System.Collections.Generic;
using WebApi.Entities;

namespace WebApi.Interfaces
{
    public interface IUserRightService
    {
        IEnumerable<UserRight> GetAll();
    }
}
