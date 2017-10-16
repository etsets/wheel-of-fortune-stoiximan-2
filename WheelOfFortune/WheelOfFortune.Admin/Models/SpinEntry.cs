using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WheelOfFortune.Admin.Models
{
    public class SpinEntry
    {
        [Key]
        [ForeignKey("HistoryEntry")]
        public string EntryId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float BetAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float ResultAmount { get; set; }


    }
}
