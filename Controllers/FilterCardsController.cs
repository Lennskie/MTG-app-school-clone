using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtg_app.Models.Card;
using mtg_lib.Library.Models;
using mtg_lib.Library.Services;

namespace mtg_app.Controllers
{

    [Route("[controller]")]
    public class FilterCardsController : Controller
    {

        CardService cardService = new CardService();
        // Route: /FilterCards/
        [Authorize]
        public IActionResult FilterCards(string rarity_code, string converted_mana_cost, string power, string thoughness)
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
                Cards = cardService.GetCardsByFilters(rarity_code, converted_mana_cost, power, thoughness).Select(c => new CardViewModel
                {
                    CardId = c.MtgId,
                    Name = c.Name,
                    Type = c.Type,
                    Variations = 0,
                    InCollection = false //check has to be added here
                }).ToList()
            });

        }
    }
}