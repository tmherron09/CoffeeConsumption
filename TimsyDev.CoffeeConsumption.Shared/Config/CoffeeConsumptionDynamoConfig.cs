using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimsyDev.CoffeeConsumption.Shared.Config
{
    public interface ICoffeeConsumptionDynamoConfig
    {
        string DynamoDBTableName { get; set; }
    }
    public class CoffeeConsumptionDynamoConfig
    {
        public string DynamoDBTableName { get; }
    }
}
