using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelOfFortune.Admin.Classes.Interfaces
{
     interface IWheelConfig
     {
          Object GetWheelConfig();
          void PostWheelConfig(JObject json);
    }
}
