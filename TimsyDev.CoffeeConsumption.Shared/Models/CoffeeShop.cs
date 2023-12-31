﻿using System.Collections.Generic;

namespace TimsyDev.CoffeeConsumption.Shared.Models
{
    public class CoffeeShop : IDDItem
    {
        public string PK { get; set; }
        public string SK { get; set; }
        public string CoffeeShopID { get; set; }
        public string CoffeeShopName { get; set; }
        public string CoffeeShopAbbr { get; set; }
        public List<CoffeeDrink> Drinks { get; set; } = new List<CoffeeDrink>();
    }
}
