using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimsyDev.CoffeeConsumption.Shared.Models
{
    // IDynamoDBItem
    public interface IDDItem
    {
        public string PK { get; set; }
        public string SK { get; set; }

    }
}
