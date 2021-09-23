using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EconomySimulator2
{
    class RenderMap
    {

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
            if (mainWindow.itemname != null)
            {
                for (int i = 0; i < ThreadTimerTest.regionCount; i++)
                {
                    if (ttt.regions[i].market.ContainsKey(mainWindow.itemname))
                    {
                        Market market = ttt.regions[i].market[mainWindow.itemname];
                        mainWindow.Dispatcher.Invoke((Action)(() =>
                        {
                            mainWindow.buttons[i].Content = ttt.regions[i].name + "\n" + $"{market.price:f2}";
                        }));
                    }
                    else
                    {
                        mainWindow.Dispatcher.Invoke((Action)(() =>
                        {
                            mainWindow.buttons[i].Content = ttt.regions[i].name;
                        }));
                    }
                }
            }
        }

    }
}
