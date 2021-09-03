using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EconomySimulator2
{
    class Region
    {
        public Dictionary<String, Market> market = new Dictionary<string, Market>();

        public void addMarket(Market m)
        {
            market.Add(m.good.name, m);
            //Debug.Print(good.name + " supply: "+ s.getBaseSupply(0));
        }

        public void calc(int time)
        {
            foreach (Market m in market.Values)
            {
                m.calc(time);
            }


        }
    }
}
