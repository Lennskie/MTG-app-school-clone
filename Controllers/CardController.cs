using System.Security.Claims;
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

        public readonly CardService _cardService = new CardService();
        public readonly UserCardService _userCardService = new UserCardService();


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
                
                ColumnCardRarity = "Card Rarity",
                ColumnCardManaCost = "Card Mana Cost",
                ColumnCardPower = "Card Power",
                
                ColumnCardInCollection = "Card Collection Status",
                
                Power = _cardService.GetPower(),
                Thoughness = _cardService.GetThoughness(),
                Rarity = _cardService.GetRarity(),
                ManaCost = _cardService.GetManaCosts(),
                Cards = _cardService.GetSetAmountOfCards(50).Select(c => new CardViewModel
                {
                    CardId = c.MtgId,
                    Name = c.Name,
                    
                    Rarity = c.RarityCode,
                    ManaCost = c.ConvertedManaCost,
                    Power = c.Power,
                    
                    Type = c.Type,
                    InCollection = false
                }).ToList()
            });

        }


        // Route: /card/<cardId>
        [Route("/cards/{cardId}")]
        public IActionResult SingleCard(string cardId)
        {
            Card? card = _cardService.GetCardFromId(cardId);
            
            int? cardsInUserCollection = 0;

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                IEnumerable<UserCard> userCards = _userCardService.GetUserCardsForUser(userId);
                UserCard selectedCard = userCards.Where(c => c.CardId == card.Id).FirstOrDefault();
                if (selectedCard != null)
                {
                    cardsInUserCollection = selectedCard.Cards;
                }

            } 
            
            

            if (card?.OriginalImageUrl == null)
            {
                card.OriginalImageUrl = _cardService.GetImageFromVariations(card);
            }
            
            
            return View(new SingleCard
            {
                ImageUrl = card?.OriginalImageUrl,
                Name = card?.Name,
                Type = card?.Type,
                CardsInCollection = cardsInUserCollection,
                Variations = _cardService.RetrieveMtgIdsFromString(card)
            });


        }
    }
}