using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace wp05_bikeshop.Logics
{
    internal class MyConverter : IValueConverter
    {
        // 대상에다가 표현할때 값을 변환
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + " km/h";
        }

        // 대상값이 바뀌어서 원본(소스)의 갑을 변환해서 표현(TwoWay가 되어야함)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse(value.ToString()) * 3;
        }
    }
}
