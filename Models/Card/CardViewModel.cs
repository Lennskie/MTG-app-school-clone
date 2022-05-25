namespace mtg_app.Models.Card;

public class CardViewModel
{
    
    public string? CardId { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    
    public string? Rarity { get; set; }
    
    public string? ManaCost { get; set; }
    
    public string? Power { get; set; }

    public bool InCollection { get; set; }
    
}