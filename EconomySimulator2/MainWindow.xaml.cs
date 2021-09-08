using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ThreadTimerTest ttt = new ThreadTimerTest();
        private Render render = new Render();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task task = Task.Run(() =>
            {
                ttt.Start();
            });
            Task task2 = Task.Run(() =>
            {
                render.Start(ttt, this);
            });
            runbutton.IsEnabled = false;
        }
    }
}
