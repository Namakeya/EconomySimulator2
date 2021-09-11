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
            pagegraph.pricechart1.Plot.SetAxisLimitsX(0, 100);
            pagegraph.pricechart1.Plot.SetAxisLimitsY(0, 1000);
            while (true)
            {
                Run(ttt, mainWindow);
                Thread.Sleep(500);
            }
        }

        public void Run(ThreadTimerTest ttt, PageMap mainWindow)
        {
            Debug.Print("render");
            PageGraph pagegraph = mainWindow.pageGraph;
            var syncObject = new object();
            lock (syncObject)
            {
                if (mainWindow.itemname != null && region.market.ContainsKey(mainWindow.itemname))
                {
                    double[] points;

                    Market market = region.market[mainWindow.itemname];
                    points = new double[market.priceLog.Count>100?100: market.priceLog.Count];
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

                    pagegraph.pricechart1.Plot.Clear();
                    pagegraph.pricechart1.Plot.Title("Price");

                    SignalPlot sp = pagegraph.pricechart1.Plot.AddSignal(points);

                    pagegraph.pricechart1.Plot.Render();

                    double[] points2;

                    points2 = new double[market.supplyLog.Count > 100 ? 100 : market.supplyLog.Count];
                    int j = 0;
                    foreach (double supply in market.supplyLog.Values)
                    {
                        if (i >= points.Length)
                        {
                            break;
                        }
                        points2[j] = supply;
                        j++;
                    }

                    pagegraph.supplychart1.Plot.Clear();
                    pagegraph.supplychart1.Plot.Title("Supply");
                    SignalPlot sp2 = pagegraph.supplychart1.Plot.AddSignal(points2);

                    pagegraph.supplychart1.Plot.Render();
                }

            }

        }
    }
}
