using System;
using System.Data;
using System.Data.Common;
using System.Drawing.Imaging;
using CivilCalc.Areas.CAL_Category.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace CivilCalc.DAL.CAL.CAL_Category
{
    public abstract class SEC_UserDALBase : DALHelper
    {
        
        #region Category Methods

        #region Method: SelectAll
        public List<SelectAll_Result> SelectAll()
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
        public List<SelectPK_Result> SelectPK(int? CategoryID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_SelectPK");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, CategoryID);

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
        public decimal? Insert(CAL_CategoryModel objCategoryModel)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_Insert");
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.NVarChar, objCategoryModel.CategoryName);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objCategoryModel.Description);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, objCategoryModel.Sequence);
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
        public bool? Update(CAL_CategoryModel objCategoryModel)        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_Update");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, objCategoryModel.CategoryID);
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.NVarChar, objCategoryModel.CategoryName);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objCategoryModel.Description);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, objCategoryModel.Sequence);
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
        public bool? Delete(int? CategoryID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_Delete");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, CategoryID);

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
        public List<SelectByCategoryNameUserName_Result> SelectByCategoryNameUserName(string? C_CategoryName, string? C_UserName)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_SelectByCategoryNameUserName");
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.VarChar, C_CategoryName);
                sqlDB.AddInParameter(dbCMD, "UserName", SqlDbType.VarChar, C_UserName);
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<SelectByCategoryNameUserName_Result>(dt);
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

    #region Entity: dbo_PR_CAL_Category_SelectAll_Result
    public partial class SelectAll_Result : DALHelper
    {
        #region Properties
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public decimal Sequence { get; set; }
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

    #region Entity: dbo_PR_CAL_Category_SelectByPK_Result
    public partial class SelectPK_Result : DALHelper
    {
        #region Properties
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public decimal Sequence { get; set; }
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

    #region Entity: dbo_PR_CAL_Category_SelectForSearch_Result
    public partial class SelectByCategoryNameUserName_Result : DALHelper
    {
        #region Properties
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public decimal Sequence { get; set; }
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

    #endregion

    
   

}
