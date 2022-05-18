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
            Console.WriteLine(rarity_code + " " + converted_mana_cost + " " + power + " " + thoughness);
            return View(new CardsViewModel
            {
                PageTitle = "Cards",
                ColumnCardName = "Card Name",
                ColumnCardType = "Card Type",
                ColumnCardVariations = "Card Variations",
                ColumnCardInCollection = "Card Collection Status",
                Cards = cardService.GetCardsByFilters(rarity_code, converted_mana_cost, power, thoughness).Select(c => new CardViewModel //change this to the service that uses the filter
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