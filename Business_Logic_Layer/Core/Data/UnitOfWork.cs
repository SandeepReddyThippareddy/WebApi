using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebAPI.BLL.Core.IConfiguration;
using WebAPI.BLL.Core.IRepositories;
using WebAPI.BLL.Core.Repositories;
using WebAPI.BLL.Models;

namespace WebAPI.BLL.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly WebApiContext _context;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSetting _jwtSetting;
        public IUserRepository UserRepository { get; private set; }

        public IEmployeeRepository EmployeeRepository { get; private set; }

        public UnitOfWork(UserManager<ApplicationUser> userManager, IOptions<JwtSetting> options, WebApiContext webApiContext, ILoggerFactory logger)
        {
            _context = webApiContext;
            _logger = logger.CreateLogger("Logs");
            _userManager = userManager;
            _jwtSetting = options.Value;
            UserRepository = new UserRepository(_context, _logger);
            EmployeeRepository = new EmployeeRepository(_userManager, _jwtSetting, _context, _logger);
        }

        //Another way of instantiating the repositories.
        //public IUserRepository UserRepository => new UserRepository(_context, _logger);
        public async Task<bool> Complete()
        {
           return await _context.SaveChangesAsync() > 0;
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