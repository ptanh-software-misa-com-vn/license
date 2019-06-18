using Anh.License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anh.PowerTools
{
    public partial class LicenseActive : Form
    {
        SerialnumberHDD ser = new SerialnumberHDD();
        SerialnumberHDD.LicenseVerify lic = new SerialnumberHDD.LicenseVerify();
        public string ProductKey { get; set; }
        public string ProductCd { get; set; }
        public LicenseActive()
        {
            InitializeComponent();
        }

        public LicenseActive(string asProductCd,string asProductKey) : this()
        {
            ProductKey = asProductKey;
            ProductCd = asProductCd;
        }
        private void txtProductKey_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtProductKey_TextChanged(object sender, EventArgs e)
        {
            
            if (txtProductKey.MaskCompleted)
            {
                DbConnect dbConn = new DbConnect();
                try
                {
                    dbConn.Open();
                    ProductKey = txtProductKey.Text;
                    bool bValid = ser.CheckProductKeyValid(dbConn, ProductCd, ProductKey);
                    if (bValid)
                    {
                        btnAuthen.Enabled = true;
                    }
                    else
                    {
                        btnAuthen.Enabled = false;
                        MessageBox.Show("There is and error in the product key." + 
                            Environment.NewLine +
                            " -Whether there is a mistake in the input." +
                            Environment.NewLine +
                            " -Aren't you looking for a product key other than this one.");
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    dbConn.Close();
                }
            }
            else
            {
                btnAuthen.Enabled = false;
            }
            
        }

        private void btnAuthen_Click(object sender, EventArgs e)
        {
            lic.ProductCd = ProductCd;
            lic.ProductKey = ProductKey;
            DbConnect dbConn = new DbConnect();
            try
            {
                dbConn.Open();
                var bSuccess = ser.RegisterLicense(dbConn, lic, ser.GetDriveSerialNumber());

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                dbConn.Close();
            }
        }
    }
}
