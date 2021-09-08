using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EconomySimulator2.facility
{
    /**amountは無効*/
    class FacilityTemporal : Facility
    {
        public static string NAME = "Temp";
        public Dictionary<Good, int> demands = new Dictionary<Good, int>();
        public Dictionary<Good, int> products = new Dictionary<Good, int>();

        public FacilityTemporal(string name, Region region) : base(name, region)
        {

        }

        public static Facility Factory(Region region)
        {
            return new FacilityTemporal(NAME, region);
        }


        public override void afterMarket(Dictionary<Good, double> sdratio, Dictionary<Good, double> spratio)
        {
            foreach (Good g in sdratio.Keys)
            {
                owner.addGoods(g, (int)(getDemand(g) * sdratio[g]));
            }
            //todo 余った財の返品
            Facility.removeFacility(this, this.amount);
        }

        public override int getDemand(Good good)
        {
            if (demands.ContainsKey(good))
            {
                return demands[good];
            }
            else
            {
                return 0;
            }
        }

        public override int getProduct(Good good)
        {
            if (products.ContainsKey(good))
            {
                return products[good];
            }
            else
            {
                return 0;
            }
        }

        public override int getStock(Good good)
        {
            return 0;
        }

        public override void mergeFacility(Facility target)
        {
            if (target is FacilityTemporal)
            {
                FacilityTemporal temp = (FacilityTemporal)target;
                foreach (Good good in temp.demands.Keys)
                {
                    int amount = temp.demands[good];
                    if (demands.ContainsKey(good))
                    {

                        if (demands[good] + amount > 0)
                        {
                            demands[good] = demands[good] + amount;
                        }
                        else
                        {
                            demands.Remove(good);
                        }
                    }
                    else
                    {
                        demands.Add(good, amount);
                    }
                }
                foreach (Good good in temp.products.Keys)
                {
                    int amount = temp.products[good];
                    if (products.ContainsKey(good))
                    {
                        if (products[good] + amount > 0)
                        {
                            products[good] = products[good] + amount;
                        }
                        else
                        {
                            products.Remove(good);
                        }
                    }
                    else
                    {
                        products.Add(good, amount);
                    }
                }
            }
            else
            {
                Debug.Print(target + " cannot merge with " + this);
            }
        }
    }
}

