using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimsyDev.CoffeeConsumption.Shared.Models
{
    public class CoffeeDrink
    {
        public string DrinkName { get; set; }
        public List<decimal> Data { get; set; } = new List<decimal>();
    }
}
