namespace TimsyDev.CoffeeConsumption.Shared.Models
{
    // IDynamoDBItem
    public interface IDDItem
    {
        public string PK { get; set; }
        public string SK { get; set; }

    }
}
