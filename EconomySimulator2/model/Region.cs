using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EconomySimulator2
{
    public class Region
    {
        public string name;
        public Dictionary<string, Market> market = new Dictionary<string, Market>();
        public Dictionary<string, Facility> facilities = new Dictionary<string, Facility>();

        public object transactionLockObject = new object();
        public List<Transaction> transactionLog = new List<Transaction>();
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
                    bool demandmod = false, producemod = false;
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
                    lock (transactionLockObject)
                    {
                        foreach (Facility facility in facilities.Values)
                        {
                            if (produce != 0 && facility.getProduct(g) != 0)
                            {
                                double amount = ((double)m.marketsupply / produce * facility.getProduct(g));
                                double moneychange = amount * m.price;
                                Debug.Print(g.name + " " + moneychange + " -> " + facility.owner);
                                facility.owner.money += moneychange;
                                transactionLog.Add(new Transaction(time, m.price, (int)amount, moneychange, m.good, facility.name, "market"));
                            }
                            if (demand != 0 && facility.getDemand(g) != 0)
                            {
                                double amount = (double)m.marketsupply / demand * facility.getDemand(g);
                                double moneychange = amount * m.price;
                                Debug.Print(g.name + " " + facility.owner + " -> " + moneychange);
                                facility.owner.money -= moneychange;
                                transactionLog.Add(new Transaction(time, m.price, (int)amount, moneychange, m.good, "market", facility.name));
                            }

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

    public class Transaction
    {
        public readonly int tick;
        public readonly double price;
        public readonly int amount;
        public readonly double total;
        public readonly Good good;
        public readonly string buyer;//Agentは消えたり生まれたりする可能性があるのでstringで保管
        public readonly string seller;
        public readonly string log;

        public Transaction(int tick, double price, int amount, double total, Good good, string buyer, string seller)
        {
            this.tick = tick;
            this.price = price;
            this.amount = amount;
            this.total = total;
            this.good = good;
            this.buyer = buyer;
            this.seller = seller;
            this.log = new StringBuilder().Append("at ").Append(tick).Append(" :\t").Append(buyer).Append(" -> ").Append(seller).Append("\t")
                .Append(amount).Append(" * ").Append(good.name).Append($"({price:f2})").Append(" = ").Append($"{total:f2}").ToString();
        }

        public override string ToString()
        {
            return log;


        }
    }
}
