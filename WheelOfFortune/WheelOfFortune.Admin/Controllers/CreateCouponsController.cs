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
using Microsoft.AspNetCore.Authorization;

namespace WheelOfFortune.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [Route("[controller]/[action]")]
    public class CreateCouponsController : Controller
    {
        int length = 6 ;
        private static Random random = new Random();
        private readonly ApplicationDbContext _context;

        public CreateCouponsController(ApplicationDbContext context) 
        {
            _context = context;
        }

        [HttpGet]
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCoupons([FromBody] JObject numberOfTickets)
        {
            int numOfTickets = numberOfTickets.Value<int>("numberOfTickets");
            
            int createdCoupons = 0;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            while (createdCoupons < numOfTickets)
            {
                string coupon = new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                Voucher voucher = new Voucher();
                voucher.Status = Voucher.VoucherStatus.New;
                voucher.VoucherCode = coupon;
                voucher.IsUsed = false;
                voucher.CreditAmount = 10;

                if (!_context.Vouchers.Any(cpn => cpn.VoucherCode.Equals(coupon))){
                    createdCoupons++;
                    _context.Vouchers.Add(voucher);
                    await _context.SaveChangesAsync();

                }
            }
            return View();
        }
        

        // POST: api/RedeemVoucher
        [HttpPost("{VoucherId}")]
        //[ValidateAntiForgeryToken]
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
        //[ValidateAntiForgeryToken]
        public async Task DeleteVouchers([FromBody] JObject NumOfCoupons)
        {
            int numberOfCoupons = NumOfCoupons.Value<int>("NumOfCoupons");
            Voucher couponToEdit;
            while (numberOfCoupons != 0 ) {
                couponToEdit = _context.Vouchers.FirstOrDefault(cpn => !cpn.IsUsed && cpn.Status.Equals(Voucher.VoucherStatus.New));
                if (couponToEdit == null)
                {
                    numberOfCoupons = 0;
                }
                else
                {
                    numberOfCoupons--;
                    couponToEdit.Status = Voucher.VoucherStatus.Revoked;
                    await _context.SaveChangesAsync();
                
                }
            }
        }
    }
}
