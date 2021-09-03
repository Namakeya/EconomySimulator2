using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    class Good
    {
        public String name;
        /**初期価格。実際に取引される価格は場所によって異なり、Market.priceで表される*/
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
