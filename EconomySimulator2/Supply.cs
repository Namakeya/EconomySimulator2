﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    class Supply
    {
        public Good good;
        public int supplyBase;
        public int stockbase;
        public int currentstock;
        public double stockgain = 0.01;
        public double maxExp = 5;

        public Supply(Good good, int supplyBase, int stockbase, int currentstock)
        {
            this.good = good;
            this.supplyBase = supplyBase;
            this.stockbase = stockbase;
            this.currentstock = currentstock;
        }

        public virtual int GetBaseSupply(int time)
        {
            return supplyBase;
        }

        /**備蓄戦略を表している。値が大きいほど備蓄の増減も大きい。価格と在庫量をもとにsigmoid関数を用いて決めているが他にも戦略はありうる*/
        public virtual double StockExp(double pricerate)
        {
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
