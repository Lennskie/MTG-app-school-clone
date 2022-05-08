using Microsoft.Build.Framework;

namespace mtg_app.Models.WebShop;

public class AddPackToCartModel
{
    
    [Required]
    public int AmountOfPacks {get; set;}
    
}