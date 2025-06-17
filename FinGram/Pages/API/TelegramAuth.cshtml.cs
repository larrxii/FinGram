using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinGram.Data;
using FinGram.Models;

namespace FinGram.Pages.API
{
    [IgnoreAntiforgeryToken]
    public class TelegramAuthModel : PageModel
    {
        private readonly AppDbContext _context;

        private readonly ILogger<TelegramAuthModel> _logger;

        public TelegramAuthModel(AppDbContext context, ILogger<TelegramAuthModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public class TelegramAuthRequest
        {
            public string Token { get; set; }
            public int TelegramId { get; set; }
        }

        public class TelegramAuthResponse
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync([FromBody] TelegramAuthRequest request)
        {            
            _logger.LogInformation("🔔 Telegram auth POST: Token = {Token}, TelegramId = {TelegramId}", request.Token, request.TelegramId);
            var link = await _context.TelegramLinks
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == request.Token && !x.IsUsed);



            if (link == null)
            {
                _logger.LogWarning("❌ Token not found or already used: {Token}", request.Token);
                return NotFound(new { error = "Invalid or used token." });
            }


            link.TelegramId = request.TelegramId;
            link.IsUsed = true;
            await _context.SaveChangesAsync();
            


            return new JsonResult(new TelegramAuthResponse
            {
                UserId = link.UserId,
                UserName = link.User.UserName,
                Email = link.User.Email
            });

        }
    }
}

