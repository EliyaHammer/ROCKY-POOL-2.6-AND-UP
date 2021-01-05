using RockyClock.ViewModel;
using RockyDLL;
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
using System.Windows.Shapes;

namespace RockyClock.View
{
    /// <summary>
    /// Interaction logic for LogsForEmpView.xaml
    /// </summary>
    public partial class LogsForEmpView : Window
    {
        public LogsForEmpVM VM { get; private set; }
        public UserInterface inter { get; set; }
        public int YearSel { get; set; }
        public int MonthSel { get; set; }
        public LogsForEmpView(LogsHolder empLogs, UserInterface inter, int year, int month)
        {
            this.YearSel = year;
            this.MonthSel = month;

                if (empLogs == null)
                    throw new NullReferenceException("לא נבחר עובד.");

                else
                {
                    InitializeComponent();
                    this.inter = inter;
                    VM = new LogsForEmpVM(empLogs, inter);
                    this.DataContext = VM;
                    LogsView.ItemsSource = VM.Logs.ToList();
                }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow main = new MainWindow();
                main.viewModel.SelectedMonth = MonthSel;
                main.viewModel.SelectedYear = YearSel;
                main.Show();
                this.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"תקלה. אנא נסה שנית או צור קשר {ex.Message}");
            }
        }

        private void EditRow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LogsView.SelectedItem == null)
                {
                    MessageBox.Show("לא נבחרה שורה.");
                }

                else
                {
                    EditRowView edit = new EditRowView((Employee)LogsView.SelectedItem, this, inter);
                    edit.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"תקלה. אנא נסה שנית או צור קשר {ex.Message}");
            }
        }

        public void DeleteRow()
        {
            try
            {
                VM.DeleteRow((Employee)LogsView.SelectedItem);
                MessageBox.Show("השורה נמחקה בהצלחה! יש לחזור אל החלון על מנת לראות את השינוי");
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void DeleteMonth()
        {
            VM.DeleteMonth(VM.Logs);
            LogsView.ItemsSource = null;
            VM.OnPropertyChanged("Logs");
            MessageBox.Show("הקובץ נמחק בהצלחה");
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            AreYouSureMonth win = new AreYouSureMonth(this);
            win.ShowDialog();
        }

        private void DeleteRowE_Click(object sender, RoutedEventArgs e)
        {
            if (LogsView.SelectedItem is null)
            {
                MessageBox.Show("לא נבחרה שורה");
            }

            else
            {
                AreYouSure win = new AreYouSure(this);
                win.ShowDialog();
            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            EmpPrint printWin = new EmpPrint(VM.Emp, VM.MonthAndYear, VM.EmpName);

            printWin.ShowDialog();
        }

        private void ShowError_Click(object sender, RoutedEventArgs e)
        {
            List<Employee> OnlyError = new List<Employee>();
            foreach (Employee em in LogsView.ItemsSource)
            {
                if (em.IsError == 1)
                    OnlyError.Add(em);
            }

            LogsView.ItemsSource = OnlyError;

        }

        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            LogsView.ItemsSource = VM.Logs;
        }
    }
}
