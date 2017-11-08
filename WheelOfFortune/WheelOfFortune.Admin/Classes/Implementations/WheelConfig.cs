using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WheelOfFortune.Admin.Classes.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;


namespace WheelOfFortune.Admin.Classes.Implementations
{
     public class WheelConfig : IWheelConfig
     {
          public object GetWheelConfig()
          {
               string allText = System.IO.File.ReadAllText(@"Data/wheel_data.json");
               //Console.WriteLine("New GET request");
               object jsonObject = JsonConvert.DeserializeObject(allText);
               return jsonObject;
          }

          public Boolean PostWheelConfig(JObject jsonObject)
          {
               string allText = System.IO.File.ReadAllText(@"Data/wheel_data_schema.json");
               JSchema schema = JSchema.Parse(allText);
               //Console.WriteLine($"New POST request " + jsonObject);
               if (jsonObject.IsValid(schema)) {
                    String s = JsonConvert.SerializeObject(jsonObject);
                    String[] file = new[] { s };
                    System.IO.File.WriteAllLines(@"Data/wheel_data.json", file);
                    Console.WriteLine("VALID SCHEMA");
                    return true;
               }
               Console.WriteLine("INVALID SCHEMA");
               return false;

          }
     }
}
