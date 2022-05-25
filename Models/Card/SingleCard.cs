namespace mtg_app.Models.Card;

public class SingleCard
{
    
    public string? ImageUrl { get; set; }
    
    public string? Name { get; set; }
    
    public string? Type { get; set; }
    
    public int? CardsInCollection { get; set; }
    
    public List<string>? Variations { get; set; }
    
}