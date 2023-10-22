namespace TimsyDev.CoffeeConsumption.Shared.Models
{
    public class DrinkerShop : IDDItem
    {
        public string PK { get; set; }
        public string SK { get; set; }
        public long CoffeeDrinkAccountId { get; set; }
        public string CoffeeShopID { get; set; }

        public DrinkerShop()
        {

        }

        public DrinkerShop(CoffeeDrinker drinker, CoffeeShop shop)
        {
            CoffeeDrinkAccountId = drinker.CoffeeDrinkAccountId;
            CoffeeShopID = shop.CoffeeShopID;
        }

    }
}
