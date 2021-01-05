using RockyDLL.DAL;
using RockyDLL.Facades;
using RockyDLL.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockyDLL
{
    public class UserInterface
    {
        private ClockFacade Clock { get; set; }

        public UserInterface(ClocksEnum clockType)
        {
            Clock = new ClockFacade(clockType);
        }

        private void StartOperation(string logLocation)
        {
            Clock.StartOperation(logLocation);
        }




        public bool Import(string fileLocation)
        {
            Clock.StartOperation(fileLocation);
            return true;
        }

        public void UpdateLog(Employee newLog)
        {
            Clock.UpdateLog(newLog);
        }

        public LogsHolder GetLogsByEmpName(string name)
        {
            return Clock.GetLogsByEmpName(name);
        }

        public Employee[] GetAllMonthAndYeah(int month, int year)
        {
            return Clock.GetAllMonthAndYeah(month, year);
        }

        public LogsHolder GetByEmpMonthYear(int month, int year, string name)
        {
            return Clock.GetByEmpMonthYear(month, year, name);
        }

        public void Export(LogsHolder[] logs, string location, string timing)
        {
            Clock.Export(logs, location, timing);
        }
        public void CleanDB()
        {
            Clock.CleanDB();
        }

        public bool DeleteMonth(Employee[] logs)
        {
            return Clock.DeleteMonth(logs);
        }

        public bool DeleteLog(Employee log)
        {
            return Clock.DeleteLog(log);
        }
    }
}