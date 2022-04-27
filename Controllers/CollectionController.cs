
using Microsoft.AspNetCore.Mvc;

namespace mtg_app.Controllers
{
    [Route("")]
    [Route("[controller]")]
    public class CollectionController : Controller
    {
        public IActionResult Collection()
        {
            return View();
        }

    }
}