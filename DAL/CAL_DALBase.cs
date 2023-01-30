using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace CivilCalc.DAL
{
    public abstract class CAL_DALBase : DALHelper
    {

        #region Method: dbo_PR_CAL_Category_SelectAll
        public List<dbo_PR_CAL_Category_SelectAll_Result> dbo_PR_CAL_Category_SelectAll()
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_SelectAll");

                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<dbo_PR_CAL_Category_SelectAll_Result>(dt);
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

        #region Method: dbo_PR_CAL_Category_SelectByPK
        public List<dbo_PR_CAL_Category_SelectByPK_Result> dbo_PR_CAL_Category_SelectByPK(int? CategoryId)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_SelectPK");
                sqlDB.AddInParameter(dbCMD, "CategoryId", SqlDbType.Int, CategoryId);

                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<dbo_PR_CAL_Category_SelectByPK_Result>(dt);
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

        #region Method: dbo_PR_CAL_Category_Insert
        public decimal? dbo_PR_CAL_Category_Insert(string? CategoryName, string? Description, int Sequence, int UserID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_Insert");
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.VarChar, CategoryName);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.VarChar, Description);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Int, Sequence);
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.Int, UserID);
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

        #region Method: dbo_PR_CAL_Category_UpdateByPK
        public bool? dbo_PR_CAL_Category_UpdateByPK(int CategoryID, string? CategoryName, string? Description, int Sequence, int UserID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_Update");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, CategoryID);
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.VarChar, CategoryName);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.VarChar, Description);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Int, Sequence);
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.Int, UserID);

                int vReturnValue = sqlDB.ExecuteNonQuery(dbCMD);
                return (vReturnValue == -1 ? false : true);
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

        #region Method: dbo_PR_CAL_Category_DeleteByPK
        public bool? dbo_PR_CAL_Category_DeleteByPK(int? CategoryID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_Delete");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, CategoryID);

                int vReturnValue = sqlDB.ExecuteNonQuery(dbCMD);
                return (vReturnValue == -1 ? false : true);
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
    }

    #region All Entities

    #region Entity: dbo_PR_CAL_Category_SelectAll_Result
    public partial class dbo_PR_CAL_Category_SelectAll_Result : DALHelper
    {
        #region Properties
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public decimal Sequence { get; set; }
        public int UserID { get; set; }

        #endregion

        #region Convert Entity to String
        public override String ToString()
        {
            return EntityToString(this);
        }
        #endregion
    }
    #endregion

    #region Entity: dbo_PR_CAL_Category_SelectByPK_Result
    public partial class dbo_PR_CAL_Category_SelectByPK_Result : DALHelper
    {
        #region Properties
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public float Sequence { get; set; }
        public int UserID { get; set; }
        #endregion

        #region Convert Entity to String
        public override String ToString()
        {
            return EntityToString(this);
        }
        #endregion
    }
    #endregion

    #endregion

}
