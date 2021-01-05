using RockyDLL.POCO;
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
    /// Interaction logic for EmpPrint.xaml
    /// </summary>
    public partial class EmpPrint : Window
    {

        public LogsHolder Holder { get; set; }
        public string Timing { get; set; }
        public string EmpName { get; set; }

        public EmpPrint(LogsHolder holder, string timing, string empName)
        {
            this.Holder = holder;
            this.Timing = timing;
            this.EmpName = empName;
            InitializeComponent();

            this.LogsView.ItemsSource = Holder.logs;
            this.TheDate.Content = Timing;
            this.TheName.Content = EmpName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog print = new PrintDialog();
            print.ShowDialog();
            print.PrintVisual(this, "Employee Logs");
        }
    }
}
