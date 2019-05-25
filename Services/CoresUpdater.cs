using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using Microsoft.AspNetCore.SignalR;

namespace PacmanWebb.Services
{
    public class CoresUpdater : IDisposable
    {
        private int prevScore;
        private bool inited = false;
       
        private IHubContext<GameHub> _hubContext;
        System.Timers.Timer sender = new System.Timers.Timer(20);
        public string ConnectionId { get; set; }
        public GameManager Gm;
        public CoresUpdater()
        {
            
        }

        public CoresUpdater(IHubContext<GameHub> hubContext, string id, GameManager gm)
        {
            _hubContext = hubContext;
            ConnectionId = id;
            Gm = gm;
        }
        public void StartSending()
        {
            if(!inited)
            {
                sender.Elapsed += Send;
                sender.AutoReset = true;
                sender.Enabled = true;
                inited = true;
            }
            else
            {
                sender.Enabled = true;
            }
        }
        public void StopSending()
        {
            sender.Enabled = false;
        }

        void Send(object source, ElapsedEventArgs a)
            {
            (_hubContext).Clients.Client(ConnectionId).SendAsync("ReceiveCors", Pacman.Program.games[ConnectionId].coresModel.cores);
            foreach (WatchedManager wm in Gm.watching)
            {
                (_hubContext).Clients.Client(wm.ConnectionId).SendAsync("ReceiveCors", Pacman.Program.games[ConnectionId].coresModel.cores);
            }
            if (prevScore != Pacman.Program.games[ConnectionId].Score)
            {
                prevScore = Pacman.Program.games[ConnectionId].Score;
                _hubContext.Clients.Client(ConnectionId).SendAsync("ReceiveScore", prevScore);
                foreach (WatchedManager wm in Gm.watching)
                {
                    _hubContext.Clients.Client(wm.ConnectionId).SendAsync("ReceiveScore", prevScore);
                }
            }

        }

        public void Dispose()
        {
         
        }
    }
}
