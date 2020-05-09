using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace CoursePlus.Shared.Models
{
    public class CustomUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public int? AvatarId { get; set; }
        [PersonalData]
        public virtual Avatar Avatar { get; set; }
    }
}
