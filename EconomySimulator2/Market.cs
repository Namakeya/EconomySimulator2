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
        public double demand;
        public Supply supply;

        /**直前の生産量*/
        public double basesupply { get; protected set; }

        /**直前の市場価格*/
        public double price { get; protected set; }

        /**直前の市場供給量*/
        public double marketsupply { get; protected set; }

        public Market(Good good, double demand, Supply supply)
        {
            this.good = good;
            this.demand = demand;
            this.supply = supply;
        }

        public void calc(int time)
        {
            //生産量
            basesupply = supply.GetBaseSupply(time);
            double pricerate = Math.Pow(demand / basesupply, 1 / (good.elasticity));
            Debug.Print(good.name + " supply: " + basesupply + " pricerate : " + pricerate + " stockrate : " + supply.StockExp(pricerate));

            //市場価格
            price = good.price * Math.Pow(demand / basesupply, 1 / (good.elasticity + supply.StockExp(pricerate)));

            //市場供給量
            marketsupply = Math.Pow(price / good.price, supply.StockExp(pricerate)) * basesupply;

            //備蓄増減 正で増え負で減る
            double stockamount = basesupply - marketsupply;

            supply.ChangeStock(stockamount);



            Debug.Print(good.name + " price: " + price + " stock: " + supply.currentstock);
        }

        /**正... 買い 負... 売り*/
        public double buy(double amount)
        {
            double prev = price;
            double pricerate = Math.Pow(demand / basesupply, 1 / (good.elasticity));
            price = good.price * Math.Pow((marketsupply+amount) / basesupply, 1 / (supply.StockExp(pricerate)));
            supply.ChangeStock(-amount);
            Debug.Print(good.name + " price: " + price + " stock: " + supply.currentstock);
            return prev;
        }


    }
}
