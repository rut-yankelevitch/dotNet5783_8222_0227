using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using BO;

namespace PL
{
    class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!File.Exists((string)value))
                    throw new Exception("");

                BitmapImage b = new BitmapImage(new Uri((string)value, UriKind.RelativeOrAbsolute));
                Console.WriteLine(b.DpiX);
                return b;
            }
            catch (Exception ex)
            {
                return new BitmapImage(new Uri(@"images\empty_image.gif", UriKind.RelativeOrAbsolute));
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
                return @"images\passport\empty_image.gif";
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
            if(value == null||(string)value=="")
            {
                return false;
            }
            return  true ;
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
            return (int)value! == 0 ? Visibility.Hidden : Visibility.Visible;
        }
        public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConvertZeroStringToHidden : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value!.ToString()! == "0" ? Visibility.Hidden : Visibility.Visible;
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
            if(value == null )
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
            return (int)value! == 0 ? Visibility.Visible: Visibility.Hidden;
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

    public class ConvertShipDateToTrue : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string? statusWindow = (values[0]!).ToString();
            return values[1] != null && statusWindow == "True" ? ((((BO.Order?)values[1])?.ShipDate < DateTime.Now) ? false : true) : false;
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
            return ((values[0]!).ToString()!.Length == 6 && (values[1]!).ToString()!.Length > 0 && (values[2]!).ToString()!.Length > 0 && (values[3]!).ToString()!.Length > 0 && values[4] != null && ((BO.Category)values[4]!) != BO.Category.None) ? true : false;
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

    public class DateTimeConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? date = (DateTime?)value;
            return date != null ? (date?.ToString("dd, MM,yy",
                      CultureInfo.InvariantCulture)) : "לא קיים תאריך";
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
            return status == BO.OrderStatus.ConfirmedOrder ? BO.OrderStatus.SendOrder : status == BO.OrderStatus.SendOrder ? BO.OrderStatus.ProvidedOrder : null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertStatusToVisible : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            BO.OrderStatus? status = (OrderStatus?)(values[0]!);
            string? statusWindow = (values[1]!).ToString();
            if (statusWindow == "False" || status == BO.OrderStatus.ProvidedOrder)
                return Visibility.Hidden;
            else
                return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //public class ConvertAmount : IValueConverter
    //{
    //    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return value==0? : null;
    //    }

    //    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


}