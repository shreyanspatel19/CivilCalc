using CivilCalc.Areas.CAL_NewCalculator.Models;
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
        public decimal? Insert(CAL_NewCalculatorModel obj_CAL_NewCalculator)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_NewCalculator_Insert");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, obj_CAL_NewCalculator.CalculatorID);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_NewCalculator.Description) ? null : obj_CAL_NewCalculator.Description.Trim());
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, obj_CAL_NewCalculator.Sequence);      
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
        public bool? Update(CAL_NewCalculatorModel obj_CAL_NewCalculator)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_NewCalculator_Update");
                sqlDB.AddInParameter(dbCMD, "NewCalculatorID", SqlDbType.Int, obj_CAL_NewCalculator.NewCalculatorID);
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, obj_CAL_NewCalculator.CalculatorID);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_NewCalculator.Description) ? null : obj_CAL_NewCalculator.Description.Trim());
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, obj_CAL_NewCalculator.Sequence);

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

        #region Method: SelectByCategoryIDUserID
        public List<SelectForSearch_Result> SelectForSearch(int CalculatorID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_NewCalculator_SelectForSearch");
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

    #region NewCalculator Entities

    #region Entity: dbo_PR_CAL_NewCalculator_SelectAll_Result
    public partial class SelectAll_Result : DALHelper
    {
        #region Properties
        public int NewCalculatorID { get; set; }
        public int CalculatorID { get; set; }
        public string? CalculatorName { get; set; }
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
        public string? CalculatorName { get; set; }
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
    public partial class SelectForSearch_Result : DALHelper
    {
        #region Properties
        public int NewCalculatorID { get; set; }
        public int CalculatorID { get; set; }
        public string? CalculatorName { get; set; }
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
