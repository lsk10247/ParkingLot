
namespace ParkingLot
{
    partial class CarLicensePlateDialog
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
            this.确定button = new System.Windows.Forms.Button();
            this.取消button = new System.Windows.Forms.Button();
            this.省comboBox = new System.Windows.Forms.ComboBox();
            this.市comboBox = new System.Windows.Forms.ComboBox();
            this.号码textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // 确定button
            // 
            this.确定button.Location = new System.Drawing.Point(139, 72);
            this.确定button.Name = "确定button";
            this.确定button.Size = new System.Drawing.Size(75, 23);
            this.确定button.TabIndex = 0;
            this.确定button.Text = "确定";
            this.确定button.UseVisualStyleBackColor = true;
            this.确定button.Click += new System.EventHandler(this.确定button_Click);
            // 
            // 取消button
            // 
            this.取消button.Location = new System.Drawing.Point(27, 72);
            this.取消button.Name = "取消button";
            this.取消button.Size = new System.Drawing.Size(75, 23);
            this.取消button.TabIndex = 1;
            this.取消button.Text = "取消";
            this.取消button.UseVisualStyleBackColor = true;
            this.取消button.Click += new System.EventHandler(this.取消button_Click);
            // 
            // 省comboBox
            // 
            this.省comboBox.FormattingEnabled = true;
            this.省comboBox.Location = new System.Drawing.Point(27, 25);
            this.省comboBox.Name = "省comboBox";
            this.省comboBox.Size = new System.Drawing.Size(39, 20);
            this.省comboBox.TabIndex = 2;
            this.省comboBox.SelectedIndexChanged += new System.EventHandler(this.省comboBox_SelectedIndexChanged);
            // 
            // 市comboBox
            // 
            this.市comboBox.FormattingEnabled = true;
            this.市comboBox.Location = new System.Drawing.Point(72, 25);
            this.市comboBox.Name = "市comboBox";
            this.市comboBox.Size = new System.Drawing.Size(40, 20);
            this.市comboBox.TabIndex = 3;
            this.市comboBox.SelectedIndexChanged += new System.EventHandler(this.市comboBox_SelectedIndexChanged);
            // 
            // 号码textBox
            // 
            this.号码textBox.Location = new System.Drawing.Point(118, 25);
            this.号码textBox.Name = "号码textBox";
            this.号码textBox.Size = new System.Drawing.Size(100, 21);
            this.号码textBox.TabIndex = 4;
            this.号码textBox.TextChanged += new System.EventHandler(this.号码textBox_TextChanged);
            // 
            // CarLicensePlateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 108);
            this.Controls.Add(this.号码textBox);
            this.Controls.Add(this.市comboBox);
            this.Controls.Add(this.省comboBox);
            this.Controls.Add(this.取消button);
            this.Controls.Add(this.确定button);
            this.Name = "CarLicensePlateDialog";
            this.Text = "CarLicensePlateDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button 确定button;
        private System.Windows.Forms.Button 取消button;
        private System.Windows.Forms.ComboBox 省comboBox;
        private System.Windows.Forms.ComboBox 市comboBox;
        private System.Windows.Forms.TextBox 号码textBox;
    }
}