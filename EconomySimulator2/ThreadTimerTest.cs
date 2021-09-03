using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace EconomySimulator2
{
    class ThreadTimerTest
    {
        public int tick;
        public Good g1;
        public Good g2;
        public Region r1;

        public void Start()
        {
           
            g1 = new Good("Grain", 100, 0.5);
            g2 = new Good("Alcohol", 100, 1.5);
            r1 = new Region();
            r1.addGood(g1, 10, new Supply(g1, 12, 10,0));
            r1.addGood(g2, 10, new Supply(g1, 12, 100,0));

            while (true)
            {
                Run();
                Thread.Sleep(500);
            }
        }

        public void Run()
        {
            Debug.Print("tick : "+tick);
            r1.calc(g1,tick);
            //r1.calc(g2,tick);
            tick++;

        }
    }
}
