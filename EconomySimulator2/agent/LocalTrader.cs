using EconomySimulator2.agent;
using EconomySimulator2.facility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EconomySimulator2
{
    class LocalTrader : Agent
    {
        public int capacity = 20;
        public Region destination;
        public int phase = 0;
        public Good tradeitem;

        public bool constructing = false;

        public override void Action(int tick)
        {
            base.Action(tick);
            if (phase == 0)
            {
                //価格差が一番大きいモノを選ぶ
                double pricedif = 0;
                tradeitem = null;
                foreach (Market m in location.market.Values)
                {
                    if (destination.market.ContainsKey(m.good.name))
                    {
                        double p = destination.market[m.good.name].price - m.price;
                        if (p > pricedif)
                        {
                            pricedif = p;
                            tradeitem = m.good;
                        }
                    }
                }
                if (tradeitem != null)
                {


                    FacilityTemporal facilityTemporal = (FacilityTemporal)Facility.makeFacility("Temp", 1, this, location);
                    Facility.addFacility(facilityTemporal, this, location);
                    facilityTemporal.demands.Add(tradeitem, capacity);
                }

                phase = 1;
                Debug.Print("trade item : " + tradeitem);
            }
            else
            {
                Region reg = destination;
                destination = location;
                location = reg;

                if (tradeitem != null)
                {
                    FacilityTemporal facilityTemporal = (FacilityTemporal)Facility.makeFacility("Temp", 1, this, location);
                    Facility.addFacility(facilityTemporal, this, location);
                    facilityTemporal.products.Add(tradeitem, this.getGoods(tradeitem));
                }

                phase = 0;
            }
            if (!constructing && money>Good.GRAIN.price*200)
            {
                Construction cons = new Construction();
                Dictionary<Good, int> needs = new Dictionary<Good, int>();
                needs.Add(Good.GRAIN, 100);
                cons.setup(this,needs,1000,5, FinConst);
                cons.location = this.location;
                cons.name = this.name + "_construction";
                Agent.addAgent(cons);
                constructing = true;
            }

        }

        public void FinConst(int tick)
        {
            this.capacity += 5;
            this.constructing = false;
        }
    }
}
