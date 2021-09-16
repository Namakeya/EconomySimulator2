using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EconomySimulator2
{
    /**需要、供給、備蓄、交易のあるマーケットを表す。ある一つの財のみを扱う。*/
    class Market
    {
        public Good good;
        public Region region;
        public double demand;//前回のdemandを記録
        public double supply;//前回のsupplyを記録
        private int maxstock = 200;
        private int stockbase = 10;
        public int currentstock;
        public double stockgain = 0.01;
        public double maxExp = 5;


        public Dictionary<int, double> priceLog = new Dictionary<int, double>();
        public Dictionary<int, int> supplyLog = new Dictionary<int, int>();

        /**直前の生産量*/
        public int basesupply { get; protected set; }

        /**直前の市場価格*/
        public double price { get; protected set; }

        /**直前の市場供給量*/
        public int marketsupply { get; protected set; }

        /**直近の平均価格*/
        public double avrprice { get; protected set; }

        /**何週間の価格を平均するか*/
        private int avrcount = 24;

        public Market(Good good, Region region)
        {
            this.good = good;
            this.region = region;
            this.avrprice = good.price;
        }

        public void SetMaxStock(int stock)
        {
            this.maxstock = stock;
            stockbase = stock / 20;
        }


        public void calc(int time, double demand, int basesupply)
        {


            this.demand = demand;
            this.supply = basesupply;


            double dsrate = demand / basesupply;
            dsrate = dsrate > 100 ? 100 : dsrate;
            double pricerate = Math.Pow(dsrate, 1 / (good.elasticity));
            if (pricerate > 100) pricerate = 100;
            double ex = StockExp(pricerate);

            Debug.Print(good.name + " supply: " + basesupply + " pricerate : " + pricerate + " stockrate : " + ex);

            //市場価格

            price = avrprice * Math.Pow(dsrate, 1 / (good.elasticity + ex));

            //todo 市場供給量 これは供給戦略によって式が変わるので要検討
            marketsupply = (int)(Math.Pow(price / avrprice, ex) * (basesupply + currentstock / demand));

            //備蓄増減 正で増え負で減る
            int stockamount = basesupply - marketsupply;

            currentstock += stockamount;



            Debug.Print(good.name + " price: " + price + " stock: " + currentstock);
            lock (priceLog)
            {
                priceLog.Add(time, price);
            }
            lock (supplyLog)
            {
                supplyLog.Add(time, marketsupply);
            }


            //todo 平均価格(avrprice)を動的に変動させると価格の乱高下が起こるのでとりあえず無視
            //平均価格は、あくまで「通念上の価格」を表すので色々な決め方がありうる
            /*
            if (priceLog.Count > avrcount)
            {
                avrprice = 0;
                for (int i = time; i > time - avrcount; i--)
                {
                    avrprice += priceLog[i];
                }
                avrprice /= avrcount;
            }
            */

        }

        /**正... 買い 負... 売り*/
        public double buy(int tick, int amount, Agent agent)
        {
            double prev = price;
            double pricerate = Math.Pow(demand / basesupply, 1 / (good.elasticity));
            double ex = StockExp(pricerate);

            price = avrprice * Math.Pow((marketsupply + amount) / basesupply, 1 / ex);

            double sumprice = avrprice / Math.Pow(basesupply, 1 / ex) * ex / (1 + ex) * (Math.Pow(marketsupply + amount - 0.5, (1 + ex) / ex) - Math.Pow(marketsupply - 0.5, (1 + ex) / ex));

            currentstock -= amount;

            Debug.Print(good.name + " sumprice: " + sumprice + " stock: " + currentstock);
            lock (region.transactionLockObject)
            {
                if (amount > 0)
                {
                    region.transactionLog.Add(new Transaction(tick, prev, -amount, -sumprice, good, agent.name, "market"));

                }
                else
                {
                    region.transactionLog.Add(new Transaction(tick, prev, amount, sumprice, good, "market", agent.name));

                }
            }
            return sumprice;
        }

        /**備蓄戦略を表している。値が大きいほど備蓄の増減も大きい。価格と在庫量をもとにsigmoid関数を用いて決めているが他にも戦略はありうる*/
        public virtual double StockExp(double pricerate)
        {
            if (pricerate > 1 && currentstock <= 0)
            {
                return 0;
            }
            else if (pricerate < 1 && currentstock > maxstock)
            {
                return 0;
            }
            if (stockbase == 0) return 0;
            double rate = maxExp * Sigmoid(stockgain, (currentstock / stockbase - 1) * (pricerate - 1));
            return rate > 100 ? 100 : rate;
        }

        public virtual void ChangeStock(int amount)
        {
            currentstock += amount;
        }

        public double Sigmoid(double a, double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x * a));
        }

    }


}
