
using Microsoft.AspNetCore.Mvc;

namespace mtg_app.Controllers;
[Route("")]
public class LandingController : Controller {

    [Route("")]
    [Route("[action]")]
    public IActionResult showLandingPage(){
        return View();
    }
}