using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace EconomySimulator2
{
    class Render
    {

        public Region region;

        public void Start(ThreadTimerTest ttt, PageMap mainWindow)
        {
            PageGraph pagegraph = mainWindow.pageGraph;
            region = ttt.regions[0];
            pagegraph.pricechart1.Plot.SetAxisLimitsX(0, 100);
            pagegraph.pricechart1.Plot.SetAxisLimitsY(0, 1000);
            while (true)
            {
                Run(ttt, mainWindow);
                Thread.Sleep(100);
            }
        }

        public void Run(ThreadTimerTest ttt, PageMap mainWindow)
        {
            //Debug.Print("render");
            PageGraph pagegraph = mainWindow.pageGraph;
            if (mainWindow.itemname != null && region.market.ContainsKey(mainWindow.itemname))
            {
                double[] points;

                Market market = region.market[mainWindow.itemname];
                lock (market.priceLog)
                {
                    points = new double[market.priceLog.Count > 100 ? 100 : market.priceLog.Count];
                    int i = 0;
                    foreach (double price in market.priceLog.Values)
                    {
                        if (i >= points.Length)
                        {
                            break;
                        }
                        points[i] = price;
                        i++;
                    }
                }


                double[] points2;
                lock (market.supplyLog)
                {
                    points2 = new double[market.supplyLog.Count > 100 ? 100 : market.supplyLog.Count];
                    int j = 0;
                    foreach (double supply in market.supplyLog.Values)
                    {
                        if (j >= points.Length)
                        {
                            break;
                        }
                        points2[j] = supply;
                        j++;
                    }
                }

                StringBuilder facilitytext = new StringBuilder();
                foreach (Facility f in region.facilities.Values)
                {
                    facilitytext.Append(f.name).Append(" : ").Append(f.amount).Append("\n");
                }

                StringBuilder agenttext = new StringBuilder();
                foreach (Agent f in Agent.agents.Values)
                {
                    if (f.location == region)
                    {
                        agenttext.Append(f.name).Append(" : ").Append($"{f.money:f0}").Append("\n");
                    }
                }

                //todo リストをロックする必要がある
                StringBuilder transactiontext = new StringBuilder();
                lock (region.transactionLockObject)
                {
                    for (int k=region.transactionLog.Count-1;k>=0;k--)
                    {
                        if (region.transactionLog[k].good.name.Equals(mainWindow.itemname))
                        {
                            transactiontext.Append(region.transactionLog[k]).Append("\n");

                        }
                    }
                }

                pagegraph.Dispatcher.Invoke((Action)(() =>
                {
                    pagegraph.pricechart1.Plot.Clear();
                    pagegraph.pricechart1.Plot.Title("Price");

                    SignalPlot sp = pagegraph.pricechart1.Plot.AddSignal(points);

                    pagegraph.pricechart1.Plot.Render();

                    pagegraph.supplychart1.Plot.Clear();
                    pagegraph.supplychart1.Plot.Title("Supply");
                    SignalPlot sp2 = pagegraph.supplychart1.Plot.AddSignal(points2);

                    pagegraph.supplychart1.Plot.Render();

                    pagegraph.facilityLabel.Content = facilitytext;

                    pagegraph.agentLabel.Content = agenttext;

                    pagegraph.historyLabel.Text = transactiontext.ToString();
                }));



            }

        }
    }
}
