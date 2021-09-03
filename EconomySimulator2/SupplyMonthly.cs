using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    class SupplyMonthly : Supply
    {

        public double[] fluct;

        public SupplyMonthly(Good good, double supplyBase, double stockbase, double currentstock, double[] fluctuation)
            : base(good, supplyBase, stockbase, currentstock)
        {
            this.fluct = fluctuation;
        }

        public override double GetBaseSupply(int time)
        {
            int month = (time % fluct.Length);
            return fluct[month] * supplyBase;
        }
    }


}
