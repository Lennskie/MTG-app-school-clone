using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtg_app.Models.Card;
using mtg_lib.Library.Models;
using mtg_lib.Library.Services;

namespace mtg_app.Controllers
{


    [Route("[controller]")]
    public class CardController : Controller
    {

        CardService cardService = new CardService();


        // Route: /card/
        [Authorize]
        [Route("")]
        [Route("[action]")]
        public IActionResult Index()
        {
            return View(new CardsViewModel
            {
                PageTitle = "Cards",
                ColumnCardName = "Card Name",
                ColumnCardType = "Card Type",
                Cards = cardService.GetSetAmountOfCards(50).Select(c => new CardViewModel
                {
                    CardId = c.MtgId,
                    Name = c.Name,
                    Type = c.Type
                }).ToList()
            });

        }


        // Route: /card/<cardId>
        [Authorize]
        [Route("{cardId}")]
        public IActionResult SingleCard(string cardId)
        {
            Card? card = cardService.GetCardFromId(cardId);

            return View(new SingleCard
            {
                ImageUrl = card?.OriginalImageUrl,
                Name = card?.Name,
                Type = card?.Type
            });


        }
    }
}