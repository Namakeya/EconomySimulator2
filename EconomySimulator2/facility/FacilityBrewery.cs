using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    class FacilityBrewery : Facility
    {
        public static string NAME = "Brewery";

        public double efficiency = 1;



        public FacilityBrewery(string name, Region region) : base(name, region)
        {

        }
        public static Facility Factory(Region region)
        {
            return new FacilityBrewery(NAME, region);
        }

        public override int getDemand(Good good)
        {
            if (good == Good.WORKER)
            {
                return 100 * amount;
            }
            else if (good == Good.GRAIN)
            {
                return 50 * amount;
            }
            else
            {
                return 0;
            }
        }

        public override int getProduct(Good good)
        {
            if (good == Good.ALCOHOL)
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
            if (good == Good.ALCOHOL)
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
            if (sdratio[Good.WORKER] < sdratio[Good.GRAIN])
            {
                if (sdratio[Good.WORKER] < 1)
                {
                    efficiency *= sdratio[Good.WORKER];
                }
            }
            else
            {
                if (sdratio[Good.GRAIN] < 1)
                {
                    efficiency *= sdratio[Good.GRAIN];
                }
            }


        }
    }
}
