using CivilCalc.DAL.CAL.CAL_Category;
using CivilCalc.DAL.CAL.CAL_Calculator;
using CivilCalc.DAL.CAL.CAL_NewCalculator;
using CivilCalc.DAL.CAL.CAL_TopCalculator;

namespace CivilCalc.DAL
{
    public class DBConfig
    {
        public static SEC_UserDAL dbCALCategory = new SEC_UserDAL();
        public static CAL_CalculatorDAL dbCALCalculator = new CAL_CalculatorDAL();
        public static CAL_NewCalculatorDAL dbCALNewCalculator = new CAL_NewCalculatorDAL();
        public static CAL_TopCalculatorDAL dbCALTopCalculator = new CAL_TopCalculatorDAL();
    }
}
