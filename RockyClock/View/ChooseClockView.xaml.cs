using RockyClock.ViewModel;
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
    /// Interaction logic for ChooseClockView.xaml
    /// </summary>
    public partial class ChooseClockView : Window
    {
        private ChooseClockVM VM { get; set; }
        public ChooseClockView()
        {
            try
            {
                VM = new ChooseClockVM();
                this.DataContext = VM;

                InitializeComponent();
            }

            catch (Exception ex)
            {
                MessageBox.Show("תקלה. אנא נסה שנית או צור קשר" + ex.Message);
            }
        }

        private void clockOne_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VM.InsertClock("Two", this);
            }

            catch (Exception ex)
            {
                MessageBox.Show("תקלה. אנא נסה שנית או צור קשר" + ex.Message);
            }
        }

        private void clockTwo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VM.InsertClock("One", this);
            }

            catch (Exception ex)
            {
                MessageBox.Show("תקלה. אנא נסה שנית או צור קשר" + ex.Message);
            }
        }

        private void clockThree_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VM.InsertClock("Three", this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("תקלה. אנא נסה שנית או צור קשר" + ex.Message);
            }
        }
    }
}
