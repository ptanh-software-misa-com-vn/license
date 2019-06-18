namespace Anh.PowerTools
{
    partial class LicenseActive
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProductKey = new System.Windows.Forms.MaskedTextBox();
            this.btnAuthen = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(406, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "This tool accesses the license activation server to process license activation.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Product Name:";
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(97, 35);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(35, 12);
            this.lblProductName.TabIndex = 2;
            this.lblProductName.Text = "label3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(334, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Enter the product key. Enter it and click [Authentication] button.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Product Key:";
            // 
            // txtProductKey
            // 
            this.txtProductKey.Location = new System.Drawing.Point(99, 80);
            this.txtProductKey.Mask = "0000-0000-0000-0000-0000-0000";
            this.txtProductKey.Name = "txtProductKey";
            this.txtProductKey.Size = new System.Drawing.Size(283, 19);
            this.txtProductKey.TabIndex = 5;
            this.txtProductKey.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txtProductKey_MaskInputRejected);
            this.txtProductKey.TextChanged += new System.EventHandler(this.txtProductKey_TextChanged);
            // 
            // btnAuthen
            // 
            this.btnAuthen.Location = new System.Drawing.Point(227, 226);
            this.btnAuthen.Name = "btnAuthen";
            this.btnAuthen.Size = new System.Drawing.Size(108, 23);
            this.btnAuthen.TabIndex = 6;
            this.btnAuthen.Text = "Authentication";
            this.btnAuthen.UseVisualStyleBackColor = true;
            this.btnAuthen.Click += new System.EventHandler(this.btnAuthen_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(342, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // LicenseActive
            // 
            this.AcceptButton = this.btnAuthen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(443, 261);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAuthen);
            this.Controls.Add(this.txtProductKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LicenseActive";
            this.Text = "License Activation Procedure";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox txtProductKey;
        private System.Windows.Forms.Button btnAuthen;
        private System.Windows.Forms.Button btnCancel;
    }
}