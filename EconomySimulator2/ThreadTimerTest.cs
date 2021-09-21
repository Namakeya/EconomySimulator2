using EconomySimulator2.facility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace EconomySimulator2
{
    class ThreadTimerTest
    {
        public int tick;

        public static readonly int regionCount = 2;
        public Region[] regions = new Region[regionCount];

        public Vector[] regionspos = new Vector[regionCount];

        public Button[] buttons = new Button[regionCount];

        public PageMap pageMap;

        //起動時に実行。ページと同じスレッド
        public void Setup(PageMap pageMap)
        {
            this.pageMap = pageMap;
            Facility.facilities.Add(FacilityFarm.NAME, FacilityFarm.Factory);
            Facility.facilities.Add(FacilityPop.NAME, FacilityPop.Factory);
            Facility.facilities.Add(FacilityBrewery.NAME, FacilityBrewery.Factory);
            Facility.facilities.Add(FacilityTemporal.NAME, FacilityTemporal.Factory);
            Facility.facilities.Add(FacilityConstruction.NAME, FacilityConstruction.Factory);

            Region r1, r2;

            r1 = new Region();
            r1.name = "Region1";

            r2 = new Region();
            r2.name = "Region2";



            Agent local = new Agent();
            local.name = r1.name + "_local";
            local.location = r1;
            Agent.addAgent(local);
            r1.localpeople = local;
            Agent local2 = new Agent();
            local2.name = r2.name + "_local";
            local2.location = r2;
            Agent.addAgent(local2);
            r2.localpeople = local2;

            LocalTrader trader = new LocalTrader();
            trader.name = r1.name + "_localtrader";
            trader.location = r1;
            trader.destination = r2;
            Agent.addAgent(trader);

            Facility.addFacility("Farm", 7, local, r1);
            Facility.addFacility("Pop", 2000, local, r1);

            Facility.addFacility("Farm", 8, local2, r2);
            Facility.addFacility("Pop", 1000, local2, r2);
            Facility.addFacility("Brewery", 4, local2, r2);

            //r1.addMarket(new Market(g2, 10, new Supply(g1, 12, 100,0)));

            regions[0] = r1;
            regionspos[0] = new Vector(30, 30);
            regions[1] = r2;
            regionspos[0] = new Vector(80, 30);

            for (int i = 0; i < regions.Length; i++)
            {
                Region r = regions[i];
                Vector v = regionspos[i];
                Button b1 = new Button();
                b1.Content = r.name;
                b1.Name = "button_" + r.name;
                /*
                Thickness margin = b1.Margin;
                margin.Left = v.X;
                margin.Right = pageMap.ActualWidth - v.X - 40;
                margin.Top = v.Y;
                margin.Bottom = pageMap.ActualHeight - v.Y - 30;
                */


                b1.Margin = new Thickness(v.X * 2 - 300, v.Y * 2 - 300, 0, 0);
                b1.Width = 60;
                b1.Height = 40;
                b1.Click += (sender, e) => ButtonDynamicEvent(sender);
                Grid.SetRow(b1, 3);
                Grid.SetColumn(b1, 0);
                pageMap.gridMain.Children.Add(b1);
                buttons[i] = b1;
            }
        }

        public void Start()
        {


            while (true)
            {
                if (pageMap.stop)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    Run();
                    Thread.Sleep(pageMap.timerdelay);
                }
            }
        }
        private void ButtonDynamicEvent(object sender)
        {
            Debug.WriteLine(((Button)sender).Name + "がクリックされました。");
            string name = ((Button)sender).Name;
            string regionname = name.Substring(7);

            foreach (Region r in regions)
            {
                if (r.name.Equals(regionname))
                {
                    pageMap.clickRegionButton(r);
                }
            }
        }
        public void Run()
        {
            Debug.Print("tick : " + tick);
            foreach (Agent agent in Agent.agents.Values)
            {
                agent.Action(tick);
            }

            foreach (Region r in regions)
            {
                r.calc(tick);
            }

            Agent.updateAgentList();
            /*
            if(r1.market["Grain"].price > r2.market["Grain"].price)
            {
                r2.market["Grain"].buy(5);
                r1.market["Grain"].buy(-5);
            }
            else
            {
                r1.market["Grain"].buy(5);
                r2.market["Grain"].buy(-5);
            }
            */


            //r1.calc(g2,tick);
            tick++;

        }
    }
}
