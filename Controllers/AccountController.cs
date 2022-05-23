
using Microsoft.AspNetCore.Mvc;


namespace mtg_app.Controllers;

[Route("[controller]")]
public class AccountController : Controller {

    [Route("")]
    [Route("[action]")]
    public IActionResult Index(){
        return View();
    }
}