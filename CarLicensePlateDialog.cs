using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingLot
{
    public partial class CarLicensePlateDialog : Form
    {
        // 车牌号属性（外部可访问）
        public string LicensePlate { get; private set; } = "";

        // 选择的省份简称
        public string Province { get; private set; } = "";

        // 选择的城市字母
        public string CityCode { get; private set; } = "";

        // 车牌号码部分
        public string PlateNumber { get; private set; } = "";

        // 省份与城市的映射关系
        private Dictionary<string, List<string>> provinceCityMap;

        public CarLicensePlateDialog()
        {
            InitializeComponent();
            InitializeData();
        }
        // 初始化数据
        private void InitializeData()
        {
            // 初始化省份和城市的映射关系
            provinceCityMap = new Dictionary<string, List<string>>
            {
                // 华北地区
                { "京", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" } },
                { "津", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" } },
                { "冀", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "R", "T" } },
                { "晋", new List<string> { "A", "B", "C", "D", "E", "F", "H", "J", "K", "L", "M" } },
                { "蒙", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M" } },
                
                // 东北地区
                { "辽", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P" } },
                { "吉", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K" } },
                { "黑", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "R" } },
                
                // 华东地区
                { "沪", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" } },
                { "苏", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N" } },
                { "浙", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" } },
                { "皖", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" } },
                { "闽", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" } },
                { "赣", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M" } },
                { "鲁", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "U", "V", "W", "X", "Y", "Z" } },
                
                // 华中地区
                { "豫", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "U", "V", "W", "X", "Y", "Z" } },
                { "鄂", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" } },
                { "湘", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" } },
                
                // 华南地区
                { "粤", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" } },
                { "桂", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "R" } },
                { "琼", new List<string> { "A", "B", "C", "D", "E", "F" } },
                
                // 西南地区
                { "川", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" } },
                { "贵", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "J" } },
                { "云", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" } },
                { "渝", new List<string> { "A", "B", "C", "D", "F", "G", "H" } },
                { "藏", new List<string> { "A", "B", "C", "D", "E", "F", "G" } },
                
                // 西北地区
                { "陕", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "V" } },
                { "甘", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P" } },
                { "青", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H" } },
                { "宁", new List<string> { "A", "B", "C", "D", "E" } },
                { "新", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" } }
            };

            // 初始化省份下拉框选项
            省comboBox.Items.Clear();
            foreach (var province in provinceCityMap.Keys)
            {
                省comboBox.Items.Add(province);
            }

            // 设置默认选择第一个省份（如果存在）
            if (省comboBox.Items.Count > 0)
            {
                省comboBox.SelectedIndex = 0;
            }
        }

        // 更新车牌预览
        private void UpdateLicensePreview()
        {
            var preview = $"{Province}{CityCode}{PlateNumber}";

            // 找到预览标签并更新
            foreach (Control control in this.Controls)
            {
                if (control.Name == "previewText")
                {
                    control.Text = preview;
                    break;
                }
            }

            // 更新完整车牌号
            LicensePlate = preview;
        }

        // 验证车牌号
        private bool ValidateLicensePlate()
        {
            // 检查省份是否选择
            if (string.IsNullOrEmpty(Province))
            {
                MessageBox.Show("请选择省份", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                省comboBox.Focus();
                return false;
            }

            // 检查城市是否选择
            if (string.IsNullOrEmpty(CityCode))
            {
                MessageBox.Show("请选择城市", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                市comboBox.Focus();
                return false;
            }

            // 检查号码是否输入
            if (string.IsNullOrEmpty(PlateNumber))
            {
                MessageBox.Show("请输入车牌号码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                号码textBox.Focus();
                return false;
            }

            // 检查号码长度（普通车牌5-6位，新能源车牌8位）
            if (PlateNumber.Length < 5 || PlateNumber.Length > 8)
            {
                MessageBox.Show("车牌号码长度应为5-8位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                号码textBox.Focus();
                return false;
            }

            // 检查号码格式（字母和数字组合）
            var pattern = @"^[A-Z0-9]{5,8}$";
            if (!Regex.IsMatch(PlateNumber, pattern))
            {
                MessageBox.Show("车牌号码只能包含大写字母和数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                号码textBox.Focus();
                return false;
            }

            // 新能源车牌特殊验证（8位，D或F开头）
            if (PlateNumber.Length == 8)
            {
                if (!(PlateNumber.StartsWith("D") || PlateNumber.StartsWith("F")))
                {
                    MessageBox.Show("新能源车牌应以D或F开头", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    号码textBox.Focus();
                    return false;
                }
            }

            // 组合完整车牌号
            LicensePlate = $"{Province}{CityCode}{PlateNumber}";

            return true;
        }

        // 静态方法：显示对话框并获取车牌号
        public static string ShowDialogAndGetLicensePlate(IWin32Window owner = null)
        {
            using (var dialog = new CarLicensePlateDialog())
            {
                if (dialog.ShowDialog(owner) == DialogResult.OK)
                {
                    return dialog.LicensePlate;
                }
                return null;
            }
        }

        // 静态方法：显示对话框并获取完整车辆信息
        public static bool ShowDialogAndGetCarInfo(out string licensePlate, out string province, out string cityCode, IWin32Window owner = null)
        {
            licensePlate = "";
            province = "";
            cityCode = "";

            using (var dialog = new CarLicensePlateDialog())
            {
                if (dialog.ShowDialog(owner) == DialogResult.OK)
                {
                    licensePlate = dialog.LicensePlate;
                    province = dialog.Province;
                    cityCode = dialog.CityCode;
                    return true;
                }
                return false;
            }
        }

        // 设置默认车牌号（用于编辑模式）
        public void SetLicensePlate(string licensePlate)
        {
            if (string.IsNullOrEmpty(licensePlate) || licensePlate.Length < 2)
                return;

            try
            {
                // 解析车牌号
                string province = licensePlate.Substring(0, 1);
                string cityCode = licensePlate.Substring(1, 1);
                string number = licensePlate.Substring(2);

                // 设置省份
                if (省comboBox.Items.Contains(province))
                {
                    省comboBox.SelectedItem = province;
                }

                // 等待城市下拉框加载
                System.Windows.Forms.Application.DoEvents();

                // 设置城市
                if (市comboBox.Items.Contains(cityCode))
                {
                    市comboBox.SelectedItem = cityCode;
                }

                // 设置号码
                号码textBox.Text = number;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"设置车牌号失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 验证车牌号格式（静态方法，可在其他地方调用）
        public static bool ValidateLicensePlateFormat(string licensePlate)
        {
            if (string.IsNullOrEmpty(licensePlate) || licensePlate.Length < 7)
                return false;

            // 基本格式：省份(1位) + 城市(1位) + 号码(5-6位)
            string pattern = @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-Z][A-Z0-9]{4,5}[A-Z0-9挂学警港澳]$";

            // 新能源车牌：省份(1位) + 城市(1位) + D/F + 6位字母数字
            string newEnergyPattern = @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-Z][DF][A-Z0-9]{5}$";

            return Regex.IsMatch(licensePlate, pattern) || Regex.IsMatch(licensePlate, newEnergyPattern);
        }

        // 解析车牌号（静态方法，可在其他地方调用）
        public static bool ParseLicensePlate(string licensePlate, out string province, out string cityCode, out string number)
        {
            province = "";
            cityCode = "";
            number = "";

            if (string.IsNullOrEmpty(licensePlate) || licensePlate.Length < 7)
                return false;

            try
            {
                province = licensePlate.Substring(0, 1);
                cityCode = licensePlate.Substring(1, 1);
                number = licensePlate.Substring(2);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void 确定button_Click(object sender, EventArgs e)
        {
            // 验证车牌号
            if (ValidateLicensePlate())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void 取消button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void 省comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (省comboBox.SelectedItem == null) return;

            Province = 省comboBox.SelectedItem.ToString();

            // 清空城市下拉框并重新填充
            市comboBox.Items.Clear();

            if (provinceCityMap.ContainsKey(Province))
            {
                foreach (var city in provinceCityMap[Province])
                {
                    市comboBox.Items.Add(city);
                }

                // 选择第一个城市
                if (市comboBox.Items.Count > 0)
                {
                    市comboBox.SelectedIndex = 0;
                }
            }

            // 更新车牌预览
            UpdateLicensePreview();
        }

        private void 市comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (市comboBox.SelectedItem == null) return;

            CityCode = 市comboBox.SelectedItem.ToString();

            // 更新车牌预览
            UpdateLicensePreview();
        }

        private void 号码textBox_TextChanged(object sender, EventArgs e)
        {
            PlateNumber = 号码textBox.Text.ToUpper();

            // 更新车牌预览
            UpdateLicensePreview();
        }
    }
}
