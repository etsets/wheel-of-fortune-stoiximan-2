﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WheelOfFortune.Admin.Models
{
    public class HistoryEntry
    {
        public enum EntryType : int
        {
            Spin = 1,
            Deposit = 2
        }

        [Key]
        public int HistoryEntryId { get; set; }

        [Required]
        public ApplicationUser CreatedBy { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TimeOccurred { get; set; }

        [Required]
        public EntryType HistoryEntryTypeId { get; set; }
    }
}
