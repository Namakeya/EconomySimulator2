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
                    facilitytext.Append(f).Append(" : ").Append(f.amount).Append("\n");
                }

                Dictionary<string,Agent> agentnames = new Dictionary<string, Agent> ();

                foreach (Agent f in Agent.agents.Values)
                {

                    if (f.location == region)
                    {

                        
                        agentnames.Add(f.name,f);
                    }
                }

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
                //このオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。が発生
                pagegraph.Dispatcher.Invoke((Action)(() =>
                {
                    pagegraph.pricechart1.Plot.Clear();
                    pagegraph.pricechart1.Plot.Title("Price");

                    if (points.Length > 0)
                    {
                        SignalPlot sp = pagegraph.pricechart1.Plot.AddSignal(points);
                    }

                    pagegraph.pricechart1.Plot.Render();

                    pagegraph.supplychart1.Plot.Clear();
                    pagegraph.supplychart1.Plot.Title("Supply");
                    if (points2.Length > 0)
                    {
                        SignalPlot sp2 = pagegraph.supplychart1.Plot.AddSignal(points2);
                    }

                    pagegraph.supplychart1.Plot.Render();

                    pagegraph.facilityLabel.Content = facilitytext;

                    
                   
                    pagegraph.historyLabel.Text = transactiontext.ToString();


                    //毎回Clearを使うと反応が良くないので、同じAgentについてはボタンを使いまわす
                    int buttonnum = 0;
                    while(buttonnum<pagegraph.agentPanel.Children.Count)
                    {
                        var b = pagegraph.agentPanel.Children[buttonnum];
                        if (b is Button)
                        {
                            Button button = (Button)b;
                            string name = button.Name[7..];
                            if (agentnames.ContainsKey(name))
                            {
                                agentnames.Remove(name);
                                buttonnum++;
                            }
                            else
                            {
                                pagegraph.agentPanel.Children.Remove(button);
                            }
                        }
                    }

                    foreach (string name in agentnames.Keys) {
                        Agent agent = agentnames[name];
                        StringBuilder agenttext = new StringBuilder();
                        agenttext.Append(agent.name).Append(" : ").Append($"{agent.money:f0}").Append("\n");
                        string text = agenttext.ToString();
                        Button b1 = new Button();
                        b1.Content = text;
                        b1.Name = "button_"+ name;
                        /*
                        Thickness margin = b1.Margin;
                        margin.Left = v.X;
                        margin.Right = pageMap.ActualWidth - v.X - 40;
                        margin.Top = v.Y;
                        margin.Bottom = pageMap.ActualHeight - v.Y - 30;
                        */


                        //b1.Width = 60;
                        b1.Height = 30;
                        b1.Click += (sender, e) => ButtonDynamicEvent(sender);
                        //Grid.SetRow(b1, 3);
                        //Grid.SetColumn(b1, 0);

                        pagegraph.agentPanel.Children.Add(b1);
                        }
                }));
                


            }

        }

        private void ButtonDynamicEvent(object sender)
        {
            Debug.WriteLine(((Button)sender).Name + "がクリックされました。");
            string name = ((Button)sender).Name;
            string agentname = name[7..];

            if (Agent.agents.ContainsKey(agentname))
            {
                Debug.WriteLine("Agent found");
                //NavigationService.Navigate(pagemap);
            }
        }
    }
}
