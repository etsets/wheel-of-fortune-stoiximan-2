using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WheelOfFortune.Models;

namespace WheelOfFortune.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Wheel> Wheels { get; set; }
        public DbSet<WheelSlice> WheelSlices { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<DepositEntry> DepositEntries { get; set; }
        public DbSet<SpinEntry> SpinEntries { get; set; }
        public DbSet<HistoryEntry> HistoryEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Wheel>().ToTable("Wheels");
            builder.Entity<WheelSlice>().ToTable("WheelSlices");
            builder.Entity<Voucher>().ToTable("Vouchers");
            builder.Entity<DepositEntry>().ToTable("DepositEntries");
            builder.Entity<SpinEntry>().ToTable("SpinEntries");
            builder.Entity<HistoryEntry>().ToTable("HistoryEntries");

            builder.Entity<DepositEntry>()
                .HasKey(d => new { d.HistoryEntryId, d.VoucherId });
        }

        public DbSet<ApplicationUser> Gamers { get; set; }
    }
}
