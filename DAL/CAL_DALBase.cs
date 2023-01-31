using System;
using System.Data;
using System.Data.Common;
using System.Drawing.Imaging;
using Microsoft.Net.Http.Headers;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace CivilCalc.DAL
{
    public abstract class CAL_DALBase : DALHelper
    {
        #region All Methods

        #region Category Methods

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
        public List<dbo_PR_CAL_Category_SelectByPK_Result> dbo_PR_CAL_Category_SelectByPK(int? CategoryID)
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
        public decimal? dbo_PR_CAL_Category_Insert(string? CategoryName, string? Description, decimal Sequence, int UserID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_Insert");
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.VarChar, CategoryName);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.VarChar, Description);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, Sequence);
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
        public bool? dbo_PR_CAL_Category_UpdateByPK(int CategoryID, string? CategoryName, string? Description, decimal Sequence, int UserID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_Update");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, CategoryID);
                sqlDB.AddInParameter(dbCMD, "CategoryName", SqlDbType.VarChar, CategoryName);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.VarChar, Description);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, Sequence);
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

        #region Method: dbo_PR_CAL_Category_Delete
        public bool? dbo_PR_CAL_Category_Delete(int? CategoryID)
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

        #endregion

        #region calculator Methods

        #region Method: dbo_PR_CAL_Calculator_SelectAll
        public List<dbo_PR_CAL_Calculator_SelectAll_Result> dbo_PR_CAL_Calculator_SelectAll()
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_SelectAll");

                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<dbo_PR_CAL_Calculator_SelectAll_Result>(dt);
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

        #region Method: dbo_PR_CAL_Calculator_SelectByPK
        public List<dbo_PR_CAL_Calculator_SelectByPK_Result> dbo_PR_CAL_Calculator_SelectByPK(int? CalculatorID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_SelectPK");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, CalculatorID);

                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<dbo_PR_CAL_Calculator_SelectByPK_Result>(dt);
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

        #region Method: dbo_PR_CAL_Calculator_Insert
        public decimal? dbo_PR_CAL_Calculator_Insert(int CategoryID, string? CalculatorName, string? CalculatorIcon, string? URLName, string? HeaderName, string? SubHeaderName, string? CalculatorDescription, string? PageSection1, string? PageSection2, string? PageSecton3, string? MetaTitle, string? MetaKeyword, string? MetaDescription, string? MetaAuthor, string? MetaOgTitle, string? MetaOgImage, string? MetaOgDescription, string? MetaOgUrl, string? MetaOgType, decimal Sequence, string? Description, int UserID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_Insert");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, CategoryID);
                sqlDB.AddInParameter(dbCMD, "CalculatorName", SqlDbType.VarChar, CalculatorName);
                sqlDB.AddInParameter(dbCMD, "CalculatorIcon", SqlDbType.VarChar, CalculatorIcon);
                sqlDB.AddInParameter(dbCMD, "URLName", SqlDbType.VarChar, URLName);
                sqlDB.AddInParameter(dbCMD, "HeaderName", SqlDbType.VarChar, HeaderName);
                sqlDB.AddInParameter(dbCMD, "SubHeaderName", SqlDbType.VarChar, SubHeaderName);
                sqlDB.AddInParameter(dbCMD, "CalculatorDescription", SqlDbType.VarChar, CalculatorDescription);
                sqlDB.AddInParameter(dbCMD, "PageSection1", SqlDbType.VarChar, PageSection1);
                sqlDB.AddInParameter(dbCMD, "PageSection2", SqlDbType.VarChar, PageSection2);
                sqlDB.AddInParameter(dbCMD, "PageSecton3", SqlDbType.VarChar, PageSecton3);
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.VarChar, MetaTitle);
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.VarChar, MetaKeyword);
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.VarChar, MetaDescription);
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.VarChar, MetaAuthor);
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.VarChar, MetaOgTitle);
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.VarChar, MetaOgImage);
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.VarChar, MetaOgDescription);
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.VarChar, MetaOgUrl);
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.VarChar, MetaOgType);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, Sequence);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.VarChar, Description);
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

        #region Method: dbo_PR_CAL_Calculator_UpdateByPK
        public bool? dbo_PR_CAL_Calculator_UpdateByPK(int CalculatorID, int CategoryID, string? CalculatorName, string? CalculatorIcon, string? URLName, string? HeaderName, string? SubHeaderName, string? CalculatorDescription, string? PageSection1, string? PageSection2, string? PageSecton3, string? MetaTitle, string? MetaKeyword, string? MetaDescription, string? MetaAuthor, string? MetaOgTitle, string? MetaOgImage, string? MetaOgDescription, string? MetaOgUrl, string? MetaOgType, decimal Sequence, string? Description, int UserID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_Update");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, CalculatorID);
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, CategoryID);
                sqlDB.AddInParameter(dbCMD, "CalculatorName", SqlDbType.VarChar, CalculatorName);
                sqlDB.AddInParameter(dbCMD, "CalculatorIcon", SqlDbType.VarChar, CalculatorIcon);
                sqlDB.AddInParameter(dbCMD, "URLName", SqlDbType.VarChar, URLName);
                sqlDB.AddInParameter(dbCMD, "HeaderName", SqlDbType.VarChar, HeaderName);
                sqlDB.AddInParameter(dbCMD, "SubHeaderName", SqlDbType.VarChar, SubHeaderName);
                sqlDB.AddInParameter(dbCMD, "CalculatorDescription", SqlDbType.VarChar, CalculatorDescription);
                sqlDB.AddInParameter(dbCMD, "PageSection1", SqlDbType.VarChar, PageSection1);
                sqlDB.AddInParameter(dbCMD, "PageSection2", SqlDbType.VarChar, PageSection2);
                sqlDB.AddInParameter(dbCMD, "PageSecton3", SqlDbType.VarChar, PageSecton3);
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.VarChar, MetaTitle);
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.VarChar, MetaKeyword);
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.VarChar, MetaDescription);
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.VarChar, MetaAuthor);
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.VarChar, MetaOgTitle);
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.VarChar, MetaOgImage);
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.VarChar, MetaOgDescription);
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.VarChar, MetaOgUrl);
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.VarChar, MetaOgType);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, Sequence);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.VarChar, Description);
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

        #region Method: dbo_PR_CAL_Calculator_Delete
        public bool? dbo_PR_CAL_Calculator_Delete(int CalculatorID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_Delete");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, CalculatorID);

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

        #endregion

        #endregion

    }

    #region All Entities

    #region Category Entities

    #region Entity: dbo_PR_CAL_Category_SelectAll_Result
    public partial class dbo_PR_CAL_Category_SelectAll_Result : DALHelper
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
        public int CategoryID { get; set; }
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

    #endregion

    #region Calculator Entities

    #region Entity: dbo_PR_CAL_Calculator_SelectAll_Result
    public partial class dbo_PR_CAL_Calculator_SelectAll_Result : DALHelper
    {
        #region Properties
        public int CalculatorID { get; set; }
        public int CategoryID { get; set; }
        public string? CalculatorName { get; set; }
        public string? CalculatorIcon { get; set; }
        public string? URLName { get; set; }
        public string? HeaderName { get; set; }
        public string? SubHeaderName { get; set; }
        public string? CalculatorDescription { get; set; }
        public string? PageSection1 { get; set; }
        public string? PageSection2 { get; set; }
        public string? PageSecton3 { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaKeyword { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaAuthor { get; set; }
        public string? MetaOgTitle { get; set; }
        public string? MetaOgImage { get; set; }
        public string? MetaOgDescription { get; set; }
        public string? MetaOgUrl { get; set; }
        public string? MetaOgType { get; set; }
        public decimal Sequence { get; set; }
        public string? Description { get; set; }
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

    #region Entity: dbo_PR_CAL_Calculator_SelectByPK_Result
    public partial class dbo_PR_CAL_Calculator_SelectByPK_Result : DALHelper
    {
        #region Properties
        public int CalculatorID { get; set; }
        public int CategoryID { get; set; }
        public string? CalculatorName { get; set; }
        public string? CalculatorIcon { get; set; }
        public string? URLName { get; set; }
        public string? HeaderName { get; set; }
        public string? SubHeaderName { get; set; }
        public string? CalculatorDescription { get; set; }
        public string? PageSection1 { get; set; }
        public string? PageSection2 { get; set; }
        public string? PageSecton3 { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaKeyword { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaAuthor { get; set; }
        public string? MetaOgTitle { get; set; }
        public string? MetaOgImage { get; set; }
        public string? MetaOgDescription { get; set; }
        public string? MetaOgUrl { get; set; }
        public string? MetaOgType { get; set; }
        public decimal Sequence { get; set; }
        public string? Description { get; set; }
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

    #endregion

}
