using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PacmanWebb.Models
{
    public class PacmanWebbContext : DbContext
    {
        public PacmanWebbContext (DbContextOptions<PacmanWebbContext> options)
            : base(options)
        {
        }

        public DbSet<PacmanWebb.Models.PlayerModel> PlayerModel { get; set; }
    }
}
