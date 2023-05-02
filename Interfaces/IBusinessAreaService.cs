using System.Collections.Generic;
using WebApi.Entities;

namespace WebApi.Interfaces
{
    public interface IBusinessAreaService
    {
        IEnumerable<BusinessArea> GetAll();
    }
}
