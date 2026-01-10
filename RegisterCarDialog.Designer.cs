
namespace ParkingLot
{
    partial class RegisterCarDialog
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
            this.车辆类型comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.品牌comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.颜色comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.车牌号textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.车主textBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.电话textBox = new System.Windows.Forms.TextBox();
            this.VIPcheckBox = new System.Windows.Forms.CheckBox();
            this.确定button = new System.Windows.Forms.Button();
            this.取消button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // 车辆类型comboBox
            // 
            this.车辆类型comboBox.FormattingEnabled = true;
            this.车辆类型comboBox.Location = new System.Drawing.Point(102, 50);
            this.车辆类型comboBox.Name = "车辆类型comboBox";
            this.车辆类型comboBox.Size = new System.Drawing.Size(121, 20);
            this.车辆类型comboBox.TabIndex = 0;
            this.车辆类型comboBox.SelectedIndexChanged += new System.EventHandler(this.车辆类型comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "车辆类型";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(255, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "品牌";
            // 
            // 品牌comboBox
            // 
            this.品牌comboBox.FormattingEnabled = true;
            this.品牌comboBox.Location = new System.Drawing.Point(290, 50);
            this.品牌comboBox.Name = "品牌comboBox";
            this.品牌comboBox.Size = new System.Drawing.Size(121, 20);
            this.品牌comboBox.TabIndex = 3;
            this.品牌comboBox.SelectedIndexChanged += new System.EventHandler(this.品牌comboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(451, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "颜色";
            // 
            // 颜色comboBox
            // 
            this.颜色comboBox.FormattingEnabled = true;
            this.颜色comboBox.Location = new System.Drawing.Point(486, 50);
            this.颜色comboBox.Name = "颜色comboBox";
            this.颜色comboBox.Size = new System.Drawing.Size(121, 20);
            this.颜色comboBox.TabIndex = 5;
            this.颜色comboBox.SelectedIndexChanged += new System.EventHandler(this.颜色comboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "车牌号";
            // 
            // 车牌号textBox
            // 
            this.车牌号textBox.Location = new System.Drawing.Point(102, 92);
            this.车牌号textBox.Name = "车牌号textBox";
            this.车牌号textBox.Size = new System.Drawing.Size(121, 21);
            this.车牌号textBox.TabIndex = 7;
            this.车牌号textBox.TextChanged += new System.EventHandler(this.车牌号textBox_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(257, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "车主";
            // 
            // 车主textBox
            // 
            this.车主textBox.Location = new System.Drawing.Point(292, 92);
            this.车主textBox.Name = "车主textBox";
            this.车主textBox.Size = new System.Drawing.Size(119, 21);
            this.车主textBox.TabIndex = 9;
            this.车主textBox.TextChanged += new System.EventHandler(this.车主textBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(453, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "电话";
            // 
            // 电话textBox
            // 
            this.电话textBox.Location = new System.Drawing.Point(488, 92);
            this.电话textBox.Name = "电话textBox";
            this.电话textBox.Size = new System.Drawing.Size(119, 21);
            this.电话textBox.TabIndex = 11;
            this.电话textBox.TextChanged += new System.EventHandler(this.电话textBox_TextChanged);
            // 
            // VIPcheckBox
            // 
            this.VIPcheckBox.AutoSize = true;
            this.VIPcheckBox.Location = new System.Drawing.Point(315, 146);
            this.VIPcheckBox.Name = "VIPcheckBox";
            this.VIPcheckBox.Size = new System.Drawing.Size(42, 16);
            this.VIPcheckBox.TabIndex = 13;
            this.VIPcheckBox.Text = "VIP";
            this.VIPcheckBox.UseVisualStyleBackColor = true;
            this.VIPcheckBox.CheckedChanged += new System.EventHandler(this.VIPcheckBox_CheckedChanged);
            // 
            // 确定button
            // 
            this.确定button.Location = new System.Drawing.Point(405, 182);
            this.确定button.Name = "确定button";
            this.确定button.Size = new System.Drawing.Size(75, 23);
            this.确定button.TabIndex = 14;
            this.确定button.Text = "确定";
            this.确定button.UseVisualStyleBackColor = true;
            this.确定button.Click += new System.EventHandler(this.确定button_Click);
            // 
            // 取消button
            // 
            this.取消button.Location = new System.Drawing.Point(172, 182);
            this.取消button.Name = "取消button";
            this.取消button.Size = new System.Drawing.Size(75, 23);
            this.取消button.TabIndex = 15;
            this.取消button.Text = "取消";
            this.取消button.UseVisualStyleBackColor = true;
            this.取消button.Click += new System.EventHandler(this.取消button_Click);
            // 
            // RegisterCarDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 224);
            this.Controls.Add(this.取消button);
            this.Controls.Add(this.确定button);
            this.Controls.Add(this.VIPcheckBox);
            this.Controls.Add(this.电话textBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.车主textBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.车牌号textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.颜色comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.品牌comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.车辆类型comboBox);
            this.Name = "RegisterCarDialog";
            this.Text = "RegisterCarDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox 车辆类型comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 品牌comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 颜色comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox 车牌号textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox 车主textBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox 电话textBox;
        private System.Windows.Forms.CheckBox VIPcheckBox;
        private System.Windows.Forms.Button 确定button;
        private System.Windows.Forms.Button 取消button;
    }
}