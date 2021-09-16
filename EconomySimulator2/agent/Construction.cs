using EconomySimulator2.facility;
using System;
using System.Collections.Generic;
using System.Text;

namespace EconomySimulator2.agent
{
    delegate void FinishConstruction(int tick);
    class Construction : Agent
    {
        public Agent owner;
        public Dictionary<Good, int> needs = new Dictionary<Good, int>();
        public int worksneeded;
        public int duration;

        public double procedure=0;

        private FinishConstruction finishConstruction;


        public void setup(Agent owner, Dictionary<Good, int> needs, int worksneeded, int duration,FinishConstruction finish)
        {
            this.owner = owner;
            this.needs = needs;
            this.worksneeded = worksneeded;
            this.duration = duration;
            finishConstruction = finish;
        }


        public override void Action(int tick)
        {
            base.Action(tick);
            owner.money += this.money;
            this.money = 0;
            //材料と労働力を消費して進行
            double newproc=(double)getGoods(Good.WORKER)/worksneeded;

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
                finishConstruction(tick);
                Agent.removeAgent(this);
                return;
            }

            FacilityTemporal facilityTemporal = (FacilityTemporal)Facility.makeFacility("Temp", 1, this, location);
            Facility.addFacility(facilityTemporal, this, location);
            double nextproc=1;
            foreach (Good g in needs.Keys)
            {
                if (this.getGoods(g) < needs[g]*(1-procedure))
                {
                    facilityTemporal.demands.Add(g, needs[g] / duration);

                }
                double pr = (double)getGoods(g) / needs[g];
                if (pr < nextproc)
                {
                    nextproc = pr;
                }
            }
            facilityTemporal.demands.Add(Good.WORKER, (int)(nextproc * worksneeded));
        }
        //todo テスト

    }
}
