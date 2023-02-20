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
        public List<CAL_CalculatorComboBoxModel> SelectComboBoxUser()
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
        public decimal? Insert(CAL_CalculatorModel objCalculatorModel)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_Insert");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, objCalculatorModel.CategoryID);
                sqlDB.AddInParameter(dbCMD, "CalculatorName", SqlDbType.NVarChar, objCalculatorModel.CalculatorName);
                sqlDB.AddInParameter(dbCMD, "CalculatorIcon", SqlDbType.NVarChar, objCalculatorModel.CalculatorIcon);
                sqlDB.AddInParameter(dbCMD, "URLName", SqlDbType.NVarChar, objCalculatorModel.URLName);
                sqlDB.AddInParameter(dbCMD, "HeaderName", SqlDbType.NVarChar, objCalculatorModel.HeaderName);
                sqlDB.AddInParameter(dbCMD, "SubHeaderName", SqlDbType.NVarChar, objCalculatorModel.SubHeaderName);
                sqlDB.AddInParameter(dbCMD, "CalculatorDescription", SqlDbType.NVarChar, objCalculatorModel.CalculatorDescription);
                sqlDB.AddInParameter(dbCMD, "PageSection1", SqlDbType.NVarChar, objCalculatorModel.PageSection1);
                sqlDB.AddInParameter(dbCMD, "PageSection2", SqlDbType.NVarChar, objCalculatorModel.PageSection2);
                sqlDB.AddInParameter(dbCMD, "PageSection3", SqlDbType.NVarChar, objCalculatorModel.PageSection3);
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.NVarChar, objCalculatorModel.MetaTitle);
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.NVarChar, objCalculatorModel.MetaKeyword);
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.NVarChar, objCalculatorModel.MetaDescription);
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.NVarChar, objCalculatorModel.MetaAuthor);
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.NVarChar, objCalculatorModel.MetaOgTitle);
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.NVarChar, objCalculatorModel.MetaOgImage);
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.NVarChar, objCalculatorModel.MetaOgDescription);
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.NVarChar, objCalculatorModel.MetaOgUrl);
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.NVarChar, objCalculatorModel.MetaOgType);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, objCalculatorModel.Sequence);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objCalculatorModel.Description);
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
        public bool? Update(CAL_CalculatorModel objCalculatorModel)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_Update");
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, objCalculatorModel.CalculatorID);
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, objCalculatorModel.CategoryID);
                sqlDB.AddInParameter(dbCMD, "CalculatorName", SqlDbType.NVarChar, objCalculatorModel.CalculatorName);
                sqlDB.AddInParameter(dbCMD, "CalculatorIcon", SqlDbType.NVarChar, objCalculatorModel.CalculatorIcon);
                sqlDB.AddInParameter(dbCMD, "URLName", SqlDbType.NVarChar, objCalculatorModel.URLName);
                sqlDB.AddInParameter(dbCMD, "HeaderName", SqlDbType.NVarChar, objCalculatorModel.HeaderName);
                sqlDB.AddInParameter(dbCMD, "SubHeaderName", SqlDbType.NVarChar, objCalculatorModel.SubHeaderName);
                sqlDB.AddInParameter(dbCMD, "CalculatorDescription", SqlDbType.NVarChar, objCalculatorModel.CalculatorDescription);
                sqlDB.AddInParameter(dbCMD, "PageSection1", SqlDbType.NVarChar, objCalculatorModel.PageSection1);
                sqlDB.AddInParameter(dbCMD, "PageSection2", SqlDbType.NVarChar, objCalculatorModel.PageSection2);
                sqlDB.AddInParameter(dbCMD, "PageSection3", SqlDbType.NVarChar, objCalculatorModel.PageSection3);
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.NVarChar, objCalculatorModel.MetaTitle);
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.NVarChar, objCalculatorModel.MetaKeyword);
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.NVarChar, objCalculatorModel.MetaDescription);
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.NVarChar, objCalculatorModel.MetaAuthor);
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.NVarChar, objCalculatorModel.MetaOgTitle);
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.NVarChar, objCalculatorModel.MetaOgImage);
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.NVarChar, objCalculatorModel.MetaOgDescription);
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.NVarChar, objCalculatorModel.MetaOgUrl);
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.NVarChar, objCalculatorModel.MetaOgType);
                sqlDB.AddInParameter(dbCMD, "Sequence", SqlDbType.Decimal, objCalculatorModel.Sequence);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objCalculatorModel.Description);
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

        #region Method: SelectByCalculatorIDUserID
        public List<SelectByCategoryIDCalculatorIDUserID_Result> SelectByCategoryIDCalculatorIDUserID(int CategoryID,int CalculatorID, int UserID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_CAL_Calculator_SelectByCategoryIDCalculatorIDUserID");
                sqlDB.AddInParameter(dbCMD, "CategoryID", SqlDbType.Int, CategoryID);
                sqlDB.AddInParameter(dbCMD, "CalculatorID", SqlDbType.Int, CalculatorID);
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.Int, UserID);
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<SelectByCategoryIDCalculatorIDUserID_Result>(dt);
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

    #region Entity: SelectByCategoryIDCalculatorIDUserID_Result
    public partial class SelectByCategoryIDCalculatorIDUserID_Result : DALHelper
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
