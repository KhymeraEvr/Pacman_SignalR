using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PacmanWebb.Areas.Identity.Pages.Account;

namespace PacmanWebb.Data
{
    public class ApplicationDbContext : IdentityDbContext<PacmanUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PacmanUser> PacmanUsers { get; set; }

    }
}
