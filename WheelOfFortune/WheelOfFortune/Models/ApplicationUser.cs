using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace WheelOfFortune.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(20)]
        public String Firstname { get; set; }

        [Required]
        [StringLength(50)]
        public String Lastname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        //[DataType(DataType.Upload)]
        [StringLength(255)]
        public String Photo { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? LastLogin { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float Balance { get; set; } = 100.0F;

        [InverseProperty("CreatedBy")]
        public ICollection<HistoryEntry> HistoryEntries { get; set; }
    }


}
