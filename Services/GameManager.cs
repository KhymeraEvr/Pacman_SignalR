using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using PacmanWebb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacmanWebb.Services
{
    public class GameManager : IDisposable
    {
        public string PlayerName { get; set; }
        public string ConnectionId;
        public CoresUpdater coresUpdater;
        public MonstersCors cors;
        public int MapId { get; set; }
        private IHubContext<GameHub> _hubContext;
        public List<WatchedManager> watching = new List<WatchedManager>();
        public GameManager(string ConnectionId)
        {            
            _hubContext = Startup.hubContext;
            this.ConnectionId = ConnectionId;
            coresUpdater = new CoresUpdater(_hubContext, ConnectionId,this);
            cors = new MonstersCors(ConnectionId);
            Pacman.FoodEaten.Actions.Add(new Tuple<string, Action<int, int>>(ConnectionId, Erase));
            Pacman.Mechanics.GameOverEvent.Actions.Add(new Tuple<string, Action>(ConnectionId, DbAddPlayer));
            Pacman.Mechanics.GameOverEvent.Actions.Add(new Tuple<string, Action>(ConnectionId, ResScoreOutput));
            Pacman.Mechanics.PlayerEaten.Actions.Add(new Tuple<string, Action>(ConnectionId, EraseLives));
            Pacman.CherryEaten.Actions.Add(new Tuple<string, Action>(ConnectionId, ScareGhosts));
           
        }

        public void addInstanceEvents()
        {
            Pacman.Program.games[ConnectionId].Maze1.timer.ScareOverActions.Add(CalmGhosts);

            foreach (Pacman.Ghost gh in Pacman.Program.games[ConnectionId].GhostsCol.Ghosts)
            {
                gh.onReturnToGame = RespanwGhost;
            }
        }

        public void Erase(int x, int y)
        {
            _hubContext.Clients.Client(ConnectionId).SendAsync("Erase", x, y);
            foreach(WatchedManager wm in watching)
            {
                _hubContext.Clients.Client(wm.ConnectionId).SendAsync("Erase", x, y);
            }
        }

        public void PopRestartMenu()
        {
            _hubContext.Clients.Client(ConnectionId).SendAsync("RestartMenu");
        }
        
        public void DbAddPlayer()
        {            
            using (var scope = Program.host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                PacmanWebbContext dbContext = services.GetRequiredService<PacmanWebbContext>();
                dbContext.PlayerModel.Add(new Models.PlayerModel { Name = Pacman.Program.games[ConnectionId].PlayerName,
                    Score = Pacman.Program.games[ConnectionId].Score });
                dbContext.SaveChanges();
            }
        }

        public void ScareGhosts()
        {
            _hubContext.Clients.Client(ConnectionId).SendAsync("ScareGhosts");
            foreach (WatchedManager wm in watching)
            {
                _hubContext.Clients.Client(wm.ConnectionId).SendAsync("ScareGhosts");
            }
        }

        public void CalmGhosts()
        {
            _hubContext.Clients.Client(ConnectionId).SendAsync("CalmGhosts");
            foreach (WatchedManager wm in watching)
            {
                _hubContext.Clients.Client(wm.ConnectionId).SendAsync("CalmGhosts");
            }
        }

        public void RespanwGhost(string ghostName)
        {
            _hubContext.Clients.Client(ConnectionId).SendAsync("RespanwGhost",ghostName);
            foreach (WatchedManager wm in watching)
            {
                _hubContext.Clients.Client(wm.ConnectionId).SendAsync("RespanwGhost",ghostName);
            }
        }

        public void EraseLives()
        {
            _hubContext.Clients.Client(ConnectionId).SendAsync("EraseLive", Pacman.Program.games[ConnectionId].Player.LivesRemain);
            foreach (WatchedManager wm in watching)
            {
                _hubContext.Clients.Client(wm.ConnectionId).SendAsync("EraseLive", Pacman.Program.games[ConnectionId].Player.LivesRemain);
            }
        }

        public void ResScoreOutput()
        {
            coresUpdater.StopSending();
            Pacman.Program.games[ConnectionId].PlayerInput.Parsing = false;
            Pacman.Program.games[ConnectionId].Maze1.GameOver = true;
            _hubContext.Clients.Client(ConnectionId).SendAsync("SetScore", Pacman.Program.games[ConnectionId].Score);
            foreach (WatchedManager wm in watching)
            {
                _hubContext.Clients.Client(wm.ConnectionId).SendAsync("Exit");
            }
            watching.Clear();
        }

        public void Dispose()
        {            
        }
    }
}
