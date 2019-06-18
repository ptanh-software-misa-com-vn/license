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
    public partial class ProductKey : Form
    {
        public ProductKey()
        {
            InitializeComponent();
			InitData();
        }

		private void InitData()
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("ProductCd", typeof(string));
			dt.Columns.Add("ProductName", typeof(string));
			dt.Rows.Add("18020002", "Dion Software Co., Ltd");
			dt.Rows.Add("18020003", "CÔNG TY CỔ PHẦN HÀNG KHÔNG VIETJET.");
			cboProduct.DataSource = dt;
			cboProduct.ValueMember = "ProductCd";
			cboProduct.DisplayMember = "ProductName";
		}

		private void ProductKey_Load(object sender, EventArgs e)
		{

		}

		private void btnCreateProductKey_Click(object sender, EventArgs e)
		{
			string productKey = KeyUtil.CreateProductKey();
			txtProductKey.Text = productKey;

		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (cboProduct.SelectedValue.ToString().Length==0)
			{
				MessageBox.Show("Please choose a product.");
				cboProduct.Focus();
				return;
			}
			if (txtProductKey.Text.Length == 0)
			{
				MessageBox.Show("Please get a product key.");
				btnCreateProductKey.Focus();
				return;
			}
			SerialnumberHDD ser = new SerialnumberHDD();
			string outKey = "";
			DbConnect dbConn = new DbConnect();
			try
			{
				dbConn.Open();
				dbConn.BeginTran();
				bool bSuccess = ser.CreateKey(dbConn, cboProduct.SelectedValue.ToString(), txtProductKey.Text, ser.GetDriveSerialNumber(), out outKey);
				if (bSuccess)
				{
					dbConn.Commit();
				}else
				{
					dbConn.Rollback();
				}

			}
			catch (Exception ex)
			{
				dbConn.Rollback();
				Debug.WriteLine(ex.ToString());
			}
			finally
			{
				dbConn.DisposeTran();
				dbConn.Close();
			}
		}
	}
}
