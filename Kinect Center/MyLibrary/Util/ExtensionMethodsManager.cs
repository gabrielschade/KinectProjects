using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Util
{
    public static class ExtensionMethodsManager
    {
        public static T To<T>(this object currentType)
        {
            try
            {
                return (T)Convert.ChangeType(currentType, typeof(T));
            }
            catch 
            {
                return default(T);
            }
        }
    }
}
