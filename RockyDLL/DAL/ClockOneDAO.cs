using ExcelDataReader;
using RockyDLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockyDLL.DAL
{
    class ClockOneDAO : ClockDAO
    {
        public List<Employee> logs { get; private set; }


        protected override Employee[] TakeData(string logLocation)
        {
            try
            {
                // NEEDS TO MAKE ALL THE CHECKUPS !!!! AND EXCEPTIONS ! 

                //clock 1: (all report) 5ya
                //sets the sreamer and reader.
                //sets the row counter as beginning of 

                using (FileStream streamer = File.Open(@logLocation, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(streamer);
                    DataSet spreadSheet = excelReader.AsDataSet();
                    DataTable AttendaceLog = spreadSheet.Tables[3];
                    DataTable Abnormal = spreadSheet.Tables[2];
                    int attendanceNumberRows = AttendaceLog.Rows.Count;
                    int runTimes = attendanceNumberRows / 21;
                    int rowCount = 2;
                    logs = new List<Employee>();


                    //attendance log SHEET: 

                    //BIG LOOP (whole block):
                    //---

                    // >>> all the upper block <<< // 
                    // gets the first row: id and name.

                    int yearCount = 0;

                    for (int f = 0; f < runTimes; f++)
                    {

                        Employee logRaw = new Employee();

                        if (!(AttendaceLog.Rows[rowCount][2] is DBNull))
                        {
                            string name = Convert.ToString(AttendaceLog.Rows[rowCount][2]);
                            name = name.Substring(5);

                            logRaw.Name = name;
                        }
                        else
                            throw new NullReferenceException($"Name is null: row- {rowCount}, column- 2");

                        if ((!(AttendaceLog.Rows[rowCount][0] is DBNull)))
                        {
                            string IDRaw = (string)AttendaceLog.Rows[rowCount][0];
                            int ID = Convert.ToInt32(IDRaw.Substring(3));
                            logRaw.ID = ID;
                        }
                        else
                            throw new NullReferenceException($"ID is null: row- {rowCount}, column-0");

                        //sets and saves the year for the rest of the block

                        int year = 0;

                        if (!(AttendaceLog.Rows[rowCount][13] is DBNull))
                        {
                            string yearRaw = (string)AttendaceLog.Rows[rowCount][13];
                            yearRaw = yearRaw.Substring(11, 4);
                            year = Convert.ToInt32(yearRaw);
                            yearCount = year;
                        }
                        else
                            throw new NullReferenceException($"Year is null: row- {rowCount}, column- 13");

                        //gets the absance days 
                        rowCount++;

                        //here was total absence

                        //sets the rowcount for the first data row.
                        rowCount += 4;

                        // >>>> All the buttom block <<<< //

                        //SMALL LOOP (one log row):
                        //---

                        // this loop is for 16 rows.
                        //left side of the block
                        int columnCounter = 0;
                        //CHECK: List<Log> logs = new List<Log>();
                        TimeSpan[] checkingTimes = new TimeSpan[4];

                        for (int j = 0; j < 16; j++)
                        {
                            columnCounter = 0;
                            //sets the date and saves it
                            string monthAndDayRaw = (string)AttendaceLog.Rows[rowCount][columnCounter];

                            int day = 0;
                            int month = 0;
                            DateTime date = new DateTime();

                            if (!(AttendaceLog.Rows[rowCount][columnCounter] is DBNull))
                            {
                                day = Convert.ToInt32(monthAndDayRaw.Substring(0, 2));
                                month = Convert.ToInt32(monthAndDayRaw.Substring(3, 2));
                                date = new DateTime(year, month, day);
                                if (date.Month == 12 && date.Day == 31)
                                    year++;
                            }
                            else
                                throw new NullReferenceException($"Date is null: row- {rowCount}, column- {columnCounter}");

                            //sets the log list for this employee

                            columnCounter++;

                            //getting the time and saves it: for the 4 cells of in and out, for one row
                            for (int i = 0; i <= 3; i++)
                            {
                                columnCounter++;
                                string timeRaw;

                                if (!(AttendaceLog.Rows[rowCount][columnCounter] is DBNull))
                                {
                                    timeRaw = (string)AttendaceLog.Rows[rowCount][columnCounter];
                                    int hour = Convert.ToInt32(timeRaw.Substring(0, 2));
                                    int minutes = Convert.ToInt32(timeRaw.Substring(3, 2));
                                    int seconds = 0;

                                    checkingTimes[i] = new TimeSpan(hour, minutes, seconds);
                                }
                                else
                                    checkingTimes[i] = new TimeSpan(0, 0, 0);
                            }

                            logRaw.ChecksInOne = checkingTimes[0];
                            logRaw.ChecksOutOne = checkingTimes[1];
                            logRaw.ChecksInTwo = checkingTimes[2];
                            logRaw.ChecksOutTwo = checkingTimes[3];
                            TimeSpan zero = new TimeSpan(0, 0, 0);
                            if ((logRaw.ChecksInOne == zero || logRaw.ChecksInOne == zero) && (logRaw.ChecksInTwo == zero || logRaw.ChecksInTwo == zero))
                            {
                                logRaw.IsAbsance = 1;
                            }

                            logRaw.Date = date;

                            logs.Add(logRaw);
                            logRaw = new Employee() { Name = logRaw.Name, ID = logRaw.ID };
                            rowCount++;
                        }

                        // the right side of the block
                        rowCount -= 16;
                        int rowCountInitialized = rowCount;
                        columnCounter = 8;

                        for (int i = 0; i < 16; i++)
                        {
                            columnCounter = 8;
                            //sets the date and saves it
                            if (AttendaceLog.Rows[rowCount][columnCounter] is DBNull)
                                break;

                            string monthAndDayRaw = (string)AttendaceLog.Rows[rowCount][columnCounter];

                            if (monthAndDayRaw == null)
                                break;

                            int day = Convert.ToInt32(monthAndDayRaw.Substring(0, 2));
                            int month = Convert.ToInt32(monthAndDayRaw.Substring(3, 2));
                            DateTime date = new DateTime(year, month, day);
                            if (date.Month == 12 && date.Day == 31)
                                year++;

                            columnCounter++;
                            string dayOfWeekRaw = (string)AttendaceLog.Rows[rowCount][columnCounter];


                            //getting the time and saves it: for the 4 cells of in and out, for one row
                            for (int x = 0; x <= 3; x++)
                            {
                                columnCounter++;
                                string timeRaw;

                                if (!(AttendaceLog.Rows[rowCount][columnCounter] is DBNull))
                                {
                                    timeRaw = (string)AttendaceLog.Rows[rowCount][columnCounter];
                                    int hour = Convert.ToInt32(timeRaw.Substring(0, 2));
                                    int minutes = Convert.ToInt32(timeRaw.Substring(3, 2));
                                    int seconds = 0;

                                    checkingTimes[x] = new TimeSpan(hour, minutes, seconds);
                                }
                                else
                                    checkingTimes[x] = new TimeSpan(0, 0, 0);

                            }

                            logRaw.ChecksInOne = checkingTimes[0];
                            logRaw.ChecksOutOne = checkingTimes[1];
                            logRaw.ChecksInTwo = checkingTimes[2];
                            logRaw.ChecksOutTwo = checkingTimes[3];
                            TimeSpan zero = new TimeSpan(0, 0, 0);
                            if ((logRaw.ChecksInOne == zero || logRaw.ChecksInOne == zero) && (logRaw.ChecksInTwo == zero || logRaw.ChecksInTwo == zero))
                            {
                                logRaw.IsAbsance = 1;
                            }
                            logRaw.Date = date;
                            rowCount++;
                            logs.Add(logRaw);
                            logRaw = new Employee() { Name = logRaw.Name, ID = logRaw.ID };
                        }

                        //CHECK     employee.Logs = logs;

                        //CHECK if (Employees != null)
                        // Employees.Add(employee);
                        // sets the counter for the next block
                        rowCount = rowCountInitialized + 17;
                    }

                    //ABNORMAL TABLE METHOD
                    rowCount = 5;
                    runTimes = Abnormal.Rows.Count - 5;

                    for (int i = 0; i < runTimes; i++)
                    {
                        int columnCount = 0;
                        Employee Rightlog = null;

                        string idRaw = (string)Abnormal.Rows[rowCount][columnCount];
                        int id = Convert.ToInt32(idRaw);

                        columnCount = 3;

                        string dateRaw = (string)Abnormal.Rows[rowCount][columnCount];
                        DateTime date = new DateTime(Convert.ToInt32(dateRaw.Substring(6)), Convert.ToInt32(dateRaw.Substring(3, 2)), Convert.ToInt32(dateRaw.Substring(0, 2)));

                        if (logs != null)
                        {
                            foreach (Employee log in logs)
                            {

                                if (log.Date == date && log.ID == id)
                                {
                                    Rightlog = log;
                                }

                            }
                        }

                        columnCount = 8;
                        string late = (string)Abnormal.Rows[rowCount][columnCount];
                        columnCount = 9;
                        string early = (string)Abnormal.Rows[rowCount][columnCount];

                        Rightlog.MinutesLate = Convert.ToInt32(late);
                        Rightlog.MinutesEarlyLeave = Convert.ToInt32(early);

                        rowCount++;
                    }

                }

                return logs.ToArray();
            }

            catch (Exception ex)
            {
                throw new Exception("אירעה בעיה בייבוא הקובץ. אנא בדוק שהקובץ תואם את השעון, או צור קשר." + ex.Message);
            }

            }
    }
}
