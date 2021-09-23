using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace EconomySimulator2
{
    class Render
    {

        public Region region;
        public PageMap mainWindow;

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
            this.mainWindow = mainWindow;
            //Debug.Print("render");
            PageGraph pagegraph = mainWindow.pageGraph;

            if (pagegraph.enabled)
            {
                if (mainWindow.itemname != null && region.market.ContainsKey(mainWindow.itemname))
                {
                    double[] pointsx;
                    double[] pointsy;

                    Market market = region.market[mainWindow.itemname];
                    lock (market.priceLog)
                    {
                        market.priceLog.ToGraph(out pointsx,out pointsy, 100);
                    }


                    double[] points2x;
                    double[] points2y;
                    lock (market.supplyLog)
                    {
                        market.supplyLog.ToGraph(out points2x, out points2y, 100);
                    }

                    StringBuilder facilitytext = new StringBuilder();
                    foreach (Facility f in region.facilities.Values)
                    {
                        facilitytext.Append(f).Append(" : ").Append(f.amount).Append("\n");
                    }

                    Dictionary<string, Agent> agentnames = new Dictionary<string, Agent>();

                    foreach (Agent f in Agent.agents.Values)
                    {

                        if (f.location == region)
                        {


                            agentnames.Add(f.name, f);
                        }
                    }

                    StringBuilder transactiontext = new StringBuilder();
                    lock (region.transactionLockObject)
                    {
                        for (int k = region.transactionLog.Count - 1; k >= 0; k--)
                        {
                            if (region.transactionLog[k].good.name.Equals(mainWindow.itemname))
                            {
                                transactiontext.Append(region.transactionLog[k]).Append("\n");

                            }
                        }
                    }
                    //このオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。を回避
                    pagegraph.Dispatcher.Invoke((Action)(() =>
                    {
                        pagegraph.pricechart1.Plot.Clear();
                        pagegraph.pricechart1.Plot.Title("Price");

                        if (pointsy.Length > 0)
                        {
                            SignalPlotXY sp = pagegraph.pricechart1.Plot.AddSignalXY(pointsx,pointsy);
                        }

                        pagegraph.pricechart1.Plot.Render();

                        pagegraph.supplychart1.Plot.Clear();
                        pagegraph.supplychart1.Plot.Title("Supply");
                        if (points2y.Length > 0)
                        {
                            SignalPlotXY sp2 = pagegraph.supplychart1.Plot.AddSignalXY(points2x, points2y);
                        }

                        pagegraph.supplychart1.Plot.Render();

                        pagegraph.facilityLabel.Content = facilitytext;



                        pagegraph.historyLabel.Text = transactiontext.ToString();


                        //毎回Clearを使うと反応が良くないので、同じAgentについてはボタンを使いまわす
                        int buttonnum = 0;
                        while (buttonnum < pagegraph.agentPanel.Children.Count)
                        {
                            var b = pagegraph.agentPanel.Children[buttonnum];
                            if (b is Button)
                            {
                                Button button = (Button)b;
                                string name = button.Name[7..];
                                if (agentnames.ContainsKey(name))
                                {
                                    Agent agent = agentnames[name];
                                    StringBuilder agenttext = new StringBuilder();
                                    agenttext.Append(agent.name).Append(" : ").Append($"{agent.money:f0}").Append("\n");
                                    string text = agenttext.ToString();
                                    button.Content = text;

                                    agentnames.Remove(name);
                                    buttonnum++;
                                }
                                else
                                {
                                    pagegraph.agentPanel.Children.Remove(button);
                                }
                            }
                        }

                        foreach (string name in agentnames.Keys)
                        {
                            Agent agent = agentnames[name];
                            StringBuilder agenttext = new StringBuilder();
                            agenttext.Append(agent.name).Append(" : ").Append($"{agent.money:f0}").Append("\n");
                            string text = agenttext.ToString();
                            Button b1 = new Button();
                            b1.Content = text;
                            b1.Name = "button_" + name;
                            /*
                            Thickness margin = b1.Margin;
                            margin.Left = v.X;
                            margin.Right = pageMap.ActualWidth - v.X - 40;
                            margin.Top = v.Y;
                            margin.Bottom = pageMap.ActualHeight - v.Y - 30;
                            */


                            //b1.Width = 60;
                            b1.Height = 30;
                            b1.Click += (sender, e) => pagegraph.ButtonDynamicEvent(sender);
                            //Grid.SetRow(b1, 3);
                            //Grid.SetColumn(b1, 0);

                            pagegraph.agentPanel.Children.Add(b1);
                        }
                    }));



                }
            }
        }


    }
}
