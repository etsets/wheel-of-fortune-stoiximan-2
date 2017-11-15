using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using WheelOfFortune.Admin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelOfFortune.Admin.Controllers
{

     [Route("api/json")]
     [Authorize(Roles = "Administrators")]

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
               //WheelConfig wheelconfig = new WheelConfig();
               return _wheelConfig.GetWheelConfig();
          }


          // POST api/json
          [Route("api/json")]
          [HttpPost]
          public IActionResult PostWheelConfig([FromBody]JObject jsonObject)
          {
               //WheelConfig wheelconfig = new WheelConfig();
               return StatusCode((int) _wheelConfig.PostWheelConfig(jsonObject));
               //return _wheelConfig.PostWheelConfig(jsonObject);

          }

          public IActionResult Index()
          {

               //var json = GetWheelConfig();
               return View(_wheelConfig.GetWheelConfig());
          }


     }
}
