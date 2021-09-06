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
        public Dictionary<Good, int> goods = new Dictionary<Good, int>();

        public void Action(int tick)
        {
            Debug.Print("current money : " + money);
        }

        public static void addAgent(Agent agent)
        {
            agents.Add(agent.name, agent);
        }
    }
}
