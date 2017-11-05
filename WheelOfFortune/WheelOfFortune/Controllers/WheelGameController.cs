using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WheelOfFortune.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WheelOfFortune.Models;
using WheelOfFortune.Additionals;
using System.Text;

namespace WheelOfFortune.Controllers
{
    [AllowAnonymous]
    [Route("[controller]/[action]")]
    public class WheelGameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WheelGameController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var wheelGames = await _context.Wheels.ToListAsync();
            return View(wheelGames);
        }

        // GET: WheelGame/Play/5
        [HttpGet]
        public async Task<IActionResult> Play(string id)
        {
            int numericWheelId;
            if (id == null || !(int.TryParse(id, out numericWheelId)))
            {
                return NotFound();
            }

            var selectedWheel = await _context.Wheels.Include(s => s.Slices).SingleOrDefaultAsync(m => m.WheelId == numericWheelId);
            if (selectedWheel == null)
            {
                return NotFound();
            }

            return View();
        }



        [HttpGet]
        public JsonResult GetWheelPlay()
        {
            StreamReader sr = new StreamReader("wheel_data_1.json");
            string result = sr.ReadToEnd();
            result=result.Replace("\r\n", "");
            result = result.Replace("\\\\", "");
            return Json(result);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Spin([FromBody]JObject userdata)
        {
            StreamReader sr = new StreamReader("wheel_data_1.json");
            string result = sr.ReadToEnd();
            result = result.Replace("\r\n", "");
            result = result.Replace("\\\\", "");
            return Json(result);
        }
    }
}