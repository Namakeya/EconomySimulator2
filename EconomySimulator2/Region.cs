using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EconomySimulator2
{
    class Region
    {
        public string name;
        public Dictionary<string, Market> market = new Dictionary<string, Market>();
        public Dictionary<string, Facility> facilities = new Dictionary<string, Facility>();

        public Agent localpeople;


        public void calc(int time)
        {
            Dictionary<Good, double> sdratio = new Dictionary<Good, double>();
            Dictionary<Good, double> spratio = new Dictionary<Good, double>();

            //財ごとに処理が分かれているので、並列処理
            foreach (Good g in Good.values.Values)

            {
                double demand = 0;
                int produce = 0;
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

                    /**0だと数学的に都合が悪いので1にする*/
                    bool demandmod=false, producemod=false;
                    if (demand == 0)
                    {
                        demand = 1;
                        demandmod = true;
                    }
                    if (produce == 0)
                    {
                        producemod = true;
                        produce = 1;
                    }
                    m.SetMaxStock(stock);
                    m.calc(time, demand, produce);
                    if (produce != 0)
                    {
                        spratio.Add(g, m.marketsupply / produce);
                    }

                    if (demand != 0)
                    {
                        Debug.Print(g.name + " supplyratio : " + m.marketsupply / demand);
                        sdratio.Add(g, m.marketsupply / demand);
                    }

                    /**需要や供給を1増やした場合は、それを地元住民が需要/供給したということにする*/
                    if (producemod)
                    {
                        double moneychange = (double)m.marketsupply / produce * 1 * m.price;
                        Debug.Print(g.name + " " + moneychange + " -> " + localpeople);
                        localpeople.money += moneychange;
                    }

                    if (demandmod)
                    {
                        double moneychange = (double)m.marketsupply / demand * 1 * m.price;
                        Debug.Print(g.name + " " + localpeople + " -> " + moneychange);
                        localpeople.money -= moneychange;
                    }
                    foreach (Facility facility in facilities.Values)
                    {
                        if (produce != 0)
                        {
                            double moneychange = (double)m.marketsupply / produce * facility.getProduct(g) * m.price;
                            Debug.Print(g.name +" " +moneychange+ " -> "+facility.owner);
                            facility.owner.money += moneychange;
                        }
                        if (demand != 0)
                        {
                            double moneychange = (double)m.marketsupply / demand * facility.getDemand(g) * m.price;
                            Debug.Print(g.name + " " + facility.owner + " -> " + moneychange);
                            facility.owner.money -= moneychange;
                        }

                    }
                }

            }
            foreach (Facility facility in facilities.Values)
            {

                facility.afterMarket(sdratio, spratio);
            }

        }
    }
}
