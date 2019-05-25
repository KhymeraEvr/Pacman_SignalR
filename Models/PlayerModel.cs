using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacmanWebb.Models
{
    public class PlayerModel
    {
        public PlayerModel() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }
}
