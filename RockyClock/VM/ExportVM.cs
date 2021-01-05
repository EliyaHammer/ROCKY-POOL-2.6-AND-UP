using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockyClock.ViewModel
{
    class ExportVM
    {
        public List<string> source { get; set; }

        public ExportVM()
        {
            this.source = new List<string>();
            source.Add("סינאל");
            source.Add("עוקץ");
        }
    }
}
