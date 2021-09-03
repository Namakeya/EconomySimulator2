using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    class Supply
    {
        public Good good;
        public double supplyBase;
        public double stockbase;
        public double currentstock;
        public double stockgain = 0.01;
        public double maxExp = 5;

        public Supply(Good good, double supplyBase, double stockbase, double currentstock)
        {
            this.good = good;
            this.supplyBase = supplyBase;
            this.stockbase = stockbase;
            this.currentstock = currentstock;
        }

        public double GetBaseSupply(int time)
        {
            int month = 1 + (time % 12);
            if (8 <= month && month <= 11)
            {
                return supplyBase*2;
            }
            else
            {
                return supplyBase / 10;
            }
        }

        public double StockExp(double pricerate)
        {
            double rate = maxExp*Sigmoid(stockgain,(currentstock / stockbase - 1) * (pricerate - 1));
            return rate > 100 ? 100 : rate;
        }

        public double Sigmoid(double a,double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x*a));
        }
    }
}
