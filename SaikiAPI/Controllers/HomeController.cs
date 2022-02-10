using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaikiAPI.Models;
using SaikiAPI.Interfaces;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace SaikiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public HomeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] UserCred userCred)
        {
            if (userCred == null) throw new ArgumentNullException(nameof(userCred));

            try
            {
                var result = await _employeeService.AuthenticateUser(userCred);

                if (result.BearerToken != null)
                {
                    return Ok(result);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(String.Format("Error in function: {0} - \n Error Message:{1} - \n Stack Trace:{2}",
                                                               System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                               exception.Message, exception?.ToString()));
            }


        }

        [HttpPost]
        [Route("UploadFile")]
        public IActionResult UploadFile(IFormFile file)
        {
            var res = _employeeService.UploadFile(file);

            if (res)
            {
                return Ok("Upload Successful");
            }
            else
            {
                return BadRequest();
            }
        }



        [HttpGet("UploadYourData")]
        [Authorize]
        public async Task<IActionResult> UploadYourData(string userId)
        {
            var res = await _employeeService.UploadDataToAzure(userId);

            if (res)
            {
                return Ok("Operation Success");
            }
            else
            {
                return Unauthorized();
            }


        }

        [Produces("application/xml")]
        [HttpGet("DownloadYourData")]
        [Authorize]
        public IActionResult DownloadYourData(string userId)
        {
            try
            {
                var res = _employeeService.DownloadDataFromAzure(userId);

                if (res != null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(res);

                    return Ok(doc);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex) when (ex.Message.Contains("File Exists"))
            {
                XmlDocument doc = new XmlDocument();
                var array = ex.Message.Split("@");
                doc.Load(array[1]);
                return Ok(doc);
            }

        }


        [HttpGet("GetSpecificUserData")]
        [Authorize]
        public async Task<IActionResult> GetSpecificUserData([FromQuery] string userId)
        {
            try
            {
                var userData = await _employeeService.GetUserData(userId);
                return Ok(userData);
            }
            catch (Exception ex) when (ex.Message.Contains("User does not exists"))
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("GetUserData")]
        [Authorize]
        public IActionResult GetUserData([FromQuery] CursorParams @params)
        {
            try
            {
                int? nextCursor = 0;
                var userData = _employeeService.GetUserData(@params, out nextCursor);

                Response.Headers.Add("X-Pagination", $"Next-Cursor = {nextCursor}");
                return Ok(userData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }

}