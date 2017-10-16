using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace WheelOfFortune.Admin.Models
{
    public class WheelSlice
    {
        [Key]
        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SliceId { get; set; }

        [ForeignKey("WheelId")]
        public Wheel ContainerWheel { get; set; }


        [Required]
        public int SliceOrder { get; set; }

        [Required]
        public int ProbabilityPercent { get; set; }

        [Required]
        [MaxLength(6), MinLength(6)]
        public String ColorHexCode { get; set; }

        [Required]
        [MaxLength(20), MinLength(1)]
        public String Label { get; set; }

        [Required]
        public float Factor { get; set; }
    }
}
