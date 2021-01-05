using RockyDLL.DAL;
using RockyDLL.POCO;
using System;

namespace RockyDLL.Facades
{
    class ExportDataFacade
    {
        private ExportMethods exportMeth { get; set; }
        public ExportDataFacade()
        {
            exportMeth = new ExportMethods();
        }

        public void Export(LogsHolder[] logs, string location, string timing)
        {
            exportMeth.ExportFile(location, logs, timing);
        }
    }
}
