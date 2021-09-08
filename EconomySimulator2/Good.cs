using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    class Good
    {
        public static Dictionary<string, Good> values = new Dictionary<string, Good>();

        public string name;
        /**初期価格。実際に取引される価格は場所によって異なり、Market.priceで表される*/
        public double price;

        public double elasticity;

        private Good(string name, double price, double elasticity)
        {
            this.name = name;
            this.price = price;
            this.elasticity = elasticity;
            values.Add(name, this);
        }

        public override string ToString()
        {
            return name;
        }

        public static Good WORKER = new Good("Worker", 5, 1);

        public static Good GRAIN = new Good("Grain", 100, 0.5);

        public static Good ALCOHOL = new Good("Alcohol", 100, 1.5);
    }
}
