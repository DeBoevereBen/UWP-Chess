using Realdolmen.UWP.Chess.Models;
using Realdolmen.UWP.Chess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Realdolmen.UWP.Chess.Converters
{
    public class BoolToBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var b = (Bools)value;
            if (b.IsSelected)
            {
                return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 255));
            }
            if (b.IsAvailableMove)
            {
                return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 0));
            }
            return new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
