using System.Collections.Generic;

namespace TimsyDev.CoffeeConsumption.Shared.Models
{
    public class CoffeeDrinker : IDDItem
    {
        public string PK { get; set; }
        public string SK { get; set; }
        public string CoffeeDrinkerName { get; set; }
        public long CoffeeDrinkAccountId { get; set; }
        public List<CoffeeShop> CoffeeShops { get; set; } = new List<CoffeeShop>();
    }
}
