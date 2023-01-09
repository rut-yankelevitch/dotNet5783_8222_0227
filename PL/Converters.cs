using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PL
{
    public class ConvertZeroToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConvertZeroToVisible : IValueConverter
    {
        //convert from source property type to target property type
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }
        //convert from target property type to source property type
        public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConvertZeroToHidden : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Hidden;
            else
                return Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConvertShipDateToTrue : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime shipDate;
            if (value != null)
            {
                shipDate = (DateTime)value;
                if (shipDate < DateTime.Now)

                   return true;
                else
                    return false;
            }
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    //public class ConvertMany : IValueConverter
    //{

    //    public static object Convert(object? value1, object? value2, object? value3, object? value4, object? value5, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value1 != null && value2 != null && value3 != null && value4 != null && value5 != null)
    //        {
    //            int id = (int)value1;
    //            //BO.Category category = (BO.Category)value2;
    //            //string name=(string)value3;
    //            float price = (float)value4;
    //            int instock = (int)value5;
    //            //if(id)
    //            return true;
    //        }
    //        else 
    //            return false;
    //    }
    //        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

}
