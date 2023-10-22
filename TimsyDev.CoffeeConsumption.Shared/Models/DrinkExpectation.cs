using System.Collections.Generic;

namespace TimsyDev.CoffeeConsumption.Shared.Models
{
    public class DrinkExpectation : IDDItem
    {
        public string PK { get; set; }
        public string SK { get; set; }
        public long CoffeeDrinkAccountId { get; set; }
        public string CoffeeShopID { get; set; }
        //public DateTime MonthYear { get; set; }
        public List<CoffeeDrink> Drinks { get; set; } = new List<CoffeeDrink>();
    }
}
