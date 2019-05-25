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
    public class DetailsModel : PageModel
    {
        private readonly PacmanWebb.Models.PacmanWebbContext _context;

        public DetailsModel(PacmanWebb.Models.PacmanWebbContext context)
        {
            _context = context;
        }

        public PlayerModel PlayerModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PlayerModel = await _context.PlayerModel.FirstOrDefaultAsync(m => m.Id == id);

            if (PlayerModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
