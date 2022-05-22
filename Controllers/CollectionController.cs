using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using mtg_app.Models.Collection.OpenPacks;
using mtg_lib.Library.Models;
using mtg_lib.Library.Services;
using mtg_app.Models.Collection;
using mtg_app.Models.Collection.Packs;

namespace mtg_app.Controllers
{

    
    [Route("[controller]")]
    public class CollectionController : Controller
    {
        
        private readonly CardService _cardService = new CardService();
        private readonly PackService _packService = new PackService();
        private readonly UserCardService _userCardService = new UserCardService();
        
        [Route("[action]")]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            return View(new CollectionViewModel
            {
                PageTitle = "Cards",
                ColumnCardName = "Card Name",
                ColumnCardType = "Card Type",
                ColumnCardVariations = "Card Variations",
                ColumnCardInCollection = "Card Collection Status",
                
                Power = _cardService.getPower(),
                Thoughness = _cardService.getThoughness(),
                Rarity = _cardService.getRarity(),
                ManaCost = _cardService.getManaCosts(),
                Cards = _cardService.GetSetAmountOfCards(50).Select(c => new CollectionCardViewModel
                {
                    CardId = c.MtgId,
                    Name = c.Name,
                    Type = c.Type,
                    // TODO: Dynamically decide on the amount of variations for a card
                    Variations = 0,
                    InCollection = _userCardService.CheckPrecenceCardForUser(userId,c.MtgId)
                }).ToList()
            });
        }

        
        [Route("[action]")]
        public IActionResult Packs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            UserPack userPack = _packService.GetUserPackFromUserId(userId);
            
            return View(new CollectionPacksViewModel
                {
                    PackAmounts = userPack.Packs
                }
            );
        }
        

        [Route("packs/[action]")]
        public IActionResult OpenPack()
        {
            IEnumerable<Card> cardsInPack = _packService.CreateRandomPack("");
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            UserPack userPack = _packService.GetUserPackFromUserId(userId);

            if (userPack.Packs <= 0)
            {
                RedirectToAction("Index");
            }
            
            
            List<OpenPacksCardViewModel> filteredCardsInPack = cardsInPack.Select(c => new OpenPacksCardViewModel
            {
                CardId = c.MtgId,
                Name = c.Name,
                Type = c.Type,
                NewCard = _userCardService.CheckPrecenceCardForUser(userId,c.MtgId),
                RarityCode = c.RarityCode,
                ImageUrl = c.OriginalImageUrl
            }).ToList();

            IEnumerable<string> listIds = filteredCardsInPack.Select(c => c.CardId);

            _userCardService.AddCardsToUserCards(userId,listIds);
            _packService.DecreasePackCountUser(userId);

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