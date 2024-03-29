﻿using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using BO;

namespace PL;
class ImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            if (!File.Exists((string)value))
                throw new Exception("");
            using (var stream = File.OpenRead((string)value))
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = stream;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                return bmp;
            }
        }
        catch (Exception ex)
        {
            return new BitmapImage(new Uri(@"..\img\empty_image.gif", UriKind.RelativeOrAbsolute));
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            return ((BitmapImage)value).UriSource.AbsolutePath;
        }
        catch
        {
            return @"..\img\empty_image.gif";
        }
    }

}


public class ConvertNullToTrue : IValueConverter
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


public class ConvertNullToFalse : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || (string)value == "")
        {
            return false;
        }
        return true;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ConvertNullToVisible : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? Visibility.Visible : Visibility.Hidden;
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
        int.TryParse(value!.ToString(), out int result);
        return result == 0 ? Visibility.Hidden : Visibility.Visible;
    }
    public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ConvertMinAmountToTrue : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        int.TryParse(value!.ToString(), out int result);
        return result > 1 ? true : false;
    }
    public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ConvertNullToHidden : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return Visibility.Hidden;
        }
        return Visibility.Visible;
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
        int.TryParse(value!.ToString(), out int result);
        return result == 0 ? Visibility.Visible : Visibility.Hidden;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class ConvertZeroToFalse : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        int.TryParse((value!).ToString(), out int result);
        return result == 0 ? false : true;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConverBoolToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value == true ? "true" : "false";
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}


public class StatusToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.OrderStatus status = (BO.OrderStatus)value;
        return status.ToString().Replace('_', ' ');

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}

public class CategoryToStringConverter : IValueConverter
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
public class ConvertDetailsToTrue : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        foreach(var item in values)
        {
            if (item.ToString()!.Length == 0)
                return false;
        }
        return true;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ConvertInputToTrue : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return ((values[0]!).ToString()!.Length == 6 && (values[1]!).ToString()!.Length > 0 && (values[2]!).ToString()!.Length > 0 && (values[3]!).ToString()!.Length > 0 && values[4] != null && ((BO.Category)values[4]!) != BO.Category.None && values[5] != null) ? true : false;
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
            return null; 
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


public class DateTimeConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DateTime? date = (DateTime?)value;
        return date != null ? (date?.ToString("dd/ MM/yy", CultureInfo.InvariantCulture)) : "לא קיים תאריך";
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class NextStatusConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.OrderStatus? status = (OrderStatus?)value;
        return status == BO.OrderStatus.Confirmed_Order ? BO.OrderStatus.Send_Order : status == BO.OrderStatus.Send_Order ? BO.OrderStatus.Provided_Order : null;
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ConvertConfirmStatusToVisible : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        BO.OrderStatus? status = (OrderStatus?)(values[0]!);
        string? statusWindow = (values[1]!).ToString();
        if (statusWindow == "False" || status != BO.OrderStatus.Confirmed_Order)
            return Visibility.Hidden;
        else
            return Visibility.Visible;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class ConvertSendStatusToVisible : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        BO.OrderStatus? status = (OrderStatus?)(values[0]!);
        string? statusWindow = (values[1]!).ToString();
        if (statusWindow == "False" || status != BO.OrderStatus.Send_Order)
            return Visibility.Hidden;
        else
            return Visibility.Visible;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ConvertShipDateToTrue : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        string? statusWindow = (values[0]!).ToString();
        return values[1] != null && statusWindow == "True" ? ((((BO.Order?)values[1])?.ShipDate < DateTime.Now) ? Visibility.Hidden : Visibility.Visible) : Visibility.Hidden;
    }


    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ConvertFalseToNotInStock : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value == false ? "The item not In Stock" : "";
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class FontSizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double fontSize = (double)value;
        double factor = double.Parse((string)parameter, CultureInfo.InvariantCulture);
        return fontSize * factor;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertTrueToHidden : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value == true ? Visibility.Hidden : Visibility.Visible;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ConvertTrueToVisible : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value == true ? Visibility.Visible : Visibility.Hidden;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertBoolToOposite : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value == true ?false : true;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ConvertTimeToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        value=(value == null )? "0" : value;
        int.TryParse(value!.ToString(), out int result);
        return result >0? $"There are still {result} seconds left" : "Order processed";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConverterDouble : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double input = (double)value;
        double multiplier = Double.Parse(parameter.ToString(), CultureInfo.InvariantCulture);
        return input * multiplier;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}







