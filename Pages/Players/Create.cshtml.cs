using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PacmanWebb.Models;

namespace PacmanWebb.Pages.Players
{
    public class CreateModel : PageModel
    {
        private readonly PacmanWebb.Models.PacmanWebbContext _context;

        public CreateModel(PacmanWebb.Models.PacmanWebbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PlayerModel PlayerModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.PlayerModel.Add(PlayerModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}