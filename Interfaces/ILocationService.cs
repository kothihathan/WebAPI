using System.Collections.Generic;
using WebApi.Entities;

namespace WebApi.Interfaces
{
    public interface ILocationService
    {
        IEnumerable<Location> GetAll();
    }
}
