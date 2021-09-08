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

        public void Start(ThreadTimerTest ttt, MainWindow mainWindow)
        {

            mainWindow.pricechart1.Plot.SetAxisLimitsX(0, 100);
            mainWindow.pricechart1.Plot.SetAxisLimitsY(0, 1000);
            while (true)
            {
                Run(ttt, mainWindow);
                Thread.Sleep(500);
            }
        }

        public void Run(ThreadTimerTest ttt, MainWindow mainWindow)
        {
            Debug.Print("render");

            if (ttt.r1.market.ContainsKey(Good.ALCOHOL.name))
            {
                var syncObject = new object();
                double[] points;
                lock (syncObject) 
                { 
                    Market market = ttt.r1.market[Good.ALCOHOL.name];
                    points = new double[market.priceLog.Count];
                    int i = 0;
                    foreach (double price in market.priceLog.Values)
                    {
                        points[i] = price;
                        i++;
                    }
                }
                mainWindow.pricechart1.Plot.Clear();

                SignalPlot sp = mainWindow.pricechart1.Plot.AddSignal(points);

                mainWindow.pricechart1.Plot.Render();

            }

        }
    }
}
