using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mtg_app.Views.Card;

public class SingleCard : PageModel
{
    public void OnGet()
    {
        Console.WriteLine("Testing!");
    }
}