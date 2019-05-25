using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PacmanWebb.Models
{
    public class MonstersCors
    {
        Walker[] monsters = new Walker[5];
        public string ConnectionId { get; set; }

        public MonstersCors(string id)
        {
            ConnectionId = id;
            monsters[0] = new Walker { Name = "Pacman" }; 
            monsters[1] = new Walker { Name = "Shadow" }; 
            monsters[2] = new Walker { Name = "Speedy" }; 
            monsters[3] = new Walker { Name = "Bashful" }; 
            monsters[4] = new Walker { Name = "Pokey" };
            Thread updater = new Thread(() => Update());
        }
        public void Update()
        {
            monsters[0].X = Pacman.Program.games[ConnectionId].Player.Xpos;
            monsters[0].Y = Pacman.Program.games[ConnectionId].Player.Ypos;
            for(int i = 1; i < monsters.Length; i++)
            {
                monsters[i].X = Pacman.Program.games[ConnectionId].GhostsCol.Ghosts[i].Xpos;
                monsters[i].Y = Pacman.Program.games[ConnectionId].GhostsCol.Ghosts[i].Ypos;
            }
        }
    }
}
