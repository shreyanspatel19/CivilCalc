using System;
using System.Data;
using System.Data.Common;
using System.Drawing.Imaging;
using CivilCalc.Areas.CAL_Calculator.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using static CivilCalc.Areas.CAL_Calculator.Models.CAL_CalculatorModel;
using static CivilCalc.Areas.CAL_Category.Models.CAL_CategoryModel;

namespace CivilCalc.DAL.CAL.CAL_Calculator
{
    public abstract class CAL_CalculatorDALBase : DALHelper
    {
        #region Calculator Methods
        #region Method: CalculatorComboBox
        public List<CAL_CalculatorComboBoxModel> SelectComboBoxCalculator()
        {

            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbMST = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_SelectComboBox");
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbMST))
                {
                    dt.Load(dr);
                }
                List<CAL_CalculatorComboBoxModel> list = new List<CAL_CalculatorComboBoxModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    CAL_CalculatorComboBoxModel vlst = new CAL_CalculatorComboBoxModel();
                    vlst.CalculatorID = Convert.ToInt32(dr["CalculatorID"]);
                    vlst.CalculatorName = dr["CalculatorName"].ToString();
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
        public decimal? Insert(CAL_CalculatorModel obj_CAL_Calculator)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_Insert");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, obj_CAL_Calculator.CategoryID);
                sqlDB.AddInParameter(dbCMD, "CalculatorName", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.CalculatorName) ? null : obj_CAL_Calculator.CalculatorName.Trim());
                sqlDB.AddInParameter(dbCMD, "CalculatorIcon", SqlDbType.NVarChar, obj_CAL_Calculator.CalculatorIcon);
                sqlDB.AddInParameter(dbCMD, "URLName", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.URLName) ? null : obj_CAL_Calculator.URLName.Trim());
                sqlDB.AddInParameter(dbCMD, "HeaderName", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.HeaderName) ? null : obj_CAL_Calculator.HeaderName.Trim());
                sqlDB.AddInParameter(dbCMD, "SubHeaderName", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.SubHeaderName) ? null : obj_CAL_Calculator.SubHeaderName.Trim());
                sqlDB.AddInParameter(dbCMD, "CalculatorDescription", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.CalculatorDescription) ? null : obj_CAL_Calculator.CalculatorDescription.Trim());
                sqlDB.AddInParameter(dbCMD, "PageSection1", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.PageSection1) ? null : obj_CAL_Calculator.PageSection1.Trim());
                sqlDB.AddInParameter(dbCMD, "PageSection2", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.PageSection2) ? null : obj_CAL_Calculator.PageSection2.Trim());
                sqlDB.AddInParameter(dbCMD, "PageSection3", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.PageSection3) ? null : obj_CAL_Calculator.PageSection3.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaTitle) ? null : obj_CAL_Calculator.MetaTitle.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaKeyword) ? null : obj_CAL_Calculator.MetaKeyword.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaDescription) ? null : obj_CAL_Calculator.MetaDescription.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaAuthor) ? null : obj_CAL_Calculator.MetaAuthor.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaOgTitle) ? null : obj_CAL_Calculator.MetaOgTitle.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaOgImage) ? null : obj_CAL_Calculator.MetaOgImage.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaOgDescription) ? null : obj_CAL_Calculator.MetaOgDescription.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaOgUrl) ? null : obj_CAL_Calculator.MetaOgUrl.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaOgType) ? null : obj_CAL_Calculator.MetaOgType.Trim());
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal,obj_CAL_Calculator.Sequence);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.Description) ? null : obj_CAL_Calculator.Description.Trim());
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
        public bool? Update(CAL_CalculatorModel obj_CAL_Calculator)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_Update");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, obj_CAL_Calculator.CalculatorID);
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, obj_CAL_Calculator.CategoryID);
                sqlDB.AddInParameter(dbCMD, "CalculatorName", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.CalculatorName) ? null : obj_CAL_Calculator.CalculatorName.Trim());
                sqlDB.AddInParameter(dbCMD, "CalculatorIcon", SqlDbType.NVarChar, obj_CAL_Calculator.CalculatorIcon);
                sqlDB.AddInParameter(dbCMD, "URLName", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.URLName) ? null : obj_CAL_Calculator.URLName.Trim());
                sqlDB.AddInParameter(dbCMD, "HeaderName", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.HeaderName) ? null : obj_CAL_Calculator.HeaderName.Trim());
                sqlDB.AddInParameter(dbCMD, "SubHeaderName", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.SubHeaderName) ? null : obj_CAL_Calculator.SubHeaderName.Trim());
                sqlDB.AddInParameter(dbCMD, "CalculatorDescription", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.CalculatorDescription) ? null : obj_CAL_Calculator.CalculatorDescription.Trim());
                sqlDB.AddInParameter(dbCMD, "PageSection1", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.PageSection1) ? null : obj_CAL_Calculator.PageSection1.Trim());
                sqlDB.AddInParameter(dbCMD, "PageSection2", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.PageSection2) ? null : obj_CAL_Calculator.PageSection2.Trim());
                sqlDB.AddInParameter(dbCMD, "PageSection3", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.PageSection3) ? null : obj_CAL_Calculator.PageSection3.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaTitle) ? null : obj_CAL_Calculator.MetaTitle.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaKeyword) ? null : obj_CAL_Calculator.MetaKeyword.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaDescription) ? null : obj_CAL_Calculator.MetaDescription.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaAuthor) ? null : obj_CAL_Calculator.MetaAuthor.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaOgTitle) ? null : obj_CAL_Calculator.MetaOgTitle.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaOgImage) ? null : obj_CAL_Calculator.MetaOgImage.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaOgDescription) ? null : obj_CAL_Calculator.MetaOgDescription.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaOgUrl) ? null : obj_CAL_Calculator.MetaOgUrl.Trim());
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.MetaOgType) ? null : obj_CAL_Calculator.MetaOgType.Trim());
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, obj_CAL_Calculator.Sequence);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(obj_CAL_Calculator.Description) ? null : obj_CAL_Calculator.Description.Trim());
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

        #region Method: SelectForSearch_Result
        public List<SelectForSearch_Result> SelectForSearch(int CategoryID,string? F_CalculatorName)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_SelectForSearch");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, CategoryID);
                sqlDB.AddInParameter(dbCMD, "CalculatorName", SqlDbType.NVarChar, F_CalculatorName);
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

        #region Method: SelectByURLName
        public List<SelectForSearch_Result> SelectByURLName(string? URLName)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_SelectURLName");
                sqlDB.AddInParameter(dbCMD, "URLName", SqlDbType.NVarChar, URLName);
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
        public string? PageSection3 { get; set; }
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
        public string? PageSection3 { get; set; }
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

    #region Entity: SelectForSearch_Result
    public partial class SelectForSearch_Result : DALHelper
    {
        #region Properties
        public int CalculatorID { get; set; }
        public int CategoryID { get; set; }

        public string? CategoryName { get; set; }
        public string? CalculatorName { get; set; }
        public string? CalculatorIcon { get; set; }
        public string? URLName { get; set; }
        public string? HeaderName { get; set; }
        public string? SubHeaderName { get; set; }
        public string? CalculatorDescription { get; set; }
        public string? PageSection1 { get; set; }
        public string? PageSection2 { get; set; }
        public string? PageSection3 { get; set; }
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
