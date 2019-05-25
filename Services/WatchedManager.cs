using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacmanWebb.Services
{
    public class WatchedManager
    {
        public string ConnectionId { get; set; }
        public WatchedManager(string ConnectionId)
        {          
            this.ConnectionId = ConnectionId;                 
        }
    }
}
