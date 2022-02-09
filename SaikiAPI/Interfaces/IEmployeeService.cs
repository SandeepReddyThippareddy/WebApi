using Microsoft.AspNetCore.Http;
using SaikiAPI.EntityModels;
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
    }
}
