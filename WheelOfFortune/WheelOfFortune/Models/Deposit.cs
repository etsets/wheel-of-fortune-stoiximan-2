using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WheelOfFortune.Models
{
    public class DepositEntry
    {
        public int HistoryEntryId { get; set; }
        [ForeignKey("HistoryEntryId")]
        public HistoryEntry BelongsToHistoryEntry { get; set; }

        public int VoucherId { get; set; }
        [ForeignKey("VoucherId")]
        public Voucher BelongsToVoucherEntry { get; set; }
    }
}
