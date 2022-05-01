namespace mtg_app.Models.Collection.OpenPacks;

public class OpenPacksViewModel
{
    
    public string? PageTitle { get; set; }
    
    public string? ColumnCardName { get; set; }
    
    public string? ColumnCardType { get; set; }
    
    public string? ColumnNewCard { get; set; }
    
    public string? ColumnRarity { get; set; }

    public List<OpenPacksCardViewModel>? Cards { get; set; }
    
}