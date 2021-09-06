using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EconomySimulator2
{
    class Region
    {
        public Dictionary<string, Market> market = new Dictionary<string, Market>();
        public Dictionary<string, Facility> facilities = new Dictionary<string, Facility>();


        public void addFacility(string name,int amount)
        {
            if (facilities.ContainsKey(name))
            {
                facilities[name].amount += amount;
            }
            else
            {
                Facility f = Facility.facilities[name](this);
                f.amount = amount;
                facilities.Add(name, f);
            }
        }

        public void calc(int time)
        {
            foreach (Good g in Good.values.Values)
            {
                double demand = 0;
                double produce = 0;
                int stock = 0;
                foreach (Facility facility in facilities.Values)
                {
                    demand += facility.getDemand(g);
                    produce += facility.getProduct(g);
                    stock += facility.getStock(g);
                }
                if (demand != 0 || produce != 0)
                {
                    Market m;
                    if (!market.ContainsKey(g.name))
                    {
                        m = new Market(g, this);
                        market.Add(g.name, m);
                    }
                    else
                    {
                        m = market[g.name];
                    }
                    m.SetMaxStock(stock);
                    m.calc(time, demand, (int)produce);
                    Debug.Print(g.name + " supplyratio : " + m.marketsupply / demand); ;
                    foreach (Facility facility in facilities.Values)
                    {

                        facility.setSupplyRatio(g, m.marketsupply / demand);
                    }
                }

            }

        }
    }
}
