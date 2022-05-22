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
        
        [Route("")]
        [Route("[action]")]
        public IActionResult Collection()
        {
            Console.WriteLine("Before retrieving userID");
            
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = "";

            Console.WriteLine("After retrieving userID");
            
            return View(new CollectionViewModel
            {
                PageTitle = "Cards",
                ColumnCardName = "Card Name",
                ColumnCardType = "Card Type",
                ColumnCardVariations = "Card Variations",
                ColumnCardInCollection = "Card Collection Status",
                
                Power = new List<int>(), //_cardService.getPower(),
                Thoughness = new List<int>(), // _cardService.getThoughness(),
                Rarity = new List<string>(), //_cardService.getRarity(),
                ManaCost = new List<int>(), //_cardService.getManaCosts(),
                Cards = _cardService.GetUserCardCollection(userId,50).Select(c => new CollectionCardViewModel
                {
                    CardId = c.MtgId,
                    Name = c.Name,
                    Type = c.Type,
                    // TODO: Dynamically decide on the amount of variations for a card
                    Variations = 0,
                    InCollection = true //_userCardService.CheckPrecenceCardForUser(userId,c.MtgId)
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
                RedirectToAction("Collection");
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