using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WheelOfFortune.Data;
using WheelOfFortune.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WheelOfFortune.Controllers
{
    public class PlayController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlayController(
            ApplicationDbContext context,
          UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
          [ResponseCache(NoStore = true, Duration = 0)]
          public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);

            var entries = await _context.HistoryEntries
                .Where(h => h.CreatedBy.Equals(user.Id))
                .ToListAsync();
            
            return View(entries);
        }

    }
}