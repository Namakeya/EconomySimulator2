using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    class SupplyMonthly : Supply
    {

        public double[] fluct;

        public SupplyMonthly(Good good, int supplyBase, int stockbase, int currentstock, double[] fluctuation)
            : base(good, supplyBase, stockbase, currentstock)
        {
            this.fluct = fluctuation;
        }

        public override int GetBaseSupply(int time)
        {
            int month = (time % fluct.Length);
            return (int)(fluct[month] * supplyBase);
        }
    }


}
