using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WheelOfFortune.Admin.Models
{
    public class Voucher
    {
        [Key]
        public int VoucherId { get; }

        [Required]
        [MaxLength(6), MinLength(6)]
        public string VoucherCode { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float CreditAmount { get; set; }

        [Required]
        public bool IsUsed { get; set; }

        [Required]
        public VoucherStatus Status { get; set; } = VoucherStatus.New;

        public enum VoucherStatus : int
        {
            Revoked = 0,
            New = 1,
            Available = 2
        }
    }
}
