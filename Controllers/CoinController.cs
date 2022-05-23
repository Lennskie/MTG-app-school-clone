
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtg_app.Models.Coin;
using mtg_lib.Library.Services;

namespace mtg_app.Controllers
{
    
    [Route("[controller]")]
    public class CoinController : Controller
    {
        
        private readonly UserCoinService _userCoinService = new UserCoinService();

        
        [Authorize]
        [Route("")]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            bool receivedFreeCoins = _userCoinService.CheckIfReceivedDefaultCoins(userId);
            bool receivedDailyCoins = _userCoinService.CheckDailyCoinsClaimed(userId);
            int userCoinBalance = _userCoinService.GetUserCoinBalance(userId);
            
            
            return View(new CoinViewModel
            {
                UserCoinBalance = userCoinBalance,
                ReceivedDefaultCoins = receivedFreeCoins,
                ReceivedDailyCoins = receivedDailyCoins
            });
        }

        
        [Authorize]
        [Route("[action]")]
        public IActionResult ReceiveFreeCoins()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _userCoinService.ReceiveDefaultCoins(userId);
            
            return RedirectToAction("Index");
        }

        
        [Authorize]
        [Route("[action]")]
        public IActionResult ReceiveDailyCoins()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _userCoinService.AddDailyCoins(userId);

            return RedirectToAction("Index");
        }
        
    }
}