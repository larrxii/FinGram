using FinGram.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Models;
using FinGram.Data;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Pages
{
    public class CertificateModel : PageModel
    {
        private readonly CertificateService _certificateService;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public CertificateModel(CertificateService certificateService, UserManager<User> userManager, AppDbContext context)
        {
            _certificateService = certificateService;
            _userManager = userManager;
            _context = context;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userResults = await _context.UserTestResults
                .Where(r => r.UserId == user.Id)
                .Include(r => r.Test)
                .ToListAsync();

            int currentPoints = userResults.Sum(r => r.Score);
            int maxPoints = await _context.Tests.SelectMany(t => t.Questions).CountAsync();

            var userName = $"{user.FirstName} {user.LastName}".Trim();
            var courseName = "Финансовая грамотность";
            var date = DateTime.UtcNow;

            var pdf = _certificateService.GenerateCertificate(userName, courseName, currentPoints, date);

            return File(pdf, "application/pdf", "Сертификат_FinGram.pdf");
        }
    }
}
