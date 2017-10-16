using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WheelOfFortune.Admin.Models
{
    public class HistoryEntry
    {
        [Key]
        public int EntryId { get; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TimeOccurred { get; set; }

        [Required]
        public EntryType HistoryEntryTypeId { get; set; }


        public enum EntryType : int
        {
            Spin = 1, 
            Deposit = 2
        }

    }
}
