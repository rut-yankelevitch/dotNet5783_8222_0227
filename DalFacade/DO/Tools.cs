using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// class for exteonsion method
    /// </summary>
    static class Tools
    {
        /// <summary>
        /// A method that prints attributes of entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns>a string describing the entity</returns>
        public static string ToStringProperty<T>(this T t)
        {
            string str = "";
            foreach (PropertyInfo item in t.GetType().GetProperties())
            {
                var enumerable = item.GetValue(t, null);

                if ((enumerable is IEnumerable) && !(enumerable is string))
                {
                    IEnumerable e = enumerable as IEnumerable;
                    foreach (var a in e)
                    {
                        str += a.ToStringProperty();
                    }
                }
                else
                {
                    str += "\n" + item.Name +
               ": " + item.GetValue(t, null);
                }
            }
            return str;
        }


        public static void ToStringPropertyToIEnumerable(IEnumerable collection, string str)
        {
            foreach (var item in collection)
            {

                str += item;
            }
        }


        /// <summary>
        /// Converts between BO type and DO and vice versa according to the required case
        /// </summary>
        /// <typeparam name="U">B0/DO</typeparam>
        /// <typeparam name="T">BO/DO</typeparam>
        /// <param name="fromEntity">BO/DO Entity-Conversion from one type to another</param>
        /// <param name="toEntity">BO/DO Entity-Converting from this type to a received type</param>
        public static void CopyBetweenEntities<U, T>(this T fromEntity, U toEntity)
        {
            //Place the type type of the resulting object to convert to its type
            Type uType = toEntity!.GetType();

            //Passing over all the properties of the object that converts it and copying the exactly equal properties
            foreach (PropertyInfo prop in fromEntity!.GetType().GetProperties())
            {
                PropertyInfo? uProp = uType.GetProperty(prop.Name);
                if (uProp?.PropertyType == prop.PropertyType)
                {
                    uProp.SetValue(toEntity, prop.GetValue(fromEntity, null), null);
                }
            }
        }
    }
}
