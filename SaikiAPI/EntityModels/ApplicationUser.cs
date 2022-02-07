using Microsoft.AspNetCore.Identity;
using System;

namespace SaikiAPI.EntityModels
{
    [Serializable]
    public class ApplicationUser : IdentityUser
    {
        
        public string BearerToken { get; set; }
    }
}
