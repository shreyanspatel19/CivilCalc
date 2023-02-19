using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using CivilCalc.Areas.MST_Configuration.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace CivilCalc.DAL.MST.MST_Configuration
{
    public abstract class MST_ConfigurationDALBase : DALHelper
    {

        #region MST_Configuration Methods

        #region Method: SelectAll
        public List<SelectAll_Result> SelectAll()
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_MST_Configuration_SelectAll");

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
        public List<SelectPK_Result> SelectPK(int? ConfigurationID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_MST_Configuration_SelectPK");
                sqlDB.AddInParameter(dbCMD, "ConfigurationID", SqlDbType.Int, ConfigurationID);

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
        public decimal? Insert(MST_ConfigurationModel objConfigurationModel)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_MST_Configuration_Insert");
                sqlDB.AddInParameter(dbCMD, "WebsiteLogoPath", SqlDbType.NVarChar, objConfigurationModel.WebsiteLogoPath);
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.NVarChar, objConfigurationModel.MetaTitle);
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.NVarChar, objConfigurationModel.MetaKeyword);
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.NVarChar, objConfigurationModel.MetaDescription);
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.NVarChar, objConfigurationModel.MetaAuthor);
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.NVarChar, objConfigurationModel.MetaOgTitle);
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.NVarChar, objConfigurationModel.MetaOgImage);
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.NVarChar, objConfigurationModel.MetaOgDescription);
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.NVarChar, objConfigurationModel.MetaOgUrl);
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.NVarChar, objConfigurationModel.MetaOgType);
                sqlDB.AddInParameter(dbCMD, "AboutWebsite", SqlDbType.NVarChar, objConfigurationModel.AboutWebsite);
                sqlDB.AddInParameter(dbCMD, "MobileAppDetail", SqlDbType.NVarChar, objConfigurationModel.MobileAppDetail);
                sqlDB.AddInParameter(dbCMD, "PlayStoreURL", SqlDbType.NVarChar, objConfigurationModel.PlayStoreURL);
                sqlDB.AddInParameter(dbCMD, "AppStoreURL", SqlDbType.NVarChar, objConfigurationModel.AppStoreURL);
                sqlDB.AddInParameter(dbCMD, "FooterHTML", SqlDbType.NVarChar, objConfigurationModel.FooterHTML);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objConfigurationModel.Description);
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
        public bool? Update(MST_ConfigurationModel objConfigurationModel)        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_MST_Configuration_Update");
                sqlDB.AddInParameter(dbCMD, "ConfigurationID", SqlDbType.Int, objConfigurationModel.ConfigurationID);
                sqlDB.AddInParameter(dbCMD, "WebsiteLogoPath", SqlDbType.NVarChar, objConfigurationModel.WebsiteLogoPath);
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.NVarChar, objConfigurationModel.MetaTitle);
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.NVarChar, objConfigurationModel.MetaKeyword);
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.NVarChar, objConfigurationModel.MetaDescription);
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.NVarChar, objConfigurationModel.MetaAuthor);
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.NVarChar, objConfigurationModel.MetaOgTitle);
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.NVarChar, objConfigurationModel.MetaOgImage);
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.NVarChar, objConfigurationModel.MetaOgDescription);
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.NVarChar, objConfigurationModel.MetaOgUrl);
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.NVarChar, objConfigurationModel.MetaOgType);
                sqlDB.AddInParameter(dbCMD, "AboutWebsite", SqlDbType.NVarChar, objConfigurationModel.AboutWebsite);
                sqlDB.AddInParameter(dbCMD, "MobileAppDetail", SqlDbType.NVarChar, objConfigurationModel.MobileAppDetail);
                sqlDB.AddInParameter(dbCMD, "PlayStoreURL", SqlDbType.NVarChar, objConfigurationModel.PlayStoreURL);
                sqlDB.AddInParameter(dbCMD, "AppStoreURL", SqlDbType.NVarChar, objConfigurationModel.AppStoreURL);
                sqlDB.AddInParameter(dbCMD, "FooterHTML", SqlDbType.NVarChar, objConfigurationModel.FooterHTML);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objConfigurationModel.Description);
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.Int, objConfigurationModel.UserID);

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
        public bool? Delete(int? ConfigurationID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_MST_Configuration_Delete");
                sqlDB.AddInParameter(dbCMD, "ConfigurationID", SqlDbType.Int, ConfigurationID);

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


        #endregion       

    }



    #region Category Entities

    #region Entity: dbo_PR_MST_Configuratio_SelectAll_Result
    public partial class SelectAll_Result : DALHelper
    {
        #region Properties
        public int ConfigurationID { get; set; }

        public string? WebsiteLogoPath { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKeyword { get; set; }

        public string? MetaDescription { get; set; }

        public string? MetaAuthor { get; set; }

        public string? MetaOgTitle { get; set; }

        public string? MetaOgImage { get; set; }

        public string? MetaOgDescription { get; set; }

        public string? MetaOgUrl { get; set; }

        public string? MetaOgType { get; set; }

        public string? AboutWebsite { get; set; }

        public string? MobileAppDetail { get; set; }

        public string? PlayStoreURL { get; set; }

        public string? AppStoreURL { get; set; }

        public string? FooterHTML { get; set; }

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

    #region Entity: dbo_PR_MST_Configuratio_SelectByPK_Result
    public partial class SelectPK_Result : DALHelper
    {
        #region Properties
        public int ConfigurationID { get; set; }

        public string? WebsiteLogoPath { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKeyword { get; set; }

        public string? MetaDescription { get; set; }

        public string? MetaAuthor { get; set; }

        public string? MetaOgTitle { get; set; }

        public string? MetaOgImage { get; set; }

        public string? MetaOgDescription { get; set; }

        public string? MetaOgUrl { get; set; }

        public string? MetaOgType { get; set; }

        public string? AboutWebsite { get; set; }

        public string? MobileAppDetail { get; set; }

        public string? PlayStoreURL { get; set; }

        public string? AppStoreURL { get; set; }

        public string? FooterHTML { get; set; }

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


    #endregion

    
   

}
