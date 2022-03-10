using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web_API.DTOs;
using Web_API.SyncDataService.Http;
using WebAPI.BLL.Core.IConfiguration;
using WebAPI.BLL.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeeDataClient _employeeDataClient;

        public EmployeeController(IUnitOfWork unitOfWork, ILogger<EmployeeController> logger, IMapper mapper, IEmployeeDataClient employeeDataClient)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _employeeDataClient = employeeDataClient;
        }

        [HttpGet(Name = "GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var empData = await _unitOfWork.EmployeeRepository.All();

            if (empData != null)
            {
                var result = _mapper.Map<IEnumerable<EmployeeReadDTO>>(empData);

                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:int}", Name = "CreateEmployeeUsingThirdPartyServices")]
        public async Task<IActionResult> CreateEmployeeUsingThirdPartyServices(int id)
        {
            if (id < 0)
                throw new ArgumentNullException("Employee Id must be greater than zero");

            var empData = await _employeeDataClient.GetDataFromEmployeeApi(id);

            if (empData != null)
            {
                //Add that data to database
                var result = _mapper.Map<Employee>(empData);

                await _unitOfWork.EmployeeRepository.Add(result);

                await _unitOfWork.CompleteAsync();

                return new JsonResult(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}