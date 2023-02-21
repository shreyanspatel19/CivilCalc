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
        public decimal? Insert(MST_ConfigurationModel obj_MST_Configuration)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_MST_Configuration_Insert");
                sqlDB.AddInParameter(dbCMD, "WebsiteLogoPath", SqlDbType.NVarChar, obj_MST_Configuration.WebsiteLogoPath);
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.NVarChar, obj_MST_Configuration.MetaTitle);
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.NVarChar, obj_MST_Configuration.MetaKeyword);
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.NVarChar, obj_MST_Configuration.MetaDescription);
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.NVarChar, obj_MST_Configuration.MetaAuthor);
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.NVarChar, obj_MST_Configuration.MetaOgTitle);
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.NVarChar, obj_MST_Configuration.MetaOgImage);
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.NVarChar, obj_MST_Configuration.MetaOgDescription);
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.NVarChar, obj_MST_Configuration.MetaOgUrl);
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.NVarChar, obj_MST_Configuration.MetaOgType);
                sqlDB.AddInParameter(dbCMD, "AboutWebsite", SqlDbType.NVarChar, obj_MST_Configuration.AboutWebsite);
                sqlDB.AddInParameter(dbCMD, "MobileAppDetail", SqlDbType.NVarChar, obj_MST_Configuration.MobileAppDetail);
                sqlDB.AddInParameter(dbCMD, "PlayStoreURL", SqlDbType.NVarChar, obj_MST_Configuration.PlayStoreURL);
                sqlDB.AddInParameter(dbCMD, "AppStoreURL", SqlDbType.NVarChar, obj_MST_Configuration.AppStoreURL);
                sqlDB.AddInParameter(dbCMD, "FooterHTML", SqlDbType.NVarChar, obj_MST_Configuration.FooterHTML);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, obj_MST_Configuration.Description);
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
        public bool? Update(MST_ConfigurationModel obj_MST_Configuration)        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_MST_Configuration_Update");
                sqlDB.AddInParameter(dbCMD, "ConfigurationID", SqlDbType.Int, obj_MST_Configuration.ConfigurationID);
                sqlDB.AddInParameter(dbCMD, "WebsiteLogoPath", SqlDbType.NVarChar, obj_MST_Configuration.WebsiteLogoPath);
                sqlDB.AddInParameter(dbCMD, "MetaTitle", SqlDbType.NVarChar, obj_MST_Configuration.MetaTitle);
                sqlDB.AddInParameter(dbCMD, "MetaKeyword", SqlDbType.NVarChar, obj_MST_Configuration.MetaKeyword);
                sqlDB.AddInParameter(dbCMD, "MetaDescription", SqlDbType.NVarChar, obj_MST_Configuration.MetaDescription);
                sqlDB.AddInParameter(dbCMD, "MetaAuthor", SqlDbType.NVarChar, obj_MST_Configuration.MetaAuthor);
                sqlDB.AddInParameter(dbCMD, "MetaOgTitle", SqlDbType.NVarChar, obj_MST_Configuration.MetaOgTitle);
                sqlDB.AddInParameter(dbCMD, "MetaOgImage", SqlDbType.NVarChar, obj_MST_Configuration.MetaOgImage);
                sqlDB.AddInParameter(dbCMD, "MetaOgDescription", SqlDbType.NVarChar, obj_MST_Configuration.MetaOgDescription);
                sqlDB.AddInParameter(dbCMD, "MetaOgUrl", SqlDbType.NVarChar, obj_MST_Configuration.MetaOgUrl);
                sqlDB.AddInParameter(dbCMD, "MetaOgType", SqlDbType.NVarChar, obj_MST_Configuration.MetaOgType);
                sqlDB.AddInParameter(dbCMD, "AboutWebsite", SqlDbType.NVarChar, obj_MST_Configuration.AboutWebsite);
                sqlDB.AddInParameter(dbCMD, "MobileAppDetail", SqlDbType.NVarChar, obj_MST_Configuration.MobileAppDetail);
                sqlDB.AddInParameter(dbCMD, "PlayStoreURL", SqlDbType.NVarChar, obj_MST_Configuration.PlayStoreURL);
                sqlDB.AddInParameter(dbCMD, "AppStoreURL", SqlDbType.NVarChar, obj_MST_Configuration.AppStoreURL);
                sqlDB.AddInParameter(dbCMD, "FooterHTML", SqlDbType.NVarChar, obj_MST_Configuration.FooterHTML);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, obj_MST_Configuration.Description);
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
