using RockyDLL.DAL;
using RockyDLL.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockyDLL.Facades
{
    class GetDataFacade
    {
        private GetDataDAO DAO { get; set; }

        public GetDataFacade()
        {
            DAO = new GetDataDAO();
        }

        public LogsHolder GetLogsByEmpName(string name)
        {
            if (name != null)
                return DAO.GetLogsByEmpName(name);

            else
                return null;
        }

        public Employee[] GetAllMonthAndYeah(int month, int year)
        {
            return DAO.GetLogsByMonthAndYear(month, year);
        }

        public LogsHolder GetByEmpMonthYear(int month, int year, string name)
        {
            if (name != null)
                return DAO.GetLogsByMonthYearAndName(month, year, name);
            else
                return null;
        }

        public void CleanDB()
        {
            DAO.CleanDB();
        }
    }
}
