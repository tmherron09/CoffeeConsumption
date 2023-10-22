using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimsyDev.CoffeeConsumption.Shared.Models;

namespace TimsyDev.CoffeeConsumption.Shared.Data
{
    public interface ICoffeeDrinkerDataService
    {
        Task<bool> PutCoffeeDrinker(CoffeeDrinker drinker);
        Task<bool> PutCoffeeShop(CoffeeShop coffeeShop);
        Task<bool> PutDrinkExpectation(DrinkExpectation drinkExpectation);
        Task<bool> PutDrinkerShop(DrinkerShop drinkerShop);
        Task<CoffeeDrinker> GetCoffeeDrinker(long coffeeDrinkAccountId);
        Task<IEnumerable<CoffeeDrinker>> GetCoffeeDrinkers();
        Task<IEnumerable<CoffeeShop>> GetAllCoffeeShops();
        Task<IEnumerable<DrinkExpectation>> GetAllDrinkExpectations();
        Task<IEnumerable<DrinkerShop>> GetAllDrinkerShops();
        Task<IEnumerable<DrinkerShop>> GetCoffeeDrinkerCoffeeShops(long coffeeDrinkAccountId);
        Task<CoffeeShop> GetCoffeeShop(string coffeeShopId);
        Task<DrinkExpectation> GetCoffeeDrinkerShopDrinkExpectations(long coffeeDrinkAccountId, string CoffeeShopId);
    }
    public class CoffeeDrinkDataService : ICoffeeDrinkerDataService
    {
        private readonly ILogger<CoffeeDrinkDataService> _logger;

        private const string Pk = "CoffeeConsumption";
        private const string SkCoffeeDrinker = "COFFEE_DRINKER";
        private const string SkDrinkerShop = "DRINKER_SHOP";
        private const string SkDrinkerExpectation = "DRINK_EXPECTATIONS";
        private const string SkCoffeeShop = "COFFEE_SHOP";
        private const string TableName = "TimsyDev_AnarchyVideo_Main";
        private readonly IAmazonDynamoDB _amazonDynamoDb;
        //private readonly ICoffeeConsumptionDynamoConfig _dynamoConfig;

        //public CoffeeDrinkDataService(ICoffeeConsumptionDynamoConfig dynamoConfig, IAmazonDynamoDB amazonDynamoDb)
        //{
        //    _dynamoConfig = dynamoConfig;
        //    _amazonDynamoDb = amazonDynamoDb;
        //}

        public CoffeeDrinkDataService(IAmazonDynamoDB amazonDynamoDb, ILogger<CoffeeDrinkDataService> logger)
        {
            _amazonDynamoDb = amazonDynamoDb;
            _logger = logger;
        }

        public async Task<bool> PutCoffeeDrinker(CoffeeDrinker drinker)
        {
            try
            {
                var putRequest = new PutItemRequest()
                {
                    Item = CreateCoffeeDrinkerItem(drinker),
                    TableName = TableName
                };

                var result = await _amazonDynamoDb.PutItemAsync(putRequest);

                return true;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Putting Coffee Drinker with Account Id: {coffeeDrinkAccountId}", drinker.CoffeeDrinkAccountId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Putting Coffee Drinker with Account Id: {coffeeDrinkAccountId}", drinker.CoffeeDrinkAccountId);
                throw;
            }
        }

        public async Task<bool> PutCoffeeShop(CoffeeShop coffeeShop)
        {
            try
            {
                var putRequest = new PutItemRequest()
                {
                    Item = CreateCoffeeShopItem(coffeeShop),
                    TableName = TableName
                };

                var result = await _amazonDynamoDb.PutItemAsync(putRequest);

                return true;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Putting CoffeeShop with CoffeeShop Id: {coffeeShopID}", coffeeShop.CoffeeShopID);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Putting CoffeeShop with CoffeeShop Id: {coffeeShopID}", coffeeShop.CoffeeShopID);
                throw;
            }
        }

        public async Task<bool> PutDrinkExpectation(DrinkExpectation drinkExpectation)
        {
            try
            {
                var putRequest = new PutItemRequest()
                {
                    Item = CreateDrinkExpectationItem(drinkExpectation),
                    TableName = TableName
                };

                var result = await _amazonDynamoDb.PutItemAsync(putRequest);

                return true;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Putting DrinkExpectation with CoffeeShop Id: {coffeeShopID}  Coffee Drink Account Id: {coffeeDrinkAccountId}", drinkExpectation.CoffeeShopID, drinkExpectation.CoffeeDrinkAccountId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Putting DrinkExpectation with CoffeeShop Id: {coffeeShopID}  Coffee Drink Account Id: {coffeeDrinkAccountId}", drinkExpectation.CoffeeShopID, drinkExpectation.CoffeeDrinkAccountId);
                throw;
            }
        }

        public async Task<bool> PutDrinkerShop(DrinkerShop drinkerShop)
        {
            try
            {
                var putRequest = new PutItemRequest()
                {
                    Item = CreateDrinkerShopItem(drinkerShop),
                    TableName = TableName
                };

                var result = await _amazonDynamoDb.PutItemAsync(putRequest);

                return true;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Putting Drinker Shop with CoffeeShop Id: {coffeeShopID} CoffeeDrinkAccount Id: {coffeeDrinkAccountId}", drinkerShop.CoffeeShopID, drinkerShop.CoffeeDrinkAccountId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Putting Drinker Shop with CoffeeShop Id: {coffeeShopID} CoffeeDrinkAccount Id: {coffeeDrinkAccountId}", drinkerShop.CoffeeShopID, drinkerShop.CoffeeDrinkAccountId);
                throw;
            }
        }

        public async Task<CoffeeDrinker> GetCoffeeDrinker(long coffeeDrinkAccountId)
        {
            CoffeeDrinker drinker = new CoffeeDrinker();
            try
            {

                Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
                {
                    { "PK", new AttributeValue { S = Pk } },
                    { "SK", new AttributeValue { S = $"{SkCoffeeDrinker}#{coffeeDrinkAccountId}" } }
                };

                GetItemRequest request = new GetItemRequest
                {
                    TableName = TableName,
                    Key = key,
                };

                var result = await _amazonDynamoDb.GetItemAsync(request);

                if (result != null)
                {
                    Dictionary<string, AttributeValue> item = result.Item;
                    if (item.Count == 0)
                    {
                        throw new Exception();
                    }

                    drinker = HydrateCoffeeDrinker(item);
                }
                return drinker;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Getting Coffee Drinker with Account Id: {coffeeDrinkAccountId}", coffeeDrinkAccountId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting Coffee Drinker with Account Id: {coffeeDrinkAccountId}", coffeeDrinkAccountId);
                throw;
            }
        }

        public async Task<IEnumerable<CoffeeDrinker>> GetCoffeeDrinkers()
        {
            List<CoffeeDrinker> coffeeDrinkers = new();
            try
            {
                var request = new QueryRequest
                {
                    TableName = TableName,
                    KeyConditionExpression = "PK = :v_PK AND begins_with(SK, :v_SKExpression)",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":v_PK", new AttributeValue { S = Pk } },
                        { ":v_SKExpression", new AttributeValue { S = $"{SkCoffeeDrinker}#" } }
                    }
                };

                do
                {
                    var response = await _amazonDynamoDb.QueryAsync(request);

                    foreach (Dictionary<string, AttributeValue> item in response.Items)
                    {
                        coffeeDrinkers.Add(HydrateCoffeeDrinker(item));
                    }

                    request.ExclusiveStartKey = response.LastEvaluatedKey;

                } while (request.ExclusiveStartKey.Count > 0);

                return coffeeDrinkers;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Getting Coffee Drinkers");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting Coffee Drinkers");
                throw;
            }
        }

        public async Task<IEnumerable<CoffeeShop>> GetAllCoffeeShops()
        {
            List<CoffeeShop> coffeeShops = new();
            try
            {
                var request = new QueryRequest
                {
                    TableName = TableName,
                    KeyConditionExpression = "PK = :v_PK AND begins_with(SK, :v_SKExpression)",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":v_PK", new AttributeValue { S = Pk } },
                        { ":v_SKExpression", new AttributeValue { S = $"{SkCoffeeShop}#" } }
                    }
                };
                do
                {
                    var response = await _amazonDynamoDb.QueryAsync(request);

                    foreach (Dictionary<string, AttributeValue> item in response.Items)
                    {
                        coffeeShops.Add(HydrateCoffeeShop(item));
                    }

                    request.ExclusiveStartKey = response.LastEvaluatedKey;
                } while (request.ExclusiveStartKey.Count > 0);

                return coffeeShops;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Getting All Coffee Shops");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting All Coffee Shops");
                throw;
            }
        }

        public async Task<IEnumerable<DrinkerShop>> GetCoffeeDrinkerCoffeeShops(long coffeeDrinkAccountId)
        {
            List<DrinkerShop> drinkerShops = new();

            try
            {

                var request = new QueryRequest
                {
                    TableName = TableName,
                    KeyConditionExpression = "PK = :v_PK AND begins_with(SK, :v_SKExpression)",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":v_PK", new AttributeValue { S = Pk } },
                        { ":v_SKExpression", new AttributeValue { S = $"{SkDrinkerShop}#{coffeeDrinkAccountId}#" } }
                    }
                };

                do
                {
                    var response = await _amazonDynamoDb.QueryAsync(request);

                    foreach (Dictionary<string, AttributeValue> item in response.Items)
                    {
                        drinkerShops.Add(HydrateDrinkerShop(item));
                    }

                    request.ExclusiveStartKey = response.LastEvaluatedKey;

                } while (request.ExclusiveStartKey.Count > 0);

                return drinkerShops;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Getting Coffee Drinker Shops for Coffee Drinker Account Id: {coffeeDrinkAccountId}", coffeeDrinkAccountId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting Coffee Drinker Shops for Coffee Drinker Account Id: {coffeeDrinkAccountId}", coffeeDrinkAccountId);
                throw;
            }
        }

        public async Task<IEnumerable<DrinkExpectation>> GetAllDrinkExpectations()
        {
            List<DrinkExpectation> drinkExpectations = new();

            try
            {

                var request = new QueryRequest
                {
                    TableName = TableName,
                    KeyConditionExpression = "PK = :v_PK AND begins_with(SK, :v_SKExpression)",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":v_PK", new AttributeValue { S = Pk } },
                        { ":v_SKExpression", new AttributeValue { S = $"{SkDrinkerExpectation}#" } }
                    }
                };

                do
                {
                    var response = await _amazonDynamoDb.QueryAsync(request);

                    foreach (Dictionary<string, AttributeValue> item in response.Items)
                    {
                        drinkExpectations.Add(HydrateDrinkerExpectation(item));
                    }

                    request.ExclusiveStartKey = response.LastEvaluatedKey;

                } while (request.ExclusiveStartKey.Count > 0);

                return drinkExpectations;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Getting All Drink Expectations");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting All Drink Expectations");
                throw;
            }
        }

        public async Task<IEnumerable<DrinkerShop>> GetAllDrinkerShops()
        {
            List<DrinkerShop> drinkerShops = new();

            try
            {

                var request = new QueryRequest
                {
                    TableName = TableName,
                    KeyConditionExpression = "PK = :v_PK AND begins_with(SK, :v_SKExpression)",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":v_PK", new AttributeValue { S = Pk } },
                        { ":v_SKExpression", new AttributeValue { S = $"{SkDrinkerShop}#" } }
                    }
                };

                do
                {
                    var response = await _amazonDynamoDb.QueryAsync(request);

                    foreach (Dictionary<string, AttributeValue> item in response.Items)
                    {
                        drinkerShops.Add(HydrateDrinkerShop(item));
                    }

                    request.ExclusiveStartKey = response.LastEvaluatedKey;

                } while (request.ExclusiveStartKey.Count > 0);

                return drinkerShops;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Getting All Drink Expectations");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting All Drink Expectations");
                throw;
            }
        }

        public async Task<DrinkExpectation> GetCoffeeDrinkerShopDrinkExpectations(long coffeeDrinkAccountId,
            string CoffeeShopId)
        {
            DrinkExpectation drinkExpectation = new DrinkExpectation();
            try
            {
                Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
                {
                    { "PK", new AttributeValue { S = Pk } },
                    { "SK", new AttributeValue { S = $"{SkDrinkerExpectation}#{coffeeDrinkAccountId}#{CoffeeShopId}" } }
                };

                GetItemRequest request = new GetItemRequest
                {
                    TableName = TableName,
                    Key = key,
                };

                var result = await _amazonDynamoDb.GetItemAsync(request);

                if (result != null)
                {
                    Dictionary<string, AttributeValue> item = result.Item;
                    if (item.Count == 0)
                    {
                        throw new Exception();
                    }

                    drinkExpectation = HydrateDrinkerExpectation(item);
                }
                return drinkExpectation;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Getting Coffee Drink Expectations for Coffee Drinker Account Id: {coffeeDrinkAccountId} and CoffeeShopId: {coffeeShopId}", coffeeDrinkAccountId, CoffeeShopId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting Coffee Drink Expectations for Coffee Drinker Account Id: {coffeeDrinkAccountId} and CoffeeShopId: {coffeeShopId}", coffeeDrinkAccountId, CoffeeShopId);
                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<CoffeeShop> GetCoffeeShop(string coffeeShopId)
        {
            var coffeeShop = new CoffeeShop();

            try
            {

                Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
                {
                    { "PK", new AttributeValue { S = Pk } },
                    { "SK", new AttributeValue { S = $"{SkCoffeeShop}#{coffeeShopId}" } }
                };

                GetItemRequest request = new GetItemRequest
                {
                    TableName = TableName,
                    Key = key,
                };

                var result = await _amazonDynamoDb.GetItemAsync(request);

                if (result != null)
                {
                    Dictionary<string, AttributeValue> item = result.Item;
                    if (item.Count == 0)
                    {
                        throw new Exception();
                    }

                    coffeeShop = HydrateCoffeeShop(item);
                }
                return coffeeShop;
            }
            catch (AmazonDynamoDBException e)
            {
                _logger.LogError(e, "Error Getting Coffee Shop for CoffeeShopId: {coffeeShopId}", coffeeShopId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting Coffee Shop for CoffeeShopId: {coffeeShopId}", coffeeShopId);
                throw;
            }
        }

        public Dictionary<string, AttributeValue> CreateCoffeeDrinkerItem(CoffeeDrinker drinker)
        {
            try
            {
                var request = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue{ S = Pk } },
                { "SK", new AttributeValue{ S = $"{SkCoffeeDrinker}#{drinker.CoffeeDrinkAccountId}" } },
                { "CoffeeDrinkAccountId", new AttributeValue { N = $"{drinker.CoffeeDrinkAccountId}" } },
                { "CoffeeDrinkerName", new AttributeValue { S = drinker.CoffeeDrinkerName} }
            };
                return request;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Coffee Drinker Item: {@coffeeDrinker}", drinker);
                throw;
            }
        }



        private CoffeeDrinker HydrateCoffeeDrinker(Dictionary<string, AttributeValue> item)
        {
            try
            {
                var drinker = new CoffeeDrinker();

                foreach (var kvp in item)
                {
                    switch (kvp.Key)
                    {
                        case "PK":
                            drinker.PK = kvp.Value.S;
                            break;
                        case "SK":
                            drinker.SK = kvp.Value.S;
                            break;
                        case "CoffeeDrinkAccountId":
                            drinker.CoffeeDrinkAccountId = long.Parse(kvp.Value.N);
                            break;
                        case "CoffeeDrinkerName":
                            drinker.CoffeeDrinkerName = kvp.Value.S;
                            break;
                    }
                }

                return drinker;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Hydrating Coffee Drinker - Dynamo Item: {@item}", item);
                throw;
            }
        }

        private DrinkerShop HydrateDrinkerShop(Dictionary<string, AttributeValue> item)
        {
            try
            {
                var drinkerShop = new DrinkerShop();

                foreach (var kvp in item)
                {
                    switch (kvp.Key)
                    {
                        case "PK":
                            drinkerShop.PK = kvp.Value.S;
                            break;
                        case "SK":
                            drinkerShop.SK = kvp.Value.S;
                            break;
                        case "CoffeeDrinkAccountId":
                            drinkerShop.CoffeeDrinkAccountId = long.Parse(kvp.Value.N);
                            break;
                        case "CoffeeShopID":
                            drinkerShop.CoffeeShopID = kvp.Value.S;
                            break;
                    }
                }

                return drinkerShop;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Hydrating DrinkerShop - Dynamo Item: {@item}", item);
                throw;
            }
        }

        private CoffeeShop HydrateCoffeeShop(Dictionary<string, AttributeValue> item)
        {
            try
            {
                var coffeeShop = new CoffeeShop();

                foreach (var kvp in item)
                {
                    switch (kvp.Key)
                    {
                        case "PK":
                            coffeeShop.PK = kvp.Value.S;
                            break;
                        case "SK":
                            coffeeShop.SK = kvp.Value.S;
                            break;
                        case "CoffeeShopAbbr":
                            coffeeShop.CoffeeShopAbbr = kvp.Value.S;
                            break;
                        case "CoffeeShopID":
                            coffeeShop.CoffeeShopID = kvp.Value.S;
                            break;
                        case "CoffeeShopName":
                            coffeeShop.CoffeeShopName = kvp.Value.S;
                            break;
                    }
                }

                return coffeeShop;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Hydrating Coffee Shop - Dynamo Item: {@item}", item);
                throw;
            }
        }

        public Dictionary<string, AttributeValue> CreateCoffeeShopItem(CoffeeShop coffeeShop)
        {
            try
            {
                var request = new Dictionary<string, AttributeValue>
                {
                    { "PK", new AttributeValue{ S = Pk } },
                    { "SK", new AttributeValue{ S = $"{SkCoffeeShop}#{coffeeShop.CoffeeShopID}" } },
                    { "CoffeeShopAbbr", new AttributeValue { S = $"{coffeeShop.CoffeeShopAbbr}" } },
                    { "CoffeeShopID", new AttributeValue { S = coffeeShop.CoffeeShopID} },
                    { "CoffeeShopName", new AttributeValue { S = coffeeShop.CoffeeShopName} }
                };
                return request;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating CoffeeShop Item: {@coffeeShop}", coffeeShop);
                throw;
            }
        }

        private DrinkExpectation HydrateDrinkerExpectation(Dictionary<string, AttributeValue> item)
        {
            try
            {
                var drinkExpectation = new DrinkExpectation();

                foreach (var kvp in item)
                {
                    switch (kvp.Key)
                    {
                        case "PK":
                            drinkExpectation.PK = kvp.Value.S;
                            break;
                        case "SK":
                            drinkExpectation.SK = kvp.Value.S;
                            break;
                        case "CoffeeDrinkAccountId":
                            drinkExpectation.CoffeeDrinkAccountId = long.Parse(kvp.Value.N);
                            break;
                        case "CoffeeShopID":
                            drinkExpectation.CoffeeShopID = kvp.Value.S;
                            break;
                        case "BlackCoffee":
                            var blackCoffeeDataItems = kvp.Value.L.Select(data => decimal.Parse(data.N)).ToList();
                            drinkExpectation.Drinks.Add(new CoffeeDrink("Black Coffee", blackCoffeeDataItems));
                            break;
                        case "CaramelBased":
                            var caramelBasedDataItems = kvp.Value.L.Select(data => decimal.Parse(data.N)).ToList();
                            drinkExpectation.Drinks.Add(new CoffeeDrink("Caramel Based", caramelBasedDataItems));
                            break;
                        case "VanillaLatte":
                            var vanillaLatterDataItems = kvp.Value.L.Select(data => decimal.Parse(data.N)).ToList();
                            drinkExpectation.Drinks.Add(new CoffeeDrink("Vanilla Latte", vanillaLatterDataItems));
                            break;
                        case "Decaf":
                            var decafDataItems = kvp.Value.L.Select(data => decimal.Parse(data.N)).ToList();
                            drinkExpectation.Drinks.Add(new CoffeeDrink("Decaf", decafDataItems));
                            break;
                    }
                }

                return drinkExpectation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Hydrating Drinker Expectation - Dynamo Item: {@item}", item);
                throw;
            }
        }


        public Dictionary<string, AttributeValue> CreateDrinkerShopItem(DrinkerShop drinkerShop)
        {
            try
            {
                var request = new Dictionary<string, AttributeValue>
                {
                    { "PK", new AttributeValue{ S = Pk } },
                    { "SK", new AttributeValue{ S = $"{SkDrinkerShop}#{drinkerShop.CoffeeDrinkAccountId}#{drinkerShop.CoffeeShopID}" } },
                    { "CoffeeDrinkAccountId", new AttributeValue { N = $"{drinkerShop.CoffeeDrinkAccountId}"} },
                    { "CoffeeShopID", new AttributeValue { S = drinkerShop.CoffeeShopID} },
                };
                return request;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Drinker Shop Item: {@drinkerShop}", drinkerShop);
                throw;
            }
        }

        public Dictionary<string, AttributeValue> CreateDrinkExpectationItem(DrinkExpectation drinkExpectation)
        {
            try
            {
                var drinkListAttributeValues = CreateCoffeeDrinkAttributes(drinkExpectation.Drinks);

                var request = new Dictionary<string, AttributeValue>
                {
                    { "PK", new AttributeValue{ S = Pk } },
                    { "SK", new AttributeValue{ S = $"${SkDrinkerExpectation}#{drinkExpectation.CoffeeDrinkAccountId}#{drinkExpectation.CoffeeShopID}" } },
                    { "CoffeeDrinkAccountId", new AttributeValue { N = $"{drinkExpectation.CoffeeDrinkAccountId}" } },
                    { "CoffeeShopID", new AttributeValue { S = drinkExpectation.CoffeeShopID} },
                    { "Decaf", new AttributeValue { L = drinkListAttributeValues["Decaf"]}},
                    { "BlackCoffee", new AttributeValue { L = drinkListAttributeValues["Black Coffee"]}},
                    { "CaramelBased", new AttributeValue { L = drinkListAttributeValues["Caramel Based"]}},
                    { "VanillaLatte", new AttributeValue { L = drinkListAttributeValues["Vanilla Latte"]}}
                };
                return request;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating DrinkExpectation Item: {@drinkExpectation}", drinkExpectation);
                throw;
            }
        }

        public Dictionary<string, List<AttributeValue>> CreateCoffeeDrinkAttributes(List<CoffeeDrink> coffeeDrinks)
        {
            var drinksDict = new Dictionary<string, List<AttributeValue>>();
            try
            {
                coffeeDrinks.ForEach(c =>
                    drinksDict.Add(c.DrinkName, CreateDrinkData(c.Data)));
                return drinksDict;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Coffee Drink Attributes");
                throw;
            }
        }

        public List<AttributeValue> CreateDrinkData(List<decimal> data)
        {
            var attrValues = new List<AttributeValue>();
            try
            {
                data.ForEach(point => attrValues.Add(new AttributeValue() { N = point.ToString() }));
                return attrValues;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating Drink Data");
                throw;
            }
        }

    }
}
