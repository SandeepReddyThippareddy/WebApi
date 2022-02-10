using System;
using System.Threading.Tasks;
using SaikiAPI.Models;

namespace SaikiAPI.Core.IRepositories
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
         //You can put the operations spesific to user here

         Task<string> GetEmail(string id);
    }
}