using Microsoft.AspNetCore.Mvc;
using mtg_app.Models.Collection.OpenPacks;
using mtg_lib.Library.Models;
using mtg_lib.Library.Services;
using mtg_app.Models.Collection;

namespace mtg_app.Controllers
{

    
    [Route("[controller]")]
    public class CollectionController : Controller
    {
        
        CardService cardService = new CardService();
        private PackService packService = new PackService();
        
        [Route("")]
        [Route("[action]")]
        public IActionResult Collection()
        {
            return View(new CollectionViewModel
            {
                PageTitle = "Cards",
                ColumnCardName = "Card Name",
                ColumnCardType = "Card Type",
                ColumnCardVariations = "Card Variations",
                ColumnCardInCollection = "Card Collection Status",
                Cards = cardService.GetSetAmountOfCards(50).Select(c => new CollectionCardViewModel
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

        
        [Route("[action]")]
        public IActionResult Packs()
        {
            return View();
        }
        
        [Route("packs/[action]")]
        public IActionResult OpenPack()
        {
            IEnumerable<Card> cardsInPack = packService.CreateRandomPack("");

            List<OpenPacksCardViewModel> filteredCardsInPack = cardsInPack.Select(c => new OpenPacksCardViewModel
            {
                CardId = c.MtgId,
                Name = c.Name,
                Type = c.Type,
                NewCard = true,
                RarityCode = c.RarityCode,
                ImageUrl = c.OriginalImageUrl,
            }).ToList();

            return View(new OpenPacksViewModel
            {
                PageTitle = "Open Pack",
                ColumnCardName = "Card Name",
                ColumnCardType = "Card Type",
                ColumnRarity = "Card Rarity Code",
                ColumnNewCard = "New Card",
                Cards = filteredCardsInPack
            });
        }
        
        
        

    }
}