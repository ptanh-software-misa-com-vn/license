namespace Anh.PowerTools
{
    partial class PowerTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ProductCd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AuthenticationStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Authentication = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductCd,
            this.ProductName,
            this.ProductKey,
            this.AuthenticationStatus,
            this.Authentication});
            this.dataGridView1.Location = new System.Drawing.Point(36, 32);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(626, 150);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ProductCd
            // 
            this.ProductCd.HeaderText = "ProductCD";
            this.ProductCd.Name = "ProductCd";
            this.ProductCd.Visible = false;
            this.ProductCd.Width = 120;
            // 
            // ProductName
            // 
            this.ProductName.HeaderText = "Product";
            this.ProductName.Name = "ProductName";
            // 
            // ProductKey
            // 
            this.ProductKey.HeaderText = "Product Key";
            this.ProductKey.Name = "ProductKey";
            this.ProductKey.Width = 150;
            // 
            // AuthenticationStatus
            // 
            this.AuthenticationStatus.HeaderText = "Authentication Status";
            this.AuthenticationStatus.Name = "AuthenticationStatus";
            this.AuthenticationStatus.Width = 130;
            // 
            // Authentication
            // 
            this.Authentication.HeaderText = "Authentication/Deauthentication";
            this.Authentication.Name = "Authentication";
            this.Authentication.Width = 200;
            // 
            // PowerTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 285);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PowerTool";
            this.Text = "PowerTools License Manager";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCd;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn AuthenticationStatus;
        private System.Windows.Forms.DataGridViewLinkColumn Authentication;
    }
}