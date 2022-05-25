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
                
                ColumnCardRarity = "Card Rarity",
                ColumnCardManaCost = "Card Mana Cost",
                ColumnCardPower = "Card Power",

                ColumnCardInCollection = "Card Collection Status",

                Power = _cardService.GetPower(),
                Thoughness = _cardService.GetThoughness(),
                Rarity = _cardService.GetRarity(),
                ManaCost = _cardService.GetManaCosts(),
                Cards = _userCardService.RetrieveCardsInUserCollection(userId).Select(c => new CollectionCardViewModel
                {
                    CardId = c.MtgId,
                    Name = c.Name,
                    Type = c.Type,
                    
                    Rarity = c.RarityCode,
                    ManaCost = c.ConvertedManaCost,
                    Power = c.Power,
                    
                    InCollection = true //no service call here because everything displayed here is part from your collection
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

            IEnumerable<string?> listIds = filteredCardsInPack.Select(c => c.CardId);

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
        
        
        [Route("SearchCollectionCard/")]
        [Authorize]
        public IActionResult SearchCollectionCard(string Name)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(new CollectionViewModel
            {
                PageTitle = "Cards",
                ColumnCardName = "Card Name",
                ColumnCardType = "Card Type",
                ColumnCardInCollection = "Card Collection Status",
                
                Power = _cardService.GetPower(),
                Thoughness = _cardService.GetThoughness(),
                Rarity = _cardService.GetRarity(),
                ManaCost = _cardService.GetManaCosts(),
                Cards = _userCardService.GetCardFromString(Name, userId).Select(c => new CollectionCardViewModel
                {
                    CardId = c.MtgId,
                    Name = c.Name,
                    Type = c.Type,
                    InCollection = true //no service call here because everything displayed here is part from your collection
                }).ToList()
            });
        }

        [Route("FilterCollectionCard/")]
        [Authorize]
        public IActionResult FilterCollectionCard(string rarity_code, string converted_mana_cost, string power, string thoughness)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(new CollectionViewModel
            {
                PageTitle = "Cards",
                ColumnCardName = "Card Name",
                ColumnCardType = "Card Type",
                ColumnCardInCollection = "Card Collection Status",
                
                Power = _cardService.GetPower(),
                Thoughness = _cardService.GetThoughness(),
                Rarity = _cardService.GetRarity(),
                ManaCost = _cardService.GetManaCosts(),
                Cards = _userCardService.GetCardsByFilters(rarity_code, converted_mana_cost, power, thoughness, userId).Select(c => new CollectionCardViewModel
                {
                    CardId = c.MtgId,
                    Name = c.Name,
                    Type = c.Type,
                    InCollection = true //no service call here because everything displayed here is part from your collection
                }).ToList()
            });
        }
    }
}