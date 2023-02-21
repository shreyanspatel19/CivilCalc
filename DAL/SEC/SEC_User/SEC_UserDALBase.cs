using System;
using System.Data;
using System.Data.Common;
using System.Drawing.Imaging;
using CivilCalc.Areas.SEC_User.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using static CivilCalc.Areas.CAL_Category.Models.CAL_CategoryModel;
using static CivilCalc.Areas.SEC_User.Models.SEC_UserModel;

namespace CivilCalc.DAL.SEC.SEC_User
{
    public abstract class SEC_UserDALBase : DALHelper
    {

        #region SEC_User Methods

        #region Method: SelectUserNamePassword
        public DataTable SelectUserNamePassword(string? Email, string? Password)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_SEC_User_SelectByUserNamePassword");
                sqlDB.AddInParameter(dbCMD, "Email", SqlDbType.NVarChar, Email);
                sqlDB.AddInParameter(dbCMD, "Password", SqlDbType.NVarChar, Password);
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }
                return dt;

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

        #region Method: SEC_UserComboBox
        public List<SEC_UserComboBoxModel> SelectComboBoxUser()
        {

            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbMST = sqlDB.GetStoredProcCommand("dbo.PR_SEC_User_SelectComboBox");
                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbMST))
                {
                    dt.Load(dr);
                }
                List<SEC_UserComboBoxModel> list = new List<SEC_UserComboBoxModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    SEC_UserComboBoxModel vlst = new SEC_UserComboBoxModel();
                    vlst.UserID = Convert.ToInt32(dr["UserID"]);
                    vlst.UserName = dr["UserName"].ToString();
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
        public decimal? Insert(SEC_UserModel obj_SEC_User)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_SEC_User_Insert");
                sqlDB.AddInParameter(dbCMD, "UserName", SqlDbType.NVarChar, obj_SEC_User.UserName);
                sqlDB.AddInParameter(dbCMD, "Password", SqlDbType.NVarChar, obj_SEC_User.Password);
                sqlDB.AddInParameter(dbCMD, "Email", SqlDbType.NVarChar, obj_SEC_User.Email);
                sqlDB.AddInParameter(dbCMD, "MobileNo", SqlDbType.NVarChar, obj_SEC_User.MobileNo);
                sqlDB.AddInParameter(dbCMD, "DisplayName", SqlDbType.NVarChar, obj_SEC_User.DisplayName);
                sqlDB.AddInParameter(dbCMD, "CreatedByUserID", SqlDbType.Int, 1);
                sqlDB.AddInParameter(dbCMD, "IsActive", SqlDbType.Bit, obj_SEC_User.IsActive);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, obj_SEC_User.Description);
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
        public bool? Update(SEC_UserModel obj_SEC_User)        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_SEC_User_Update");
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.NVarChar, obj_SEC_User.UserID);
                sqlDB.AddInParameter(dbCMD, "UserName", SqlDbType.NVarChar, obj_SEC_User.UserName);
                sqlDB.AddInParameter(dbCMD, "Password", SqlDbType.NVarChar, obj_SEC_User.Password);
                sqlDB.AddInParameter(dbCMD, "Email", SqlDbType.NVarChar, obj_SEC_User.Email);
                sqlDB.AddInParameter(dbCMD, "MobileNo", SqlDbType.NVarChar, obj_SEC_User.MobileNo);
                sqlDB.AddInParameter(dbCMD, "DisplayName", SqlDbType.NVarChar, obj_SEC_User.DisplayName);
                sqlDB.AddInParameter(dbCMD, "CreatedByUserID", SqlDbType.Int, obj_SEC_User.CreatedByUserID);
                sqlDB.AddInParameter(dbCMD, "IsActive", SqlDbType.Bit, obj_SEC_User.IsActive);
                sqlDB.AddInParameter(dbCMD, "Description", SqlDbType.NVarChar, obj_SEC_User.Description);

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

        #region Method: SelectForSearch
        public List<SelectForSearch_Result> SelectForSearch(int UserID)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(myConnectionString);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_SEC_User_SelectForSearch");
                sqlDB.AddInParameter(dbCMD, "UserID", SqlDbType.VarChar, UserID);
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

    #region Entity: dbo_PR_SEC_User_SelectUserNamePassword_Result
    public partial class SelectUserNamePassword_Result : DALHelper
    {
        #region Properties
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
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
    public partial class SelectForSearch_Result : DALHelper
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
