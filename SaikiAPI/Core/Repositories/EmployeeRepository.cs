using Microsoft.Extensions.Logging;
using SaikiAPI.Core.IRepositories;
using SaikiAPI.Data;
using SaikiAPI.Models;

namespace SaikiAPI.Core.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(WebApiContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}