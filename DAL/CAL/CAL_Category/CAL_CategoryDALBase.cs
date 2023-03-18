using System;
using System.Data;
using System.Data.Common;
using System.Drawing.Imaging;
using System.Reflection.Metadata;
using CivilCalc.Areas.CAL_Category.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using static CivilCalc.Areas.CAL_Category.Models.CAL_CategoryModel;

namespace CivilCalc.DAL.CAL.CAL_Category
{
    public abstract class CAL_CategoryDALBase : DALHelper
    {

        #region Category Methods

        #region Method: SelectForPage
        public List<SelectAll_Result> SelectForPage(int PageOffset, int PageSize, string CategoryName)
        {

            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_SelectForPage");
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.VarChar, CategoryName);
                sqlDB.AddInParameter(dbCMD, "PageOffset", SqlDbType.Int, PageOffset);
                sqlDB.AddInParameter(dbCMD, "PageSize", SqlDbType.Int, PageSize);
                sqlDB.AddInParameter(dbCMD, "TotalRecords", SqlDbType.Int, 10);
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

        #region Method: CategoryComboBox
        public List<CAL_CategoryComboBoxModel> SelectComboBoxCategory()
        {

            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbMST = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_SelectComboBox");
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbMST))
                {
                    dt.Load(dr);
                }
                List<CAL_CategoryComboBoxModel> list = new List<CAL_CategoryComboBoxModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    CAL_CategoryComboBoxModel vlst = new CAL_CategoryComboBoxModel();
                    vlst.CategoryID = Convert.ToInt32(dr["CategoryID"]);
                    vlst.CategoryName = dr["CategoryName"].ToString();
                    list.Add(vlst);
                }
                return list;

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
        public decimal? Insert(CAL_CategoryModel obj_CAL_Category)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_Insert");
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Category.CategoryName) ? null : obj_CAL_Category.CategoryName.Trim());
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Category.Description) ? null : obj_CAL_Category.Description.Trim());
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, obj_CAL_Category.Sequence);
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
        public bool? Update(CAL_CategoryModel obj_CAL_Category)        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_Update");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, obj_CAL_Category.CategoryID);
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Category.CategoryName) ? null : obj_CAL_Category.CategoryName.Trim());
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Category.Description) ? null : obj_CAL_Category.Description.Trim());
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, obj_CAL_Category.Sequence);
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

        #region Method: SelectForSearch
        public List<SelectForSearch_Result> SelectForSearch(string? F_CategoryName)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_SelectForSearch");
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.NVarChar, F_CategoryName);
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
    public partial class SelectForSearch_Result : DALHelper
    {
        #region Properties
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public decimal Sequence { get; set; }
        public int UserID { get; set; }
        public int i { get; set; }
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
