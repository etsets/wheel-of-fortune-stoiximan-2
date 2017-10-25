using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WheelOfFortune.Admin.Models;

namespace WheelOfFortune.Admin.Controllers
{
    [Produces("application/json")]
    [Route("api/CreateCoupons")]
    public class CreateCouponsController : Controller
    {
        int length = 6 ;
        private static Random random = new Random();
        // GET: api/CreateCoupons
        [HttpGet]
        public string Get()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // GET: api/CreateCoupons/5
        [HttpGet("{numberOfTickets}", Name = "Get")]
        public IEnumerable<string> Get(int numberOfTickets)
        {
            List<string> coupons = new List<string>();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for (int i = 0 ; i<= numberOfTickets; i++)
            {
                string coupon = new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                Voucher voucher = new Voucher();
                voucher.Status = Voucher.VoucherStatus.New;
                voucher.VoucherCode = coupon;
                voucher.IsUsed = true;
                voucher.CreditAmount = 10;
                coupons.Insert(0,coupon); 
            }
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
