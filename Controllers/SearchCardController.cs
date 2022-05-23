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
        public IActionResult SearchCard(string Name)
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
                Cards = cardService.GetCardFromString(Name).Select(c => new CardViewModel
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
    }
}






