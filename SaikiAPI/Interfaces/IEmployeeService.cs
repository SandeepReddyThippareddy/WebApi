using Microsoft.AspNetCore.Http;
using SaikiAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaikiAPI.Interfaces
{
    public interface IEmployeeService
    {
        Task<ApplicationUser> AuthenticateUser(UserCred userCred);
        Task<bool> UploadDataToAzure(string userId);
        string DownloadDataFromAzure(string userId);
        Task<ApplicationUser> GetUserData(string userId);

        bool UploadFile(IFormFile file);

        IEnumerable<Employee> GetUserData(CursorParams cursorParams, out int? nextCursor);
    }
}
