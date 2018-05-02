using Kumbler.Authentication.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kumbler.Authentication.Context
{
    public class AuthDbContext : IdentityDbContext<KumblerUser>
    {
        public AuthDbContext(DbContextOptions options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
