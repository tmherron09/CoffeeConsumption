using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimsyDev.CoffeeConsumption.Shared.Models
{
    public class CoffeeDrinker : IDDItem
    {
        public string PK { get; set; }
        public string SK { get; set; }
        public string CoffeeDrinkerName { get; set; }
        public long CoffeeDrinkAccountID { get; set; }
        public List<CoffeeShop> CoffeeShops { get; set; } = new List<CoffeeShop>();
    }
}
