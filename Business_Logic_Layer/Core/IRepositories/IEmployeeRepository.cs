using Microsoft.AspNetCore.Http;
using WebAPI.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.BLL.Core.IRepositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<ApplicationUser> AuthenticateUser(UserCred userCred);
        Task<bool> UploadDataToAzure(string userId);
        string DownloadDataFromAzure(string userId);
        Task<ApplicationUser> GetUserData(string userId);

        bool UploadFile(IFormFile file);

        IEnumerable<Employee> GetUserData(CursorParams cursorParams, out int? nextCursor);
    }
}