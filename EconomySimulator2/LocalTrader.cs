using EconomySimulator2.facility;
using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    class LocalTrader:Agent
    {
        public int capacity;
        public Region destination;

        public override void Action(int tick)
        {
            base.Action(tick);
            FacilityTemporal facilityTemporal = (FacilityTemporal)Facility.makeFacility("Temp", 1, this, location);
            Facility.addFacility(facilityTemporal, this, location);
            facilityTemporal.demands.Add(Good.GRAIN, 50);
            //todo 移動して売却(Temporal で produceを設定)
        }
    }
}
