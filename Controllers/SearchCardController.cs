using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtg_app.Models.Card;
using mtg_lib.Library.Models;
using mtg_lib.Library.Services;

namespace mtg_app.Controllers
{

    [Route("[controller]")]
    public class SearchCardController : Controller
    {
        CardService cardService = new CardService();
        [Authorize]

        public IActionResult SingleCard(string Name)
        {
            Card? card = cardService.GetCardFromString(Name);

            Console.WriteLine(Name);

            if (card?.OriginalImageUrl == null)
            {
                //Console.WriteLine("Retrieving new imageUrl");
                card.OriginalImageUrl =  cardService.GetImageFromVariations(card);
            }
            
            return View(new SingleCard
            {
                ImageUrl = card?.OriginalImageUrl,
                Name = card?.Name,
                Type = card?.Type,
                Variations = cardService.RetrieveMtgIdsFromString(card)
            });
        }
    }
}