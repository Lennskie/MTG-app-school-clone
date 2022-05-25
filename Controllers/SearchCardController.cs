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
        CardService _cardService = new CardService();
        
        [Authorize]
        public IActionResult SearchCard(string Name)
        {
            return View(new CardsViewModel
            {
                PageTitle = "Cards",
                ColumnCardName = "Card Name",
                ColumnCardType = "Card Type",
                ColumnCardRarity = "Card Rarity",
                ColumnCardManaCost = "Card Mana Cost",
                ColumnCardPower = "Card Power",
                ColumnCardInCollection = "Card Collection Status",
                Power = _cardService.GetPower(),
                Thoughness = _cardService.GetThoughness(),
                Rarity = _cardService.GetRarity(),
                ManaCost = _cardService.GetManaCosts(),
                Cards = _cardService.GetCardFromString(Name).Select(c => new CardViewModel
                {
                    CardId = c.MtgId,
                    Name = c.Name,
                    Type = c.Type,
                    Rarity = c.RarityCode,
                    ManaCost = c.ConvertedManaCost,
                    Power = c.Power,
                    InCollection = false
                }).ToList()
            });
        }
    }
}






