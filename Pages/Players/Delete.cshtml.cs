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
    public class DeleteModel : PageModel
    {
        private readonly PacmanWebb.Models.PacmanWebbContext _context;

        public DeleteModel(PacmanWebb.Models.PacmanWebbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PlayerModel = await _context.PlayerModel.FindAsync(id);

            if (PlayerModel != null)
            {
                _context.PlayerModel.Remove(PlayerModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
