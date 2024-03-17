using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using Otus.Teaching.PromoCodeFactory.WebHost.Services;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController
        : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly EmployeeService _employeeService;

        public EmployeesController(IRepository<Employee> employeeRepository , EmployeeService employeeService)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x => 
                new EmployeeShortResponse()
                    {
                        Id = x.Id,
                        Email = x.Email,
                        FullName = x.FullName,
                    }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();
            
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



        // <summary>
        /// Получить данные всех сотрудников v2
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeesAsyncV2")]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsyncV2()
        {
           return await _employeeService.GetEmployeesAsync();
        }

        /// <summary>
        /// Получить данные сотрудника по Id v2
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeeByIdAsyncV2{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsyncV2(Guid id)
        {
            return await _employeeService.GetEmployeeByIdAsync(id);
        }


        /// <summary>
        /// Создать нового сотрудника v2
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        [HttpPost("CreateEmployee")]
        public async Task<ActionResult<Employee>> CreateEmployee (EmployeeCreate employeeDto)
        {
            return await _employeeService.CreateEmployee(employeeDto);
        }

        /// <summary>
        /// обновить данные пользователя v2
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateEmployee")]
        public async Task<ActionResult<Employee>> UpdateEmployee(EmployeeUpdate employeeDto)
        {
            return await _employeeService.UpdateEmployee(employeeDto);
        }
        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteEmployee")]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            await _employeeService.DeleteEmployee(id);

            return Ok();          
        }

    }
}