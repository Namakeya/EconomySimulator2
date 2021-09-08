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
        private ThreadTimerTest ttt = new ThreadTimerTest();
        private Render render = new Render();
        public PageMap()
        {
            InitializeComponent();
            pageGraph = new PageGraph(this);
        }

        private void buttonToGraph_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(pageGraph);
        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = Task.Run(() =>
            {
                ttt.Start();
            });
            Task task2 = Task.Run(() =>
            {
                render.Start(ttt, this);
            });
            runButton.IsEnabled = false;
        }
    }
}
