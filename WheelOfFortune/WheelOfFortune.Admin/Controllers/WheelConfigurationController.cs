using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;
using WheelOfFortune.Admin.Classes.Interfaces;
using WheelOfFortune.Admin.Classes.Implementations;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelOfFortune.Admin.Controllers
{
     [Authorize(Roles = "Admin")]
     public class WheelConfigurationController : Controller
    {
          private readonly IWheelConfig _wheelConfig;

          public WheelConfigurationController(IWheelConfig wheelConfig)
          {
               _wheelConfig = wheelConfig;
          }


          // GET: api/json
          [HttpGet]
          [Route("api/json")]
          [AllowAnonymous]
          public object GetWheelConfig()
          {
               return _wheelConfig.GetWheelConfig();
          }


          // POST api/json
          [Route("api/json")]
          [HttpPost]
          [ValidateAntiForgeryToken]
          public void PostWheelConfig([FromBody]JObject jsonObject)
          {
               _wheelConfig.PostWheelConfig(jsonObject);
               
          }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_wheelConfig.GetWheelConfig());
        }


     }
}
