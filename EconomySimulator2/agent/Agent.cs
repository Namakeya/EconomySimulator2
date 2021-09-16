using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EconomySimulator2
{
    class Agent
    {
        public static Dictionary<string, Agent> agents = new Dictionary<string, Agent>();
        //agent.Action()中にagentsに新しいものを追加するとエラーになるので、まず下に追加してから後で上に追加
        public static Dictionary<string, Agent> nextagents = new Dictionary<string, Agent>();
        //同じく削除してもエラーなので下に追加してから後で削除
        public static List<string> nextremoveagent = new List<string>();
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
            nextagents.Add(agent.name, agent);
        }

        public static void removeAgent(Agent agent)
        {
            if (agents.ContainsKey(agent.name) && agents[agent.name] == agent)
            {
                nextremoveagent.Add(agent.name);
            }
        }

        public static void updateAgentList()
        {
            foreach (string key in nextremoveagent)
            {
                agents.Remove(key);
            }
            nextremoveagent.Clear();
            foreach(string key in nextagents.Keys)
            {
                agents.Add(key, nextagents[key]);
            }
            nextagents.Clear();
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

        public override string ToString()
        {
            return name;
        }
    }
}
