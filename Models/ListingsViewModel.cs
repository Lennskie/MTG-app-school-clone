using System.Collections.Generic;

namespace mtg_app.Models {
    public class ListingsViewModel {
        
        public string? PageTitle {get; set;}
        public string? LinkTitle {get; set;}

        public List<ListingViewModel>? Listings {get; set;}
    }
}
