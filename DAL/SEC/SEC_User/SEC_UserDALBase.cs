using System;
using System.Data;
using System.Data.Common;
using System.Drawing.Imaging;
using CivilCalc.Areas.SEC_User.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace CivilCalc.DAL.SEC.SEC_User
{
    public abstract class SEC_UserDALBase : DALHelper
    {

        #region SEC_User Methods

        #region Method: SelectAll
        public List<SelectAll_Result> SelectAll()
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_SEC_User_SelectAll");

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
        public List<SelectPK_Result> SelectPK(int? UserID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_SEC_User_SelectPK");
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.Int, UserID);

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
        public decimal? Insert(SEC_UserModel objUserModel)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_SEC_User_Insert");
                sqlDB.AddInParameter(dbCMD, "UserName", SqlDbType.NVarChar, objUserModel.UserName);
                sqlDB.AddInParameter(dbCMD, "Password", SqlDbType.NVarChar, objUserModel.Password);
                sqlDB.AddInParameter(dbCMD, "Email", SqlDbType.NVarChar, objUserModel.Email);
                sqlDB.AddInParameter(dbCMD, "MobileNo", SqlDbType.NVarChar, objUserModel.MobileNo);
                sqlDB.AddInParameter(dbCMD, "DisplayName", SqlDbType.NVarChar, objUserModel.DisplayName);
                sqlDB.AddInParameter(dbCMD, "CreatedByUserID", SqlDbType.Int, 1);
                sqlDB.AddInParameter(dbCMD, "IsActive", SqlDbType.Bit, objUserModel.IsActive);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objUserModel.Description);
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
        public bool? Update(SEC_UserModel objUserModel)        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_SEC_User_Update");
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.NVarChar, objUserModel.UserID);
                sqlDB.AddInParameter(dbCMD, "UserName", SqlDbType.NVarChar, objUserModel.UserName);
                sqlDB.AddInParameter(dbCMD, "Password", SqlDbType.NVarChar, objUserModel.Password);
                sqlDB.AddInParameter(dbCMD, "Email", SqlDbType.NVarChar, objUserModel.Email);
                sqlDB.AddInParameter(dbCMD, "MobileNo", SqlDbType.NVarChar, objUserModel.MobileNo);
                sqlDB.AddInParameter(dbCMD, "DisplayName", SqlDbType.NVarChar, objUserModel.DisplayName);
                sqlDB.AddInParameter(dbCMD, "CreatedByUserID", SqlDbType.Int, objUserModel.CreatedByUserID);
                sqlDB.AddInParameter(dbCMD, "IsActive", SqlDbType.Bit, objUserModel.IsActive);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, objUserModel.Description);

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
        public bool? Delete(int? UserID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_SEC_User_Delete");
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.Int, UserID);

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

        #region Method: SelectByUserName
        public List<SelectByUserName_Result> SelectByUserName(string? F_UserName)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_SEC_User_SelectByUserName");
                sqlDB.AddInParameter(dbCMD, "UserName", SqlDbType.VarChar, F_UserName);
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return ConvertDataTableToEntity<SelectByUserName_Result>(dt);
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



    #region SEC_User Entities

    #region Entity: dbo_PR_SEC_User_SelectAll_Result
    public partial class SelectAll_Result : DALHelper
    {
        #region Properties
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? DisplayName { get; set; }
        public int CreatedByUserID { get; set; }

        public string? Description { get; set; }
        public Boolean IsActive { get; set; }

        #endregion

        #region Convert Entity to String
        public override string ToString()
        {
            return EntityToString(this);
        }
        #endregion
    }
    #endregion

    #region Entity: dbo_PR_SEC_User_SelectByPK_Result
    public partial class SelectPK_Result : DALHelper
    {
        #region Properties
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? DisplayName { get; set; }
        public int CreatedByUserID { get; set; }

        public string? Description { get; set; }
        public Boolean IsActive { get; set; }
        #endregion

        #region Convert Entity to String
        public override string ToString()
        {
            return EntityToString(this);
        }
        #endregion
    }
    #endregion

    #region Entity: dbo_PR_SEC_User_SelectForSearch_Result
    public partial class SelectByUserName_Result : DALHelper
    {
        #region Properties
        public int UserID { get; set; }

        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? DisplayName { get; set; }
        public int CreatedByUserID { get; set; }

        public string? Description { get; set; }
        public Boolean IsActive { get; set; }

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
