using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WheelOfFortune.Admin.Models;
using WheelOfFortune.Admin.Data;

namespace WheelOfFortune.Admin.Controllers
{
    [Produces("application/json")]
    [Route("api/CreateCoupons")]
    public class CreateCouponsController : Controller
    {
        int length = 6 ;
        private static Random random = new Random();
        private readonly ApplicationDbContext _context;

         public CreateCouponsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CreateCoupons/5
        [HttpGet("{numberOfTickets}", Name = "Get")]
        public async Task<IEnumerable<string>> GetAsync(int numberOfTickets)
        {
            var existingCoupons = from s  in _context.Vouchers
                                            select s;

            List<string> coupons = new List<string>();
            List<Voucher> newCoupons = new List<Voucher>();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            while (coupons.Count < numberOfTickets)
            {
                string coupon = new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                Voucher voucher = new Voucher();
                voucher.Status = Voucher.VoucherStatus.New;
                voucher.VoucherCode = coupon;
                voucher.IsUsed = true;
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
            return couponsToReturn;
        }
        
        // POST: api/CreateCoupons
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/CreateCoupons/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{CouponId}")]
        public void Delete(int CouponId)
        {
        }
    }
}
