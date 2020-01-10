using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace PQS.Views.CustomControls
{
    public class Replacer : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            int i;

            bool parsed = int.TryParse(value.ToString(), out i);

            return parsed ? i : (object)null;
        }
    }
}
