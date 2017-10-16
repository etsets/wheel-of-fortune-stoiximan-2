using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WheelOfFortune.Admin.Models
{
    public class Wheel
    {
        [Key]
        public int WheelId { get; }

        [Required]
        [MaxLength(50), MinLength(6)]
        public string WheelName { get; set; }

        public String WheelDescription { get; set; } = String.Empty;

        [Required]
        public bool IsEnabled { get; set; } = true;


    }
}
