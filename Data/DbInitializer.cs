using PacmanWebb.Models;
using System;
using System.Linq;


namespace PacmanWebb.Models
{
    public static class DbInitializer
    {
        public static void Initialize(PacmanWebbContext context)
        {
           //context.Database.EnsureCreated();

            // Look for any students.
            if (context.PlayerModel.Any())
            {
                return;   // DB has been seeded
            }

            var players = new PlayerModel[]
            {
                new PlayerModel{Name = "Bo", Score=124},
                new PlayerModel{Name = "Burnham", Score=544 },
                new PlayerModel{Name = "Louis", Score=123},
                new PlayerModel{Name = "Ck", Score=120},
                new PlayerModel{Name = "George", Score=120},
                new PlayerModel{Name = "Carlin", Score=223},
                new PlayerModel{Name = "Michael", Score=120},
                new PlayerModel{Name = "Hirst", Score=435 }
            };
            foreach (PlayerModel pl in players)
            {
                context.PlayerModel.Add(pl);
            }
            context.SaveChanges();           
        }
    }
}
 