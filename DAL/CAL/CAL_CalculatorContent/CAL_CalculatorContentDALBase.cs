using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Drawing.Imaging;
using System.Xml.Linq;
using CivilCalc.Areas.CAL_CalculatorContent.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;


namespace CivilCalc.DAL.CAL.CAL_CalculatorContent
{
    public abstract class CAL_CalculatorContentDALBase : DALHelper
    {
        #region CalculatorContent Methods

        #region Method: SelectAll
        public List<SelectAll_Result> SelectAll()
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_CalculatorContent_SelectAll");

                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<SelectAll_Result>(dt);
            }
            catch (Exception ex)
            {
                var vExceptionHandler = ExceptionHandler(ex);
                if (vExceptionHandler.IsToThrowAnyException)
                    throw vExceptionHandler.ExceptionToThrow;
                return null;
            }
        }
        #endregion

        #region Method: SelectPK
        public List<SelectPK_Result> SelectPK(int? CalculatorContentID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_CalculatorContent_SelectPK");
                sqlDB.AddInParameter(dbCMD, "CalculatorContentID", SqlDbType.Int, CalculatorContentID);

                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<SelectPK_Result>(dt);
            }
            catch (Exception ex)
            {
                var vExceptionHandler = ExceptionHandler(ex);
                if (vExceptionHandler.IsToThrowAnyException)
                    throw vExceptionHandler.ExceptionToThrow;
                return null;
            }
        }
        #endregion

        #region Method: Insert
        public decimal? Insert(CAL_CalculatorContentModel obj_CAL_Calculator)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_CalculatorContent_Insert");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, obj_CAL_Calculator.CalculatorID);
                sqlDB.AddInParameter(dbCMD, "PageContent", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.PageContent) ? null : obj_CAL_Calculator.PageContent.Trim());
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, obj_CAL_Calculator.Sequence);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.Description) ? null : obj_CAL_Calculator.Description.Trim());
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.Int, 1);
                var vResult = sqlDB.ExecuteScalar(dbCMD);
                if (vResult == null)
                    return null;

                return (decimal)Convert.ChangeType(vResult, vResult.GetType());
            }
            catch (Exception ex)
            {
                var vExceptionHandler = ExceptionHandler(ex);
                if (vExceptionHandler.IsToThrowAnyException)
                    throw vExceptionHandler.ExceptionToThrow;
                return null;
            }
        }
        #endregion

        #region Method: Update
        public bool? Update(CAL_CalculatorContentModel obj_CAL_Calculator)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_CalculatorContent_Update");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, obj_CAL_Calculator.CalculatorID);
                sqlDB.AddInParameter(dbCMD, "CalculatorContentID", SqlDbType.Int, obj_CAL_Calculator.CalculatorContentID);
                sqlDB.AddInParameter(dbCMD, "PageContent", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.PageContent) ? null : obj_CAL_Calculator.PageContent.Trim());
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, obj_CAL_Calculator.Sequence);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.Description) ? null : obj_CAL_Calculator.Description.Trim());
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.Int, 1);

                int vReturnValue = sqlDB.ExecuteNonQuery(dbCMD);
                return vReturnValue == -1 ? false : true;
            }
            catch (Exception ex)
            {
                var vExceptionHandler = ExceptionHandler(ex);
                if (vExceptionHandler.IsToThrowAnyException)
                    throw vExceptionHandler.ExceptionToThrow;
                return null;
            }
        }
        #endregion

        #region Method: Delete
        public bool? Delete(int? CalculatorID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_Delete");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, CalculatorID);

                int vReturnValue = sqlDB.ExecuteNonQuery(dbCMD);
                return vReturnValue == -1 ? false : true;
            }
            catch (Exception ex)
            {
                var vExceptionHandler = ExceptionHandler(ex);
                if (vExceptionHandler.IsToThrowAnyException)
                    throw vExceptionHandler.ExceptionToThrow;
                return null;
            }
        }
        #endregion

        #region Method: SelectForSearch_Result
        //public List<SelectForSearch_Result> SelectForSearch(int CategoryID,string? F_CalculatorName)
        //{
        //    try
        //    {
        //        SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
        //        DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_CalculatorContent_SelectForSearch");
        //        sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, CategoryID);
        //        sqlDB.AddInParameter(dbCMD, "CalculatorName", SqlDbType.NVarChar, F_CalculatorName);
        //        DataTable dt = new DataTable();
        //        using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
        //        {
        //            dt.Load(dr);
        //        }

        //        return ConvertDataTableToEntity<SelectForSearch_Result>(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        var vExceptionHandler = ExceptionHandler(ex);
        //        if (vExceptionHandler.IsToThrowAnyException)
        //            throw vExceptionHandler.ExceptionToThrow;
        //        return null;
        //    }
        //}
        #endregion

        #region Method: SelectByURLName
        public List<SelectForSearch_Result> SelectByURLName(int? CalculatorID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_CalculatorContent_SelectByCalculator");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, CalculatorID);
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<SelectForSearch_Result>(dt);
            }
            catch (Exception ex)
            {
                var vExceptionHandler = ExceptionHandler(ex);
                if (vExceptionHandler.IsToThrowAnyException)
                    throw vExceptionHandler.ExceptionToThrow;
                return null;
            }
        }
        #endregion

        #endregion       
    }

    #region Calculator Entities

    #region Entity: dbo_PR_CAL_Calculator_SelectAll_Result
    public partial class SelectAll_Result : DALHelper
    {
        #region Properties
        public int CalculatorContentID { get; set; }
        public int CalculatorID { get; set; }
        public string? CalculatorName { get; set; }
        public string? PageContent { get; set; }
        public decimal Sequence { get; set; }
        public string? Description { get; set; }
        public int UserID { get; set; }
        public string? UserName { get; set; }

        #endregion

        #region Convert Entity to String
        public override string ToString()
        {
            return EntityToString(this);
        }
        #endregion

    }
    #endregion

    #region Entity: dbo_PR_CAL_Calculator_SelectByPK_Result
    public partial class SelectPK_Result : DALHelper
    {
        #region Properties
        public int CalculatorContentID { get; set; }
        public int CalculatorID { get; set; }
        public string? PageContent { get; set; }
        public decimal Sequence { get; set; }
        public string? Description { get; set; }
        public int UserID { get; set; }

        #endregion

        #region Convert Entity to String
        public override string ToString()
        {
            return EntityToString(this);
        }
        #endregion
    }
    #endregion

    #region Entity: SelectForSearch_Result
    public partial class SelectForSearch_Result : DALHelper
    {
        #region Properties
        public int CalculatorContentID { get; set; }
        public int CalculatorID { get; set; }
        public string? PageContent { get; set; }
        public decimal Sequence { get; set; }
        public string? Description { get; set; }
        public int UserID { get; set; }

        #endregion

        #region Convert Entity to String
        public override string ToString()
        {
            return EntityToString(this);
        }
        #endregion
    }
    #endregion

    #endregion

}
