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
            Facility.facilities.Add(FacilityFarm.NAME, FacilityFarm.Factory);
            Facility.facilities.Add(FacilityPop.NAME, FacilityPop.Factory);

            r1 = new Region();
            r1.addFacility("Farm", 7);
            r1.addFacility("Pop", 2000);
            r2 = new Region();
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
            //r2.calc(tick);
            /*
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
            */
            
            
            //r1.calc(g2,tick);
            tick++;

        }
    }
}
