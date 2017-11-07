using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WheelOfFortune.Admin.Models;
using WheelOfFortune.Admin.Data;
using WheelOfFortune.Admin.Additionals;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace WheelOfFortune.Admin.Controllers
{
    [Produces("application/json")]
    [Route("[controller]/[action]")]
    public class CreateCouponsController : Controller
    {
        public async Task<IActionResult> CouponsList(string sortOrder, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IsUsedSortParm"] = String.IsNullOrEmpty(sortOrder) ? "is_used_desc" : "";
            
            var coupons = from s in _context.Vouchers
                           select s;
            switch (sortOrder)
            {
                case "is_used_desc":
                    coupons = coupons.OrderByDescending(s => s.IsUsed);
                    break;
                default:
                    coupons = coupons.OrderBy(s => s.VoucherCode);
                    break;
            }

            int pageSize = 100;
            return View(await PaginatedList<Voucher>.CreateAsync(coupons.AsNoTracking(), page ?? 1, pageSize));
        }
        int length = 6 ;
        private static Random random = new Random();
        private readonly ApplicationDbContext _context;

         public CreateCouponsController(ApplicationDbContext context) 
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupons([FromBody] JObject numberOfTickets)
        {
            int numOfTickets = numberOfTickets.Value<int>("numberOfTickets");

            var existingCoupons = from s  in _context.Vouchers
                                            select s;

            List<string> coupons = new List<string>();
            List<Voucher> newCoupons = new List<Voucher>();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            while (coupons.Count < numOfTickets)
            {
                string coupon = new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                Voucher voucher = new Voucher();
                voucher.Status = Voucher.VoucherStatus.New;
                voucher.VoucherCode = coupon;
                voucher.IsUsed = false;
                voucher.CreditAmount = 10;

                if (!existingCoupons.Any(cpn => cpn.VoucherCode.Equals(coupon))){
                    newCoupons.Add(voucher);
                    existingCoupons.Append(voucher);
                    coupons.Insert(0,coupon); 
                }
            }

            _context.Vouchers.AddRange(newCoupons.AsEnumerable());
            await _context.SaveChangesAsync();

            IEnumerable<string> couponsToReturn = coupons.AsEnumerable();
            return View();
        }
        

        // POST: api/RedeemVoucher
        [HttpPost("{VoucherId}")]
        public async Task<bool> RedeemVoucher(int VoucherId)
        {
           Voucher coupon =  _context.Vouchers.
               Where(cpn => cpn.VoucherId == VoucherId).SingleOrDefault();
            if (coupon != null)
            {
                coupon.IsUsed = true;
                _context.Vouchers.Update(coupon);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public async Task DeleteVouchers([FromBody] JObject NumOfCoupons)
        {
            int numberOfCoupons = NumOfCoupons.Value<int>("NumOfCoupons");
            List<Voucher> existingCoupons = (from s in _context.Vouchers
                                  select s).ToList<Voucher>();
            List<Voucher> couponsToRemove = new List<Voucher>();
            Voucher couponToRemove;
            while (numberOfCoupons != 0 ) {
                couponToRemove = existingCoupons.FirstOrDefault(cpn => cpn.IsUsed);
                if (couponToRemove == null)
                {
                    numberOfCoupons = 0;
                }
                else
                {
                    existingCoupons.Remove(couponToRemove);
                    couponsToRemove.Append(couponToRemove);
                }
            }
            _context.Vouchers.RemoveRange(couponsToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
