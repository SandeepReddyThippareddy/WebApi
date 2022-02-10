using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.BLL.Core.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get;}
        IEmployeeRepository EmployeeRepository {get;}
        Task<bool> Complete();
    }
}