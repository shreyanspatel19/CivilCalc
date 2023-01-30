using System.Configuration;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using System;


namespace CivilCalc.DAL
{

    public class DALHelper
    {
        #region Database Connection String
        public static string myConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("myconnectionstring");
        #endregion


        #region ExceptionHandler
        public ExceptionHandlerResult ExceptionHandler(Exception ex)
        {
            /**********************************************************************
             *  Turn ON/OFF exception by setting 'IsToThrowAnyException'
             **********************************************************************/
            bool isToThrowAnyException = true;


            /**********************************************************************
             *  Write your code to modify the value of 'exceptionToThrow'
             *  else set default value as below.
             **********************************************************************/
            Exception exceptionToThrow = ex;

            ExceptionHandlerResult vExceptionHandlerResult = new ExceptionHandlerResult()
            {
                IsToThrowAnyException = isToThrowAnyException,
                ExceptionToThrow = exceptionToThrow
            };

            return vExceptionHandlerResult;
        }
        #endregion


        #region ConvertDataTableToEntity
        public static List<T> ConvertDataTableToEntity<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                Type temp = typeof(T);
                T obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName && !dr[column.ColumnName].Equals(System.DBNull.Value))
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                        else
                            continue;
                    }
                }

                data.Add(obj);
            }
            return data;
        }


        #endregion


        #region EntityToString
        public string EntityToString<T>(T obj)
        {
            String ENT_String = String.Empty;
            ENT_String = JsonConvert.SerializeObject(obj);

            ENT_String = ENT_String.Replace(":", " → ");
            ENT_String = ENT_String.Replace(",", " ░ ");
            ENT_String = ENT_String.Replace("\"", "");
            ENT_String = ENT_String.Replace("{", "");
            ENT_String = ENT_String.Replace("}", "");
            return ENT_String;
        }
        #endregion
    }

    public class ExceptionHandlerResult
    {
        public bool IsToThrowAnyException { get; set; }
        public Exception? ExceptionToThrow { get; set; }
    }
}
