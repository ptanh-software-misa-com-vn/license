namespace Anh.PowerTools
{
	partial class ProductKey
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
			this.btnCreateProductKey = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cboProduct = new System.Windows.Forms.ComboBox();
			this.txtProductKey = new System.Windows.Forms.TextBox();
			this.btnRegister = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnCreateProductKey
			// 
			this.btnCreateProductKey.Location = new System.Drawing.Point(231, 46);
			this.btnCreateProductKey.Name = "btnCreateProductKey";
			this.btnCreateProductKey.Size = new System.Drawing.Size(119, 23);
			this.btnCreateProductKey.TabIndex = 0;
			this.btnCreateProductKey.Text = "Create Product Key";
			this.btnCreateProductKey.UseVisualStyleBackColor = true;
			this.btnCreateProductKey.Click += new System.EventHandler(this.btnCreateProductKey_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(26, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "Product";
			// 
			// cboProduct
			// 
			this.cboProduct.FormattingEnabled = true;
			this.cboProduct.Location = new System.Drawing.Point(96, 16);
			this.cboProduct.Name = "cboProduct";
			this.cboProduct.Size = new System.Drawing.Size(121, 20);
			this.cboProduct.TabIndex = 2;
			// 
			// txtProductKey
			// 
			this.txtProductKey.Location = new System.Drawing.Point(96, 84);
			this.txtProductKey.Name = "txtProductKey";
			this.txtProductKey.ReadOnly = true;
			this.txtProductKey.Size = new System.Drawing.Size(254, 19);
			this.txtProductKey.TabIndex = 3;
			// 
			// btnRegister
			// 
			this.btnRegister.Location = new System.Drawing.Point(231, 143);
			this.btnRegister.Name = "btnRegister";
			this.btnRegister.Size = new System.Drawing.Size(147, 23);
			this.btnRegister.TabIndex = 4;
			this.btnRegister.Text = "Register Product Key";
			this.btnRegister.UseVisualStyleBackColor = true;
			this.btnRegister.Click += new System.EventHandler(this.button1_Click);
			// 
			// ProductKey
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(436, 261);
			this.Controls.Add(this.btnRegister);
			this.Controls.Add(this.txtProductKey);
			this.Controls.Add(this.cboProduct);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCreateProductKey);
			this.Name = "ProductKey";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.ProductKey_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCreateProductKey;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboProduct;
		private System.Windows.Forms.TextBox txtProductKey;
		private System.Windows.Forms.Button btnRegister;
	}
}

