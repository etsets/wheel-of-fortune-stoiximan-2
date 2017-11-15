using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System.Net;

namespace WheelOfFortune.Admin.Services
{
     public class WheelConfig : IWheelConfig
     {
          public JObject GetWheelConfig()
          {
               string allText = System.IO.File.ReadAllText(@"Data/wheel_data.json");
               Object jsonObject = JsonConvert.DeserializeObject(allText);
               return (JObject)jsonObject;
          }

          public HttpStatusCode PostWheelConfig(JObject jsonObject)
          {
               try
               {
                    string allText = System.IO.File.ReadAllText(@"Data/wheel_data_schema.json");
                    JSchema schema = JSchema.Parse(allText);
                    if (jsonObject.IsValid(schema))
                    {
                         String s = JsonConvert.SerializeObject(jsonObject);
                         String[] file = new[] { s };
                         System.IO.File.WriteAllLines(@"Data/wheel_data.json", file);
                         Console.WriteLine("VALID SCHEMA");
                         return HttpStatusCode.OK;
                    }
                    else
                    {
                         return HttpStatusCode.BadRequest;
                    }
                    
               }

               catch(ArgumentNullException e)
               {
                    Console.WriteLine(e);
                    return HttpStatusCode.UnsupportedMediaType;
               }

               catch (Exception e)
               {
                    Console.WriteLine(e);
                    return HttpStatusCode.InternalServerError;
               }               
          }
     }
}
