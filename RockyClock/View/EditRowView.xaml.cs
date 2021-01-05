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
    /// Interaction logic for EditRowView.xaml
    /// </summary>
    public partial class EditRowView : Window
    {
        private EditRowVM VM { get; set; }
        private LogsForEmpView windowBack { get; set; }
        public EditRowView(Employee row, LogsForEmpView windowBack, UserInterface inter)
        {
            try
            {
                if (row == null)
                {
                    throw new NullReferenceException("לא נבחרה שורה.");
                }

                this.windowBack = windowBack;

                InitializeComponent();
                VM = new EditRowVM(row, inter);
                this.DataContext = VM;
                RowView.ItemsSource = VM.row;
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                    MessageBox.Show(ex.Message);
                else
                MessageBox.Show("תקלה. אנא נסה שנית או צור קשר");
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                updateRowChanges();
        }

        private void updateRowChanges()
        {

            try
            {
                Employee oldRow = VM.row[0];
                Employee newRow = (Employee)RowView.Items[0];
                newRow.CalculateIsError();
                newRow.IsManuallyChanged = 1;
                windowBack.VM.Logs.ToList().ForEach((e) => { if (e.ID == oldRow.ID && e.Date == oldRow.Date) { e = newRow; } });

                VM.inter.UpdateLog(newRow);
                windowBack.VM.Emp.logs = (Employee[])windowBack.VM.Logs;
                LogsHolder newEmp = windowBack.VM.Emp;
                newEmp.CalculateAll();
                windowBack.VM.Emp = newEmp;
                windowBack.LogsView.ItemsSource = windowBack.VM.Emp.logs;
                this.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("אירעה תקלה בעדכון השורה.  אנא צור קשר. " + ex.Message);
            }

        }
    }
}
