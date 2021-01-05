using RockyClock.ViewModel;
using RockyDLL;
using RockyDLL.POCO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;
using PrintDialog = System.Windows.Controls.PrintDialog;

namespace RockyClock.View
{
    /// <summary>
    /// Interaction logic for AllLogsView.xaml
    /// </summary>
    public partial class AllLogsView : Window
    {
        public AllLogsVM VM { get; private set; }
        public UserInterface inter { get; set; }
        public int MonthSel { get; set; }
        public int YearSel { get; set; }
        public MainWindow Hidden { get; private set; }
        public AllLogsView(Employee[] logs, UserInterface inter, MainWindow hidden, int year, int month)
        {
            this.MonthSel = month;
            this.YearSel = year;

                if (logs == null || logs.Length <= 0)
                {
                    throw new NullReferenceException("אין מידע להצגה לתאריכים אלו.");
                }

                InitializeComponent();
                this.inter = inter;
                VM = new AllLogsVM(logs, inter);
                this.DataContext = VM;
                LogsView.ItemsSource = VM.Logs.ToList();
                 this.Hidden = hidden;
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
                MessageBox.Show("תקלה. אנא נסה שנית או צור קשר" + ex.Message);
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

                    AllLogsEditRowView edit = new AllLogsEditRowView((Employee)LogsView.SelectedItem, this, inter);
                    edit.ShowDialog();
                }

            }


            catch (Exception ex)
            {
                MessageBox.Show("תקלה. אנא נסה שנית או צור קשר" + ex.Message);
            }
        }

        private void DeleteRowF_Click(object sender, RoutedEventArgs e)
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

        public void DeleteRow ()
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

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
                AreYouSureMonth win = new AreYouSureMonth(this);
                win.ShowDialog();
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

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            //divide to arrays
            List<Employee> AllLogs = this.VM.Logs.ToList();
            List<AllPrint> allWindows = new List<AllPrint>();

            double numberOfWindows = AllLogs.Count / 39.0;
            int finalNumber = 0;

            if (numberOfWindows.ToString().Length > 1)
            {
                finalNumber = (int)numberOfWindows + 1;
            }
            else if (numberOfWindows < 1)
            {
                finalNumber = 1;
            }
            else
                finalNumber = (int)numberOfWindows;


            //make windows for each array
            if (finalNumber == 1)
            {
                allWindows.Add(new AllPrint(AllLogs.ToArray(), VM.MonthAndYear));
                //here print this
            }

            else
            {
                //loop for first 32 > remover first 32
                //again 
                //if last > loop for the end of count
                List<Employee> removedList = AllLogs;

                for (int i = 0; i < finalNumber; i++)
                {
                    if (i == finalNumber - 1)
                    {
                        allWindows.Add(new AllPrint(removedList.ToArray(), VM.MonthAndYear));
                    }

                    else
                    {
                        List<Employee> chosenLogs = new List<Employee>();

                        for (int j = 0; j < 39; j++)
                        {
                            chosenLogs.Add(removedList[j]);
                        }

                        removedList.RemoveRange(0, 39);
                        allWindows.Add(new AllPrint(chosenLogs.ToArray(), VM.MonthAndYear));
                    }
                }
            }

            MessageBox.Show($"אתה עומד להדפיס {allWindows.Count()} עמודים, יש לאשר כל אחד מהם בנפרד");
            foreach (AllPrint p in allWindows)
                p.ShowDialog();


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

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                //choose a path
                FolderBrowserDialog f = new FolderBrowserDialog();
                f.ShowDialog();
                string fileName;
                string month;
                string year;

                if (VM.MonthAndYear.Length > 6)
                {
                    month = VM.MonthAndYear.Substring(0, 2);
                    year = VM.MonthAndYear.Substring(3);
                    fileName = $"{month}_{year}";
                }

                else
                {
                    month = VM.MonthAndYear.Substring(0, 1);
                    year = VM.MonthAndYear.Substring(2);
                    fileName = $"{month}_{year}";
                }

                string path = f.SelectedPath + $"\\{fileName}.xls";

                List<LogsHolder> allHolders = new List<LogsHolder>();

                for (int i = 0; i < Hidden.EmpList.Items.Count; i++)
                {
                    LogsHolder holder = inter.GetByEmpMonthYear(Convert.ToInt32(month), Convert.ToInt32(year), Hidden.viewModel.Names[i]);
                    allHolders.Add(holder);
                }

                inter.Export(allHolders.ToArray(), path, VM.MonthAndYear);

            }

            catch (Exception ex)
            {
                MessageBox.Show($"אירעה תקלה בייצוא: {ex.Message}");
            }
        }
    }
}
