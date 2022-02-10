using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.BLL.Core.IRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeeController> _logger;
        public EmployeeController(IUnitOfWork unitOfWork, ILogger<EmployeeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet(Name = "GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var empData = await _unitOfWork.EmployeeRepository.All();

            if (empData != null){
                return new JsonResult(empData);
            }else{
                return BadRequest();
            }
        }
    }
}