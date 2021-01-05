namespace RockyDLL
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Windows.Forms;

    [Table("Employee")]
    public partial class Employee
    {
        [NotMapped]
        public TimeSpan HourlyHours { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }

        public TimeSpan ChecksInOne { get; set; }

        public TimeSpan ChecksOutOne { get; set; }

        public int MinutesLate { get; set; }

        public int MinutesEarlyLeave { get; set; }

        public int IsAbsance { get; set; }

        public TimeSpan ChecksInTwo { get; set; }

        public TimeSpan ChecksOutTwo { get; set; }

        public int IsError { get; set; }

        public int IsManuallyChanged { get; set; }

        public double HoursPerDay { get; set; }

        public void CalculateIsError()
        {
            try
            {
                Date = Date.Date;
                CalculateIsAbsence();

                TimeSpan zero = new TimeSpan(0, 0, 0);
                IsError = 0;

                if ((IsAbsance == 0) && (ChecksInOne != zero || ChecksInTwo != zero || ChecksOutOne != zero || ChecksOutTwo != zero))
                {
                    if (ChecksInOne != zero)
                    {
                        if ((ChecksOutOne == zero && ChecksInTwo != zero) || (ChecksOutOne == zero && ChecksOutTwo == zero))
                            IsError = 1;
                    }

                    if (ChecksInTwo != zero)
                    {
                        if (ChecksOutTwo == zero)
                            IsError = 1;
                    }

                    if (ChecksInOne == zero && ChecksOutOne != zero)
                        IsError = 1;

                    if (ChecksInOne == zero && ChecksInTwo == zero && (ChecksOutTwo != zero || ChecksOutOne != zero))
                        IsError = 1;
                }

                CalculateHourPerDay();
            }

            catch (Exception ex)
            {
                MessageBox.Show("An error occourd:  " + ex.Message);
            }
        }

        public void CalculateIsAbsence()
        {
            TimeSpan zero = new TimeSpan(0, 0, 0);
            if (ChecksInOne != zero || ChecksInTwo != zero || ChecksOutOne != zero || ChecksOutTwo != zero)
                IsAbsance = 0;
            else
                IsAbsance = 1;
        }

        public void CalculateHourPerDay()
        {
            TimeSpan zero = new TimeSpan(0, 0, 0);
            TimeSpan one = new TimeSpan(0, 0, 0);
            TimeSpan two = new TimeSpan(0, 0, 0);

            if (IsError == 0)
            {
                //checkin one and one
                if (ChecksInOne != zero && ChecksOutOne != zero)
                {
                    one = ChecksOutOne.Subtract(ChecksInOne);
                }

                //checkin one and two and checkin two is 0
                else if (ChecksInOne != zero && ChecksOutOne == zero && ChecksInTwo == zero && ChecksOutTwo != zero)
                {
                    one = ChecksOutTwo.Subtract(ChecksInOne);
                }

                //check in two and two
                if (ChecksInTwo != zero && ChecksOutTwo != zero)
                {
                    two = ChecksOutTwo.Subtract(ChecksInTwo);
                }

                HoursPerDay = (one + two).TotalHours;
            }

            else
                HoursPerDay = 0;


            //converting to timespan

            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            //check if theres a dot
            if (HoursPerDay.ToString().Contains("."))
            {

                ///splitting to hours and leftovers
                string[] subbedFirst = HoursPerDay.ToString().Split('.');

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
                hours = (int)HoursPerDay;
            }


            //assign to timespan
            HourlyHours = new TimeSpan(hours, minutes, seconds);

        }
    }
}
