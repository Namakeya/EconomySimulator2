using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EconomySimulator2.pages.render
{
    class RenderAgent
    {
        Agent agent;
        PageMap mainWindow;
        public void Start(ThreadTimerTest ttt, PageMap mainWindow)
        {
            while (true)
            {
                Run(ttt, mainWindow);
                Thread.Sleep(100);
            }
        }

        public void Run(ThreadTimerTest ttt, PageMap mainWindow)
        {
            this.mainWindow = mainWindow;
            
            //Debug.Print("render");
            PageAgent pagegraph = mainWindow.pageAgent;
            agent = pagegraph.agent; 
            if (pagegraph.enabled)
            {
                double[] pointsx;
                double[] pointsy;

                lock (agent.moneyLog)
                {
                    agent.moneyLog.ToGraph(out pointsx, out pointsy, 100);
                }


                double[] points2x=null;
                double[] points2y = null;
                lock (agent.goodsLog)
                {
                    if (mainWindow.itemname != null && agent.goodsLog.ContainsKey(Good.values[mainWindow.itemname]))
                    {
                        LogDictionary dic = agent.goodsLog[Good.values[mainWindow.itemname]];
                        dic.ToGraph(out points2x, out points2y, 100);
                    }

                
                }
                //このオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。が発生
                pagegraph.Dispatcher.Invoke((Action)(() =>
                {
                    pagegraph.moneychart1.Plot.Clear();
                    pagegraph.moneychart1.Plot.Title("Money");

                    if (pointsy.Length > 0)
                    {
                        SignalPlotXY sp = pagegraph.moneychart1.Plot.AddSignalXY(pointsx, pointsy);
                    }

                    pagegraph.moneychart1.Plot.Render();

                    pagegraph.goodschart1.Plot.Clear();
                    pagegraph.goodschart1.Plot.Title("Goods");
                    if (points2y.Length > 0)
                    {
                        SignalPlotXY sp2 = pagegraph.goodschart1.Plot.AddSignalXY(points2x, points2y);
                    }

                    pagegraph.goodschart1.Plot.Render();

                }));




            }
        }

    }
}
