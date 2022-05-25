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
        [Route("/cards")]
        public IActionResult Index()
        {
            return View(new CardsViewModel
            {
                PageTitle = "Cards",
                ColumnCardName = "Card Name",
                ColumnCardType = "Card Type",
                ColumnCardVariations = "Card Variations",
                ColumnCardInCollection = "Card Collection Status",
                Power = cardService.GetPower(),
                Thoughness = cardService.GetThoughness(),
                Rarity = cardService.GetRarity(),
                ManaCost = cardService.GetManaCosts(),
                Cards = cardService.GetSetAmountOfCards(50).Select(c => new CardViewModel
                {
                    CardId = c.MtgId,
                    Name = c.Name,
                    Type = c.Type,
                    // TODO: Dynamically decide on the amount of variations for a card
                    Variations = 0,
                    InCollection = false
                }).ToList()
            });

        }


        // Route: /card/<cardId>
        [Authorize]
        [Route("/cards/{cardId}")]
        public IActionResult SingleCard(string cardId)
        {
            Card? card = cardService.GetCardFromId(cardId);

            if (card?.OriginalImageUrl == null)
            {
                //Console.WriteLine("Retrieving new imageUrl");
                if (card != null) card.OriginalImageUrl = cardService.GetImageFromVariations(card);
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