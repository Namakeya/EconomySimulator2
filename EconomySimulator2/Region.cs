using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EconomySimulator2
{
    class Region
    {
        public Dictionary<String, double> demand = new Dictionary<string, double>();
        public Dictionary<String, Supply> supply = new Dictionary<string, Supply>();

        public void addGood(Good good,double d, Supply s)
        {
            demand.Add(good.name, d);
            supply.Add(good.name, s);
            //Debug.Print(good.name + " supply: "+ s.getBaseSupply(0));
        }

        public void calc(Good good,int time)
        {
            double d = demand[good.name];
            Supply s = supply[good.name];

            double basesupply = s.GetBaseSupply(time);
            double pricerate = Math.Pow(d / basesupply, 1 / (good.elasticity));
            Debug.Print(good.name+" supply: "+basesupply+" pricerate : "+pricerate+" stockrate : "+s.StockExp(pricerate));


            double price = good.price*Math.Pow(d / basesupply, 1 / (good.elasticity+s.StockExp(pricerate)));
            double stockamount = (Math.Pow(price / good.price, s.StockExp(pricerate))-1) * basesupply;

            s.currentstock -= stockamount;

            Debug.Print(good.name+" price: "+price+" stock: "+s.currentstock);
        }
    }
}
