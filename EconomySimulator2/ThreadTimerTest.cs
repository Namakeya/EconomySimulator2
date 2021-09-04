using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace EconomySimulator2
{
    class ThreadTimerTest
    {
        public int tick;
        public Good g1;
        public Good g2;
        public Region r1;
        public Region r2;

        public void Start()
        {

            g1 = new Good("Grain", 100, 0.5);
            g2 = new Good("Alcohol", 100, 1.5);
            r1 = new Region();
            r1.addMarket(new Market(g1, 100, new SupplyMonthly(g1, 120, 100, 0,
                new double[] { 0.3, 0.3, 0.3, 0.3, 0.3, 2, 2, 2, 2, 2, 0.3, 0.3 })));
            r2 = new Region();
            r2.addMarket(new Market(g1, 100, new SupplyMonthly(g1, 120, 100, 0,
                new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 })));
            //r1.addMarket(new Market(g2, 10, new Supply(g1, 12, 100,0)));

            while (true)
            {
                Run();
                Thread.Sleep(500);
            }
        }

        public void Run()
        {
            Debug.Print("tick : " + tick);
            r1.calc(tick);
            r2.calc(tick);
            if(r1.market["Grain"].price > r2.market["Grain"].price)
            {
                r2.market["Grain"].buy(5);
                r1.market["Grain"].buy(-5);
            }
            else
            {
                r1.market["Grain"].buy(5);
                r2.market["Grain"].buy(-5);
            }
            
            
            //r1.calc(g2,tick);
            tick++;

        }
    }
}
