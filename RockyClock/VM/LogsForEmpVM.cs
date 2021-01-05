using RockyDLL;
using RockyDLL.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RockyClock.ViewModel
{
    public class LogsForEmpVM : INotifyPropertyChanged
    {
        public LogsForEmpVM(LogsHolder empLogs, UserInterface inter)
        {
            try
            {
                this.Emp = empLogs;
                EmpName = Emp.logs[0].Name;
                string month = Emp.logs[0].Date.Month.ToString();
                string year = Emp.logs[0].Date.Year.ToString();
                MonthAndYear = $"{month}/{year}";
                this.Inter = inter;
                this.Logs = emp.logs;
            }

            catch (Exception ex)
            {
                MessageBox.Show("תקלה. אנא נסה שנית או צור קשר" + ex.Message);
            }
        }

        public UserInterface Inter { get; private set; }
        private LogsHolder emp;
        public LogsHolder Emp { get { return emp; } set { emp = value; OnPropertyChanged("Emp"); } }
        public string EmpName { get; set; }
        public string MonthAndYear { get; set; }
        private Employee[] logs;
        public Employee[] Logs { get { return logs; } set { logs = value; foreach (Employee l in logs) { l.CalculateIsError(); } Emp.logs = Logs; Emp.CalculateAll(); OnPropertyChanged("Emp"); OnPropertyChanged("Logs");/*Emp.logs = Logs; Emp.CalculateAll(); OnPropertyChanged("Emp"); OnPropertyChanged("IsManuallyChanged"); OnPropertyChanged("Logs"); OnPropertyChanged("IsError"); OnPropertyChanged("HoursPerDay");*/ } }
        public Employee SelectedRow { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public void DeleteRow(Employee log)
        {
            try
            {
                if (log is null)
                {
                    throw new NullReferenceException("לא נבחרה שורה");
                }

                else
                {
                    Inter.DeleteLog(log);
                    Logs.ToList().Remove(log);
                    emp.logs = Logs;
                    OnPropertyChanged("Logs");
                    OnPropertyChanged("Emp");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void DeleteMonth(Employee[] logs)
        {
            try
            {
                if (logs is null || logs.Length <= 0)
                {
                    throw new NullReferenceException("אין מידע למחיקה");
                }

                else
                {
                    Inter.DeleteMonth(logs);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
