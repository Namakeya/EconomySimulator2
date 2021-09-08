using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    class FacilityPop : Facility
    {
        public static string NAME = "Pop";

        public double efficiency = 1;

        public FacilityPop(string name, Region region) : base(name, region)
        {

        }
        public static Facility Factory(Region region)
        {
            return new FacilityPop(NAME, region);
        }

        public override int getDemand(Good good)
        {
            if (good == Good.GRAIN)
            {
                return (int)(0.1 * amount);
            }
            else if (good == Good.ALCOHOL)
            {
                return (int)(0.01 * amount);
            }
            else
            {
                return 0;
            }
        }

        public override int getProduct(Good good)
        {
            if (good == Good.WORKER)
            {
                return (int)(amount * efficiency);
            }
            else
            {
                return 0;
            }
        }

        public override int getStock(Good good)
        {
            if (good == Good.WORKER)
            {
                return (int)(amount * 0.01);
            }
            else
            {
                return (int)(amount * 0.01);
            }
        }

        public override void afterMarket(Dictionary<Good, double> sdratio, Dictionary<Good, double> spratio)
        {
            efficiency = 1;
            if (sdratio[Good.GRAIN] < 1)
            {
                efficiency *= 1 - 0.7 * (1 - sdratio[Good.GRAIN]);
            }
        }
    }
}
