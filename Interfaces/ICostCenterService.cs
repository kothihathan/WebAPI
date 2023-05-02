using System.Collections.Generic;
using WebApi.Entities;

namespace WebApi.Interfaces
{
    public interface ICostCenterService
    {
        IEnumerable<CostCenter> GetAll();
    }
}
