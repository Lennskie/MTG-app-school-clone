using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        private readonly UserPackService _packService = new UserPackService();
        private readonly UserCardService _userCardService = new UserCardService();
        
        [Authorize]
        [Route("/collection")]
        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            //List<Card> testList = convertList(userid); //had to remove this because the cleaner code makes for a stack overflow

            return View(new CollectionViewModel
            {
                PageTitle = "Cards",
                ColumnCardName = "Card Name",
                ColumnCardType = "Card Type",
                ColumnCardVariations = "Card Variations",
                ColumnCardInCollection = "Card Collection Status",
                
                Power = _cardService.GetPower(),
                Thoughness = _cardService.GetThoughness(),
                Rarity = _cardService.GetRarity(),
                ManaCost = _cardService.GetManaCosts(),
                Cards = _userCardService.GetUserCardsForUser(userId).Select(c => new CollectionCardViewModel
                {
                    CardId = (_cardService.GetCardFromUserTableId(c.CardId.ToString())).MtgId,
                    Name = (_cardService.GetCardFromUserTableId(c.CardId.ToString())).Name,
                    Type = (_cardService.GetCardFromUserTableId(c.CardId.ToString())).Type,
                    // TODO: Dynamically decide on the amount of variations for a card
                    Variations = 0,
                    InCollection = _userCardService.CheckPrecenceCardForUser(userId,(_cardService.GetCardFromUserTableId(c.CardId.ToString())).MtgId)
                }).ToList()
            });
        }

        
        [Authorize]
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
        

        [Authorize]
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