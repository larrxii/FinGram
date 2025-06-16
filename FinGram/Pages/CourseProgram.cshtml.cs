using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Pages
{
    public class CourseProgramModel : PageModel
    {
        private readonly AppDbContext _context;

        public CourseProgramModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Test> Tests { get; set; } = new();

        public async Task OnGetAsync()
        {
            Tests = await _context.Tests
                .OrderBy(t => t.Id) 
                .ToListAsync();
        }
    }
}
