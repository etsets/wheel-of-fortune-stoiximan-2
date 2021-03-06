﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WheelOfFortune.Admin.Data;
using WheelOfFortune.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WheelOfFortune.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [HttpGet]
        [ResponseCache(NoStore = true, Duration = 0)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);

            var entries = await _context.Users
               // .Where(h => h.CreatedBy.Equals(user.Id))
                //.OrderBy(h => h.TimeOccurred)
                .ToListAsync();
            return View(entries);
        }

        [HttpGet]
        public async Task<IActionResult> SpinDetails(int hid)
        {
            var user = await _userManager.GetUserAsync(User);

            var spindetails = await _context.SpinEntries
                .Where(s => s.HistoryEntryId.Equals(hid))
                .Include(s => s.BelongsToHistoryEntry)
                .FirstOrDefaultAsync();
            return View(spindetails);
        }

        [HttpGet]
        public async Task<IActionResult> DepositDetails(int hid)
        {
            var user = await _userManager.GetUserAsync(User);

            var depositdetails = await _context.DepositEntries
                .Where(d => d.HistoryEntryId.Equals(hid))
                .Include(d => d.BelongsToHistoryEntry)
                .Include( d => d.BelongsToVoucherEntry)
                .FirstOrDefaultAsync();
            return View(depositdetails);
        }

    }
}