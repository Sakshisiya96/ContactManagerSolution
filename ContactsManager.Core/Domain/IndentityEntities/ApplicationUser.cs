using System;
using Microsoft.AspNetCore.Identity;

namespace ContactsManager.Core.Domain.IndentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }//login name can be differnt from the peron name
    }
}
