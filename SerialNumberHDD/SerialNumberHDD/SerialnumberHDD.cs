using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Security.Cryptography;
using Oracle.DataAccess.Client;

namespace Anh.License
{
    /// <summary>
    /// Hôm nay, mình xin hướng dẫn các bạn cách tạo License cho ứng dụng. Sau khi, tạo ứng dụng xong, nếu các bạn muốn phân phối ứng dụng của mình theo License product.
    /// Ở bài viết này, mình hướng dẫn các bạn thuật toán cơ bản để bảo mật ứng dụng.
    /// 1. Đầu tiên, mình lấy serial number của HDD.Vì serial number HDD là duy nhất, nên ứng dụng mình cài vào máy tính nào thì chỉ sử dụng được một máy, cài đặt vào ổ cứng khác thì ứng dụng sẽ không hoạt động.
    /// 2. Tạo khóa token (chuỗi bảo mật).
    /// 3. Sử dụng thuật toán SHA1, serial number HDD với token của mình.Bạn có thể sử dụng thuật toán MD5, hay 1 thuật toán mã hóa bất kỳ.
    /// </summary>
    public class SerialnumberHDD
    {
        #region FIELD
        private LicenseMode _licenseMode;
        public LicenseMode LicenseMode
        {
            get { return _licenseMode; }
            set { _licenseMode = value; }
        }
        private string _token = TokenUtil.DefaultToken;
        #endregion

        #region Property
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }
        public string GetDriveSerialNumber()
        {
            ManagementObjectSearcher hdd = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (ManagementObject hd in hdd.Get())
            {
                string HDD_Serial = (string)hd["SerialNumber"];
                return HDD_Serial;
            }

            throw new Exception("Cannot find any HDD.");
        }
        #endregion

        #region Method
        #region SHA512
        public string SHA512(string asSerialNumber)
        {
            ASCIIEncoding ASCIIENC = new ASCIIEncoding();
            string strreturn = null;
            byte[] bytesourcetxt = ASCIIENC.GetBytes(asSerialNumber);
            SHA512CryptoServiceProvider SHA512Hash = new SHA512CryptoServiceProvider();
            byte[] bytehash = SHA512Hash.ComputeHash(bytesourcetxt);
            strreturn = Convert.ToBase64String(bytehash);
            //foreach (byte b in bytehash)
            //    strreturn += b.ToString("X8");
            return strreturn;
        }
        #endregion
        #endregion

        #region Generate Product Key
        public bool CreateKey(DbConnect adbConn, string asProductCd, string asProductKey, string asSerialHDD, out string outKey)
        {
            string key = CreateKey(asProductKey + asSerialHDD);
            bool bSuccess = InsertLicenseKey(adbConn, asProductCd, asProductKey, key);
            if (bSuccess)
            {
                outKey = key;
            }
            else
            {
                outKey = "";
            }

            return bSuccess;
        }

        public string CreateKey(string asKey)
        {
            string key = SHA512(asKey + _token);
            return key;
        }

        private string InsertLicenseKeySQL()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("INSERT INTO W011_LICENSE_KEY                          ");
            sbSQL.AppendLine("     ( RENBAN                                         ");
            sbSQL.AppendLine("     , LICENSE_KEY                                    ");
            sbSQL.AppendLine("     , PRODUCT_CD                                     ");
            sbSQL.AppendLine("     , PRODUCT_KEY                                    ");
            sbSQL.AppendLine("     , IN_DATE                                        ");
            sbSQL.AppendLine("     , IN_TIME                                        ");
            sbSQL.AppendLine("     , IN_TAN_CD                                      ");
            sbSQL.AppendLine("     , IN_IP_ADD                                      ");
            sbSQL.AppendLine("     , UP_DATE                                        ");
            sbSQL.AppendLine("     , UP_TIME                                        ");
            sbSQL.AppendLine("     , UP_TAN_CD                                      ");
            sbSQL.AppendLine("     , UP_IP_ADD                                      ");
            sbSQL.AppendLine("     )                                                ");
            sbSQL.AppendLine("VALUES                                                ");
            sbSQL.AppendLine("     ( (SELECT COUNT(*)                               ");
            sbSQL.AppendLine("          FROM W011_LICENSE_KEY                       ");
            sbSQL.AppendLine("       ) + 1                                          ");
            sbSQL.AppendLine("     , :license_key                                   ");
            sbSQL.AppendLine("     , :product_cd                                    ");
            sbSQL.AppendLine("     , :product_key                                   ");
            sbSQL.AppendLine("     , :in_date                                       ");
            sbSQL.AppendLine("     , :in_time                                       ");
            sbSQL.AppendLine("     , :in_tan_cd                                     ");
            sbSQL.AppendLine("     , :in_ip_add                                     ");
            sbSQL.AppendLine("     , :up_date                                       ");
            sbSQL.AppendLine("     , :up_time                                       ");
            sbSQL.AppendLine("     , :up_tan_cd                                     ");
            sbSQL.AppendLine("     , :up_ip_add                                     ");
            sbSQL.AppendLine("     )                                                ");

            return sbSQL.ToString();
        }
        #endregion

        #region CheckProductKeyValid
        public bool CheckProductKeyValid(DbConnect adbConn, string asProductCd, string asProductKey)
        {
            OracleDataReader oraRead = null;
            string sSQL = CheckProductKeyValidSQL();
            OracleCommand myCmd = new OracleCommand(sSQL, adbConn.Conn);
            myCmd.BindByName = true;
            myCmd.Parameters.Add("product_key", OracleDbType.Varchar2,30).Value = asProductKey;
            myCmd.Parameters.Add("product_cd", OracleDbType.Varchar2,10).Value = asProductCd;
            oraRead = myCmd.ExecuteReader();
            if (oraRead.Read())
            {
                return int.Parse(oraRead[0].ToString()) > 0;
            }
            return false;
        }

        private string CheckProductKeyValidSQL()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("SELECT COUNT(*)                                   ");
            sbSQL.AppendLine("  FROM W011_LICENSE_KEY                           ");
            sbSQL.AppendLine(" WHERE PRODUCT_KEY = :product_key                 ");
            sbSQL.AppendLine("   AND PRODUCT_CD  = :product_cd                  ");

            return sbSQL.ToString();
        }
        #endregion
        #region RegisterLicense

        public bool RegisterLicense(DbConnect adbConn, LicenseVerify aLicenseVerify, string asSerialHDD)
        {
            bool bResult = false;
            switch (LicenseMode)
            {
                case LicenseMode.OnlyOneHDD:
                    aLicenseVerify.LicenseKey = CreateKey(aLicenseVerify.ProductKey + asSerialHDD);
                    if (CheckSameKey(adbConn, aLicenseVerify.LicenseKey, aLicenseVerify.ProductCd))
                    {
                        bResult = true;
                    }
                    else
                    {
                        throw new LicenseException("Khóa không hợp lệ!");
                    }
                    break;
                case LicenseMode.LimitCountHDD:
                    aLicenseVerify.LicenseKey = CreateKey(aLicenseVerify.ProductKey);
                    if (CheckSameKey(adbConn, aLicenseVerify.LicenseKey, aLicenseVerify.ProductCd))
                    {
                        int iCntLic = GetExistLicense1(adbConn, aLicenseVerify.LicenseKey, aLicenseVerify.ProductCd);
                        if (iCntLic == 0)
                        {
                            bResult = true;
                        }
                        else
                        {
                            int iMaxLic = GetMaxLicense(adbConn,aLicenseVerify.LicenseKey, aLicenseVerify.ProductCd);
                            if (iCntLic <= iMaxLic)
                            {
                                int iCntLicHDD = GetExistLicense2(adbConn, aLicenseVerify.LicenseKey, aLicenseVerify.ProductCd, asSerialHDD);
                                if (iCntLic == iMaxLic && iCntLicHDD > 0)
                                {
                                    bResult = true;
                                }
                                else
                                {
                                    throw new LicenseException("Quá giới hạn đăng ký.");
                                }
                            }
                            else
                            {
                                throw new LicenseException("Quá giới hạn đăng ký.");
                            }
                        }
                    }
                    else
                    {
                        throw new LicenseException("Khóa không hợp lệ!");
                    }
                    break;
                case LicenseMode.ByCompany:
                    break;
                default:
                    break;
            }

            if (bResult)
            {
                UpdateLicense(adbConn, aLicenseVerify, asSerialHDD);
            }
            return bResult;
        }

        private bool CheckSameKey(DbConnect adbConn, string asLicenseKey, string asProductCd)
        {
            string sSQL = CheckSameKeySQL();
            OracleCommand myCmd = new OracleCommand(sSQL, adbConn.Conn);
            myCmd.BindByName = true;
            myCmd.Parameters.Add("license_key", OracleDbType.Varchar2).Value = asLicenseKey;
            myCmd.Parameters.Add("product_cd", OracleDbType.Varchar2).Value = asProductCd;
            int iCnt = int.Parse(myCmd.ExecuteScalar().ToString());
            return iCnt > 0;
        }

        private string CheckSameKeySQL()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("SELECT COUNT(*)                                   ");
            sbSQL.AppendLine("  FROM W011_LICENSE_KEY                           ");
            sbSQL.AppendLine(" WHERE LICENSE_KEY = :license_key                 ");
            sbSQL.AppendLine("   AND PRODUCT_CD  = :product_cd                  ");

            return sbSQL.ToString();
        }

        private bool InsertLicenseKey(DbConnect adbConn, string asProductCd, string asProductKey, string asLicenseKey)
        {
            int iResult = 0;
            string sLogTanCd = "";
            string sIpAdd = "";
            string sNowYmd = DateTime.Now.ToString("yyyy/MM/dd");
            string sNowTime = DateTime.Now.ToString("HH:mm:ss");
            string sSQL = InsertLicenseKeySQL();
            OracleCommand myCmd = new OracleCommand(sSQL, adbConn.Conn);
            myCmd.BindByName = true;
            myCmd.Parameters.Clear();
            myCmd.Parameters.Add("license_key", OracleDbType.Varchar2).Value = asLicenseKey;
            myCmd.Parameters.Add("product_cd", OracleDbType.Varchar2).Value = asProductCd;
            myCmd.Parameters.Add("product_key", OracleDbType.Varchar2).Value = asProductKey;
            myCmd.Parameters.Add("in_date", OracleDbType.Varchar2).Value = sNowYmd;
            myCmd.Parameters.Add("in_time", OracleDbType.Varchar2).Value = sNowTime;
            myCmd.Parameters.Add("in_tan_cd", OracleDbType.Varchar2).Value = sLogTanCd;
            myCmd.Parameters.Add("in_ip_add", OracleDbType.Varchar2).Value = sIpAdd;
            myCmd.Parameters.Add("up_date", OracleDbType.Varchar2).Value = sNowYmd;
            myCmd.Parameters.Add("up_time", OracleDbType.Varchar2).Value = sNowTime;
            myCmd.Parameters.Add("up_tan_cd", OracleDbType.Varchar2).Value = sLogTanCd;
            myCmd.Parameters.Add("up_ip_add", OracleDbType.Varchar2).Value = sIpAdd;

            iResult = myCmd.ExecuteNonQuery();
            return iResult > 0;
        }

        private void UpdateLicense(DbConnect adbConn, LicenseVerify aLicenseVerify, string asSerialHDD)
        {
            switch (_licenseMode)
            {
                case LicenseMode.OnlyOneHDD:
                    UpdateLicense1(adbConn, aLicenseVerify, asSerialHDD);
                    break;
                case LicenseMode.LimitCountHDD:
                    UpdateLicense2(adbConn, aLicenseVerify, asSerialHDD);
                    break;
                case LicenseMode.ByCompany:
                    break;
                default:
                    break;
            }

        }

        #endregion

        #region LicenseMode.OnlyOneHDD
        private bool UpdateLicense1(DbConnect adbConn, LicenseVerify aLicenseVerify, string asSerialHDD)
        {
            int iResult = 0;
            string sLogTanCd = aLicenseVerify.LogTanCd;
            string sIpAdd = aLicenseVerify.UserHostAddress;
            string sNowYmd = DateTime.Now.ToString("yyyy/MM/dd");
            string sNowTime = DateTime.Now.ToString("HH:mm:ss");
            string sSQL = "";
            OracleCommand myCmd = new OracleCommand(sSQL, adbConn.Conn);
            myCmd.BindByName = true;
            if (GetExistLicense1(adbConn, aLicenseVerify.LicenseKey, aLicenseVerify.ProductCd) > 0)
            {
                sSQL = UpdateLicense1SQL();
                myCmd.Parameters.Clear();
                myCmd.Parameters.Add("license_key", OracleDbType.Varchar2).Value = aLicenseVerify.LicenseKey;
                myCmd.Parameters.Add("product_cd", OracleDbType.Varchar2).Value = aLicenseVerify.ProductCd;
                myCmd.Parameters.Add("renban", OracleDbType.Int32).Value = 1;
                myCmd.Parameters.Add("up_date", OracleDbType.Varchar2).Value = sNowYmd;
                myCmd.Parameters.Add("up_time", OracleDbType.Varchar2).Value = sNowTime;
                myCmd.Parameters.Add("up_tan_cd", OracleDbType.Varchar2).Value = sLogTanCd;
                myCmd.Parameters.Add("up_ip_add", OracleDbType.Varchar2).Value = sIpAdd;
                myCmd.Parameters.Add("serial_hdd", OracleDbType.Varchar2).Value = asSerialHDD;
            }
            else
            {
                sSQL = InsertLicense1SQL();
                myCmd.Parameters.Clear();
                myCmd.Parameters.Add("license_key", OracleDbType.Varchar2).Value = aLicenseVerify.LicenseKey;
                myCmd.Parameters.Add("product_cd", OracleDbType.Varchar2).Value = aLicenseVerify.ProductCd;
                myCmd.Parameters.Add("renban", OracleDbType.Int32).Value = 1;
                myCmd.Parameters.Add("in_date", OracleDbType.Varchar2).Value = sNowYmd;
                myCmd.Parameters.Add("in_time", OracleDbType.Varchar2).Value = sNowTime;
                myCmd.Parameters.Add("in_tan_cd", OracleDbType.Varchar2).Value = sLogTanCd;
                myCmd.Parameters.Add("in_ip_add", OracleDbType.Varchar2).Value = sIpAdd;
                myCmd.Parameters.Add("up_date", OracleDbType.Varchar2).Value = sNowYmd;
                myCmd.Parameters.Add("up_time", OracleDbType.Varchar2).Value = sNowTime;
                myCmd.Parameters.Add("up_tan_cd", OracleDbType.Varchar2).Value = sLogTanCd;
                myCmd.Parameters.Add("up_ip_add", OracleDbType.Varchar2).Value = sIpAdd;
                myCmd.Parameters.Add("serial_hdd", OracleDbType.Varchar2).Value = asSerialHDD;
            }

            iResult = myCmd.ExecuteNonQuery();
            return iResult > 0;
        }
        private int GetExistLicense1(DbConnect adbConn, string asLicenseKey, string asProductCd)
        {
            string sSQL = CheckExistLicense1SQL();
            OracleCommand myCmd = new OracleCommand(sSQL, adbConn.Conn);
            myCmd.BindByName = true;
            myCmd.Parameters.Add("license_key", OracleDbType.Varchar2).Value = asLicenseKey;
            myCmd.Parameters.Add("product_cd", OracleDbType.Varchar2).Value = asProductCd;
            int iCnt = int.Parse(myCmd.ExecuteScalar().ToString());
            return iCnt;
        }
        private string CheckExistLicense1SQL()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("SELECT COUNT(*)                                   ");
            sbSQL.AppendLine("  FROM W010_LICENSE                               ");
            sbSQL.AppendLine(" WHERE LICENSE_KEY = :license_key                 ");
            sbSQL.AppendLine("   AND PRODUCT_CD  = :product_cd                  ");

            return sbSQL.ToString();
        }
        private string UpdateLicense1SQL()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("UPDATE W010_LICENSE                               ");
            sbSQL.AppendLine("   SET LICENSE_KEY = :license_key                 ");
            sbSQL.AppendLine("     , PRODUCT_CD  = :product_cd                  ");
            sbSQL.AppendLine("     , RENBAN      = :renban                      ");
            sbSQL.AppendLine("     , IN_DATE     = :in_date                     ");
            sbSQL.AppendLine("     , IN_TIME     = :in_time                     ");
            sbSQL.AppendLine("     , IN_TAN_CD   = :in_tan_cd                   ");
            sbSQL.AppendLine("     , IN_IP_ADD   = :in_ip_add                   ");
            sbSQL.AppendLine("     , UP_DATE     = :up_date                     ");
            sbSQL.AppendLine("     , UP_TIME     = :up_time                     ");
            sbSQL.AppendLine("     , UP_TAN_CD   = :Up_Tan_Cd                   ");
            sbSQL.AppendLine("     , UP_IP_ADD   = :up_ip_add                   ");
            sbSQL.AppendLine("     , SERIAL_HDD  = :serial_hdd                  ");
            sbSQL.AppendLine("     , UPD_CNT     = UPD_CNT + 1                  ");
            sbSQL.AppendLine(" WHERE LICENSE_KEY = :license_key                 ");
            sbSQL.AppendLine("   AND PRODUCT_CD  = :product_cd                  ");

            return sbSQL.ToString();
        }
        private string InsertLicense1SQL()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("INSERT INTO W010_LICENSE                              ");
            sbSQL.AppendLine("     ( PRODUCT_CD                                     ");
            sbSQL.AppendLine("     , LICENSE_KEY                                    ");
            sbSQL.AppendLine("     , RENBAN                                         ");
            sbSQL.AppendLine("     , IN_DATE                                        ");
            sbSQL.AppendLine("     , IN_TIME                                        ");
            sbSQL.AppendLine("     , IN_TAN_CD                                      ");
            sbSQL.AppendLine("     , IN_IP_ADD                                      ");
            sbSQL.AppendLine("     , UP_DATE                                        ");
            sbSQL.AppendLine("     , UP_TIME                                        ");
            sbSQL.AppendLine("     , UP_TAN_CD                                      ");
            sbSQL.AppendLine("     , UP_IP_ADD                                      ");
            sbSQL.AppendLine("     , SERIAL_HDD                                     ");
            sbSQL.AppendLine("     , UPD_CNT                                        ");
            sbSQL.AppendLine("     )                                                ");
            sbSQL.AppendLine("VALUES                                                ");
            sbSQL.AppendLine("     ( :product_cd                                    ");
            sbSQL.AppendLine("     , :license_key                                   ");
            sbSQL.AppendLine("     , :renban                                        ");
            sbSQL.AppendLine("     , :in_date                                       ");
            sbSQL.AppendLine("     , :in_time                                       ");
            sbSQL.AppendLine("     , :in_tan_cd                                     ");
            sbSQL.AppendLine("     , :in_ip_add                                     ");
            sbSQL.AppendLine("     , :up_date                                       ");
            sbSQL.AppendLine("     , :up_time                                       ");
            sbSQL.AppendLine("     , :up_tan_cd                                     ");
            sbSQL.AppendLine("     , :up_ip_add                                     ");
            sbSQL.AppendLine("     , :serial_hdd                                    ");
            sbSQL.AppendLine("     , 1                                              ");
            sbSQL.AppendLine("     )                                                ");

            return sbSQL.ToString();
        }
        #endregion

        #region LicenseMode.LimitCountHDD
        private bool UpdateLicense2(DbConnect adbConn, LicenseVerify aLicenseVerify, string asSerialHDD)
        {
            int iResult = 0;
            string sLogTanCd = aLicenseVerify.LogTanCd;
            string sIpAdd = aLicenseVerify.UserHostAddress;
            string sNowYmd = DateTime.Now.ToString("yyyy/MM/dd");
            string sNowTime = DateTime.Now.ToString("HH:mm:ss");
            string sSQL = "";
            OracleCommand myCmd = new OracleCommand(sSQL, adbConn.Conn);
            myCmd.BindByName = true;
            int iCnt = (int)myCmd.ExecuteScalar();
            if (GetExistLicense2(adbConn, aLicenseVerify.LicenseKey, aLicenseVerify.ProductCd, asSerialHDD) > 0)
            {
                sSQL = UpdateLicense2SQL();
                myCmd.Parameters.Clear();
                myCmd.Parameters.Add("license_key", OracleDbType.Varchar2).Value = aLicenseVerify.LicenseKey;
                myCmd.Parameters.Add("product_cd", OracleDbType.Varchar2).Value = aLicenseVerify.ProductCd;
                myCmd.Parameters.Add("renban", OracleDbType.Int32).Value = 1;
                myCmd.Parameters.Add("up_date", OracleDbType.Varchar2).Value = sNowYmd;
                myCmd.Parameters.Add("up_time", OracleDbType.Varchar2).Value = sNowTime;
                myCmd.Parameters.Add("up_tan_cd", OracleDbType.Varchar2).Value = sLogTanCd;
                myCmd.Parameters.Add("up_ip_add", OracleDbType.Varchar2).Value = sIpAdd;
                myCmd.Parameters.Add("serial_hdd", OracleDbType.Varchar2).Value = asSerialHDD;
            }
            else
            {
                sSQL = InsertLicense2SQL();
                myCmd.Parameters.Clear();
                myCmd.Parameters.Add("license_key", OracleDbType.Varchar2).Value = aLicenseVerify.LicenseKey;
                myCmd.Parameters.Add("product_cd", OracleDbType.Varchar2).Value = aLicenseVerify.ProductCd;
                myCmd.Parameters.Add("renban", OracleDbType.Int32).Value = 1;
                myCmd.Parameters.Add("in_date", OracleDbType.Varchar2).Value = sNowYmd;
                myCmd.Parameters.Add("in_time", OracleDbType.Varchar2).Value = sNowTime;
                myCmd.Parameters.Add("in_tan_cd", OracleDbType.Varchar2).Value = sLogTanCd;
                myCmd.Parameters.Add("in_ip_add", OracleDbType.Varchar2).Value = sIpAdd;
                myCmd.Parameters.Add("up_date", OracleDbType.Varchar2).Value = sNowYmd;
                myCmd.Parameters.Add("up_time", OracleDbType.Varchar2).Value = sNowTime;
                myCmd.Parameters.Add("up_tan_cd", OracleDbType.Varchar2).Value = sLogTanCd;
                myCmd.Parameters.Add("up_ip_add", OracleDbType.Varchar2).Value = sIpAdd;
                myCmd.Parameters.Add("serial_hdd", OracleDbType.Varchar2).Value = asSerialHDD;
            }

            iResult = myCmd.ExecuteNonQuery();
            return iResult > 0;
        }
        private int GetExistLicense2(DbConnect adbConn, string asLicenseKey, string asProductCd, string asSerialHDD)
        {
            string sSQL = CheckExistLicense2SQL();
            OracleCommand myCmd = new OracleCommand(sSQL, adbConn.Conn);
            myCmd.BindByName = true;
            myCmd.Parameters.Add("license_key", OracleDbType.Varchar2).Value = asLicenseKey;
            myCmd.Parameters.Add("product_cd", OracleDbType.Varchar2).Value = asProductCd;
            myCmd.Parameters.Add("serial_hdd", OracleDbType.Varchar2).Value = asSerialHDD;
            int iCnt = (int)myCmd.ExecuteScalar();
            return iCnt;
        }
        private int GetMaxLicense(DbConnect adbConn, string asLicenseKey, string asProductCd)
        {
            string sSQL = GetMaxLicenseSQL();
            OracleCommand myCmd = new OracleCommand(sSQL, adbConn.Conn);
            myCmd.BindByName = true;
            myCmd.Parameters.Add("license_key", OracleDbType.Varchar2).Value = asLicenseKey;
            myCmd.Parameters.Add("product_cd", OracleDbType.Varchar2).Value = asProductCd;
            int iCnt = (int)myCmd.ExecuteScalar();
            return iCnt;
        }
        private string CheckExistLicense2SQL()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("SELECT COUNT(*)                                   ");
            sbSQL.AppendLine("  FROM W010_LICENSE                               ");
            sbSQL.AppendLine(" WHERE LICENSE_KEY = :license_key                 ");
            sbSQL.AppendLine("   AND PRODUCT_CD  = :product_cd                  ");
            sbSQL.AppendLine("   AND SERIAL_HDD  = :serial_hdd                  ");

            return sbSQL.ToString();
        }
        private string GetMaxLicenseSQL()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("SELECT MAX(MAX_HDD)                               ");
            sbSQL.AppendLine("  FROM W010_LICENSE                               ");
            sbSQL.AppendLine(" WHERE LICENSE_KEY = :license_key                 ");
            sbSQL.AppendLine("   AND PRODUCT_CD  = :product_cd                  ");

            return sbSQL.ToString();
        }
        private string UpdateLicense2SQL()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("UPDATE W010_LICENSE                               ");
            sbSQL.AppendLine("   SET LICENSE_KEY = :license_key                 ");
            sbSQL.AppendLine("     , PRODUCT_CD  = :product_cd                  ");
            sbSQL.AppendLine("     , RENBAN      = :renban                      ");
            sbSQL.AppendLine("     , IN_DATE     = :in_date                     ");
            sbSQL.AppendLine("     , IN_TIME     = :in_time                     ");
            sbSQL.AppendLine("     , IN_TAN_CD   = :in_tan_cd                   ");
            sbSQL.AppendLine("     , IN_IP_ADD   = :in_ip_add                   ");
            sbSQL.AppendLine("     , UP_DATE     = :up_date                     ");
            sbSQL.AppendLine("     , UP_TIME     = :up_time                     ");
            sbSQL.AppendLine("     , UP_TAN_CD   = :Up_Tan_Cd                   ");
            sbSQL.AppendLine("     , UP_IP_ADD   = :up_ip_add                   ");
            sbSQL.AppendLine("     , SERIAL_HDD  = :serial_hdd                  ");
            sbSQL.AppendLine("     , UPD_CNT     = UPD_CNT + 1                  ");
            sbSQL.AppendLine(" WHERE LICENSE_KEY = :license_key                 ");
            sbSQL.AppendLine("   AND PRODUCTc_CD = :product_cd                  ");
            sbSQL.AppendLine("   AND SERIAL_HDD  = :serial_hdd                  ");

            return sbSQL.ToString();
        }
        private string InsertLicense2SQL()
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("INSERT INTO W010_LICENSE                              ");
            sbSQL.AppendLine("     ( LICENSE_KEY                                    ");
            sbSQL.AppendLine("     , PRODUCT_CD                                     ");
            sbSQL.AppendLine("     , RENBAN                                         ");
            sbSQL.AppendLine("     , IN_DATE                                        ");
            sbSQL.AppendLine("     , IN_TIME                                        ");
            sbSQL.AppendLine("     , IN_TAN_CD                                      ");
            sbSQL.AppendLine("     , IN_IP_ADD                                      ");
            sbSQL.AppendLine("     , UP_DATE                                        ");
            sbSQL.AppendLine("     , UP_TIME                                        ");
            sbSQL.AppendLine("     , UP_TAN_CD                                      ");
            sbSQL.AppendLine("     , UP_IP_ADD                                      ");
            sbSQL.AppendLine("     , SERIAL_HDD                                     ");
            sbSQL.AppendLine("     , UPD_CNT                                        ");
            sbSQL.AppendLine("     , MAX_HDD                                        ");
            sbSQL.AppendLine("     )                                                ");
            sbSQL.AppendLine("VALUES                                                ");
            sbSQL.AppendLine("     ( :license_key                                   ");
            sbSQL.AppendLine("     , :product_cd                                    ");
            sbSQL.AppendLine("     , :renban                                        ");
            sbSQL.AppendLine("     , :license_key                                   ");
            sbSQL.AppendLine("     , :in_date                                       ");
            sbSQL.AppendLine("     , :in_time                                       ");
            sbSQL.AppendLine("     , :in_tan_cd                                     ");
            sbSQL.AppendLine("     , :in_ip_add                                     ");
            sbSQL.AppendLine("     , :up_date                                       ");
            sbSQL.AppendLine("     , :up_time                                       ");
            sbSQL.AppendLine("     , :up_tan_cd                                     ");
            sbSQL.AppendLine("     , :up_ip_add                                     ");
            sbSQL.AppendLine("     , :serial_hdd                                    ");
            sbSQL.AppendLine("     , 1                                              ");
            sbSQL.AppendLine("     , 3                                              ");
            sbSQL.AppendLine("     )                                                ");

            return sbSQL.ToString();
        }
        #endregion

        #region LicenseVerify
        public sealed class LicenseVerify
        {
            public LicenseVerify()
            {

            }
            public string ProductKey { get; set; }
            public string ProductCd { get; set; }
            public string LicenseKey { internal get; set; }
            public string LogTanCd { get; set; }
            public string UserHostAddress { get; set; }
            public string KeyLicense
            {
                private get;
                set;
            }

            public bool IsValid
            {
                get;
                private set;
            }
        }
        #endregion
    }

    public enum LicenseMode
    {
        OnlyOneHDD = 0,
        LimitCountHDD = 1,
        ByCompany = 2
    }

    public class LicenseException : Exception
    {
        public LicenseException(string message) : base(message)
        {
        }
    }
}
