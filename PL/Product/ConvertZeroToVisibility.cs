using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL.Product;

public class ConvertZeroToVisibility : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int intValue = (int)value;
        if (intValue==0)
        {
            return Visibility.Hidden; //Visibility.Collapsed;
        }
        else
        {
            return Visibility.Visible;
        }
    }
    //convert from target property type to source property type
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
