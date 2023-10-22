using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using TimsyDev.CoffeeConsumption.Shared.Models;

namespace CoffeeConsumption.Shared.Services
{
    public interface IMockDataService
    {
        CoffeeDrinker CreateMockCoffeeDrinker();
        //CoffeeShop CreateMockCoffeeShop();
        List<CoffeeDrink> CreateMockCoffeeDrinkData();
        List<CoffeeShop> CreateMockCoffeeShops();

        List<DrinkerShop> CreateMockDrinkerShopLinking(List<CoffeeDrinker> coffeeDrinkers,
            List<CoffeeShop> coffeeShops);
        List<DrinkExpectation> CreateMockDrinkExpectation(List<DrinkerShop> drinkerShops);

    }

    public class MockDataService : IMockDataService
    {
        private readonly Faker _faker = new Faker();
        private Random random = new Random();


        private string[] drinkNames = new string[] { "Decaf", "Black Coffee", "Caramel Based", "Vanilla Latte" };

        private Dictionary<string, string> CoffeeShopNames = new Dictionary<string, string>()
        {
            { "Billy Goat Coffee", "BGC" },
            { "Ursa Minor Coffee", "URMC" },
            { "Mocha Magic Cafe", "MMC" },
            { "Perk Up Coffeehouse", "PUCH" },
            { "Steamy Beans Cafe", "SBC" },
            { "Morning Brew Coffee", "MBC" },
            { "Espresso Elegance", "ESEL" },
            { "Aroma Haven Cafe", "AHC" },
            { "Cafe Central", "CC" },
            { "Bean Buzz Cafe", "BBC" },
            { "The Grind House", "TGH" },
            { "Bean Town Cafe", "BTC" },
            { "Cafe Mocha", "CM" },
            { "Java Junction", "JJ" },
            { "Cup o' Joe Cafe", "COJC" },
            { "Morning Perk Cafe", "MPC" },
            { "Bean Counter Coffee", "BCC" },
            { "Cafe Arabica", "CA" },
            { "Latte Lounge", "LL" },
            { "Cappuccino Cove", "CCV" },
            { "Sunrise Sip Coffee", "SSC" },
            { "Cafe Noir", "CN" },
            { "The Steamy Mug", "TSM" },
            { "Brewed Awakening", "BA" },
            { "Cafe de l'Amour", "CDLA" },
            { "Espresso Express", "EE" },
            { "The Roasted Bean", "TRB" },
            { "Cuppa Joy Coffee", "CJC" },
            { "Percolate Palace", "PP" },
            { "Mug & Muffin Cafe", "MNMC" }
        };


        public CoffeeDrinker CreateMockCoffeeDrinker()
        {

            var coffeeDrinker = new CoffeeDrinker()
            {
                CoffeeDrinkAccountId = long.Parse(_faker.Finance.Account(8)),
                CoffeeDrinkerName = _faker.Name.FullName()
            };
            return coffeeDrinker;
        }

        public List<CoffeeShop> CreateMockCoffeeShops()
        {
            var coffeeShops = new List<CoffeeShop>();

            foreach (var entry in CoffeeShopNames)
            {
                coffeeShops.Add(new()
                {
                    CoffeeShopID = _faker.Random.AlphaNumeric(9).ToUpper(),
                    CoffeeShopName = entry.Key,
                    CoffeeShopAbbr = entry.Value,
                    Drinks = CreateMockCoffeeDrinkData()
                });
            }
            return coffeeShops;
        }

        public List<DrinkerShop> CreateMockDrinkerShopLinking(List<CoffeeDrinker> coffeeDrinkers,
            List<CoffeeShop> coffeeShops)
        {
            var drinkerShops = new List<DrinkerShop>();

            coffeeDrinkers = coffeeDrinkers.OrderBy(x => random.Next()).ToList();
            coffeeShops = coffeeShops.OrderBy(x => random.Next()).ToList();
            var currentIndex = 0;

            foreach (var drinker in coffeeDrinkers)
            {
                var linkCount = random.Next(3, 11);

                for (int i = 0; i < linkCount; i++)
                {
                    int index = currentIndex % coffeeShops.Count;
                    drinkerShops.Add(new DrinkerShop(drinker, coffeeShops[index]));
                    currentIndex++;
                }
            }

            foreach (var shop in coffeeShops)
            {
                drinkerShops.Add(new DrinkerShop(coffeeDrinkers[currentIndex % coffeeDrinkers.Count], shop));
            }

            return drinkerShops;
        }

        public List<CoffeeShop> PopulateMockCoffeeShopDrinks(List<CoffeeShop> shops)
        {
            foreach (var shop in shops)
            {
                shop.Drinks = CreateMockCoffeeDrinkData();
            }

            return shops;
        }

        public List<DrinkExpectation> CreateMockDrinkExpectation(List<DrinkerShop> drinkerShops)
        {
            var drinkExpectations = new List<DrinkExpectation>();
            foreach (var shop in drinkerShops)
            {
                drinkExpectations.Add(new()
                {
                    CoffeeDrinkAccountId = shop.CoffeeDrinkAccountId,
                    CoffeeShopID = shop.CoffeeShopID,
                    Drinks = CreateMockCoffeeDrinkData()
                });
            }
            return drinkExpectations;
        }

        public List<CoffeeDrink> CreateMockCoffeeDrinkData()
        {
            var drinks = new List<CoffeeDrink>();
            foreach (var drinkName in drinkNames)
            {
                drinks.Add(new CoffeeDrink(drinkName, CreateMockData(0, 100)));
            }
            return drinks;
        }



        private List<decimal> CreateMockData(int min, int max)
        {
            var data = new List<decimal>();
            for (int i = 0; i < 12; i++)
            {
                data.Add(_faker.Random.Int(min, max));
            }

            return data;
        }
    }
}
