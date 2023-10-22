namespace CoffeeConsumption.Conductor.API.Config
{
    public interface IAppConfig
    {
        string WithOrigins { get; set; }
    }
    public class AppConfig : IAppConfig
    {
        public string WithOrigins { get; set; }
    }
}
