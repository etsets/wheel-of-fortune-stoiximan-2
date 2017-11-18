using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelOfFortune.Models
{
    public class DepositResponse
    {
        public int status { get; set; }
        public string message { get; set; }

        public DepositResponse(int status, string message)
        {
            this.status = status;
            this.message = message;
        }
    }
}
