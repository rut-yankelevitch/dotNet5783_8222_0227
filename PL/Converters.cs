using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace PL;

public class ConvertZeroToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? true : false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ConvertZeroToVisible : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value==null? Visibility.Visible: Visibility.Hidden;
        }
        public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ConvertZeroToHidden : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Hidden : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Category category = (BO.Category)value;
            return category.ToString().Replace('_', ' ');

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }


    public class ConvertShipDateToTrue : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value!=null?((((BO.Order?)value)?.ShipDate<DateTime.Now)?false:true):false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
public class ConvertInputToTrue : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return ((values[0]!).ToString()!.Length == 6 && (values[1]!).ToString()!.Length > 0 && (values[2]!).ToString()!.Length > 0 && (values[3]!).ToString()!.Length > 0 && values[4] != null && ((BO.Category)values[4]!) != BO.Category.None )?true:false;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class ConverDoubleToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double valueDouble = (double)value;
        return valueDouble.ToString();
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string text = (string)value;
        double price;
        if (!double.TryParse(text, out price))
        {
            return null; //Some default value
        }
        return price;
    }
}
public class ConverIntToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int valueInt = (int)value;
        return valueInt.ToString();
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string text = (string)value;
        int inStock;
        if (!int.TryParse(text, out inStock))
        {
           
            return null; //Some default value
        }
        return inStock;
    }
}