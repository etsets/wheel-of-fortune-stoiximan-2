using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WheelOfFortune.Models
{
    public class SpinEntry
    {
        [Key]
        public int HistoryEntryId { get; set; }
        [ForeignKey("HistoryEntryId")]
        public HistoryEntry BelongsToHistoryEntry { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float BetAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float ResultAmount { get; set; }
    }
}
