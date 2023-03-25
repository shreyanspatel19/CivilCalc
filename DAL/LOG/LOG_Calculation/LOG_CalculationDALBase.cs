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

        #region Method: Insert
        public decimal? Insert(LOG_CalculationModel obj_LOG_Calculation)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_LOG_Calculation_Insert");
                sqlDB.AddInParameter(dbCMD, "CalculationID", SqlDbType.Int, 1);
                sqlDB.AddInParameter(dbCMD, "ScreenName", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ScreenName) ? null : obj_LOG_Calculation.ScreenName.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamA", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamA) ? null : obj_LOG_Calculation.ParamA.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamB", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamB) ? null : obj_LOG_Calculation.ParamB.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamC", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamC) ? null : obj_LOG_Calculation.ParamC.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamD", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamD) ? null : obj_LOG_Calculation.ParamD.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamE", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamE) ? null : obj_LOG_Calculation.ParamE.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamF", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamF) ? null : obj_LOG_Calculation.ParamF.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamG", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamG) ? null : obj_LOG_Calculation.ParamG.Trim());

                sqlDB.AddInParameter(dbCMD, "ParamH", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamH) ? null : obj_LOG_Calculation.ParamH.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamI", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamI) ? null : obj_LOG_Calculation.ParamI.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamJ", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamJ) ? null : obj_LOG_Calculation.ParamJ.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamK", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamK) ? null : obj_LOG_Calculation.ParamK.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamL", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamL) ? null : obj_LOG_Calculation.ParamL.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamM", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamM) ? null : obj_LOG_Calculation.ParamM.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamN", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamN) ? null : obj_LOG_Calculation.ParamN.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamO", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamO) ? null : obj_LOG_Calculation.ParamO.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamP", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamP) ? null : obj_LOG_Calculation.ParamP.Trim());

                sqlDB.AddInParameter(dbCMD, "ParamQ", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamQ) ? null : obj_LOG_Calculation.ParamQ.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamR", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamR) ? null : obj_LOG_Calculation.ParamR.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamS", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamS) ? null : obj_LOG_Calculation.ParamS.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamT", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamT) ? null : obj_LOG_Calculation.ParamT.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamU", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamU) ? null : obj_LOG_Calculation.ParamU.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamV", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamV) ? null : obj_LOG_Calculation.ParamV.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamW", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamW) ? null : obj_LOG_Calculation.ParamW.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamX", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamX) ? null : obj_LOG_Calculation.ParamX.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamY", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamY) ? null : obj_LOG_Calculation.ParamY.Trim());
                sqlDB.AddInParameter(dbCMD, "ParamZ", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_LOG_Calculation.ParamZ) ? null : obj_LOG_Calculation.ParamZ.Trim());
                sqlDB.AddInParameter(dbCMD, "@Created", SqlDbType.DateTime, null);
                sqlDB.AddInParameter(dbCMD, "@Modified", SqlDbType.DateTime, null);
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
