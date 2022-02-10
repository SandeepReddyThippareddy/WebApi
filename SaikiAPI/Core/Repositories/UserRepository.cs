using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SaikiAPI.Core.IRepositories;
using SaikiAPI.Data;
using SaikiAPI.Models;

namespace SaikiAPI.Core.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(WebApiContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<string> GetEmail(string id)
        {
            try
            {
                var user = await _dbSet.FindAsync(id);

                if (user != null)
                {
                    return user.Email.ToString();
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Error at {0}", typeof(UserRepository)));

                return string.Empty;
            }

        }

        public override async Task<bool> Upsert(ApplicationUser entity)
        {
            var existingUser = await _dbSet.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

            if (existingUser == null)
            {
                if (await Add(existingUser) != null)
                    return true;
                else
                    return false;
            }
            else
            {
                existingUser.UserName = entity.UserName;
                existingUser.Email = entity.Email;
                existingUser.PhoneNumber = entity.PhoneNumber;

            
                return true;

            }


        }
    }
}