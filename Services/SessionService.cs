using System.Text.Json;

namespace mtg_app.Services;

public class SessionService
{

    public void StoreCartPacks(ISession session, int amountOfPacks)
    {
        int currentAmountOfPacks = GetAmountOfPacksCart(session);
        
        storeSessionData(session, "Cart", currentAmountOfPacks + amountOfPacks);
    }


    public int GetAmountOfPacksCart(ISession session)
    {
        string? amountOfPacksString = session.GetString("Cart");

        if (amountOfPacksString == null)
        {
            return 0;
        }
        
        int amountOfPacks = int.Parse(amountOfPacksString);

        return amountOfPacks;
    }
    
    
    public void ClearSession(ISession session)
    {
        session.Clear();
    }

    private void storeSessionData(ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }
    
    
}