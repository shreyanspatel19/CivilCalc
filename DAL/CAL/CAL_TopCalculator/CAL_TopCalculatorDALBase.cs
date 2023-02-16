using CivilCalc.Areas.CAL_TopCalculator.Models;
using CivilCalc.DAL.CAL.CAL_TopCalculator;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Drawing.Imaging;
using System.Data.Common;
using System.Data;
using System;

namespace CivilCalc.DAL.CAL.CAL_TopCalculator
{
    public class CAL_TopCalculatorDALBase : DALHelper
    {
        #region TopCalculator Methods

        #region Method: SelectAll
        public List<SelectAll_Result> SelectAll()
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_TopCalculator_SelectAll");

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
        public List<SelectPK_Result> SelectPK(int? TopCalculatorID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_TopCalculator_SelectPK");
                sqlDB.AddInParameter(dbCMD,"TopCalculatorID", SqlDbType.Int, TopCalculatorID);

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
        public decimal? Insert(CAL_TopCalculatorModel objTopCalculatorModel)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_TopCalculator_Insert");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, objTopCalculatorModel.CalculatorID);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objTopCalculatorModel.Description);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, objTopCalculatorModel.Sequence);
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
        public bool? Update(CAL_TopCalculatorModel objTopCalculatorModel)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_TopCalculator_Updete");
                sqlDB.AddInParameter(dbCMD, "TopCalculatorID", SqlDbType.Int, objTopCalculatorModel.TopCalculatorID);
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, objTopCalculatorModel.CalculatorID);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objTopCalculatorModel.Description);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, objTopCalculatorModel.Sequence);

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
        public bool? Delete(int? TopCalculatorID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_TopCalculator_Delete");
                sqlDB.AddInParameter(dbCMD, "TopCalculatorID", SqlDbType.Int, TopCalculatorID);

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
        public List<SelectByTopCalculatorNameUserName_Result> SelectByTopCalculatorNameUserName(string? F_CategoryName, string? F_UserName)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_TopCalculator_SelectByCategoryNameUserName");
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.VarChar, F_CategoryName);
                sqlDB.AddInParameter(dbCMD, "UserName", SqlDbType.VarChar, F_UserName);
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<SelectByTopCalculatorNameUserName_Result>(dt);
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

    #region TopCalculator Entities

    #region Entity: dbo_PR_CAL_TopCalculator_SelectAll_Result
    public partial class SelectAll_Result : DALHelper
    {
        #region Properties
        public int TopCalculatorID { get; set; }
        public int CalculatorID { get; set; }
        public string CalculatorName { get; set; }
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

    #region Entity: dbo_PR_CAL_TopCalculator_SelectByPK_Result
    public partial class SelectPK_Result : DALHelper
    {
        #region Properties
        public int TopCalculatorID { get; set; }
        public int CalculatorID { get; set; }
        public string CalculatorName { get; set; }
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

    #region Entity: dbo_PR_CAL_TopCalculator_SelectForSearch_Result
    public partial class SelectByTopCalculatorNameUserName_Result : DALHelper
    {
        #region Properties
        public int TopCalculatorID { get; set; }
        public int CalculatorID { get; set; }
        public string CalculatorName { get; set; }
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
