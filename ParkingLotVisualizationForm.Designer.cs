
namespace ParkingLot
{
    partial class ParkingLotVisualizationForm
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
            this.components = new System.ComponentModel.Container();
            this.tlpParkingSpots = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotalSpots = new System.Windows.Forms.Label();
            this.lblUsedSpots = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // tlpParkingSpots
            // 
            this.tlpParkingSpots.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpParkingSpots.ColumnCount = 10;
            this.tlpParkingSpots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpParkingSpots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpParkingSpots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpParkingSpots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpParkingSpots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpParkingSpots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpParkingSpots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpParkingSpots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpParkingSpots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpParkingSpots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpParkingSpots.Location = new System.Drawing.Point(3, 40);
            this.tlpParkingSpots.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tlpParkingSpots.Name = "tlpParkingSpots";
            this.tlpParkingSpots.RowCount = 5;
            this.tlpParkingSpots.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpParkingSpots.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpParkingSpots.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpParkingSpots.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpParkingSpots.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpParkingSpots.Size = new System.Drawing.Size(540, 250);
            this.tlpParkingSpots.TabIndex = 0;
            // 
            // lblTotalSpots
            // 
            this.lblTotalSpots.AutoSize = true;
            this.lblTotalSpots.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalSpots.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTotalSpots.Location = new System.Drawing.Point(0, 0);
            this.lblTotalSpots.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalSpots.Name = "lblTotalSpots";
            this.lblTotalSpots.Size = new System.Drawing.Size(73, 20);
            this.lblTotalSpots.TabIndex = 0;
            this.lblTotalSpots.Text = "总车位：0";
            // 
            // lblUsedSpots
            // 
            this.lblUsedSpots.AutoSize = true;
            this.lblUsedSpots.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.lblUsedSpots.Location = new System.Drawing.Point(129, 0);
            this.lblUsedSpots.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUsedSpots.Name = "lblUsedSpots";
            this.lblUsedSpots.Size = new System.Drawing.Size(73, 20);
            this.lblUsedSpots.TabIndex = 1;
            this.lblUsedSpots.Text = "已占用：0";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.btnRefresh.Location = new System.Drawing.Point(263, -2);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 26);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click_1);
            // 
            // ParkingLotVisualizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 295);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblUsedSpots);
            this.Controls.Add(this.lblTotalSpots);
            this.Controls.Add(this.tlpParkingSpots);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ParkingLotVisualizationForm";
            this.Text = "可视化";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpParkingSpots;
        private System.Windows.Forms.Label lblTotalSpots;
        private System.Windows.Forms.Label lblUsedSpots;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}