using System;
using System.Drawing;
using System.Windows.Forms;
using ParkingLotManager;

namespace ParkingLot
{
    public partial class ParkingLotVisualizationForm : Form
    {
        ParkingLotManager.ParkingLotManager _parkingManager = ParkingLotManager.ParkingLotManager.Instance;
        // 车位总数（可从配置/Manager获取）
        private const int TotalSpots = 50;
        private ToolTip _parkingSpotToolTip = null;

        public ParkingLotVisualizationForm(ParkingLotManager.ParkingLotManager parkingManager)
        {
            _parkingSpotToolTip = new ToolTip();
            InitializeComponent();
            // 第三步：初始化ParkingManager，增加空值检查
            _parkingManager = parkingManager ?? ParkingLotManager.ParkingLotManager.Instance;
            if (_parkingManager == null)
            {
                throw new InvalidOperationException("ParkingLotManager 实例未初始化！请检查单例实现。");
            }

            // 第四步：检查核心控件，提前发现问题
            if (tlpParkingSpots == null)
            {
                throw new InvalidOperationException("tlpParkingSpots 控件未找到！请检查窗体设计器。");
            }
            // 初始化加载车位
            LoadParkingSpots();
            UpdateSpotStats();
        }

        // 加载车位网格
        private void LoadParkingSpots()
        {
            tlpParkingSpots.Controls.Clear();
            // 遍历所有车位，生成车位控件
            for (int i = 0; i < TotalSpots; i++)
            {
                var spotLabel = new Label
                {
                    Text = $"车位{i + 1}",
                    BorderStyle = BorderStyle.FixedSingle,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(1)
                };

                // 检查车位是否被占用，设置样式
                var car = _parkingManager.GetCarBySpotNumber(i + 1); // 需在ParkingLotManager实现该方法
                if (car != null)
                {
                    spotLabel.BackColor = Color.LightCoral; // 占用：红色
                    _parkingSpotToolTip.SetToolTip(spotLabel, "车牌号：{car.LicensePlate}\n入场时间：{car.EntryTime}");
                }
                else
                {
                    spotLabel.BackColor = Color.LightGreen; // 空闲：绿色
                    _parkingSpotToolTip.SetToolTip(spotLabel, "空闲车位");
                }

                tlpParkingSpots.Controls.Add(spotLabel);

            }
        }

        // 更新车位统计信息
        private void UpdateSpotStats()
        {
            int usedSpots = _parkingManager.GetUsedSpotCount(); // 需在ParkingLotManager实现该方法
            lblTotalSpots.Text = $"总车位：{TotalSpots}";
            lblUsedSpots.Text = $"已占用：{usedSpots}";
        }

        // 刷新按钮点击事件
        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            LoadParkingSpots();
            UpdateSpotStats();
            MessageBox.Show("车位状态已刷新！");
        }

    }
}
