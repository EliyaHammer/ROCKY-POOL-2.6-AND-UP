using ExcelDataReader.Log;
using RockyDLL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockyDLL.DAL
{
    abstract class ClockDAO : IClockDAO
    {
        //the all report
        protected abstract Employee[] TakeData(string logLocation);
        protected void PutInDB(Employee[] logs)
        {
            try
            {
                foreach (Employee l in logs)
                {
                    l.CalculateIsError();
                }

                using (MyEntity entity = new MyEntity())
                {
                    entity.Employees.AddRange(logs);
                    entity.SaveChanges();
                }
            }

            catch (Exception e)
            {
                throw new Exception("אירעה תקלה. אנא צור קשר " + e.Message);
            }
        }
        public void StartOperation(string logLocation)
        {
            Employee[] logs = TakeData(logLocation);
                using (MyEntity entity = new MyEntity())
                {
                    foreach (Employee log in logs)
                    {
                        foreach(Employee e in entity.Employees)
                        {
                            if (e.ID == log.ID && e.Date == log.Date)
                            {
                                throw new Exception("שורה אחת או יותר כבר יובאה. יש למחוקן בכדי לבצע את הייבוא.");
                            }
                        }

                    }
                }
                PutInDB(logs);
           
        }
        public bool Update(Employee newLog)
        {
            try
            {
                using (MyEntity entity = new MyEntity())
                {
                    DateTime date = new DateTime(newLog.Date.Year, newLog.Date.Month, newLog.Date.Day);

                    Employee found = null;

                    for (int i = 0; i < entity.Employees.Count(); i++)
                    {
                        if (entity.Employees.ToArray()[i].Date == date && entity.Employees.ToArray()[i].ID == newLog.ID)
                        {
                            found = entity.Employees.ToArray()[i];
                            break;
                        }
                    }


                    if (found != null)
                    {
                        for (int i = 0; i < entity.Employees.Count(); i++)
                        {
                            if (entity.Employees.ToArray()[i] == found)
                            {
                                entity.Employees.Remove(found);
                                entity.SaveChanges();
                            }
                        }

                        entity.Employees.Add(newLog);
                        entity.SaveChanges();

                        return true;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה בעיה בעדכון השורה. אנא צור קשר" + ex.Message);
            }
            return false;
            /*   foreach (Employee e in entity.Employees)
               {
                   if (e.Date == date && e.ID == newLog.ID)
                   {
                       found = e;
                       break;
                   }
               }
               if (found != null)
               {
                   entity.Employees.ToList().ForEach((e) => { if (e == found) { e = newLog; } });
                   entity.SaveChanges();
                   return true;
               }
               */

        }
        public bool DeleteMonth(Employee[] logs)
        {
            try
            {
                using (MyEntity entity = new MyEntity())
                {
                    for (int i = 0; i < logs.Length; i ++)
                    {
                        foreach (Employee e in entity.Employees)
                        {
                            if (e.ID == logs[i].ID && e.Date == logs[i].Date)
                            {
                                entity.Employees.Remove(e);
                            }
                        }
                    }
                    entity.SaveChanges();
                    return true;

                }
            }

            catch (Exception ex)
            {
               throw new Exception("אירעה בעיה במחיקת הקובץ. אנא צור קשר" + ex.Message);
            }
        }
        public bool DeleteLog (Employee log)
        {
            using (MyEntity entity = new MyEntity())
            {
                try
                {
                    foreach (Employee e in entity.Employees)
                    {
                        if (e.ID == log.ID && e.Date == log.Date)
                        {
                            entity.Employees.Remove(e);
                        }
                    }
                    entity.SaveChanges();
                    return true;
                }

                catch (Exception ex)
                {
                    throw new Exception("אירעה בעיה במחיקת הקובץ. אנא צור קשר" + ex.Message);
                }
            }
        }
    }
}
