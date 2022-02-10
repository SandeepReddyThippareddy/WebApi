using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SaikiAPI.Core.IRepositories;
using SaikiAPI.Core.Repositories;

namespace SaikiAPI.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly WebApiContext _context;
        private readonly ILogger _logger;
        public IUserRepository UserRepository {get; private set;}

        public IEmployeeRepository EmployeeRepository { get; private set; }

        public UnitOfWork(WebApiContext webApiContext, ILoggerFactory logger)
        {
            _context = webApiContext;
            _logger = logger.CreateLogger("Logs");
            UserRepository = new UserRepository(_context, _logger);
            EmployeeRepository = new EmployeeRepository(_context, _logger);
        }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }

        // public async Task Dispose()
        // {
        //     await _context.DisposeAsync();
        // }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}