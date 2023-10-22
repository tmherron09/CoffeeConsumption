using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimsyDev.CoffeeConsumption.Shared.Data;
using TimsyDev.CoffeeConsumption.Shared.Models;

namespace CoffeeConsumption.Shared.Services
{
    public interface ICoffeeDrinkService
    {
        Task<IEnumerable<CoffeeDrinker>> GetListOfDrinkers();
        Task<IEnumerable<CoffeeDrinker>> GetAllCoffeeDrinkers();
        Task<IEnumerable<DrinkExpectation>> GetAllDrinkExpectations();
        Task<IEnumerable<DrinkerShop>> GetAllDrinkerShops();
        Task<bool> PutCoffeeShop(CoffeeShop coffeeShop);
        Task<bool> PutDrinkerShop(DrinkerShop drinkerShop);
        Task<bool> PutCoffeeDrinker(CoffeeDrinker coffeeDrinker);
        Task<IEnumerable<CoffeeShop>> GetAllCoffeeShops();
    }
    public class CoffeeDrinkService : ICoffeeDrinkService
    {
        public ILogger<CoffeeDrinkService> _logger;
        public ICoffeeDrinkerDataService _coffeeDrinkerDataService;

        public CoffeeDrinkService(ILogger<CoffeeDrinkService> logger, ICoffeeDrinkerDataService coffeeDrinkerDataService)
        {
            _logger = logger;
            _coffeeDrinkerDataService = coffeeDrinkerDataService;
        }


        public async Task<IEnumerable<CoffeeDrinker>> GetListOfDrinkers()
        {
            try
            {
                return await _coffeeDrinkerDataService.GetCoffeeDrinkers();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting List of All Drinkers");

                throw;
            }
        }

        public async Task<IEnumerable<CoffeeDrinker>> GetAllCoffeeDrinkers()
        {
            try
            {
                var coffeeDrinkers = await _coffeeDrinkerDataService.GetCoffeeDrinkers();
                var drinkerShops = new List<DrinkerShop>();
                foreach (var drinker in coffeeDrinkers)
                {
                    var drinkerShop =
                        await _coffeeDrinkerDataService.GetCoffeeDrinkerCoffeeShops(drinker.CoffeeDrinkAccountId);
                    drinkerShops.AddRange(drinkerShop);
                }

                //var coffeeShops = new List<CoffeeShop>();
                foreach (var drinkerShop in drinkerShops)
                {
                    var coffeeShop = await _coffeeDrinkerDataService.GetCoffeeShop(drinkerShop.CoffeeShopID);
                    //coffeeShops.Add(coffeeShop);
                    coffeeDrinkers.Single(x => x.CoffeeDrinkAccountId == drinkerShop.CoffeeDrinkAccountId).CoffeeShops.Add(coffeeShop);
                }

                foreach (var drinker in coffeeDrinkers)
                {
                    foreach (var coffeeShop in drinker.CoffeeShops)
                    {
                        var drinkerExpectation =
                            await _coffeeDrinkerDataService.GetCoffeeDrinkerShopDrinkExpectations(
                                drinker.CoffeeDrinkAccountId, coffeeShop.CoffeeShopID);
                        coffeeShop.Drinks = drinkerExpectation.Drinks;
                    }
                }

                return coffeeDrinkers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting List of All Drinkers");
                throw;
            }
        }

        public async Task<bool> PutCoffeeShop(CoffeeShop coffeeShop)
        {
            try
            {
                return await _coffeeDrinkerDataService.PutCoffeeShop(coffeeShop);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> PutCoffeeDrinker(CoffeeDrinker coffeeDrinker)
        {
            try
            {
                return await _coffeeDrinkerDataService.PutCoffeeDrinker(coffeeDrinker);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CoffeeShop>> GetAllCoffeeShops()
        {
            try
            {
                return await _coffeeDrinkerDataService.GetAllCoffeeShops();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DrinkExpectation>> GetAllDrinkExpectations()
        {
            try
            {
                return await _coffeeDrinkerDataService.GetAllDrinkExpectations();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IEnumerable<DrinkerShop>> GetAllDrinkerShops()
        {
            try
            {
                return await _coffeeDrinkerDataService.GetAllDrinkerShops();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> PutDrinkerShop(DrinkerShop drinkerShop)
        {
            try
            {
                return await _coffeeDrinkerDataService.PutDrinkerShop(drinkerShop);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
