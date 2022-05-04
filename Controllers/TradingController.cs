using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//using mtg_lib.Library.Services;

namespace mtg_app.Controllers;
[Route("[controller]")]
public class TradingController : Controller {

    // Route: /Trading/ 
    //TODO: fix this Route
    [Authorize]
    [Route("")]
    [Route("[action]")]
    public IActionResult Index(){
        return View();
    }
}