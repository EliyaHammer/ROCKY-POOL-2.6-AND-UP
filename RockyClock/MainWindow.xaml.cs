using RockyClock.View;
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

namespace RockyClock
{
    public partial class MainWindow : Window
    {
        public MainWindowVM viewModel { get; private set; }
        public MainWindow()
        {
            viewModel = new MainWindowVM(this);
            this.DataContext = this.viewModel;

            InitializeComponent();

        }


        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.Import();
            }

            catch (Exception ex)
            {
                MessageBox.Show("תקלה. אנא נסה שנית או צור קשר" + ex.Message);
            }

        }

        private void ShowByEmpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LogsForEmpView logsView = new LogsForEmpView(viewModel.Interface.GetByEmpMonthYear(viewModel.SelectedMonth, viewModel.SelectedYear, viewModel.SelectedEmp), viewModel.Interface, viewModel.SelectedYear, viewModel.SelectedMonth);
                logsView.Show();
                this.Close();

            }

            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                    MessageBox.Show(ex.Message);

                else
                MessageBox.Show("תקלה. אנא נסה שנית או צור קשר" + ex.Message);
            }
        }

        private void ShowAllWorkers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AllLogsView allView = new AllLogsView(viewModel.Interface.GetAllMonthAndYeah(viewModel.SelectedMonth, viewModel.SelectedYear), viewModel.Interface, this, viewModel.SelectedYear, viewModel.SelectedMonth);
                allView.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                    MessageBox.Show(ex.Message);

                else
                MessageBox.Show(ex.Message);
            }
        }
    }
}
