
namespace mtg_app.Models.Card;

public class CardsViewModel
{
    public string? PageTitle { get; set; }
    
    public string? ColumnCardName { get; set; }
    
    public string? ColumnCardType { get; set; }

    public List<CardViewModel>? Cards { get; set; }
    
    
}