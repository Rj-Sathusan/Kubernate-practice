using DevPro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevPro.Domain.Entities
{
    public class Product : Base
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}