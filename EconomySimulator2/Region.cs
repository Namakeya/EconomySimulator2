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


        public void addFacility(string name,int amount,Agent owner)
        {
            string facname = name + "_" + owner.name;
            if (facilities.ContainsKey(facname))
            {
                facilities[facname].amount += amount;
            }
            else
            {
                Facility f = Facility.facilities[name](this);
                f.amount = amount;
                f.owner = owner;
                facilities.Add(facname, f);
            }
        }

        public void calc(int time)
        {
            Dictionary<Good, double> supplyRatio = new Dictionary<Good, double>();

            //財ごとに処理が分かれているので、並列処理
            foreach(Good g in Good.values.Values)

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
                    m.SetMaxStock(stock);
                    m.calc(time, demand, produce);
                    Debug.Print(g.name + " supplyratio : " + m.marketsupply / demand);
                    supplyRatio.Add(g, m.marketsupply / demand);
                    foreach (Facility facility in facilities.Values)
                    {
                        if (produce != 0)
                        {
                            //Debug.Print(g.name + " marketsupply: " + m.marketsupply +" produce: "+ facility.getProduct(g));
                            facility.owner.money += (double)m.marketsupply / produce * facility.getProduct(g) * m.price;
                        }
                        if (demand != 0)
                        {
                            //Debug.Print(g.name + " cost " + m.marketsupply / demand * facility.getDemand(g) * m.price);
                            facility.owner.money -= (double)m.marketsupply / demand * facility.getDemand(g) * m.price;
                        }

                    }
                }

            }
            foreach (Facility facility in facilities.Values)
            {

                facility.setSupplyRatio(supplyRatio);
            }

        }
    }
}
