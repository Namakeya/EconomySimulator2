using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EconomySimulator2
{
    class Agent
    {
        public static Dictionary<string, Agent> agents = new Dictionary<string, Agent>();
        public string name;
        public double money;
        public Region location;
        public Dictionary<string, Facility> facilities = new Dictionary<string, Facility>();
        private Dictionary<Good, int> goods = new Dictionary<Good, int>();

        public virtual void Action(int tick)
        {
            Debug.Print(name + " money : " + money);
        }

        public static void addAgent(Agent agent)
        {
            agents.Add(agent.name, agent);
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
    }
}
