﻿using EconomySimulator2.pages;
using EconomySimulator2.pages.render;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EconomySimulator2
{
    /// <summary>
    /// PageMap.xaml の相互作用ロジック
    /// </summary>
    public partial class PageMap : Page
    {
        public PageGraph pageGraph;
        public PageAgent pageAgent;
        private ThreadTimerTest ttt;
        private Render render = new Render();
        private RenderMap renderMap = new RenderMap();
        private RenderAgent renderAgent = new RenderAgent();
        public string itemname;
        public int timerdelay = 500;
        public bool stop = false;
        public bool isrunning = false;

        public PageMap()
        {
            InitializeComponent();
            foreach (Good good in Good.values.Values)
            {
                MyComboBox.Items.Add(good.name);
            }
            pageGraph = new PageGraph(this);
            pageAgent = new PageAgent(this);
            ttt = new ThreadTimerTest();
            ttt.Setup(this);
        }

        public void setComboBoxSelection(object s)
        {
            MyComboBox.SelectedItem = s;
            itemname = (string)s;
        }

        public void clickRegionButton(object region)
        {
            render.region = (Region)region;
            NavigationService.Navigate(pageGraph);
            pageGraph.enabled = true;
        }


        private void runButton_Click(object sender, RoutedEventArgs e)
        {

            changeSpeed(800);
        }

        public void changeSpeed(int delay)
        {
            this.stop = false;
            this.timerdelay = delay;
            if (!isrunning)
            {
                isrunning = true;


                Task task = Task.Run(() =>
                {
                    ttt.Start();
                });
                Task task2 = Task.Run(() =>
                {
                    render.Start(ttt, this);
                });
                Task task3 = Task.Run(() =>
                {
                    renderMap.Start(ttt, this);
                });
                Task task4 = Task.Run(() =>
                {
                    renderAgent.Start(ttt, this);
                });

            }
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pageGraph.setComboBoxSelection(MyComboBox.SelectedItem);
            itemname = (string)MyComboBox.SelectedItem;
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            this.stop = true;
        }

        private void x3Button_Click(object sender, RoutedEventArgs e)
        {
            changeSpeed(200);
        }

        private void x2Button_Click(object sender, RoutedEventArgs e)
        {
            changeSpeed(500);
        }
    }
}
