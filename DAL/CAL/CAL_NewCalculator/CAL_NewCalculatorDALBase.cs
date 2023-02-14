﻿using CivilCalc.Areas.CAL_NewCalculator.Models;
using CivilCalc.DAL.CAL.CAL_NewCalculator;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Drawing.Imaging;
using System.Data.Common;
using System.Data;
using System;

namespace CivilCalc.DAL.CAL.CAL_NewCalculator
{
    public class CAL_NewCalculatorDALBase : DALHelper
    {
        #region NewCalculator Methods

        #region Method: SelectAll
        public List<SelectAll_Result> SelectAll()
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_NewCalculator_SelectAll");

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
        public List<SelectPK_Result> SelectPK(int? NewCalculatorID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_NewCalculator_SelectPK");
                sqlDB.AddInParameter(dbCMD, "NewCalculatorID", SqlDbType.Int, NewCalculatorID);

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
        public decimal? Insert(CAL_NewCalculatorModel objNewCalculatorModel)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_NewCalculator_Insert");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, objNewCalculatorModel.CalculatorID);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objNewCalculatorModel.Description);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, objNewCalculatorModel.Sequence);      
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
        public bool? Update(CAL_NewCalculatorModel objNewCalculatorModel)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_NewCalculator_Update");
                sqlDB.AddInParameter(dbCMD, "NewCalculatorID", SqlDbType.Int, objNewCalculatorModel.NewCalculatorID);
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, objNewCalculatorModel.CalculatorID);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objNewCalculatorModel.Description);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, objNewCalculatorModel.Sequence);

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
        public bool? Delete(int? NewCalculatorID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_NewCalculator_Delete");
                sqlDB.AddInParameter(dbCMD, "NewCalculatorID", SqlDbType.Int, NewCalculatorID);

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

        #region Method: SelectByCategoryNameUserName
        public List<SelectByNewCalculatorNameUserName_Result> SelectByNewCalculatorNameUserName(string? F_CategoryName, string? F_UserName)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_NewCalculator_SelectByCategoryNameUserName");
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.VarChar, F_CategoryName);
                sqlDB.AddInParameter(dbCMD, "UserName", SqlDbType.VarChar, F_UserName);
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<SelectByNewCalculatorNameUserName_Result>(dt);
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

    #region NewCalculator Entities

    #region Entity: dbo_PR_CAL_NewCalculator_SelectAll_Result
    public partial class SelectAll_Result : DALHelper
    {
        #region Properties
        public int NewCalculatorID { get; set; }
        public int CalculatorID { get; set; }
        public string? Description { get; set; }
        public decimal Sequence { get; set; }
       
        #endregion

        #region Convert Entity to String
        public override string ToString()
        {
            return EntityToString(this);
        }
        #endregion
    }
    #endregion

    #region Entity: dbo_PR_CAL_NewCalculator_SelectByPK_Result
    public partial class SelectPK_Result : DALHelper
    {
        #region Properties
        public int NewCalculatorID { get; set; }
        public int CalculatorID { get; set; }
        public string? Description { get; set; }
        public decimal Sequence { get; set; }
        #endregion

        #region Convert Entity to String
        public override string ToString()
        {
            return EntityToString(this);
        }
        #endregion
    }
    #endregion

    #region Entity: dbo_PR_CAL_NewCalculator_SelectForSearch_Result
    public partial class SelectByNewCalculatorNameUserName_Result : DALHelper
    {
        #region Properties
        public int NewCalculatorID { get; set; }
        public int CalculatorID { get; set; }
        public string? Description { get; set; }
        public decimal Sequence { get; set; }

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