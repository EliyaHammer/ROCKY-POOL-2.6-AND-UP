using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockyDLL.POCO
{
    public class LogsHolder
    {
        public Employee[] logs { get; set; }
        public string HourlyTotalHours { get; set; }
        public string Hourly125 { get; set; }
        public string Hourly150 { get; set; }
        public int TotalAbsence { get; private set; }
        public int TotalDaysOfWork { get; private set; }
        public int? TotalEarlyLeave { get; private set; }
        public int? TotalMinutesLate { get; private set; }
        private double totalWorkingHours { get; set; }
        public string TotalWorkingHours { get; private set; }
        public string Total125 { get; private set; }
        public string Total150 { get; private set; }
        public LogsHolder(Employee[] logs)
        {
            this.logs = logs;
            CalculateAll();
        }

        public void CalculateAll()
        {
            try
            {
                CalcuateTotalEarlyLeave();
                CalculateTotalLates();
                CalculateAbsence();
                CalculateTotalWorkingHours();
                CalculateTotalWorkingDays();
            }

            catch (Exception ex)
            {
                MessageBox.Show("אירעה תקלה בחישוב השעות. אנא צור קשר. " + ex.Message);
            }
        }//ready

        private void CalculateAbsence()
        {
            TotalAbsence = 0;

            foreach (Employee log in logs)
            {
                if (log.IsAbsance == 1)
                    TotalAbsence++;
            }
        }//check

        private void CalculateTotalWorkingDays()
        {
            int total = logs.Length - TotalAbsence;
            this.TotalDaysOfWork = total;
        }//check

        private void CalculateTotalLates()
        {
            TotalMinutesLate = 0;

            try
            {
                foreach (Employee log in logs)
                {
                        TotalMinutesLate += log.MinutesLate;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show("תקלה: " + e.Message);
            }
        } // check

        private void CalcuateTotalEarlyLeave()
        {
            TotalEarlyLeave = 0;

            try
            {
                foreach (Employee log in logs)
                {
                        TotalEarlyLeave += log.MinutesEarlyLeave;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show("תקלה: " + e.Message);
            }

        }
        private void CalculateTotalWorkingHours()
        {
            try
            {
                double t125 = 0;
                double t150 = 0;
                totalWorkingHours = 0;

                foreach (Employee l in logs)
                {
                    totalWorkingHours += l.HoursPerDay;

                    if (l.HoursPerDay > 8.5 && l.HoursPerDay < 10.5)
                        t125 += l.HoursPerDay - 8.5;
                    else if (l.HoursPerDay > 8.5 && l.HoursPerDay >= 10.5)
                        t125 += 2;

                    if (l.HoursPerDay >= 10.5)
                    {
                        t150 = l.HoursPerDay - 10.5;
                    }
                }

                Total125 = t125.ToString();
                if (Total125.Length > 5)
                    Total125 = Total125.Substring(0, 5);

                Total150 = t150.ToString();
                if (Total150.Length > 5)
                    Total150 = Total150.Substring(0, 5);

                TotalWorkingHours = $"{totalWorkingHours}";
                if (TotalWorkingHours.Length > 5)
                    TotalWorkingHours = TotalWorkingHours.Substring(0, 5);



                //reverting to timespans : t125, t150, tnormal

                //// >> tnormal
                int hours = 0;
                int minutes = 0;
                int seconds = 0;

                //check if theres a dot
                if (TotalWorkingHours.ToString().Contains("."))
                {

                    ///splitting to hours and leftovers
                    string[] subbedFirst = TotalWorkingHours.ToString().Split('.');

                    //assigning the first to hours
                    hours = Convert.ToInt32(subbedFirst[0]);

                    //making the leftovers as 0.leftover
                    string leftovers = $"0.{subbedFirst[1]}";

                    //calculating the leftovers to minutes
                    double leftMinutes = Convert.ToDouble(leftovers);
                    leftMinutes = leftMinutes * 60;

                    //splitting the leftminutes to minutes and leftovers
                    string[] minutesSubbed;
                    if (leftMinutes.ToString().Contains("."))
                    {
                        minutesSubbed = leftMinutes.ToString().Split('.');

                        //assigning the minutes
                        minutes = Convert.ToInt32(minutesSubbed[0]);

                        // change seconds to 0.leftovers
                        string secondsLeft = $"0.{minutesSubbed[1]}";
                        double sl = Convert.ToDouble(secondsLeft);
                        seconds = Convert.ToInt32(sl);
                        seconds = seconds * 60;
                    }

                    else
                    {
                        minutes = (int)leftMinutes;
                    }


                    //end of hours contains
                }

                else
                {
                    //if hours dont contain dot, then assign
                    hours = Convert.ToInt32(TotalWorkingHours);
                }


                //assign to timespan
                HourlyTotalHours = $"{hours}:{minutes}:{seconds}";


                //// >>  t125
                int hours12 = 0;
                int minutes12 = 0;
                int seconds12 = 0;

                //check if theres a dot
                if (Total125.ToString().Contains("."))
                {

                    ///splitting to hours and leftovers
                    string[] subbedFirst = Total125.ToString().Split('.');

                    //assigning the first to hours
                    hours12 = Convert.ToInt32(subbedFirst[0]);

                    //making the leftovers as 0.leftover
                    string leftovers = $"0.{subbedFirst[1]}";

                    //calculating the leftovers to minutes
                    double leftMinutes = Convert.ToDouble(leftovers);
                    leftMinutes = leftMinutes * 60;

                    //splitting the leftminutes to minutes and leftovers
                    string[] minutesSubbed;
                    if (leftMinutes.ToString().Contains("."))
                    {
                        minutesSubbed = leftMinutes.ToString().Split('.');

                        //assigning the minutes
                        minutes12 = Convert.ToInt32(minutesSubbed[0]);

                        // change seconds to 0.leftovers
                        string secondsLeft = $"0.{minutesSubbed[1]}";
                        double sl = Convert.ToDouble(secondsLeft);
                        seconds12 = Convert.ToInt32(sl);
                        seconds12 = seconds12 * 60;
                    }

                    else
                    {
                        minutes12 = (int)leftMinutes;
                    }


                    //end of hours contains
                }

                else
                {
                    //if hours dont contain dot, then assign
                    hours12 = Convert.ToInt32(Total125);
                }


                //assign to timespan
                Hourly125 = $"{hours12}:{minutes12}:{seconds12}";



                //// >> t150
                int hours15 = 0;
                int minutes15 = 0;
                int seconds15 = 0;

                //check if theres a dot
                if (Total150.ToString().Contains("."))
                {

                    ///splitting to hours and leftovers
                    string[] subbedFirst = Total150.ToString().Split('.');

                    //assigning the first to hours
                    hours15 = Convert.ToInt32(subbedFirst[0]);

                    //making the leftovers as 0.leftover
                    string leftovers = $"0.{subbedFirst[1]}";

                    //calculating the leftovers to minutes
                    double leftMinutes = Convert.ToDouble(leftovers);
                    leftMinutes = leftMinutes * 60;

                    //splitting the leftminutes to minutes and leftovers
                    string[] minutesSubbed;
                    if (leftMinutes.ToString().Contains("."))
                    {
                        minutesSubbed = leftMinutes.ToString().Split('.');

                        //assigning the minutes
                        minutes15 = Convert.ToInt32(minutesSubbed[0]);

                        // change seconds to 0.leftovers
                        string secondsLeft = $"0.{minutesSubbed[1]}";
                        double sl = Convert.ToDouble(secondsLeft);
                        seconds15 = Convert.ToInt32(sl);
                        seconds15 = seconds15 * 60;
                    }

                    else
                    {
                        minutes15 = (int)leftMinutes;
                    }


                    //end of hours contains
                }

                else
                {
                    //if hours dont contain dot, then assign
                    hours15 = Convert.ToInt32(Total150);
                }


                //assign to timespan
                Hourly150 = $"{hours15}:{minutes15}:{seconds15}";
            }

            catch (Exception ex)
            {
                MessageBox.Show("אירעה תקלה בחישוב השעות. אנא צור קשר. " + ex.Message);
            }
            }
    }

}



