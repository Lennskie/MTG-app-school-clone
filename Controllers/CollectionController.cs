using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mtg_app.Controllers
{
    [Route("[controller]")]
    public class CollectionController : Controller
    {
        // Route: /Collection/ 
        //TODO: fix this Route
        [Authorize]
        [Route("")]
        [Route("[action]")]

        public IActionResult showTradinshowCollectionPage(){
            return View();
        }

    }
}