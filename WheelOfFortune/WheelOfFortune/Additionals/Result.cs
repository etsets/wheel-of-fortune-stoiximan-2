using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelOfFortune.Additionals
{
    public class Result
    {
        public int probability { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public bool win { get; set; }
        public string resultText { get; set; }
        public UserData userData { get; set; }
    }

    public class UserData
    {
        public string score { get; set; } 
    }
}
