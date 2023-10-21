using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimsyDev.CoffeeConsumption.Shared.Models
{
    public class CoffeeShop
    {
        public string CoffeeShopID { get; set; }
        public string CoffeeShopName { get; set; }
        public string CoffeeShopAbbr { get; set; }
        public List<CoffeeDrink> Drinks { get; set; } = new List<CoffeeDrink>();
    }
}
