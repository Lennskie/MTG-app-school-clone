namespace mtg_app.Models.Collection.OpenPacks;

public class OpenPacksCardViewModel
{
    public string? CardId { get; set; }
    
    public string? Name { get; set; }
    
    public string? Type { get; set; }
    
    
    public string? RarityCode { get; set; }

    public bool NewCard { get; set; }
    
    public string? ImageUrl { get; set; }
}