using System.Collections.Generic;
using WebApi.Entities;

namespace WebApi.Interfaces
{
    public interface ICompanyService
    {
        IEnumerable<Company> GetAll();
    }
}
