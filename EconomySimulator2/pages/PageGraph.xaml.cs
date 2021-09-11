using System;
using System.Collections.Generic;
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
        }

        public void setComboBoxSelection(object s)
        {
            MyComboBox.SelectedItem = s;
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pagemap.setComboBoxSelection(MyComboBox.SelectedItem);
        }

    }
}
