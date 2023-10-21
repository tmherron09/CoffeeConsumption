using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimsyDev.CoffeeConsumption.Shared.Models;

namespace TimsyDev.CoffeeConsumption.Shared.Data
{
    public interface ICoffeDrinkerDataService
    {
        public Task<bool> PutCoffeeDrinker(CoffeeDrinker drinker);
    }
    public class CoffeeDrinkDataService : ICoffeDrinkerDataService
    {
        private readonly string _tableName;
        private const string _pk = "CoffeeConsumption";
        private const string _skCoffeeDrinker = "COFFEE_DRINKER";

        public CoffeeDrinkDataService()
        {
            
        }

        public Task<bool> PutCoffeeDrinker(CoffeeDrinker drinker)
        {
            throw new NotImplementedException();
            var putRequest = new PutItemRequest
            {

            };
        }

        public Dictionary<string, AttributeValue> CreateCoffeeDrinkerItem(CoffeeDrinker drinker)
        {
            try
            {
                var request = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue{ S = _pk } },
                { "SK", new AttributeValue{ S = $"${_skCoffeeDrinker}#{drinker.CoffeeDrinkAccountID}" } },
                { "CoffeeDrinkAccountID", new AttributeValue { N = $"{drinker.CoffeeDrinkAccountID}" } },
                { "CoffeeDrinkerName", new AttributeValue { S = drinker.CoffeeDrinkerName} }
            };
                return request;
            } catch(Exception ex)
            {
                // Add Logger.
                throw;
            }
        }
    }
}
