using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BO
{
    static class Tools
    {
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
    //    public static AutoMapper()
    //{
    //    var config = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, UserDTO>());
    //    var mapper = config.CreateMapper();
    //    UserDTO userDTO = mapper.Map<UserDTO>(user);

    //}
    }

}
