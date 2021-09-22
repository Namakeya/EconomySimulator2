using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EconomySimulator2
{
    public delegate Facility FacilityFactory(Region region);
    public abstract class Facility
    {
        //そのRegionに無いFacilityを作るときにはFacilityFactoryが新しいインスタンスを作る
        public static Dictionary<string, FacilityFactory> facilities = new Dictionary<string, FacilityFactory>();
        public string name;
        public int amount;
        public Region region;
        public Agent owner;

        public Facility(string name, Region region)
        {
            this.name = name;
            this.region = region;
            this.amount = 1;
        }

        public abstract int getDemand(Good good);

        public abstract int getProduct(Good good);

        /**@param sdratio : 市場供給量/需要
         * @param spratio : 市場供給量/生産量
         */
        public abstract void afterMarket(Dictionary<Good, double> sdratio, Dictionary<Good, double> spratio);

        public abstract int getStock(Good good);

        public virtual void mergeFacility(Facility target)
        {
            this.amount += target.amount;
        }

        public static Facility makeFacility(string name, int amount, Agent owner, Region region)
        {
            string fullname = name + "_" + region.name + "_" + owner.name;

            Facility f = Facility.facilities[name](region);
            f.amount = amount;
            f.owner = owner;
            f.name = fullname;
            return f;
        }

        public static Facility addFacility(string name, int amount, Agent owner, Region region)
        {

            return addFacility(makeFacility(name, amount, owner, region), owner, region);

        }
        public static Facility addFacility(Facility facility, Agent owner, Region region)
        {
            string fullname = facility.name;
            if (region.facilities.ContainsKey(fullname))
            {
                region.facilities[fullname].mergeFacility(facility);
                return region.facilities[fullname];
            }
            else
            {
                region.facilities.Add(fullname, facility);
                owner.facilities.Add(fullname, facility);
                return facility;
            }
        }

        public static void removeFacility(Facility facility, int amount)
        {
            if (facility.region.facilities.ContainsValue(facility))
            {
                facility.amount -= amount;
                if (facility.amount <= 0)
                {
                    facility.region.facilities.Remove(facility.name);
                    facility.owner.facilities.Remove(facility.name);
                }
            }
            else
            {
                Debug.Print(facility.region.name + " does not have facility " + facility.name);
            }
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
