using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtg_app.Models.Card;
using mtg_lib.Library.Models;
using mtg_lib.Library.Services;

namespace mtg_app.Controllers
{

    [Route("[controller]")]
    public class CardSearchController : Controller
    {
        CardService cardService = new CardService();

        // Route: /SearchCard/Search?Name=<cardName>
        [Authorize]
        [Route("{cardName}")]
        public IActionResult SingleCard(string cardName)
        {
            Card? card = cardService.GetCardFromString(cardName);

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