using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using WheelOfFortune.Admin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelOfFortune.Admin.Controllers
{
     [Route("api/json")]
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

          public JObject GetWheelConfig()
          {
               return _wheelConfig.GetWheelConfig();
          }


          // POST api/json
          [Route("api/json")]
          [HttpPost]
          [ValidateAntiForgeryToken] 
          public IActionResult PostWheelConfig([FromBody]JObject jsonObject)
          {
               return StatusCode((int) _wheelConfig.PostWheelConfig(jsonObject));
          }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_wheelConfig.GetWheelConfig());
        }


     }
}
