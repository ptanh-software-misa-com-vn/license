using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anh.PowerTools
{
    public partial class PowerTool : Form
    {
        public PowerTool()
        {
            InitializeComponent();
			
            dataGridView1.Rows.Add("18020002", "Dion Software Co., Ltd", "3704 -6465-9901-8165-7819-7135", "Not Authenticated", "Athentication Procedure");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn)
            {
                using (LicenseActive frm = new LicenseActive(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()))
                {
                    frm.ShowDialog();
                }
            }
        }
    }
}
