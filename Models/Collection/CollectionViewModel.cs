
namespace mtg_app.Models.Collection;

public class CollectionViewModel
{
    public string? PageTitle { get; set; }
    
    public string? ColumnCardName { get; set; }
    
    public string? ColumnCardType { get; set; }

    public string? ColumnCardInCollection { get; set; }
    
    public string? ColumnCardVariations { get; set; }
    
    public List<CollectionCardViewModel>? Cards { get; set; }

    public List<int> Power { get; set; }

    public List<int> Thoughness { get; set; }

    public List<int> ManaCost { get; set; }

    public List<String> Rarity { get; set; }
    
    
}