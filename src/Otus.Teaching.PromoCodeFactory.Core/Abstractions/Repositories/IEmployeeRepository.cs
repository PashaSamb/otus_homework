using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> Create (Employee item);
        Task<Employee> Update (Employee item);
        Task<Employee> GetEmployeeWithRoles (Guid id);
        Task AddRange(ICollection<Employee> employees);
        Task Delete (Guid id);
    }
}
