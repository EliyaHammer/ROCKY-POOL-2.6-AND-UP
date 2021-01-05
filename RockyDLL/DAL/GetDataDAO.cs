using RockyDLL;
using RockyDLL.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockyDLL.DAL
{
    class GetDataDAO
    {
        public LogsHolder GetLogsByEmpName(string name)
        {
            if (name == null)
            {
                throw new NullReferenceException();
            }

            try
            {
                LogsHolder result = null;

                using (MyEntity entity = new MyEntity())
                {
                    List<Employee> logs = new List<Employee>();

                    logs =
                        (from Employee in entity.Employees
                         where Employee.Name.ToLower() == name.ToLower()
                         orderby Employee.Date ascending
                         select Employee).ToList();

                    foreach (Employee log in logs)
                    {
                        log.CalculateIsError();
                    }

                    result = new LogsHolder(logs.ToArray());
                    result.CalculateAll();
                }

                return result;
            }

            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                MessageBox.Show("לא נבחר עובד.");

                else
                    MessageBox.Show("אירעה תקלה:" + ex.Message);

                return null;
            }
        }
        public Employee[] GetLogsByMonthAndYear(int month, int year)
        {

            if (month == 0 || year == 0)
            {
                throw new NullReferenceException("יש לבחור תאריך.");
            }

            try
            {
                List<Employee> logs = new List<Employee>();

                using (MyEntity entity = new MyEntity())
                {

                    logs =
                        (from Employee in entity.Employees
                         where Employee.Date.Month == month && Employee.Date.Year == year
                         orderby Employee.Name
                         select Employee).ToList();
                }

                foreach (Employee l in logs)
                {
                    l.CalculateHourPerDay();
                }

                return logs.ToArray();
            }

            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                    MessageBox.Show("לא נבחר תאריך.");
                else
                    MessageBox.Show("אירעה תקלה:" + ex.Message);

                return null;
            }


        }
        public LogsHolder GetLogsByMonthYearAndName(int month, int year, string name)
        {
            if (month == 0 || year == 0 || name == null)
            {
                throw new NullReferenceException();
            }

            try
            {
                LogsHolder result = null;

                if (name != null)
                {
                    Employee[] logs = null;
                    List<Employee> newLogs = new List<Employee>();

                    logs = GetLogsByMonthAndYear(month, year);
                    for (int i = 0; i < logs.Count(); i++)
                    {
                        if (logs[i].Name.ToLower() == name.ToLower())
                            newLogs.Add(logs[i]);
                    }


                    foreach (Employee log in logs)
                    {
                        log.CalculateIsError();
                    }
                    result = new LogsHolder(newLogs.ToArray());
                    result.CalculateAll();
                    
                }

                return result;
            }

            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                    MessageBox.Show("תאריך או עובד לא נבחרו");

                else
                    MessageBox.Show("אירעה תקלה: " + ex.Message);
                return null;
            }
        }
        public void CleanDB()
        {
            try
            {
                int year = Convert.ToInt32(DateTime.Now.Year);
                int yearToDelete = year - 2;

                using (MyEntity entity = new MyEntity())
                {
                    Employee[] logsToDelete =
                        (from e in entity.Employees
                         where e.Date.Year == yearToDelete
                         select e).ToArray();

                    entity.Employees.RemoveRange(logsToDelete);
                    entity.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("תקלה: " + ex.Message);
            }
        }

    }
}
