using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WheelOfFortune.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WheelOfFortune.Models;
using WheelOfFortune.Additionals;
using WheelOfFortune.ViewComponents;
using System.Text;

namespace WheelOfFortune.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class WheelGameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WheelGameController(ApplicationDbContext context,
                                   UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _context.Database.EnsureCreated();
            var wheelGames = await _context.Wheels.ToListAsync();
            return View(wheelGames);
        }

        // GET: WheelGame/Play/5
        [HttpGet]
        public async Task<IActionResult> Play()
        {
            return View();
        }



        [HttpGet]
        public JsonResult GetWheelPlay()
        {
            return Json("{}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Spin([FromBody]JObject userdata)
        {
            var curUser = await _userManager.GetUserAsync(HttpContext.User);

            JToken st1 = userdata["spinResult"]["userData"]["score"];
            JToken st2 = userdata["betAmount"];

            //userData score
            double score = Convert.ToDouble(st1.ToString());
            double moneyBet = Convert.ToDouble(st2.ToString());
            double finalAmount = (moneyBet * score);
            var newSpinEntry = new SpinEntry()
            {
                BetAmount = (float)moneyBet,
                ResultAmount = (float)(finalAmount),
                BelongsToHistoryEntry = new HistoryEntry()
                {
                    CreatedBy = curUser,
                    TimeOccurred = DateTime.Now,
                    HistoryEntryTypeId = HistoryEntry.EntryType.Spin
                }
            };

            _context.Add(newSpinEntry);
            _context.SaveChanges();

             

            var gamerToUpdate = await _context.Gamers.SingleOrDefaultAsync(s => s.Id == curUser.Id);
            double oldBalance = gamerToUpdate.Balance;
            gamerToUpdate.Balance += (float)finalAmount;
            if (await TryUpdateModelAsync<ApplicationUser>(gamerToUpdate))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Failed to save balance and spin result. ");
                }
            }

            return PartialView("_LoginPartial", curUser);
        }

        [HttpGet]
        public async Task<ActionResult> GetUpdatedBalance()
        {
            var curUser = await _userManager.GetUserAsync(HttpContext.User);
            return PartialView("_LoginPartial", curUser);
        }

    }
}