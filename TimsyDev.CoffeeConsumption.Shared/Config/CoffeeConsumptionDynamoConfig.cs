namespace TimsyDev.CoffeeConsumption.Shared.Config
{
    public interface ICoffeeConsumptionDynamoConfig
    {
        string DynamoDBTableName { get; set; }
    }
    public class CoffeeConsumptionDynamoConfig : ICoffeeConsumptionDynamoConfig
    {
        public string DynamoDBTableName { get; set; }
    }
}
