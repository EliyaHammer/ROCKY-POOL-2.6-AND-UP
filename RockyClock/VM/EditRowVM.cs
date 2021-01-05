using RockyDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RockyClock.ViewModel
{
    class EditRowVM
    {
        public Employee[] row { get; set; }
        public UserInterface inter { get; private set; }
        public EditRowVM(Employee row, UserInterface inter)
        {
            if (row == null)
            {
                MessageBox.Show("לא נבחרה שורה");
            }

            else
            {
                this.row = new Employee[1] { row };
                this.inter = inter;
            }
        }


    }
}
