using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using WheelOfFortune.Admin.Services;

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
          [AllowAnonymous]

          public JObject GetWheelConfig()
          {
               return _wheelConfig.GetWheelConfig();
          }


          // POST api/json
          [HttpPost]
          [ValidateAntiForgeryToken] 
          public IActionResult PostWheelConfig([FromBody]JObject jsonObject)
          {
               return StatusCode((int) _wheelConfig.PostWheelConfig(jsonObject));
          }

          [Route("/WheelConfiguration")]
          public IActionResult Index()
          {

               //var json = GetWheelConfig();
               return View(_wheelConfig.GetWheelConfig());
          }

     }
}
