using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2
{
    delegate Facility FacilityFactory(Region region);
    abstract class Facility
    {
        //そのRegionに無いFacilityを作るときにはFacilityFactoryが新しいインスタンスを作る
        public static Dictionary<string, FacilityFactory> facilities = new Dictionary<string, FacilityFactory>();
        public string name;
        public int amount;
        public Region region;
        public Agent owner;

        public Facility(string name,Region region)
        {
            this.name = name;
            this.region = region;
            this.amount = 1;
        }

        public abstract int getDemand(Good good);

        public abstract int getProduct(Good good);

        public abstract void setSupplyRatio(Dictionary<Good,double> ratio);

        public abstract int getStock(Good good);
    }
}
