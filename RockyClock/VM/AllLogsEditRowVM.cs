using RockyDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RockyClock.ViewModel
{
    class AllLogsEditRowVM
    {
        public Employee[] SelectedRow { get; set; }
        public UserInterface inter { get; private set; }
        public AllLogsEditRowVM(Employee selectedRow, UserInterface inter)
        {
            if (selectedRow == null)
            {
                MessageBox.Show("לא נבחרה שורה.");
            }

            else
            {
                this.inter = inter;
                this.SelectedRow = new Employee[1] { selectedRow };
            }
        }
    }
}
