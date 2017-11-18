using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WheelOfFortune.Data;
using WheelOfFortune.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WheelOfFortune.Controllers
{
    [Authorize]

    public class DepositController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext context_;


        public DepositController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            context_ = context;
            _userManager = userManager;

        }


        public async Task<IActionResult> Index()
        {
            var current_user = await _userManager.GetUserAsync(HttpContext.User);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Usecoupon(string vouchercode)
        {
            var current_user = await _userManager.GetUserAsync(HttpContext.User);

            var voucher = context_.Set<Voucher>()
                .Where(c => c.VoucherCode.Equals(vouchercode, StringComparison.OrdinalIgnoreCase) && c.IsUsed == false && c.Status == Voucher.VoucherStatus.New)
                .SingleOrDefault();

            if (voucher != null)
            {

                using (var transaction = context_.Database.BeginTransaction())
                {

                    try
                    {
                        var amount = voucher.CreditAmount;

                        HistoryEntry NewHistoryEntry = new HistoryEntry
                        {
                            CreatedBy = current_user,
                            HistoryEntryTypeId = HistoryEntry.EntryType.Deposit,
                            TimeOccurred = DateTime.Now
                        };


                        context_.Add(NewHistoryEntry);
                        context_.SaveChanges();

                        DepositEntry entry = new DepositEntry
                        {
                            HistoryEntryId = NewHistoryEntry.HistoryEntryId,
                            VoucherId = voucher.VoucherId
                        };

                        context_.Add(entry);


                        current_user.Balance = current_user.Balance + voucher.CreditAmount;
                        voucher.IsUsed = true;
                        
                        

                        context_.Update(current_user);
                        context_.Update(voucher);

                        context_.SaveChanges();

                        transaction.Commit();

                        return new JsonResult(new DepositResponse(1, voucher.CreditAmount + "&euro; was added to your account.", current_user.Balance));
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();

                        return new JsonResult(new DepositResponse(-2, "An unkknown error occured, please try again later", 0));
                    }

                }
            }
            else
            {

                return new JsonResult(new DepositResponse(-1, "Voucher not found", 0));
            }

        }
    }
}