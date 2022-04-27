using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mtg_app.Models;
//using mtg_lib.Library.Services;

namespace mtg_app.Controllers;
public class AccountController : Controller {

    [Route("[action]")]
    public IActionResult showAccountPage(){
        return View();
    }
}