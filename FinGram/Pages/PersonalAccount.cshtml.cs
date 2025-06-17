using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FinGram.Pages
{
    [Authorize]
    public class PersonalAccountModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public PersonalAccountModel(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string TelegramLink { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return BadRequest("Invalid user ID");
            }
            var user = await _userManager.FindByIdAsync(userIdStr);
            if (user == null)
            {
                return NotFound();
            }
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;

            var existing = _context.TelegramLinks.FirstOrDefault(t => t.UserId == userId && !t.IsUsed);

            string token;

            if (existing != null)
            {
                token = existing.Token;
            }
            else
            {
                token = Guid.NewGuid().ToString("N");

                _context.TelegramLinks.Add(new TelegramLink
                {
                    Token = token,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    IsUsed = false
                });

                await _context.SaveChangesAsync();
            }

            TelegramLink = $"https://t.me/FinGramAssistBot?start={token}";

            return Page();
        }
    }
}