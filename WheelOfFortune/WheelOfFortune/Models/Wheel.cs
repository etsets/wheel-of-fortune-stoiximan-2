using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WheelOfFortune.Models
{
    public class Wheel
    {
        [Key]
        public int WheelId { get; set; }

        [Required]
        [MaxLength(50), MinLength(6)]
        public String WheelName { get; set; }

        public String WheelDescription { get; set; } = String.Empty;

        [Required]
        public bool IsEnabled { get; set; } = true;

        public ICollection<WheelSlice> Slices { get; set; }
    }
}
