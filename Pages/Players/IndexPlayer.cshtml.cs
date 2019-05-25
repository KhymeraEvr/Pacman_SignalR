using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PacmanWebb.Models;

namespace PacmanWebb.Pages.Players
{
    public class IndexModel : PageModel//jk
    {
        private readonly PacmanWebb.Models.PacmanWebbContext _context;

        public IndexModel(PacmanWebb.Models.PacmanWebbContext context)
        {
            _context = context;
        }

        public IList<PlayerModel> PlayerModel { get;set; }

        //public async Task OnGetAsync()
        //{
        //    PlayerModel = await _context.PlayerModel.ToListAsync();
        //}

        public async Task OnGetAsync(string searchString)
        {
            var players = from m in _context.PlayerModel
                         orderby m.Score descending select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                players = players.Where(s => s.Name == searchString).OrderByDescending(s=>s.Name);
            }

            PlayerModel = await players.ToListAsync();
        }
    }
}
