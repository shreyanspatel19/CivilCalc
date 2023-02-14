using System;
using System.Data;
using System.Data.Common;
using System.Drawing.Imaging;
using CivilCalc.Areas.CAL_Calculator.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace CivilCalc.DAL.CAL.CAL_Calculator
{
    public abstract class CAL_CalculatorDALBase : DALHelper
    {
        #region Calculator Methods

        #region Method: SelectAll
        public List<SelectAll_Result> SelectAll()
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
        public List<SelectPK_Result> SelectPK(int? CalculatorID)
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
        public decimal? Insert(CAL_CalculatorModel objCalculatorModel)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_Insert");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, objCalculatorModel.CategoryID);
                sqlDB.AddInParameter(dbCMD, "CalculatorName", SqlDbType.VarChar, objCalculatorModel.CalculatorName);
                sqlDB.AddInParameter(dbCMD, "CalculatorIcon", SqlDbType.VarChar, objCalculatorModel.CalculatorIcon);
                sqlDB.AddInParameter(dbCMD, "URLName", SqlDbType.VarChar, objCalculatorModel.URLName);
                sqlDB.AddInParameter(dbCMD, "HeaderName", SqlDbType.VarChar, objCalculatorModel.HeaderName);
                sqlDB.AddInParameter(dbCMD, "SubHeaderName", SqlDbType.VarChar, objCalculatorModel.SubHeaderName);
                sqlDB.AddInParameter(dbCMD, "CalculatorDescription", SqlDbType.VarChar, objCalculatorModel.CalculatorDescription);
                sqlDB.AddInParameter(dbCMD, "PageSection1", SqlDbType.VarChar, objCalculatorModel.PageSection1);
                sqlDB.AddInParameter(dbCMD, "PageSection2", SqlDbType.VarChar, objCalculatorModel.PageSection2);
                sqlDB.AddInParameter(dbCMD, "PageSecton3", SqlDbType.VarChar, objCalculatorModel.PageSecton3);
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.VarChar, objCalculatorModel.MetaTitle);
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.VarChar, objCalculatorModel.MetaKeyword);
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.VarChar, objCalculatorModel.MetaDescription);
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.VarChar, objCalculatorModel.MetaAuthor);
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.VarChar, objCalculatorModel.MetaOgTitle);
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.VarChar, objCalculatorModel.MetaOgImage);
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.VarChar, objCalculatorModel.MetaOgDescription);
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.VarChar, objCalculatorModel.MetaOgUrl);
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.VarChar, objCalculatorModel.MetaOgType);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, objCalculatorModel.Sequence);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.VarChar, objCalculatorModel.Description);
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.Int, objCalculatorModel.UserID);
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
        public bool? Update(CAL_CalculatorModel objCalculatorModel)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Category_Update");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, objCalculatorModel.CalculatorID);
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, objCalculatorModel.CategoryID);
                sqlDB.AddInParameter(dbCMD, "CalculatorName", SqlDbType.VarChar, objCalculatorModel.CalculatorName);
                sqlDB.AddInParameter(dbCMD, "CalculatorIcon", SqlDbType.VarChar, objCalculatorModel.CalculatorIcon);
                sqlDB.AddInParameter(dbCMD, "URLName", SqlDbType.VarChar, objCalculatorModel.URLName);
                sqlDB.AddInParameter(dbCMD, "HeaderName", SqlDbType.VarChar, objCalculatorModel.HeaderName);
                sqlDB.AddInParameter(dbCMD, "SubHeaderName", SqlDbType.VarChar, objCalculatorModel.SubHeaderName);
                sqlDB.AddInParameter(dbCMD, "CalculatorDescription", SqlDbType.VarChar, objCalculatorModel.CalculatorDescription);
                sqlDB.AddInParameter(dbCMD, "PageSection1", SqlDbType.VarChar, objCalculatorModel.PageSection1);
                sqlDB.AddInParameter(dbCMD, "PageSection2", SqlDbType.VarChar, objCalculatorModel.PageSection2);
                sqlDB.AddInParameter(dbCMD, "PageSecton3", SqlDbType.VarChar, objCalculatorModel.PageSecton3);
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.VarChar, objCalculatorModel.MetaTitle);
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.VarChar, objCalculatorModel.MetaKeyword);
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.VarChar, objCalculatorModel.MetaDescription);
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.VarChar, objCalculatorModel.MetaAuthor);
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.VarChar, objCalculatorModel.MetaOgTitle);
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.VarChar, objCalculatorModel.MetaOgImage);
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.VarChar, objCalculatorModel.MetaOgDescription);
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.VarChar, objCalculatorModel.MetaOgUrl);
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.VarChar, objCalculatorModel.MetaOgType);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, objCalculatorModel.Sequence);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.VarChar, objCalculatorModel.Description);
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.Int, objCalculatorModel.UserID);

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

        #region Method: SelectByCalculatorNameUserName
        public List<SelectByCalculatorNameUserName_Result> SelectByCalculatorNameUserName(string? F_CalculatorName, string? F_UserName)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_SelectByCalculatorNameUserName");
                sqlDB.AddInParameter(dbCMD, "CalculatorName", SqlDbType.VarChar, F_CalculatorName);
                sqlDB.AddInParameter(dbCMD, "UserName", SqlDbType.VarChar, F_UserName);
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<SelectByCalculatorNameUserName_Result>(dt);
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
        public override string ToString()
        {
            return EntityToString(this);
        }
        #endregion
    }
    #endregion

    #region Entity: dbo_PR_CAL_Calculator_SelectForSearch_Result
    public partial class SelectByCalculatorNameUserName_Result : DALHelper
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
        public override string ToString()
        {
            return EntityToString(this);
        }
        #endregion
    }
    #endregion

    #endregion

}
