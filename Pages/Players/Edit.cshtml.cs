using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PacmanWebb.Models;

namespace PacmanWebb.Pages.Players
{
    public class EditModel : PageModel
    {
        private readonly PacmanWebb.Models.PacmanWebbContext _context;

        public EditModel(PacmanWebb.Models.PacmanWebbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PlayerModel PlayerModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string name)
        {
            if (name == "")
            {
                return NotFound();
            }

            PlayerModel = await _context.PlayerModel.FirstOrDefaultAsync(m => m.Name == name);

            if (PlayerModel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PlayerModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerModelExists(PlayerModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PlayerModelExists(int id)
        {
            return _context.PlayerModel.Any(e => e.Id == id);
        }
    }
}
