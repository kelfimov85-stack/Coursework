using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;

namespace Coursework.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public Categories? Category { get; set; }
        public List<Basket> Baskets { get; set; }
    }
}
