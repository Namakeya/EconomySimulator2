using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
    /// PageGraph.xaml の相互作用ロジック
    /// </summary>
    public partial class PageGraph : Page
    {
        public PageMap pagemap;
        public bool enabled = false;
        public PageGraph(PageMap pagemap)
        {
            InitializeComponent();
            this.pagemap = pagemap;
            foreach (Good good in Good.values.Values)
            {
                MyComboBox.Items.Add(good.name);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(pagemap);
            enabled = false;
        }

        public void setComboBoxSelection(object s)
        {
            MyComboBox.SelectedItem = s;
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pagemap.setComboBoxSelection(MyComboBox.SelectedItem);
        }

        public void ButtonDynamicEvent(object sender)
        {
            Debug.WriteLine(((Button)sender).Name + "がクリックされました。");
            string name = ((Button)sender).Name;
            string agentname = name[7..];

            if (Agent.agents.ContainsKey(agentname))
            {
                Debug.WriteLine("Agent found");
                this.pagemap.pageAgent.enabled = true;
                this.pagemap.pageAgent.agent = Agent.agents[agentname];
                NavigationService.Navigate(this.pagemap.pageAgent);

            }
        }

    }
}
