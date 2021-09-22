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
                double[] points;

                lock (agent.moneyLog)
                {
                    points = new double[agent.moneyLog.Count > 100 ? 100 : agent.moneyLog.Count];
                    int i = 0;
                    foreach (double price in agent.moneyLog.Values)
                    {
                        if (i >= points.Length)
                        {
                            break;
                        }
                        points[i] = price;
                        i++;
                    }
                }


                double[] points2=null;
                lock (agent.goodsLog)
                {
                    if (mainWindow.itemname != null && agent.goodsLog.ContainsKey(Good.values[mainWindow.itemname]))
                    {
                        Dictionary<int,int> dic = agent.goodsLog[Good.values[mainWindow.itemname]];
                        points2 = new double[dic.Count > 100 ? 100 :dic.Count];
                        int j = 0;
                        foreach (double supply in dic.Values)
                        {
                            if (j >= points.Length)
                            {
                                break;
                            }
                            points2[j] = supply;
                            j++;
                        }
                    }
                
                }
                //このオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。が発生
                pagegraph.Dispatcher.Invoke((Action)(() =>
                {
                    pagegraph.moneychart1.Plot.Clear();
                    pagegraph.moneychart1.Plot.Title("Money");

                    if (points.Length > 0)
                    {
                        SignalPlot sp = pagegraph.moneychart1.Plot.AddSignal(points);
                    }

                    pagegraph.moneychart1.Plot.Render();

                    pagegraph.goodschart1.Plot.Clear();
                    pagegraph.goodschart1.Plot.Title("Supply");
                if (points2 != null &&  points2.Length > 0)
                    {
                        SignalPlot sp2 = pagegraph.goodschart1.Plot.AddSignal(points2);
                    }

                    pagegraph.goodschart1.Plot.Render();

                }));




            }
        }

    }
}
