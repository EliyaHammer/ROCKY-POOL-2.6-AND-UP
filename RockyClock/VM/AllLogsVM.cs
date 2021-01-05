using RockyDLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RockyClock.ViewModel
{
    public class AllLogsVM : INotifyPropertyChanged
    {
        private Employee[] logs;

        public event PropertyChangedEventHandler PropertyChanged;
        public Employee[] Logs { get { return logs; } set { logs = value; OnPropertyChanged("Logs"); } }
        public string MonthAndYear { get; set; }
        public Employee SelectedRow { get; set; }
        public UserInterface Inter { get; private set; }
        public AllLogsVM(Employee[] logs, UserInterface inter)
        {
            try
            {
                this.Logs = logs;
                string month = Logs[0].Date.Month.ToString();
                string year = Logs[0].Date.Year.ToString();
                MonthAndYear = $"{month}/{year}";
                this.Inter = inter;
            }

            catch (Exception ex)
            {
                MessageBox.Show("תקלה. אנא נסה שנית או צור קשר" + ex.Message);
            }

        }
        public void DeleteRow (Employee log)
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
                    OnPropertyChanged("Logs");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        public void DeleteMonth (Employee[] logs)
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

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
