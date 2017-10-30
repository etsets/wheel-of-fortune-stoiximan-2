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
    [Route("api/json")]
     //[Authorize(Roles = "Administrators")]
     public class WheelConfigController : Controller
    {
          // GET: api/json
          [HttpGet]
        public object GetWheelConfig()
        {
               WheelConfig wheelconfig = new WheelConfig();
               return wheelconfig.GetWheelConfig();
          }


          // POST api/json
          [HttpPost]
        public void PostWheelConfig([FromBody]JObject jsonObject)
          {
               WheelConfig wheelconfig = new WheelConfig();
               wheelconfig.PostWheelConfig(jsonObject);
          }

          
     }
}
