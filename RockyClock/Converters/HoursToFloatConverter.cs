using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RockyClock.Converters
{
    class HoursToFloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Value = value.ToString();
            int seperate = 0;

            for (int i = 0; i < Value.Length; i ++)
            {
                if (Value[i] == '.')
                {
                    seperate = i;
                    break;
                }
            }

            if (seperate > 0)
            {
                Value = Value.Substring(0, seperate + 2);
            }
            return (string)Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
