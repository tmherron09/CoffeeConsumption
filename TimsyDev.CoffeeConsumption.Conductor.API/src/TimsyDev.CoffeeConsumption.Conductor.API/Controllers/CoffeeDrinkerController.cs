using CoffeeConsumption.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimsyDev.CoffeeConsumption.Shared.Models;

namespace CoffeeConsumption.Conductor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeDrinkerController : ControllerBase
    {
        private readonly ILogger<CoffeeDrinkerController> _logger;
        private readonly ICoffeeDrinkService _coffeeDrinkService;
        private readonly IMockDataService _mockDataService;

        public CoffeeDrinkerController(ILogger<CoffeeDrinkerController> logger, ICoffeeDrinkService coffeeDrinkService, IMockDataService mockDataService)
        {
            _logger = logger;
            _coffeeDrinkService = coffeeDrinkService;
            _mockDataService = mockDataService;
        }

        [HttpGet]
        [Route("list/drinkers")]
        public async Task<IActionResult> GetListOfDrinkers()
        {
            try
            {
                var coffeeDrinkers = await _coffeeDrinkService.GetListOfDrinkers();

                return Ok(coffeeDrinkers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting List of All Drinkers");
                throw;
            }
        }
        [HttpGet]
        [Route("list/coffeeShops")]
        public async Task<IActionResult> GetListOfCoffeeShops()
        {
            try
            {
                var coffeeShops = await _coffeeDrinkService.GetAllCoffeeShops(); ;

                return Ok(coffeeShops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting List of All Drinkers");
                throw;
            }
        }

        [HttpGet]
        [Route("alldrinkers")]
        public async Task<IActionResult> GetAllCoffeeDrinkers()
        {
            try
            {
                var coffeeDrinkers = await _coffeeDrinkService.GetAllCoffeeDrinkers();

                return Ok(coffeeDrinkers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting List of All Drinkers");
                throw;
            }
        }

        [HttpGet]
        [Route("allDrinkExpectations")]
        public async Task<IActionResult> GetAllDrinkExpectations()
        {
            try
            {
                var coffeeDrinkers = await _coffeeDrinkService.GetAllDrinkExpectations();

                return Ok(coffeeDrinkers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting List of All Drinkers");
                throw;
            }
        }

        [HttpGet]
        [Route("allDrinkerShops")]
        public async Task<IActionResult> GetAllDrinkerShops()
        {
            try
            {
                var coffeeDrinkers = await _coffeeDrinkService.GetAllDrinkerShops();

                return Ok(coffeeDrinkers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting List of All Drinkers");
                throw;
            }
        }

        [HttpGet]
        [Route("mock/coffeeShops")]
        public IActionResult CreateMockCoffeeShops()
        {
            try
            {
                return Ok(_mockDataService.CreateMockCoffeeShops());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Mock CoffeeShops");
                throw;
            }
        }

        [HttpGet]
        [Route("mock/coffeeDrinker")]
        public IActionResult CreateMockCoffeeDrinkers()
        {
            try
            {
                var coffeeDrinkers = new List<CoffeeDrinker>();
                for (int i = 0; i < 10; i++)
                {
                    coffeeDrinkers.Add(_mockDataService.CreateMockCoffeeDrinker());
                }
                return Ok(coffeeDrinkers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Mock CoffeeShops");
                throw;
            }
        }

        [HttpGet]
        [Route("mock/drinkerShops")]
        public async Task<IActionResult> CreateMockDrinkerShops()
        {
            try
            {
                var drinkerShops = new List<DrinkerShop>();
                var coffeeDrinkers = (await _coffeeDrinkService.GetListOfDrinkers()).ToList();
                var coffeeShops = (await _coffeeDrinkService.GetAllCoffeeShops()).ToList();

                drinkerShops = _mockDataService.CreateMockDrinkerShopLinking(coffeeDrinkers, coffeeShops);

                var distinctLinks = drinkerShops.DistinctBy(x => $"{x.CoffeeDrinkAccountId}#{x.CoffeeShopID}").ToList();

                List<Tuple<long, int>> coffeeDrinkerCounts = new();
                List<Tuple<string, int>> coffeeShopCounts = new();

                foreach (var drinker in coffeeDrinkers)
                {
                    var count = distinctLinks.Count(x => x.CoffeeDrinkAccountId == drinker.CoffeeDrinkAccountId);
                    coffeeDrinkerCounts.Add(new Tuple<long, int>(drinker.CoffeeDrinkAccountId, count));
                }
                foreach (var shop in coffeeShops)
                {
                    var count = distinctLinks.Count(x => x.CoffeeShopID == shop.CoffeeShopID);
                    coffeeShopCounts.Add(new Tuple<string, int>(shop.CoffeeShopID, count));
                }

                return Ok(new { CoffeeDrinkerCounts = coffeeDrinkerCounts, CoffeeShopCounts = coffeeShopCounts });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Mock CoffeeShops");
                throw;
            }
        }

        [HttpGet]
        [Route("mock/drinkerShops/insertOne")]
        public async Task<IActionResult> CreateMockDrinkerShopsAndInsertOne()
        {
            try
            {
                var drinkerShops = new List<DrinkerShop>();
                var coffeeDrinkers = (await _coffeeDrinkService.GetListOfDrinkers()).ToList();
                var coffeeShops = (await _coffeeDrinkService.GetAllCoffeeShops()).ToList();

                drinkerShops = _mockDataService.CreateMockDrinkerShopLinking(coffeeDrinkers, coffeeShops);

                await _coffeeDrinkService.PutDrinkerShop(drinkerShops[0]);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Mock CoffeeShops");
                throw;
            }
        }

        [HttpGet]
        [Route("mock/drinkerShops/insertAll")]
        public async Task<IActionResult> CreateMockDrinkerShopsAndInsertAll()
        {
            try
            {
                var drinkerShops = new List<DrinkerShop>();
                var coffeeDrinkers = (await _coffeeDrinkService.GetListOfDrinkers()).ToList();
                var coffeeShops = (await _coffeeDrinkService.GetAllCoffeeShops()).ToList();

                drinkerShops = _mockDataService.CreateMockDrinkerShopLinking(coffeeDrinkers, coffeeShops);

                var distinctLinks = drinkerShops.DistinctBy(x => $"{x.CoffeeDrinkAccountId}#{x.CoffeeShopID}").ToList();
                foreach (var drinker in distinctLinks)
                {
                    await _coffeeDrinkService.PutDrinkerShop(drinker);
                }

                List<Tuple<long, int>> coffeeDrinkerCounts = new();
                List<Tuple<string, int>> coffeeShopCounts = new();

                foreach (var drinker in coffeeDrinkers)
                {
                    var count = distinctLinks.Count(x => x.CoffeeDrinkAccountId == drinker.CoffeeDrinkAccountId);
                    coffeeDrinkerCounts.Add(new Tuple<long, int>(drinker.CoffeeDrinkAccountId, count));
                }
                foreach (var shop in coffeeShops)
                {
                    var count = distinctLinks.Count(x => x.CoffeeShopID == shop.CoffeeShopID);
                    coffeeShopCounts.Add(new Tuple<string, int>(shop.CoffeeShopID, count));
                }

                return Ok(new { CoffeeDrinkerCounts = coffeeDrinkerCounts, CoffeeShopCounts = coffeeShopCounts });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Mock CoffeeShops");
                throw;
            }
        }

        //CreateMockDrinkerShopLinking

        [HttpGet]
        [Route("mock/coffeeDrinker/insertOne")]
        public async Task<IActionResult> CreateMockCoffeeDrinkersAndInsertOne()
        {
            try
            {
                var coffeeDrinkers = new List<CoffeeDrinker>();
                for (int i = 0; i < 10; i++)
                {
                    coffeeDrinkers.Add(_mockDataService.CreateMockCoffeeDrinker());
                }

                await _coffeeDrinkService.PutCoffeeDrinker(coffeeDrinkers[0]);
                var returnDrinkers = await _coffeeDrinkService.GetListOfDrinkers();
                return Ok(returnDrinkers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Mock CoffeeShops");
                throw;
            }
        }

        [HttpGet]
        [Route("mock/coffeeDrinker/insertAll")]
        public async Task<IActionResult> CreateMockCoffeeDrinkersAndInsertAll()
        {
            try
            {
                for (int i = 0; i < 30; i++)
                {
                    await _coffeeDrinkService.PutCoffeeDrinker(_mockDataService.CreateMockCoffeeDrinker());
                }

                var returnDrinkers = await _coffeeDrinkService.GetListOfDrinkers();
                return Ok(returnDrinkers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Mock CoffeeShops");
                throw;
            }
        }

        [HttpGet]
        [Route("mock/coffeeShops/insertOne")]
        public async Task<IActionResult> CreateMockCoffeeShopsAndInsertOne()
        {
            try
            {
                var result = _mockDataService.CreateMockCoffeeShops();
                await _coffeeDrinkService.PutCoffeeShop(result.ToList()[10]);
                var returnShops = await _coffeeDrinkService.GetAllCoffeeShops();

                return Ok(returnShops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Mock CoffeeShops");
                throw;
            }
        }

        [HttpGet]
        [Route("mock/coffeeShops/insertAll")]
        public async Task<IActionResult> CreateMockCoffeeShopsAndInsertAll()
        {
            try
            {
                var result = _mockDataService.CreateMockCoffeeShops();
                foreach (var shop in result)
                {
                    await _coffeeDrinkService.PutCoffeeShop(shop);
                }
                var returnShops = await _coffeeDrinkService.GetAllCoffeeShops();

                return Ok(returnShops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Mock CoffeeShops");
                throw;
            }
        }

    }
}
