using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    class FacilityFarm : Facility
    {
        public static string NAME = "Farm";

        public double efficiency = 1;



        public FacilityFarm(string name, Region region) : base(name, region)
        {

        }
        public static Facility Factory(Region region)
        {
            return new FacilityFarm(NAME, region);
        }

        public override int getDemand(Good good)
        {
            if (good == Good.WORKER)
            {
                return 100 * amount;
            }
            else
            {
                return 0;
            }
        }

        public override int getProduct(Good good)
        {
            if (good == Good.GRAIN)
            {
                return (int)(30 * amount * efficiency);
            }
            else
            {
                return 0;
            }
        }

        public override int getStock(Good good)
        {
            if (good == Good.GRAIN)
            {
                return amount * 100;
            }
            else
            {
                return 0;
            }
        }

        public override void afterMarket(Dictionary<Good, double> sdratio, Dictionary<Good, double> spratio)
        {
            efficiency = 1;
            if (sdratio[Good.WORKER] < 1)
            {
                //労働力の供給/需要 = 生産効率
                efficiency *= sdratio[Good.WORKER];
            }
        }
    }
}
