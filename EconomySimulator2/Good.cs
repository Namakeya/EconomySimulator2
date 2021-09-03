using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    class Good
    {
        public String name;
        public double price;

        public double elasticity;

        public Good(string name, double price, double elasticity)
        {
            this.name = name;
            this.price = price;
            this.elasticity = elasticity;
        }
    }
}
