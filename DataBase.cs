using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using Oracle.DataAccess.Client;
using System.Collections.Generic;
using Engine;

namespace DongFang
{
    /// <summary>
    /// DataBase 的摘要说明。
    /// </summary>
    public class DataBase
    {
        /// <summary>
        /// 110
        /// </summary>
        public static string odbcConnStr = ConfigXml.GetXmlNode("odbcConnection", "user id = smart; password=lzxMZD135468;data source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.5.75)(PORT = 1521)))(CONNECT_DATA = (SERVICE_NAME = db75)));");

        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static OracleDataReader getSysSqlSdr(string sqlStr)
        {
            OracleConnection conn = new OracleConnection(odbcConnStr);
            conn.Open();
            OracleCommand myCommand = new OracleCommand(sqlStr, conn);
            myCommand.CommandTimeout = 9999;
            OracleDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return result;
        }

        /// <summary>
        /// 110
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public static double ExecuteScalar(string Query)
        {
            double Result = 0;
            try
            {

                using (OracleConnection myConnection = new OracleConnection(odbcConnStr))
                {
                    myConnection.Open();
                    using (OracleCommand myCommand = new OracleCommand(Query, myConnection))
                    {
                        myCommand.CommandTimeout = 0;

                        double.TryParse(myCommand.ExecuteScalar().ToString(), out Result);
                    }
                }
            }
            catch
            {
                Result = -1;

            }

            return Result;
        }

        public static long ExecuteScalarLong(string Query)
        {
            long Result = 0;
            try
            {
                using (OracleConnection myConnection = new OracleConnection(odbcConnStr))
                {
                    myConnection.Open();
                    using (OracleCommand myCommand = new OracleCommand(Query, myConnection))
                    {
                        myCommand.CommandTimeout = 0;
                        long.TryParse(myCommand.ExecuteScalar().ToString(), out Result);
                    }
                }
            }
            catch
            {
                Result = -1;
            }
            return Result;
        }

        public static double ExecuteScalarString(string Query)
        {
            double Result = 0;
            try
            {

                using (OracleConnection myConnection = new OracleConnection(odbcConnStr))
                {
                    myConnection.Open();
                    using (OracleCommand myCommand = new OracleCommand(Query, myConnection))
                    {
                        myCommand.CommandTimeout = 0;

                        myCommand.ExecuteScalar().ToString();
                    }
                }
            }
            catch
            {
                Result = -1;

            }

            return Result;
        }

        /// <summary>
        /// 需要人工关闭数据库
        /// </summary>
        /// <param name="myCommand"></param>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static OracleDataReader getSysSqlSdr(OracleCommand myCommand)
        {
            myCommand.CommandTimeout = 9999;
            OracleDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return result;
        }

   
        public static bool exeSysSqlStr(string sqlStr)
        {
            bool result = false;
            OracleConnection conn = new OracleConnection(odbcConnStr);
            try
            {
                OracleCommand cmd = new OracleCommand(sqlStr, conn);
                cmd.CommandTimeout = 9999;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
                result = true;
            }
            catch (Exception e)
            {
                conn.Close();
                conn.Dispose();
                LogUtility.LogErr("错误提示：" + e.Message + "Oracle SQL语句 ：" + sqlStr + " 时间：" + DateTime.Now.ToString());
            }
            return result;
        }
    }
}