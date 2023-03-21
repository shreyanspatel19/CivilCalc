using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.IO;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;
using System.Data.SqlTypes;
using System.Configuration;
using System.Data.OleDb;
using System.Drawing.Printing;
using System.Security.Cryptography;

namespace CivilEngineeringCalculators
{
    public class CommonFunctions
    {
        #region Constructor
        public CommonFunctions()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion Constructor


        #region Security
        public static string GetClientIP()
        {
            string ip = "";
            string strHostName = "";
            strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            //ip = addr[2].ToString();
            return ip;
        }

        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        public static string GetOS(String UserAgent)
        {
            String OsName = UserAgent;
            if (UserAgent.IndexOf("Windows NT 6.3") > 0)
                OsName = "Windows 8.1";
            else if (UserAgent.IndexOf("Windows NT 6.2") > 0)
                OsName = "Windows 8";
            else if (UserAgent.IndexOf("Windows NT 6.1") > 0)
                OsName = "Windows 7";
            else if (UserAgent.IndexOf("Windows NT 6.0") > 0)
                OsName = "Windows Vista";
            else if (UserAgent.IndexOf("Windows NT 5.2") > 0)
                OsName = "Windows Server 2003; Windows XP x64 Edition";
            else if (UserAgent.IndexOf("Windows NT 5.1") > 0)
                OsName = "Windows XP";
            else if (UserAgent.IndexOf("Windows NT 5.01") > 0)
                OsName = "Windows 2000, Service Pack 1 (SP1)";
            else if (UserAgent.IndexOf("Windows NT 5.0") > 0)
                OsName = "Windows 2000";
            else if (UserAgent.IndexOf("Windows NT 4.0") > 0)
                OsName = "Microsoft Windows NT 4.0";
            else if (UserAgent.IndexOf("Win 9x 4.90") > 0)
                OsName = "Windows Millennium Edition (Windows Me)";
            else if (UserAgent.IndexOf("Windows 98") > 0)
                OsName = "Windows 98";
            else if (UserAgent.IndexOf("Windows 95") > 0)
                OsName = "Windows 95";
            else if (UserAgent.IndexOf("Windows CE") > 0)
                OsName = "Windows CE";

            return OsName;
        }


        public static Boolean IsLocked(SqlString LockLevel)
        {
            if (LockLevel.Value != String.Empty)
                return true;
            else
                return false;
        }


        #endregion Security

        #region Common
        public static void ImageResize(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        public static void ImageResizeHeightWidth(int Height, int Width, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = Width;
                var newHeight = Height;
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        public static string GetInRuppe(String Amount)
        {
            String Amt = "";
            if (Amount != String.Empty)
            {
                decimal parsed = decimal.Parse(Amount, CultureInfo.InvariantCulture);
                CultureInfo hindi = new CultureInfo("hi-IN");
                Amt = string.Format(hindi, "{0:c}", parsed);
            }
            return Amt;
        }
        
        public static string ToOrdinal(Int32 i, SqlString word)
        {
            string suffix = "<sup>th</sup>";
            string final = "";
            if (word != "")
            {
                switch (i % 100)
                {
                    case 11:
                    case 12:
                    case 13:
                        break;
                    default:
                        switch (i % 10)
                        {
                            case 1:
                                suffix = "<sup>st</sup>";
                                break;
                            case 2:
                                suffix = "<sup>nd</sup>";
                                break;
                            case 3:
                                suffix = "<sup>rd</sup>";
                                break;
                        }
                        break;
                }
                final = i.ToString() + suffix + " " + word.ToString();
            }
            return final;
        }
                        
        public static Int32 HourFormatsToMinutes(String HourFormat)
        {
            //HourFormat is as per dtpTimeSelector of the theme: eg. 12:20 AM
            return Convert.ToInt32(DateTime.Parse(HourFormat).Hour) * 60 + Convert.ToInt32(DateTime.Parse(HourFormat).Minute);
        }

        public static string GetReportFileName(string FileName)
        {
            FileName = FileName.Replace(".", "_");
            FileName = FileName.Replace(",", "_");
            FileName = FileName.Replace("*", "_");
            FileName = FileName.Replace("\"", "_");
            FileName = FileName.Replace("\\", "_");
            FileName = FileName.Replace("/", "_");
            FileName = FileName.Replace("[", "_");
            FileName = FileName.Replace("]", "_");
            FileName = FileName.Replace(":", "_");
            FileName = FileName.Replace(";", "_");
            FileName = FileName.Replace("|", "_");
            FileName = FileName.Replace("=", "_");
            FileName = FileName.Replace("?", "_");
            FileName = FileName.Replace("<", "_");
            FileName = FileName.Replace(">", "_");
            FileName = FileName.Replace(" ", "_");
            FileName = FileName.Replace("-", "_");
            FileName = FileName.Replace("__", "_");
            FileName = FileName.Replace("__", "_");
            FileName = FileName.Replace("__", "_");

            return FileName;
        }

        public static string ConvertDatatableToXML_New(DataTable dt)
        {
            String xmlData = String.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                xmlData += "<" + dt.TableName.ToString() + ">\r\n";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType.Name == "DateTime" && dr[dc.ColumnName.ToString()].ToString().Trim() != String.Empty)
                        xmlData += "<" + dc.ColumnName.ToString() + ">" + Convert.ToDateTime(dr[dc.ColumnName.ToString()].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "</" + dc.ColumnName.ToString() + ">\r\n";
                    else if (dr[dc.ColumnName.ToString()].ToString() != String.Empty)
                        xmlData += "<" + dc.ColumnName.ToString() + ">" + dr[dc.ColumnName.ToString()].ToString() + "</" + dc.ColumnName.ToString() + ">\r\n";
                    else
                        xmlData += "<" + dc.ColumnName.ToString() + "></" + dc.ColumnName.ToString() + ">\r\n";
                }
                xmlData += "</" + dt.TableName.ToString() + ">\r\n";
            }
            return xmlData;
        }

        #endregion Common

        #region Create Matrix

        public static DataTable DataTableToDataMatrix(DataTable dt, String PrimaryKeyColumns, String TableColumns, String TableColumnsTitle, String MatrixColumns, String MatrixColumnsTitle, String CellColumn, String OrderBy)
        {
            DataTable dtMatrix = new DataTable();

            String[] TableColumnsArray = TableColumns.Split(new char[] { ',' });
            String[] TableColumnsTitleArray = TableColumnsTitle.Split(new char[] { ',' });
            String[] MatrixColumnsArray = MatrixColumns.Split(new char[] { ',' });
            String[] MatrixColumnsTitleArray = MatrixColumnsTitle.Split(new char[] { ',' });
            String[] OrderByArray = OrderBy.Split(new char[] { ',' });

            #region Step 1 : Add Table Columns to dtMatrix
            for (Int32 i = 0; i < TableColumnsArray.Length; i++)
            {
                DataColumn dc = new DataColumn(TableColumnsArray[i], dt.Columns[TableColumnsArray[i]].DataType);
                dc.Caption = TableColumnsTitleArray[i];
                dtMatrix.Columns.Add(dc);
            }
            #endregion Step 1 : Add Table Columns to dtMatrix

            #region Step 2 : Add Matrix Columns to dtMatrix
            // For Each Column of Matrix Column Array
            for (Int32 i = 0; i < MatrixColumnsArray.Length; i++)
            {
                // Verify each and every row for each column
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[MatrixColumnsArray[i]].Equals(DBNull.Value))
                        continue;

                    // If dtMatrix already contains column
                    if (dtMatrix.Columns.Contains(dr[MatrixColumnsArray[i]].ToString()))
                        continue;

                    DataColumn dcNew = new DataColumn(dr[MatrixColumnsArray[i]].ToString());

                    // Caption is assigned with Matrix Columns Title Array
                    dcNew.Caption = MatrixColumnsTitleArray[i];

                    // Replace Columns Names in caption with Column Value
                    foreach (DataColumn dc in dt.Columns)
                        dcNew.Caption = dcNew.Caption.Replace("~" + dc.ColumnName + "~", dr[dc.ColumnName].ToString());

                    dtMatrix.Columns.Add(dcNew);
                }
            }
            #endregion Step 2 : Add Matrix Columns to dtMatrix

            #region Step 3 : Transfer Data
            // Each row of dt
            foreach (DataRow dr in dt.Rows)
            {
                // Each Matrix Column
                for (Int32 i = 0; i < MatrixColumnsArray.Length; i++)
                {
                    Int32 j = 0;
                    for (; j < dtMatrix.Rows.Count; j++)
                    {
                        if (dtMatrix.Rows[j][PrimaryKeyColumns].ToString() == dr[PrimaryKeyColumns].ToString())
                            break;
                    }
                    // If Table Row is added to Matrix Row then j < Matrix row so following condition is false else it will add new row
                    #region Check Whether Row Is Added or Not
                    if (j == dtMatrix.Rows.Count)
                    {
                        DataRow drNew = dtMatrix.NewRow();

                        foreach (String _TableColumns in TableColumnsArray)
                        {
                            if (dr[_TableColumns].Equals(DBNull.Value))
                                continue;
                            drNew[_TableColumns] = dr[_TableColumns];
                        }

                        dtMatrix.Rows.Add(drNew);
                    }
                    #endregion Check Whether Row Is Added or Not

                    // Set Value to Matrix Column and Table Row
                    if (!dr[CellColumn].Equals(DBNull.Value))
                        dtMatrix.Rows[j][dr[MatrixColumnsArray[i]].ToString()] = dr[CellColumn];
                }
            }
            #endregion Step 3 : Transfer Data

            return dtMatrix;
        }

        #endregion

     
        #region Conversion

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
        }
        public static string ConvertDatatableToXML(DataTable dt)
        {
            String xmlData = String.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                xmlData += "<" + dt.TableName.ToString() + ">\r\n";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType.Name == "DateTime" && dr[dc.ColumnName.ToString()].ToString().Trim() != String.Empty)
                        xmlData += "<" + dc.ColumnName.ToString() + ">" + Convert.ToDateTime(dr[dc.ColumnName.ToString()].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "</" + dc.ColumnName.ToString() + ">\r\n";
                    else if (dr[dc.ColumnName.ToString()].ToString() != String.Empty)
                        xmlData += "<" + dc.ColumnName.ToString() + ">" + dr[dc.ColumnName.ToString()].ToString() + "</" + dc.ColumnName.ToString() + ">\r\n";
                    else
                        xmlData += "<" + dc.ColumnName.ToString() + "></" + dc.ColumnName.ToString() + ">\r\n";
                }
                xmlData += "</" + dt.TableName.ToString() + ">\r\n";
            }


            return xmlData;
        }

        #endregion Conversion

        #region Custom Developer Function




        #endregion Custom Developer Function

        //Quantity Estimator Common Functions

        public static decimal FeetToMeter(Decimal Feet)
        {
            return Feet * Convert.ToDecimal(0.3048);
        }

        #region MeterAndCMToMeter
        public static Decimal MeterAndCMToMeter(Decimal Meter, Decimal CM)
        {
            return (Meter + (CM * Convert.ToDecimal(0.01)));
        }
        #endregion MeterAndCMToMeter

        #region FeetAndInchToFeet
        public static Decimal FeetAndInchToFeet(Decimal Feet, Decimal Inch)
        {
            return (Feet + (Inch * Convert.ToDecimal(0.0833333)));
        }

        #endregion FeetAndInchToFeet

        #region Calculate Volume

        public static Decimal Volume(Decimal Length, Decimal Width, Decimal Depth)
        {
            Decimal Volume = Length * Width * Depth;
            return Volume;
        }

        #endregion Calculate Volume

        #region Convert Feet/Inch  For Volume

        public static Decimal ConvertFeetAndInchForVolume(Decimal VolumeInMeter)
        {
            Decimal Feet_Inch = VolumeInMeter * Convert.ToDecimal(35.3147);
            return Feet_Inch;
        }

        #endregion Convert Feet/Inch  For Volume

        #region Convert Meter/CM For Volume

        public static Decimal ConvertMeterAndCMForVolume(Decimal VolumeInFeet)
        {
            Decimal Meter_CM = VolumeInFeet / Convert.ToDecimal(35.3147);
            return Meter_CM;
        }

        #endregion Convert Feet/Inch For Volume

        #region Calculate Area
        public static Decimal Area(Decimal Length, Decimal Width)
        {
            Decimal Area = Length * Width;
            return Area;
        }
        #endregion Calculate Area

        #region Calculate Meter/CM For Area
        public static Decimal ConvertMeterAndCMForArea(Decimal VolumeInFeet)
        {
            Decimal Meter_CM = VolumeInFeet / Convert.ToDecimal(10.7639);
            return Meter_CM;
        }
        #endregion Calculate Meter/CM For Area

        #region Calculate Feet/Inch For Area
        public static Decimal ConvertFeetAndInchForArea(Decimal VolumeInFeet)
        {
            Decimal Feet_Inch = VolumeInFeet * Convert.ToDecimal(10.7639);
            return Feet_Inch;
        }
        #endregion Calculate Feet/Inch For Area

    }
}