using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WheelOfFortune.Admin.Data;
using WheelOfFortune.Admin.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WheelOfFortune.Admin.Additionals;

namespace WheelOfFortune.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public ApplicationUsersController(ApplicationDbContext context,
                                          UserManager<ApplicationUser> userManager,
                                          SignInManager<ApplicationUser> signInManager,
                                          RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: ApplicationUsers
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;


            var gamers = from s in _context.Gamers
                           select s;
            switch (sortOrder)
            {
                case "name_desc":
                    gamers = gamers.OrderByDescending(s => s.UserName);
                    break;
                case "Email":
                    gamers = gamers.OrderBy(s => s.Email);
                    break;
                default:
                    gamers = gamers.OrderBy(s => s.Email);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<ApplicationUser>.CreateAsync(gamers.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: ApplicationUsers/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Gamers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        [HttpGet]
        public async Task<IActionResult> History(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historyUser = await _context.HistoryEntries
                .Where(h => h.CreatedBy.Equals(id))
                .ToListAsync();
            if (historyUser == null)
            {
                return NotFound();
            }

            return View(historyUser);
        }

        [HttpGet]
        public async Task<IActionResult> SpinDetails(int hid)
        {
                 var spindetails = await _context.SpinEntries
                .Where(s => s.HistoryEntryId.Equals(hid))
                .Include(s => s.BelongsToHistoryEntry)
                .FirstOrDefaultAsync();
            
            return View(spindetails);
        }

        [HttpGet]
        public async Task<IActionResult> DepositDetails(int hid)
        {
             var depositdetails = await _context.DepositEntries
                .Where(d => d.HistoryEntryId.Equals(hid))
                .Include(d => d.BelongsToHistoryEntry)
                .Include(d => d.BelongsToVoucherEntry)
                .FirstOrDefaultAsync();

            return View(depositdetails);
        }

        // GET: ApplicationUsers/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Gamers.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, 
            [Bind("Id,UserName,Email,EmailConfirmed,PhoneNumber,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userToUpdate = await _context.Gamers.SingleOrDefaultAsync(s => s.Id == id);
                if (await TryUpdateModelAsync<ApplicationUser>(
                    userToUpdate,
                    "",
                    s => s.LockoutEnd, s => s.LockoutEnabled, s => s.AccessFailedCount))
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ApplicationUserExists(applicationUser.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Unable to save changes. " +
                                        "Try again, and if the problem persists, " +
                                        "see your system administrator.");
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }     
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Gamers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.Gamers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Gamers.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.Gamers.Any(e => e.Id == id);
        }
    }
}
