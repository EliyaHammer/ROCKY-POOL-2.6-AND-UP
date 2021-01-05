using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RockyClock.Converters
{
    class DayOfWeekConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Value = value.ToString();
            string Result = null;

            switch (Value)
            {
                case "Sunday": Result = "ראשון";
                    break;
                case "Monday": Result = "שני";
                    break;
                case "Tuesday":
                    Result = "שלישי";
                    break;
                case "Wednesday":
                    Result = "רביעי";
                    break;
                case "Thursday":
                    Result = "חמישי";
                    break;
                case "Friday":
                    Result = "שישי";
                    break;
                case "Saturday":
                    Result = "שבת";
                    break;
            }

            return (string)Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
