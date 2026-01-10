using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingLot
{
    public partial class RegisterCarDialog : Form
    {
        // 车辆信息属性（外部可访问）
        public string LicensePlate { get; private set; } = "";
        public VehicleType SelectedVehicleType { get; private set; }
        public CarBrand SelectedCarBrand { get; private set; }
        public CarColor SelectedCarColor { get; private set; }
        public string OwnerName { get; private set; } = "";
        public string OwnerPhone { get; private set; } = "";
        public bool IsVIP { get; private set; } = false;

        // 是否为编辑模式
        private bool isEditMode = false;
        private Car originalCar = null;

        public RegisterCarDialog()
        {
            InitializeComponent();
            InitializeData();
        }

        // 构造函数重载：用于编辑已有车辆信息
        public RegisterCarDialog(Car car) : this()
        {
            if (car == null) return;

            isEditMode = true;
            originalCar = car;

            // 设置对话框标题
            this.Text = "编辑车辆信息";

            // 填充现有车辆信息
            PopulateCarData(car);
        }

        // 初始化数据
        private void InitializeData()
        {
            // 初始化车辆类型下拉框
            车辆类型comboBox.Items.Clear();
            foreach (VehicleType type in Enum.GetValues(typeof(VehicleType)))
            {
                车辆类型comboBox.Items.Add(type);
            }
            if (车辆类型comboBox.Items.Count > 0)
            {
                车辆类型comboBox.SelectedIndex = 0;
            }

            // 初始化品牌下拉框
            品牌comboBox.Items.Clear();
            foreach (CarBrand brand in Enum.GetValues(typeof(CarBrand)))
            {
                品牌comboBox.Items.Add(brand);
            }
            if (品牌comboBox.Items.Count > 0)
            {
                品牌comboBox.SelectedIndex = 0;
            }

            // 初始化颜色下拉框
            颜色comboBox.Items.Clear();
            foreach (CarColor color in Enum.GetValues(typeof(CarColor)))
            {
                颜色comboBox.Items.Add(color);
            }
            if (颜色comboBox.Items.Count > 0)
            {
                颜色comboBox.SelectedIndex = 0;
            }

            // 设置文本框属性
            车牌号textBox.CharacterCasing = CharacterCasing.Upper;
            车牌号textBox.MaxLength = 10;

            // 设置电话文本框只能输入数字
            电话textBox.KeyPress += 电话textBox_KeyPress;

            // 设置窗体属性
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // 如果编辑模式，禁用车牌号修改
            if (isEditMode)
            {
                车牌号textBox.ReadOnly = true;
                车牌号textBox.BackColor = SystemColors.Control;
            }
        }

        // 填充现有车辆数据
        private void PopulateCarData(Car car)
        {
            // 设置车牌号
            车牌号textBox.Text = car.LicensePlate;
            LicensePlate = car.LicensePlate;

            // 设置车辆类型
            if (车辆类型comboBox.Items.Contains(car.Type))
            {
                车辆类型comboBox.SelectedItem = car.Type;
            }

            // 设置品牌
            if (品牌comboBox.Items.Contains(car.Brand))
            {
                品牌comboBox.SelectedItem = car.Brand;
            }

            // 设置颜色
            if (颜色comboBox.Items.Contains(car.Color))
            {
                颜色comboBox.Items.Contains(car.Color);
                颜色comboBox.SelectedItem = car.Color;
            }

            // 设置车主信息
            车主textBox.Text = car.OwnerName;
            OwnerName = car.OwnerName;

            // 设置电话
            电话textBox.Text = car.OwnerPhone;
            OwnerPhone = car.OwnerPhone;

            // 设置VIP状态
            VIPcheckBox.Checked = car.IsVIP;
            IsVIP = car.IsVIP;
        }

        // 验证输入数据
        private bool ValidateInput()
        {
            // 验证车牌号
            LicensePlate = 车牌号textBox.Text.Trim();
            if (string.IsNullOrEmpty(LicensePlate))
            {
                MessageBox.Show("请输入车牌号", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                车牌号textBox.Focus();
                return false;
            }

            // 车牌号格式验证（如果不是编辑模式）
            if (!isEditMode && !CarLicensePlateDialog.ValidateLicensePlateFormat(LicensePlate))
            {
                MessageBox.Show("车牌号格式不正确，请输入正确的车牌号", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                车牌号textBox.Focus();
                return false;
            }

            // 验证车辆类型
            if (车辆类型comboBox.SelectedItem == null)
            {
                MessageBox.Show("请选择车辆类型", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                车辆类型comboBox.Focus();
                return false;
            }
            SelectedVehicleType = (VehicleType)车辆类型comboBox.SelectedItem;

            // 验证品牌
            if (品牌comboBox.SelectedItem == null)
            {
                MessageBox.Show("请选择车辆品牌", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                品牌comboBox.Focus();
                return false;
            }
            SelectedCarBrand = (CarBrand)品牌comboBox.SelectedItem;

            // 验证颜色
            if (颜色comboBox.SelectedItem == null)
            {
                MessageBox.Show("请选择车辆颜色", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                颜色comboBox.Focus();
                return false;
            }
            SelectedCarColor = (CarColor)颜色comboBox.SelectedItem;

            // 验证车主姓名
            OwnerName = 车主textBox.Text.Trim();
            if (string.IsNullOrEmpty(OwnerName))
            {
                MessageBox.Show("请输入车主姓名", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                车主textBox.Focus();
                return false;
            }

            // 验证电话（可选）
            OwnerPhone = 电话textBox.Text.Trim();
            if (!string.IsNullOrEmpty(OwnerPhone) && OwnerPhone.Length < 7)
            {
                MessageBox.Show("电话号码长度至少7位", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                电话textBox.Focus();
                return false;
            }

            // 获取VIP状态
            IsVIP = VIPcheckBox.Checked;

            return true;
        }

        // 车辆类型选择变化事件
        private void 车辆类型comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (车辆类型comboBox.SelectedItem != null)
            {
                SelectedVehicleType = (VehicleType)车辆类型comboBox.SelectedItem;

                // 根据车辆类型可以做一些UI调整
                // 例如，如果是电动车，可以显示充电相关信息
                // 这里可以扩展
            }
        }

        // 品牌选择变化事件
        private void 品牌comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (品牌comboBox.SelectedItem != null)
            {
                SelectedCarBrand = (CarBrand)品牌comboBox.SelectedItem;
            }
        }

        // 颜色选择变化事件
        private void 颜色comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (颜色comboBox.SelectedItem != null)
            {
                SelectedCarColor = (CarColor)颜色comboBox.SelectedItem;

                // 可以在这里预览颜色
                // 例如：根据选择的颜色改变某个预览区域的背景色
                // 这里可以扩展
            }
        }

        // 车牌号文本框变化事件
        private void 车牌号textBox_TextChanged(object sender, EventArgs e)
        {
            LicensePlate = 车牌号textBox.Text.ToUpper().Trim();

            // 实时验证车牌号格式
            if (!string.IsNullOrEmpty(LicensePlate) && !isEditMode)
            {
                bool isValid = CarLicensePlateDialog.ValidateLicensePlateFormat(LicensePlate);
                车牌号textBox.ForeColor = isValid ? Color.Black : Color.Red;
            }
        }

        // 车主文本框变化事件
        private void 车主textBox_TextChanged(object sender, EventArgs e)
        {
            OwnerName = 车主textBox.Text.Trim();
        }

        // 电话文本框变化事件
        private void 电话textBox_TextChanged(object sender, EventArgs e)
        {
            OwnerPhone = 电话textBox.Text.Trim();
        }

        // 电话文本框按键事件（限制只能输入数字和退格键）
        private void 电话textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 允许数字、退格、加号、减号、空格
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != (char)Keys.Back &&
                e.KeyChar != '+' &&
                e.KeyChar != '-' &&
                e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        // VIP复选框变化事件
        private void VIPcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            IsVIP = VIPcheckBox.Checked;

            // 如果是VIP，可以显示一些额外信息或选项
            if (IsVIP)
            {
                // 可以在这里添加VIP相关的额外设置
                // 例如：显示VIP有效期输入框等
            }
        }

        // 取消按钮点击事件
        private void 取消button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // 确定按钮点击事件
        private void 确定button_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                // 如果是编辑模式，检查车牌号是否被其他车辆使用
                if (!isEditMode)
                {
                    // 检查车牌号是否已存在
                    ParkingLotManager.ParkingLotManager manager = ParkingLotManager.ParkingLotManager.Instance;
                    if (manager.GetCarByLicense(LicensePlate) != null)
                    {
                        MessageBox.Show("该车牌号已注册，请使用其他车牌号", "验证错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // 静态方法：显示对话框并创建车辆对象
        public static Car ShowDialogAndCreateCar(IWin32Window owner = null)
        {
            using (var dialog = new RegisterCarDialog())
            {
                if (dialog.ShowDialog(owner) == DialogResult.OK)
                {
                    return CreateCarFromDialog(dialog);
                }
                return null;
            }
        }

        // 静态方法：显示对话框并编辑车辆对象
        public static bool ShowDialogAndEditCar(Car car, IWin32Window owner = null)
        {
            using (var dialog = new RegisterCarDialog(car))
            {
                if (dialog.ShowDialog(owner) == DialogResult.OK)
                {
                    // 更新车辆信息
                    car.Type = dialog.SelectedVehicleType;
                    car.Brand = dialog.SelectedCarBrand;
                    car.Color = dialog.SelectedCarColor;
                    car.OwnerName = dialog.OwnerName;
                    car.OwnerPhone = dialog.OwnerPhone;
                    car.IsVIP = dialog.IsVIP;
                    return true;
                }
                return false;
            }
        }

        // 从对话框数据创建车辆对象
        private static Car CreateCarFromDialog(RegisterCarDialog dialog)
        {
            var car = new Car(dialog.LicensePlate, dialog.SelectedVehicleType)
            {
                Brand = dialog.SelectedCarBrand,
                Color = dialog.SelectedCarColor,
                OwnerName = dialog.OwnerName,
                OwnerPhone = dialog.OwnerPhone,
                IsVIP = dialog.IsVIP
            };
            return car;
        }
    }
}