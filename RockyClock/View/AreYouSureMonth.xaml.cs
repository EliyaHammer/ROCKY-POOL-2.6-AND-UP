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
using System.Windows.Shapes;

namespace RockyClock.View
{
    /// <summary>
    /// Interaction logic for AreYouSureMonth.xaml
    /// </summary>
    public partial class AreYouSureMonth : Window
    {
        LogsForEmpView first { get; set; }
        AllLogsView second { get; set; }
        public AreYouSureMonth(Window apply)
        {
            InitializeComponent();

            if (apply is LogsForEmpView)
                first = (LogsForEmpView)apply;
            else
                second = (AllLogsView)apply;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (first != null)
            {
                first.DeleteMonth();
                this.Close();
            }

            else if (second != null)
            {
                second.DeleteMonth();
                this.Close();
            }
        }
    }
}
