using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Realdolmen.UWP.Chess.Converters
{
    public class BoolToBackGroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var b = (bool)value;
            return b ? new SolidColorBrush(Windows.UI.Color.FromArgb(255, 75, 75, 75)) : new SolidColorBrush(Windows.UI.Color.FromArgb(255, 200, 200, 200));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
