using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{

    public class InMemoryEmployeeRepository :  IEmployeeRepository 
    {  
        private readonly ApplicationDbContext _dbContext;

        public InMemoryEmployeeRepository(ApplicationDbContext dbContext )
        {        
            _dbContext = dbContext;
        }

        public async Task<Employee> Create(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);      
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task Delete(Employee item)
        {
            _dbContext.Employees.Remove(item);
            await _dbContext.SaveChangesAsync();           
        }

        public async Task<Employee> Update(Employee item)
        {
            _dbContext.Employees.Update(item);
            await _dbContext.SaveChangesAsync();

            return item;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var data =  await _dbContext.Employees.ToListAsync();
            return data;
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            return await  _dbContext.Employees.Where(x=>x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddRange(ICollection<Employee> employees)
        {
            await _dbContext.AddRangeAsync(employees);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Employee> GetEmployeeWithRoles(Guid id)
        {
            return await _dbContext.Employees.Where(x=>x.Id==id).Include(x=>x.Roles).FirstOrDefaultAsync();
        }
    }
}
