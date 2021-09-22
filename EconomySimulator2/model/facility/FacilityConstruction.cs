using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EconomySimulator2.facility
{
    delegate void FinishConstruction(FacilityConstruction facilityConstruction);
    class FacilityConstruction : Facility
    {
        public static string NAME = "Construction";

        public Dictionary<Good, int> needs = new Dictionary<Good, int>();
        public Dictionary<Good, int> goods = new Dictionary<Good, int>();
        public int worksneeded;
        public int duration;

        public double procedure = 0;

        public Dictionary<Good, int> demands = new Dictionary<Good, int>();

        private FinishConstruction finishConstruction;

        public FacilityConstruction(string name, Region region) : base(name, region)
        {

        }
        public void setup(Dictionary<Good, int> needs, int worksneeded, int duration, FinishConstruction finish)
        {
            this.needs = needs;
            this.worksneeded = worksneeded;
            this.duration = duration;
            finishConstruction = finish;
        }
        public static Facility Factory(Region region)
        {
            return new FacilityConstruction(NAME, region);
        }
        public void addGoods(Good good, int amount)
        {
            if (amount == 0) return;
            if (goods.ContainsKey(good))
            {
                if (goods[good] + amount > 0)
                {
                    goods[good] = goods[good] + amount;
                }
                else
                {
                    goods.Remove(good);
                }
            }
            else
            {
                goods.Add(good, amount);
            }
        }

        public int getGoods(Good good)
        {
            if (goods.ContainsKey(good))
            {
                return goods[good];
            }
            else
            {
                return 0;
            }
        }
        public override void afterMarket(Dictionary<Good, double> sdratio, Dictionary<Good, double> spratio)
        {
            foreach (Good g in sdratio.Keys)
            {
                addGoods(g, (int)(getDemand(g) * sdratio[g]));
            }
            //材料と労働力を消費して進行
            double newproc = (double)getGoods(Good.WORKER) / worksneeded;

            foreach (Good g in needs.Keys)
            {
                double pr = (double)getGoods(g) / needs[g];
                if (pr < newproc) newproc = pr;
            }
            addGoods(Good.WORKER, -getGoods(Good.WORKER));
            foreach (Good g in needs.Keys)
            {
                addGoods(g, -(int)(newproc * needs[g]));
            }

            procedure += newproc;

            if (procedure >= 1)
            {
                finishConstruction(this);
                Facility.removeFacility(this, 1);
                return;
            }


            double nextproc = 1;
            demands.Clear();
            foreach (Good g in needs.Keys)
            {
                if (this.getGoods(g) < needs[g] * (1 - procedure))
                {
                    demands.Add(g, needs[g] / duration);

                }
                double pr = (double)getGoods(g) / needs[g];
                if (pr < nextproc)
                {
                    nextproc = pr;
                }
            }
            demands.Add(Good.WORKER, (int)(nextproc * worksneeded));
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
            return 0;
        }

        public override int getStock(Good good)
        {
            return 0;
        }

        public override void mergeFacility(Facility target)
        {
            Debug.Fail("!Construction should not be merged!");
            Debug.Assert(false);
        }

        public override string ToString()
        {
            return this.name + $"{procedure * 100:f0}" + "%";
        }
    }
}
