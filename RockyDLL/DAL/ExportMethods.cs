using OfficeOpenXml;
using RockyDLL;
using RockyDLL.POCO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockyDLL.DAL
{
    class ExportMethods
    {
        public void ExportFile(string path, LogsHolder[] allLogs, string timing)
        {
            try
            {
                if (allLogs.Count() == 0)
                {
                    MessageBox.Show("אין מידע לחודש זה");
                }

                else
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        excel.Workbook.Worksheets.Add("WorkSheetOne");
                        ExcelWorksheet sheet = excel.Workbook.Worksheets[0];
                        

                        int Rcounter = 1;
                        int Ccounter = 1;

                        ///loop for each holder
                        for (int i = 0; i < allLogs.Count(); i++)
                        {
                            //count the rows
                            sheet.Cells[Rcounter, Ccounter].Value = "עובד:";
                            sheet.Cells[Rcounter, Ccounter].Style.Font.Bold = true;
                            Ccounter += 1;
                            sheet.Cells[Rcounter, Ccounter].Value = $"{allLogs[i].logs[0].Name}";
                            sheet.Cells[Rcounter, Ccounter].Style.Font.Bold = true;
                            Ccounter += 1;
                            sheet.Cells[Rcounter, Ccounter].Value = "תקופה:";
                            sheet.Cells[Rcounter, Ccounter].Style.Font.Bold = true;
                            Ccounter += 1;
                            sheet.Cells[Rcounter, Ccounter].Value = $"{timing}";
                            sheet.Cells[Rcounter, Ccounter].Style.Font.Bold = true;

                            Ccounter = 1;
                            Rcounter += 1;

                            ///loop for each log
                            for (int j = 0; j < allLogs[i].logs.Length; j++)
                            {
                                sheet.Cells[Rcounter, Ccounter].Value = "תאריך:";
                                sheet.Cells[Rcounter, Ccounter].Style.Font.Bold = true;
                                Ccounter += 1;
                                sheet.Cells[Rcounter, Ccounter].Value = $"{allLogs[i].logs[j].Date}";
                                Ccounter += 1;
                                sheet.Cells[Rcounter, Ccounter].Value = "סה''כ שעות:";
                                sheet.Cells[Rcounter, Ccounter].Style.Font.Bold = true;
                                Ccounter += 1;
                                sheet.Cells[Rcounter, Ccounter].Value = $"{allLogs[i].logs[j].HourlyHours}";
                                Ccounter = 1;
                                Rcounter += 1;
                            }

                            sheet.Cells[Rcounter, Ccounter].Value = "סה''כ שעות:";
                            sheet.Cells[Rcounter, Ccounter].Style.Font.Bold = true;
                            Ccounter += 1;
                            sheet.Cells[Rcounter, Ccounter].Value = $"{allLogs[i].HourlyTotalHours}";
                            sheet.Cells[Rcounter, Ccounter].Style.Font.Bold = true;
                            Ccounter += 1;
                            sheet.Cells[Rcounter, Ccounter].Value = "סה''כ ימי עבודה:";
                            sheet.Cells[Rcounter, Ccounter].Style.Font.Bold = true;
                            Ccounter += 1;
                            sheet.Cells[Rcounter, Ccounter].Value = $"{allLogs[i].TotalDaysOfWork}";
                            sheet.Cells[Rcounter, Ccounter].Style.Font.Bold = true;


                            Ccounter = 1;
                            Rcounter += 3;
                        }

                        if (path is null || path == "")
                        {
                            MessageBox.Show("לא נבחרה תיקייה");
                        }

                        else
                        {
                            FileInfo theFile = new FileInfo(path);
                            excel.SaveAs(theFile);
                            MessageBox.Show("ייצוא בוצע בהצלחה!");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                
                MessageBox.Show($"אירעה תקלה בייצוא הדו''ח: {ex.Message}");
            }
        }
    }

}
