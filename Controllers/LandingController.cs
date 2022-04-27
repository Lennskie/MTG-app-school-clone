using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mtg_app.Models;
//using mtg_lib.Library.Services;

namespace mtg_app.Controllers;
[Route("")]
public class LandingController : Controller {

    [Route("")]
    [Route("[action]")]
    public IActionResult showLandingPage(){
        return View();
    }
}