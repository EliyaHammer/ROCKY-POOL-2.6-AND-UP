using RockyDLL;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace RockyClock.View
{
    /// <summary>
    /// Interaction logic for AllPrint.xaml
    /// </summary>
    public partial class AllPrint : Window
    {
        private Employee[] TheLogs { get; set; }
        public string Timing { get; set; }

        public AllPrint(Employee[] theLogs, string timing)
        {
            this.TheLogs = theLogs;
            this.Timing = timing;
            InitializeComponent();
            this.LogsView.ItemsSource = TheLogs;
            this.TheDate.Content = Timing;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog print = new PrintDialog();
            print.ShowDialog();
            print.PrintVisual(this, "Employee Logs");
        }
    }
}
