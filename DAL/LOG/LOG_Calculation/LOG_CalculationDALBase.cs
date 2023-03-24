using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using CivilCalc.Areas.LOG_Calculation.Models;
using CivilCalc.DAL.LOG.LOG_Calculation;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace CivilCalc.DAL.LOG.LOG_Calculation
{
    public class LOG_CalculationDALBase :  DALHelper
    {
        #region LOG_Calculation Method

        #region Method: SelectAll
        public List<SelectAll_Result> SelectAll()
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_LOG_Calculation_SelectAll");

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

        #region Method: Delete
        public bool? Delete(int? CalculationID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_LOG_Calculation_Delete");
                sqlDB.AddInParameter(dbCMD, "CalculationID", SqlDbType.Int, CalculationID);

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
        public List<SelectForSearch_Result> SelectForSearch(string? F_ScreenName)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_LOG_Calculation_SelectForSearch");
                sqlDB.AddInParameter(dbCMD, "ScreenName", SqlDbType.NVarChar, F_ScreenName);
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


    #region Category Entities

    #region Entity: dbo_PR_LOG_Calculation_SelectAll_Result
    public partial class SelectAll_Result : DALHelper
    {
        #region Properties
        public int CalculationID { get; set; }
        public string? ScreenName { get; set; }
        public string? ParamA { get; set; }
        public string? ParamB { get; set; }
        public string? ParamC { get; set; }
        public string? ParamD { get; set; }
        public string? ParamE { get; set; }
        public string? ParamF { get; set; }
        public string? ParamG { get; set; }
        public string? ParamH { get; set; }
        public string? ParamI { get; set; }
        public string? ParamJ { get; set; }
        public string? ParamK { get; set; }
        public string? ParamL { get; set; }
        public string? ParamM { get; set; }
        public string? ParamN { get; set; }
        public string? ParamO { get; set; }
        public string? ParamP { get; set; }
        public string? ParamQ { get; set; }
        public string? ParamR { get; set; }
        public string? ParamS { get; set; }
        public string? ParamT { get; set; }
        public string? ParamU { get; set; }
        public string? ParamV { get; set; }
        public string? ParamW { get; set; }
        public string? ParamX { get; set; }
        public string? ParamY { get; set; }
        public string? ParamZ { get; set; }
        #endregion

        #region Convert Entity to String
        public override string ToString()
        {
            return EntityToString(this);
        }
        #endregion
    }
    #endregion

    #region Entity: dbo_PR_LOG_Calculation_SelectForSearch_Result
    public partial class SelectForSearch_Result : DALHelper
    {
        #region Properties
        public int CalculationID { get; set; }
        public string? ScreenName { get; set; }
        public string? ParamA { get; set; }
        public string? ParamB { get; set; }
        public string? ParamC { get; set; }
        public string? ParamD { get; set; }
        public string? ParamE { get; set; }
        public string? ParamF { get; set; }
        public string? ParamG { get; set; }
        public string? ParamH { get; set; }
        public string? ParamI { get; set; }
        public string? ParamJ { get; set; }
        public string? ParamK { get; set; }
        public string? ParamL { get; set; }
        public string? ParamM { get; set; }
        public string? ParamN { get; set; }
        public string? ParamO { get; set; }
        public string? ParamP { get; set; }
        public string? ParamQ { get; set; }
        public string? ParamR { get; set; }
        public string? ParamS { get; set; }
        public string? ParamT { get; set; }
        public string? ParamU { get; set; }
        public string? ParamV { get; set; }
        public string? ParamW { get; set; }
        public string? ParamX { get; set; }
        public string? ParamY { get; set; }
        public string? ParamZ { get; set; }

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
