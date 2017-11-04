﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WheelOfFortune.Data;

namespace WheelOfFortune.Controllers
{
    public class PlayController : Controller
    {

          [Authorize]
          [ResponseCache(NoStore = true, Duration = 0)]
          public IActionResult Index()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }


    }
}