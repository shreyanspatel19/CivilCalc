using CivilCalc.DAL.CAL.CAL_Category;
using CivilCalc.DAL.CAL.CAL_Calculator;
using CivilCalc.DAL.CAL.CAL_NewCalculator;
using CivilCalc.DAL.CAL.CAL_TopCalculator;
using CivilCalc.DAL.SEC.SEC_User;
using CivilCalc.Areas.MST_Configuration.Models;
using CivilCalc.DAL.MST.MST_Configuration;
using CivilCalc.DAL.LOG.LOG_Calculation;

namespace CivilCalc.DAL
{
    public class DBConfig
    {
        public static CAL_CategoryDAL       dbCALCategory = new CAL_CategoryDAL();
        public static SEC_UserDAL           dbSECUser = new SEC_UserDAL();
        public static MST_ConfigurationDAL  dbMSTConfiguration = new MST_ConfigurationDAL();
        public static CAL_CalculatorDAL     dbCALCalculator = new CAL_CalculatorDAL();
        public static CAL_NewCalculatorDAL  dbCALNewCalculator = new CAL_NewCalculatorDAL();
        public static CAL_TopCalculatorDAL  dbCALTopCalculator = new CAL_TopCalculatorDAL();
        public static LOG_CalculationDAL dbLOGCalculation = new LOG_CalculationDAL();
    }
}
