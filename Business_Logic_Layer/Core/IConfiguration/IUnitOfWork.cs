using System.Threading.Tasks;
using WebAPI.BLL.Core.IRepositories;

namespace WebAPI.BLL.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get;}
        IEmployeeRepository EmployeeRepository {get;}
        Task<bool> CompleteAsync();
    }
}