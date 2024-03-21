using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employee;
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeService(IEmployeeRepository employee, IRepository<Employee> employeeRepository)
        {
            _employee = employee;   
            _employeeRepository = employeeRepository;
           // InitData();
        }

        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync ()
        {
            var employees = await _employee.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employee.GetEmployeeWithRoles(id);

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        public async Task<Employee> CreateEmployee(EmployeeCreate employeeDto)
        {
            var employee = new Employee();
           
            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Email = employeeDto.Email;

            return (await _employee.Create(employee));
        }

        public async Task DeleteEmployee(Employee employee)
        {
            await _employee.Delete(employee);
        }

        public async Task<Employee> UpdateEmployee (EmployeeUpdate employeeDto)
        {

            var employee = await _employee.GetByIdAsync(employeeDto.Id);

            if (employee!=null)
            {
                employee.Id = employeeDto.Id;
                employee.FirstName = employeeDto.FirstName;
                employee.LastName = employeeDto.LastName;
                employee.Email = employeeDto.Email;

                return await _employee.Update(employee);
            }

            return null;              
        }


        public async Task<Employee?> GetEmployeeAsyncbyId (Guid id)
        {
            return await _employee.GetByIdAsync(id);
        }

     /*   public  async Task InitData ()
        {
            var fakeEmployee = await _employeeRepository.GetAllAsync();
            if (fakeEmployee != null)
            {
               await _employee.AddRange(fakeEmployee.ToList());
            }
            
        }*/
    }
}
