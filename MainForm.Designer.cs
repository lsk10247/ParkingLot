
namespace ParkingLot
{
    partial class MainForm
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
            //Ensures that any ESRI libraries that have been used are unloaded in the correct order. 
            //Failure to do this may result in random crashes on exit due to the operating system unloading 
            //the libraries in the incorrect order. 
            ESRI.ArcGIS.ADF.COMSupport.AOUninitialize.Shutdown();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuExitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.车辆设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.信息查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停车ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.离场ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.车位设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.加载地图ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.车位信息查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加车位ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制矩形车位ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.绘制多边形车位ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改信息ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.停止操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusBarXY = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.车辆设置ToolStripMenuItem,
            this.车位设置ToolStripMenuItem,
            this.停止操作ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(859, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewDoc,
            this.menuOpenDoc,
            this.menuSaveDoc,
            this.menuSaveAs,
            this.menuSeparator,
            this.menuExitApp});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(39, 21);
            this.menuFile.Text = "File";
            // 
            // menuNewDoc
            // 
            this.menuNewDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuNewDoc.Image")));
            this.menuNewDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuNewDoc.Name = "menuNewDoc";
            this.menuNewDoc.Size = new System.Drawing.Size(180, 22);
            this.menuNewDoc.Text = "New Document";
            this.menuNewDoc.Click += new System.EventHandler(this.menuNewDoc_Click);
            // 
            // menuOpenDoc
            // 
            this.menuOpenDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuOpenDoc.Image")));
            this.menuOpenDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuOpenDoc.Name = "menuOpenDoc";
            this.menuOpenDoc.Size = new System.Drawing.Size(180, 22);
            this.menuOpenDoc.Text = "Open Document...";
            this.menuOpenDoc.Click += new System.EventHandler(this.menuOpenDoc_Click);
            // 
            // menuSaveDoc
            // 
            this.menuSaveDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuSaveDoc.Image")));
            this.menuSaveDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuSaveDoc.Name = "menuSaveDoc";
            this.menuSaveDoc.Size = new System.Drawing.Size(180, 22);
            this.menuSaveDoc.Text = "SaveDocument";
            this.menuSaveDoc.Click += new System.EventHandler(this.menuSaveDoc_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(180, 22);
            this.menuSaveAs.Text = "Save As...";
            this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
            // 
            // menuSeparator
            // 
            this.menuSeparator.Name = "menuSeparator";
            this.menuSeparator.Size = new System.Drawing.Size(177, 6);
            // 
            // menuExitApp
            // 
            this.menuExitApp.Name = "menuExitApp";
            this.menuExitApp.Size = new System.Drawing.Size(180, 22);
            this.menuExitApp.Text = "Exit";
            this.menuExitApp.Click += new System.EventHandler(this.menuExitApp_Click);
            // 
            // 车辆设置ToolStripMenuItem
            // 
            this.车辆设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.信息查询ToolStripMenuItem,
            this.停车ToolStripMenuItem,
            this.离场ToolStripMenuItem,
            this.修改信息ToolStripMenuItem});
            this.车辆设置ToolStripMenuItem.Name = "车辆设置ToolStripMenuItem";
            this.车辆设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.车辆设置ToolStripMenuItem.Text = "车辆";
            // 
            // 信息查询ToolStripMenuItem
            // 
            this.信息查询ToolStripMenuItem.Name = "信息查询ToolStripMenuItem";
            this.信息查询ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.信息查询ToolStripMenuItem.Text = "信息查询";
            // 
            // 停车ToolStripMenuItem
            // 
            this.停车ToolStripMenuItem.Name = "停车ToolStripMenuItem";
            this.停车ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.停车ToolStripMenuItem.Text = "停车";
            this.停车ToolStripMenuItem.Click += new System.EventHandler(this.停车ToolStripMenuItem_Click);
            // 
            // 离场ToolStripMenuItem
            // 
            this.离场ToolStripMenuItem.Name = "离场ToolStripMenuItem";
            this.离场ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.离场ToolStripMenuItem.Text = "离场";
            this.离场ToolStripMenuItem.Click += new System.EventHandler(this.离场ToolStripMenuItem_Click);
            // 
            // 修改信息ToolStripMenuItem
            // 
            this.修改信息ToolStripMenuItem.Name = "修改信息ToolStripMenuItem";
            this.修改信息ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.修改信息ToolStripMenuItem.Text = "修改信息";
            // 
            // 车位设置ToolStripMenuItem
            // 
            this.车位设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.加载地图ToolStripMenuItem1,
            this.车位信息查询ToolStripMenuItem,
            this.添加车位ToolStripMenuItem1,
            this.修改信息ToolStripMenuItem2});
            this.车位设置ToolStripMenuItem.Name = "车位设置ToolStripMenuItem";
            this.车位设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.车位设置ToolStripMenuItem.Text = "车位";
            // 
            // 加载地图ToolStripMenuItem1
            // 
            this.加载地图ToolStripMenuItem1.Name = "加载地图ToolStripMenuItem1";
            this.加载地图ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.加载地图ToolStripMenuItem1.Text = "加载地图";
            this.加载地图ToolStripMenuItem1.Click += new System.EventHandler(this.加载地图ToolStripMenuItem1_Click);
            // 
            // 车位信息查询ToolStripMenuItem
            // 
            this.车位信息查询ToolStripMenuItem.Name = "车位信息查询ToolStripMenuItem";
            this.车位信息查询ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.车位信息查询ToolStripMenuItem.Text = "信息查询";
            this.车位信息查询ToolStripMenuItem.Click += new System.EventHandler(this.车位信息查询ToolStripMenuItem_Click);
            // 
            // 添加车位ToolStripMenuItem1
            // 
            this.添加车位ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.绘制矩形车位ToolStripMenuItem,
            this.绘制多边形车位ToolStripMenuItem});
            this.添加车位ToolStripMenuItem1.Name = "添加车位ToolStripMenuItem1";
            this.添加车位ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.添加车位ToolStripMenuItem1.Text = "添加车位";
            // 
            // 绘制矩形车位ToolStripMenuItem
            // 
            this.绘制矩形车位ToolStripMenuItem.Name = "绘制矩形车位ToolStripMenuItem";
            this.绘制矩形车位ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制矩形车位ToolStripMenuItem.Text = "矩形";
            this.绘制矩形车位ToolStripMenuItem.Click += new System.EventHandler(this.绘制矩形车位ToolStripMenuItem_Click);
            // 
            // 绘制多边形车位ToolStripMenuItem
            // 
            this.绘制多边形车位ToolStripMenuItem.Name = "绘制多边形车位ToolStripMenuItem";
            this.绘制多边形车位ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.绘制多边形车位ToolStripMenuItem.Text = "多边形";
            this.绘制多边形车位ToolStripMenuItem.Click += new System.EventHandler(this.绘制多边形车位ToolStripMenuItem_Click);
            // 
            // 修改信息ToolStripMenuItem2
            // 
            this.修改信息ToolStripMenuItem2.Name = "修改信息ToolStripMenuItem2";
            this.修改信息ToolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
            this.修改信息ToolStripMenuItem2.Text = "修改信息";
            this.修改信息ToolStripMenuItem2.Click += new System.EventHandler(this.修改信息ToolStripMenuItem2_Click);
            // 
            // 停止操作ToolStripMenuItem
            // 
            this.停止操作ToolStripMenuItem.Name = "停止操作ToolStripMenuItem";
            this.停止操作ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.停止操作ToolStripMenuItem.Text = "停止操作";
            this.停止操作ToolStripMenuItem.Click += new System.EventHandler(this.停止操作ToolStripMenuItem_Click);
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(238, 53);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(621, 466);
            this.axMapControl1.TabIndex = 2;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);
            this.axMapControl1.OnMapReplaced += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMapReplacedEventHandler(this.axMapControl1_OnMapReplaced);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 25);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(859, 28);
            this.axToolbarControl1.TabIndex = 3;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.axTOCControl1.Location = new System.Drawing.Point(3, 53);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(235, 466);
            this.axTOCControl1.TabIndex = 4;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(466, 278);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 5;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 53);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 488);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarXY});
            this.statusStrip1.Location = new System.Drawing.Point(3, 519);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(856, 22);
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusBar1";
            // 
            // statusBarXY
            // 
            this.statusBarXY.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.statusBarXY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusBarXY.Name = "statusBarXY";
            this.statusBarXY.Size = new System.Drawing.Size(57, 17);
            this.statusBarXY.Text = "Test 123";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 541);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "ArcEngine Controls Application";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuNewDoc;
        private System.Windows.Forms.ToolStripMenuItem menuOpenDoc;
        private System.Windows.Forms.ToolStripMenuItem menuSaveDoc;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem menuExitApp;
        private System.Windows.Forms.ToolStripSeparator menuSeparator;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusBarXY;
        private System.Windows.Forms.ToolStripMenuItem 车辆设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 车位设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 信息查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停车ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 离场ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 加载地图ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 车位信息查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加车位ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 修改信息ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 停止操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制矩形车位ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 绘制多边形车位ToolStripMenuItem;
    }
}

