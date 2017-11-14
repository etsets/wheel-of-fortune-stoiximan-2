using Newtonsoft.Json.Linq;
using System.Net;

namespace WheelOfFortune.Admin.Services
{
     public interface IWheelConfig
     {
          JObject GetWheelConfig();
          HttpStatusCode PostWheelConfig(JObject json);
    }
}
