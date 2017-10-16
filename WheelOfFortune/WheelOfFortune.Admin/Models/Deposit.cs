using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WheelOfFortune.Admin.Models
{
    public class Deposit
    {
        [Key]
        [ForeignKey("HistoryEntry")]
        public string EntryId { get; set; }

        [Key]
        [ForeignKey("Voucher")]
        public string VoucherId { get; set; }
    }
}
