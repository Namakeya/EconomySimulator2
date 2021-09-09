using EconomySimulator2.facility;
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
        public Region r1;
        public Region r2;

        public void Start()
        {
            Facility.facilities.Add(FacilityFarm.NAME, FacilityFarm.Factory);
            Facility.facilities.Add(FacilityPop.NAME, FacilityPop.Factory);
            Facility.facilities.Add(FacilityBrewery.NAME, FacilityBrewery.Factory);
            Facility.facilities.Add(FacilityTemporal.NAME, FacilityTemporal.Factory);

            r1 = new Region();
            r1.name = "Region 1";

            r2 = new Region();
            r2.name = "Region 2";

            Agent local = new Agent();
            local.name = r1.name + "_local";
            local.location = r1;
            Agent.addAgent(local);
            r1.localpeople = local;
            Agent local2 = new Agent();
            local2.name = r2.name + "_local";
            local2.location = r2;
            Agent.addAgent(local2);
            r2.localpeople = local2;

            LocalTrader trader = new LocalTrader();
            trader.name = r1.name + "_localtrader";
            trader.location = r1;
            trader.destination = r2;
            Agent.addAgent(trader);

            Facility.addFacility("Farm", 7, local, r1);
            Facility.addFacility("Pop", 2000, local, r1);

            Facility.addFacility("Farm", 8, local2, r2);
            Facility.addFacility("Pop", 1000, local2, r2);
            Facility.addFacility("Brewery", 4, local2, r2);

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
            foreach (Agent agent in Agent.agents.Values)
            {
                agent.Action(tick);
            }
            r1.calc(tick);
            r2.calc(tick);
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
