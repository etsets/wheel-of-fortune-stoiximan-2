using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WheelOfFortune.Models;
using WheelOfFortune.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace WheelOfFortune.ViewComponents
{
    public class BalanceViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BalanceViewComponent(ApplicationDbContext context,
                                   UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = await GetUserAsync();
            return View(currentUser);
        }
        private async Task<ApplicationUser> GetUserAsync()
        {
            //return _context.ToDo.Where(x => x.IsDone == isDone && x.Priority <= maxPriority).ToListAsync();
            var curUser = await _userManager.GetUserAsync(HttpContext.User);
            return curUser;
        }

    }
}
