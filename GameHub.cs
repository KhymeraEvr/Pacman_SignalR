using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;

namespace PacmanWebb
{
    public class GameHub : Hub
    {

        public static Dictionary<string, Services.GameManager> games = new Dictionary<string, Services.GameManager>();
        public async Task SendMessage(string message)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", message);

        }

        public async Task SendCores()
        {
           await Clients.Client(Context.ConnectionId).SendAsync("ReceiveCors", Pacman.Program.games[Context.ConnectionId].coresModel.cores);
        }

        public void GetInput(string direction)
        {
            if (!Pacman.Program.games.Keys.Contains(Context.ConnectionId)) return;
            Pacman.Program.games[Context.ConnectionId].PlayerInput.tryPassDirection(direction);
        }


        public async Task StartGame(int mapId)
        {           
            if (!games.Keys.Contains(Context.ConnectionId))
            {
                games.Add(Context.ConnectionId, new Services.GameManager(Context.ConnectionId));
            }
            else
            {
                Pacman.Program.games[Context.ConnectionId].Maze1.GameOver = false;
                Pacman.Program.games[Context.ConnectionId].PlayerInput.Parsing = true;
            }
            games[Context.ConnectionId].MapId = mapId;
            Pacman.Program.Setup(Context.ConnectionId, mapId);
            games[Context.ConnectionId].addInstanceEvents();
            Task t1 = DrawCoins();
            Task t2 = DrawCherries();
            games[Context.ConnectionId].coresUpdater.StartSending();
            await Task.WhenAll(t1, t2);
           
        }

        public async Task WatchGame(string playerName)
        {
            foreach (Services.GameManager gm in games.Values)
            {
                if (gm.PlayerName == playerName)
                {

                    gm.watching.Add(new Services.WatchedManager(Context.ConnectionId));
                    Task t1 = Clients.Client(Context.ConnectionId).SendAsync("WatchGame", games[gm.ConnectionId].MapId);
                    Task t3 = DrawSmbdsCoins(gm.ConnectionId);
                    Task t2 = DrawSmbdsCherries(gm.ConnectionId);
                    await Task.WhenAll(t1, t2, t3);
                    return;
                }
            }
            await Clients.Client(Context.ConnectionId).SendAsync("NoPlayerFound");
        }

        public void GetPlayerName(string playerName)
        {
            games[Context.ConnectionId].PlayerName = playerName;
            Pacman.Program.games[Context.ConnectionId].PlayerName = playerName;
        }

        public async Task DrawCherries()
        {
            foreach (Pacman.Cherry cherry in Pacman.Program.games[Context.ConnectionId].Maze1.Chers)
            {
                await DrawSomething("cherry", (int)cherry.Xpos, (int)cherry.Ypos);
            }
        }

        public async Task DrawCoins()
        {
            int width = Pacman.Program.games[Context.ConnectionId].Maze1.Width;
            int length = Pacman.Program.games[Context.ConnectionId].Maze1.Length;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (Pacman.Program.games[Context.ConnectionId].Maze1.coins[i, j]) await DrawSomething("coin", j, i);
                }
            }
        }

        public async Task DrawSomething(string name, int x, int y)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("DrawFood", name, x, y);
        }

        public async Task DrawSmbdsCherries(string ConnectionId)
        {
            foreach (Pacman.Cherry cherry in Pacman.Program.games[ConnectionId].Maze1.Chers)
            {
                if (cherry.IsActive) await Clients.Client(Context.ConnectionId).SendAsync("DrawFood","cherry", (int)cherry.Xpos, (int)cherry.Ypos);
            }
        }

        public async Task DrawSmbdsCoins(string ConnectionId)
        {
            int width = Pacman.Program.games[ConnectionId].Maze1.Width;
            int length = Pacman.Program.games[ConnectionId].Maze1.Length;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (Pacman.Program.games[ConnectionId].Maze1.coins[i, j]) await DrawSomething("coin", j, i);
                }
            }
        }
    }
}
