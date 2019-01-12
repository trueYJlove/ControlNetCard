using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Engine
{
    public class PubStatic
    {
        /// <summary>
        /// 使用MD5加密
        /// </summary>
        /// <param name="str">原值</param>
        /// <returns>加密后的值</returns>
        public static string MD5(string str)
        {
            if (str.Length == 0)
            {
                return string.Empty;
            }
            string retunvale = string.Empty;
            try
            {
                retunvale = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }
            catch (Exception)
            {
                retunvale = string.Empty;
            }
            return retunvale;
        }
    }
}
