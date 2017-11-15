using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelOfFortune.Admin.Classes.Interfaces
{
     public interface IWheelConfig
     {
          Object GetWheelConfig();
          Boolean PostWheelConfig(JObject json);
    }
}
