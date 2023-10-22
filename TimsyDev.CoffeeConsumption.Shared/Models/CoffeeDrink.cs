using System.Collections.Generic;

namespace TimsyDev.CoffeeConsumption.Shared.Models
{
    public class CoffeeDrink
    {
        public string DrinkName { get; set; }
        public List<decimal> Data { get; set; } = new List<decimal>();

        public CoffeeDrink(string drinkName, List<decimal> data)
        {
            DrinkName = drinkName;
            Data = data;
        }
    }
}
