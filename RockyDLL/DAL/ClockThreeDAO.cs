using ExcelDataReader;
using ExcelDataReader.Log;
using RockyDLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RockyDLL.DAL
{
    class ClockThreeDAO : ClockDAO
    {
        public List<Employee> logs { get; private set; }

        // the hebrew - RS-20
        protected override Employee[] TakeData(string logLocation)
        {

            try
            {
                int rowStart = 2;
                int column = 9;

                using (FileStream streamer = File.Open(@logLocation, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(streamer);
                    DataSet spreadSheet = excelReader.AsDataSet();
                    DataTable AttendaceLog = spreadSheet.Tables[4];
                    int currentTable = 4;
                    DataTable Summery = spreadSheet.Tables[1];
                    logs = new List<Employee>();

                    // need to run as number of blocks
                    // loop for whole sheet
                    for (int x = 0; x < 3; x++) // MUST check if its going down as well ! this is only for the side. 
                                                // if makes another log > check if there's another table after this one.
                    {
                        Employee log = null;

                        string name = null;
                        if (AttendaceLog.Rows[rowStart][column] is DBNull || AttendaceLog.Rows[rowStart][column] is null)
                        {
                            break;
                        }
                        else
                            name = (string)AttendaceLog.Rows[rowStart][column];
                        log = new Employee();
                        log.Name = name;

                        rowStart++;
                        string idRaw = null;
                        if (!(AttendaceLog.Rows[rowStart][column] is DBNull))
                        {
                            idRaw = (string)AttendaceLog.Rows[rowStart][column];
                            log.ID = Convert.ToInt32(idRaw);
                        }
                        else
                            throw new NullReferenceException($"ID is null: row- {rowStart}, column- {column}");

                        column -= 8;
                        string yearAndMonth = null;
                        int year = 0;
                        int month = 0;

                        if (!(AttendaceLog.Rows[rowStart][column] is DBNull))
                        {
                            yearAndMonth = (string)AttendaceLog.Rows[rowStart][column];
                            year = Convert.ToInt32(yearAndMonth.Substring(0, 4));
                            month = Convert.ToInt32(yearAndMonth.Substring(5, 2));
                        }
                        else
                            throw new NullReferenceException($"Year and month is null: row- {rowStart}, column- {column}");

                        column -= 1;
                        int columnStartLog = column;
                        rowStart = 11;

                        // loop for one block
                        for (int i = 0; i < AttendaceLog.Rows.Count - 11; i++)
                        {
                            //log

                            column = columnStartLog;

                            TimeSpan time = new TimeSpan();
                            TimeSpan[] checkingTimes = new TimeSpan[4];
                            int day = 0;
                            DateTime date = new DateTime();

                            if (!(AttendaceLog.Rows[rowStart][column] is DBNull))
                            {
                                string allRaw = (string)AttendaceLog.Rows[rowStart][column];
                                if (allRaw != null && allRaw.Length > 1)
                                {
                                    day = Convert.ToInt32(allRaw.Substring(0, 2));
                                    date = new DateTime(year, month, day);
                                }
                            }

                            column++;

                            //checking times loop
                            for (int j = 0; j < 4; j++)
                            {
                                int hour = 0;
                                int minutes = 0;
                                int seconds = 0;

                                if (AttendaceLog.Rows[rowStart][column] is DBNull || AttendaceLog.Rows[rowStart][column] is null || AttendaceLog.Rows[rowStart][column].ToString() == "נעדר")

                                {

                                }

                                else
                                {
                                    string timeRaw = (string)AttendaceLog.Rows[rowStart][column];
                                    int.TryParse(timeRaw.Substring(0, 2), out hour);
                                    int.TryParse(timeRaw.Substring(3, 2), out minutes);
                                }

                                time = new TimeSpan(hour, minutes, seconds);
                                checkingTimes[j] = time;

                                switch (j)
                                {
                                    case 0:
                                        column += 2;
                                        break;
                                    case 1:
                                        column += 3;
                                        break;
                                    case 2:
                                        column += 2;
                                        break;
                                }

                            }

                            DateTime check = new DateTime();
                            if (date != check)
                            {
                                log.Date = date;
                                // here is the new code for putting values by order and not by the log's order.
                                // down with note is the original one.

                                //assigning only the ones with value by order to a new list of timespans.

                                List<TimeSpan> checking = new List<TimeSpan>();
                                TimeSpan zero = new TimeSpan(0, 0, 0);

                                for (int f = 0; f < 4; f++)
                                {
                                    if (checkingTimes[f] != zero)
                                        checking.Add(checkingTimes[f]);
                                }

                                //need to put the list in order
                                for (int h = 0; h < checking.Count; h++)
                                {
                                    if (h == 0)
                                        log.ChecksInOne = checking[h];
                                    if (h == 1)
                                        log.ChecksOutOne = checking[h];
                                    if (h == 2)
                                        log.ChecksInTwo = checking[h];
                                    if (h == 3)
                                        log.ChecksOutTwo = checking[h];
                                }


                                //this is the original code > exactly the values in the log:

                                /*
                                log.ChecksInOne = checkingTimes[0];
                                log.ChecksOutOne = checkingTimes[1];
                                log.ChecksInTwo = checkingTimes[2];
                                log.ChecksOutTwo = checkingTimes[3];
                                */

                                if ((log.ChecksInOne == zero || log.ChecksInOne == zero) && (log.ChecksInTwo == zero || log.ChecksInTwo == zero))
                                    log.IsAbsance = 1;

                                logs.Add(log);
                                Employee newLog = new Employee() { Name = log.Name, ID = log.ID };
                                log = newLog;
                            }
                            rowStart++;

                        }

                        rowStart = 2;
                        column += 16;

                        if (x == 2)
                        {
                            if (spreadSheet.Tables.Count > currentTable + 1)
                            {
                                x = 0;
                                AttendaceLog = spreadSheet.Tables[currentTable + 1];
                                currentTable += 1;
                                rowStart = 2;
                                column = 9;
                            }

                        }
                    }



                    AttendaceLog = spreadSheet.Tables[3];
                    rowStart = 4;
                    int columnStart = 0;

                    for (int i = 0; i < AttendaceLog.Rows.Count - 4; i++)
                    {
                        columnStart = 0;

                        string idRaw = (string)AttendaceLog.Rows[rowStart][columnStart];
                        int id = Convert.ToInt32(idRaw);
                        columnStart++;

                        columnStart += 2;
                        string dateRaw = (string)AttendaceLog.Rows[rowStart][columnStart];
                        int year = Convert.ToInt32(dateRaw.Substring(0, 4));
                        int month = Convert.ToInt32(dateRaw.Substring(5, 2));
                        int day = Convert.ToInt32(dateRaw.Substring(8, 2));
                        DateTime date = new DateTime(year, month, day);

                        columnStart++;

                        columnStart += 4;

                        double lateRaw = (double)AttendaceLog.Rows[rowStart][columnStart];
                        int late = Convert.ToInt32(lateRaw);

                        columnStart++;
                        double earlyRaw = (double)AttendaceLog.Rows[rowStart][columnStart];
                        int early = Convert.ToInt32(earlyRaw);

                        logs.ForEach((e) => { if (e.ID == id && e.Date == date) { e.MinutesLate = late; e.MinutesEarlyLeave = early; } });

                        rowStart++;
                    }

                    return logs.ToArray();


                }

            }

            catch (Exception ex)
            {
                throw new Exception("אירעה בעיה בייבוא הקובץ. אנא בדוק שהקובץ תואם את השעון, או צור קשר." + ex.Message);
            }
        }
    }
}
