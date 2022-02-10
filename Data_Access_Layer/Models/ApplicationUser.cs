using Microsoft.AspNetCore.Identity;
using System;

namespace WebAPI.BLL.Models
{
    [Serializable]
    public class ApplicationUser : IdentityUser
    {
        
        public string BearerToken { get; set; }
    }
}
