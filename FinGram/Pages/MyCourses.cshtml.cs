using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinGram.Pages
{
    public class MyCoursesModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;


        public MyCoursesModel(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public int MaxPoints { get; set; }
        public int CurrentPoints { get; set; }
        public int ProgressPercent { get; set; }
        public bool IsCertificateEligible { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var allTests = await _context.Tests
            .Include(t => t.Questions)
            .ToListAsync();

            MaxPoints = allTests.Sum(t => t.Questions.Count);

            var userResults = await _context.UserTestResults
                .Where(r => r.UserId == user.Id)
                .Include(r => r.Test)
                .ToListAsync();

            CurrentPoints = userResults.Sum(r => r.Score);

            bool hasPassedFinal = userResults.Any(r => r.Test.IsFinal);

            var allLessonsCount = await _context.Lessons.CountAsync();

            var userLessonsCount = await _context.UserLessons
                .CountAsync(ul => ul.UserId == user.Id);

            ProgressPercent = (int)((double)userLessonsCount / allLessonsCount * 100);

            IsCertificateEligible = MaxPoints > 0 &&
                                    CurrentPoints >= MaxPoints / 2 &&
                                    hasPassedFinal;

            return Page();
        }
    }
}
