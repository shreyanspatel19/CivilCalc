using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;

namespace CivilCalc.DAL
{
    public class CAL_DALBase
    {
        #region [dbo].[PR_CAL_Catagory_SelectAll]
        public DataTable dbo_PR_CAL_Catagory_SelectAll(String Connection)
        {
            try
            {
                SqlDatabase SQLDB =new SqlDatabase(Connection);
                DbCommand dbCommand = SQLDB.GetStoredProcCommand("PR_CAL_Catagory_SelectAll");
                DataTable dt = new DataTable(); 
                using(IDataReader dr = SQLDB.ExecuteReader(dbCommand))
                {
                    dt.Load(dr);
                }
                return dt;
            }
            catch(Exception ex)
            {
                return null;

            }
        }
        #endregion
    }
}
