using Microsoft.AspNetCore.Identity;
using System;

namespace SaikiAPI.Models
{
    [Serializable]
    public class ApplicationUser : IdentityUser
    {
        
        public string BearerToken { get; set; }
    }
}
