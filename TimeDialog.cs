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
    public partial class TimeDialog : Form
    {
        // 选择的日期时间属性
        public DateTime SelectedDateTime { get; private set; }

        // 是否为仅选择时间模式（true: 仅时间，false: 日期和时间）
        public bool TimeOnlyMode { get; set; }

        // 最小允许时间
        public DateTime MinDateTime { get; set; }

        // 最大允许时间
        public DateTime MaxDateTime { get; set; }

        // 是否允许选择过去时间
        public bool AllowPastTime { get; set; }

        public TimeDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        // 构造函数重载：设置默认时间
        public TimeDialog(DateTime defaultTime) : this()
        {
            dateTimePicker1.Value = defaultTime;
            SelectedDateTime = defaultTime;
        }

        // 初始化对话框
        private void InitializeDialog()
        {
            // 设置窗体属性
            this.Text = "选择时间";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(350, 200);

            // 设置默认值
            SelectedDateTime = DateTime.Now.AddMinutes(10);
            MinDateTime = DateTime.MinValue;
            MaxDateTime = DateTime.MaxValue;
            AllowPastTime = false;
            TimeOnlyMode = false;

            // 初始化DateTimePicker
            dateTimePicker1.Location = new Point(20, 30);
            dateTimePicker1.Size = new Size(300, 20);
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm";
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.Value = SelectedDateTime;

            // 设置按钮
            button1.Text = "确定";
            button1.Location = new Point(100, 100);
            button1.Size = new Size(80, 30);
            button1.Click += button1_Click;

            button2.Text = "取消";
            button2.Location = new Point(200, 100);
            button2.Size = new Size(80, 30);
            button2.Click += button2_Click;

            // 添加标签
            var label = new Label
            {
                Text = "请选择时间:",
                Location = new Point(20, 10),
                Size = new Size(200, 20),
                Font = new Font("Microsoft YaHei", 9, FontStyle.Regular)
            };

            this.Controls.Add(label);
            this.Controls.Add(dateTimePicker1);
            this.Controls.Add(button1);
            this.Controls.Add(button2);

            // 绑定值变化事件
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
        }

        // 设置仅选择时间模式
        public void SetTimeOnlyMode(bool timeOnly)
        {
            TimeOnlyMode = timeOnly;
            if (timeOnly)
            {
                dateTimePicker1.Format = DateTimePickerFormat.Time;
                dateTimePicker1.ShowUpDown = true;
                this.Text = "选择时间";
            }
            else
            {
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm";
                dateTimePicker1.ShowUpDown = true;
                this.Text = "选择日期时间";
            }
        }

        // 设置时间范围
        public void SetDateTimeRange(DateTime minTime, DateTime maxTime)
        {
            MinDateTime = minTime;
            MaxDateTime = maxTime;

            dateTimePicker1.MinDate = minTime;
            dateTimePicker1.MaxDate = maxTime;
        }

        // 设置默认时间并自动调整范围
        public void SetDefaultDateTime(DateTime defaultTime)
        {
            if (defaultTime < MinDateTime) defaultTime = MinDateTime;
            if (defaultTime > MaxDateTime) defaultTime = MaxDateTime;

            dateTimePicker1.Value = defaultTime;
            SelectedDateTime = defaultTime;
        }

        // 验证选择的时间
        private bool ValidateSelectedTime()
        {
            SelectedDateTime = dateTimePicker1.Value;

            // 检查是否在允许范围内
            if (SelectedDateTime < MinDateTime)
            {
                MessageBox.Show($"选择的时间不能早于 {MinDateTime:yyyy-MM-dd HH:mm}",
                    "时间无效", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (SelectedDateTime > MaxDateTime)
            {
                MessageBox.Show($"选择的时间不能晚于 {MaxDateTime:yyyy-MM-dd HH:mm}",
                    "时间无效", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 检查是否允许选择过去时间
            if (!AllowPastTime && SelectedDateTime < DateTime.Now)
            {
                var result = MessageBox.Show("选择的时间已过时，是否继续使用？",
                    "时间已过时", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    dateTimePicker1.Value = DateTime.Now;
                    SelectedDateTime = DateTime.Now;
                    return false;
                }
            }

            return true;
        }

        // 日期时间选择变化事件
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SelectedDateTime = dateTimePicker1.Value;

            // 可以在这里添加实时验证或显示提示
            if (!AllowPastTime && SelectedDateTime < DateTime.Now)
            {
                // 可以显示一个提示标签
                ShowWarningLabel("选择的时间已过时");
            }
            else
            {
                HideWarningLabel();
            }
        }

        // 显示警告标签
        private void ShowWarningLabel(string message)
        {
            var warningLabel = this.Controls.OfType<Label>()
                .FirstOrDefault(l => l.Name == "warningLabel");

            if (warningLabel == null)
            {
                warningLabel = new Label
                {
                    Name = "warningLabel",
                    Text = message,
                    Location = new Point(20, 70),
                    Size = new Size(300, 20),
                    ForeColor = Color.Red,
                    Font = new Font("Microsoft YaHei", 8, FontStyle.Italic)
                };
                this.Controls.Add(warningLabel);
                this.Height += 25;
            }
            else
            {
                warningLabel.Text = message;
            }
        }

        // 隐藏警告标签
        private void HideWarningLabel()
        {
            var warningLabel = this.Controls.OfType<Label>()
                .FirstOrDefault(l => l.Name == "warningLabel");

            if (warningLabel != null)
            {
                this.Controls.Remove(warningLabel);
                this.Height -= 25;
            }
        }

        // 确定按钮点击事件
        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateSelectedTime())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // 取消按钮点击事件
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // 静态方法：显示对话框并获取时间
        public static DateTime? ShowDialogAndGetTime(IWin32Window owner = null,
            string title = "选择时间", DateTime? defaultTime = null,
            bool allowPastTime = false)
        {
            using (var dialog = new TimeDialog())
            {
                dialog.Text = title;
                dialog.AllowPastTime = allowPastTime;

                if (defaultTime.HasValue)
                {
                    dialog.SetDefaultDateTime(defaultTime.Value);
                }

                if (dialog.ShowDialog(owner) == DialogResult.OK)
                {
                    return dialog.SelectedDateTime;
                }
                return null;
            }
        }

        // 静态方法：显示对话框并获取时间（带时间范围限制）
        public static DateTime? ShowDialogAndGetTimeWithRange(IWin32Window owner = null,
            string title = "选择时间", DateTime? defaultTime = null,
            DateTime? minTime = null, DateTime? maxTime = null,
            bool allowPastTime = false)
        {
            using (var dialog = new TimeDialog())
            {
                dialog.Text = title;
                dialog.AllowPastTime = allowPastTime;

                if (minTime.HasValue && maxTime.HasValue)
                {
                    dialog.SetDateTimeRange(minTime.Value, maxTime.Value);
                }
                else if (minTime.HasValue)
                {
                    dialog.SetDateTimeRange(minTime.Value, DateTime.MaxValue);
                }
                else if (maxTime.HasValue)
                {
                    dialog.SetDateTimeRange(DateTime.MinValue, maxTime.Value);
                }

                if (defaultTime.HasValue)
                {
                    dialog.SetDefaultDateTime(defaultTime.Value);
                }

                if (dialog.ShowDialog(owner) == DialogResult.OK)
                {
                    return dialog.SelectedDateTime;
                }
                return null;
            }
        }

        // 静态方法：仅选择时间（不选择日期）
        public static TimeSpan? ShowDialogAndGetTimeOnly(IWin32Window owner = null,
            string title = "选择时间", TimeSpan? defaultTime = null)
        {
            using (var dialog = new TimeDialog())
            {
                dialog.Text = title;
                dialog.SetTimeOnlyMode(true);

                if (defaultTime.HasValue)
                {
                    DateTime today = DateTime.Today;
                    dialog.SetDefaultDateTime(today.Add(defaultTime.Value));
                }

                if (dialog.ShowDialog(owner) == DialogResult.OK)
                {
                    return dialog.SelectedDateTime.TimeOfDay;
                }
                return null;
            }
        }

        // 静态方法：选择停车时长（以分钟为单位）
        public static int? ShowDialogAndGetDuration(IWin32Window owner = null,
            string title = "选择停车时长", int defaultMinutes = 60,
            int minMinutes = 15, int maxMinutes = 1440) // 默认15分钟到24小时
        {
            using (var dialog = new DurationDialog(defaultMinutes, minMinutes, maxMinutes))
            {
                dialog.Text = title;

                if (dialog.ShowDialog(owner) == DialogResult.OK)
                {
                    return dialog.SelectedMinutes;
                }
                return null;
            }
        }
    }

    // 时长选择对话框（选择多少分钟）
    public class DurationDialog : Form
    {
        private NumericUpDown minutesUpDown;
        private Button okButton;
        private Button cancelButton;

        public int SelectedMinutes { get; private set; }

        public DurationDialog(int defaultMinutes = 60, int minMinutes = 15, int maxMinutes = 1440)
        {
            InitializeComponent(defaultMinutes, minMinutes, maxMinutes);
        }

        private void InitializeComponent(int defaultMinutes, int minMinutes, int maxMinutes)
        {
            this.Text = "选择时长";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(300, 200);

            // 创建控件
            Label label = new Label
            {
                Text = "请选择停车时长（分钟）:",
                Location = new Point(20, 30),
                Size = new Size(200, 20)
            };

            minutesUpDown = new NumericUpDown
            {
                Location = new Point(20, 60),
                Size = new Size(100, 20),
                Minimum = minMinutes,
                Maximum = maxMinutes,
                Value = defaultMinutes,
                Increment = 15 // 15分钟递增
            };

            // 显示小时和分钟的标签
            Label hourLabel = new Label
            {
                Text = $"约 {defaultMinutes / 60} 小时 {defaultMinutes % 60} 分钟",
                Location = new Point(130, 60),
                Size = new Size(150, 20),
                ForeColor = Color.Blue
            };

            minutesUpDown.ValueChanged += (s, e) =>
            {
                int totalMinutes = (int)minutesUpDown.Value;
                int hours = totalMinutes / 60;
                int minutes = totalMinutes % 60;

                hourLabel.Text = $"约 {hours} 小时 {minutes} 分钟";
            };

            okButton = new Button
            {
                Text = "确定",
                Location = new Point(60, 120),
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };

            cancelButton = new Button
            {
                Text = "取消",
                Location = new Point(160, 120),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            okButton.Click += (s, e) =>
            {
                SelectedMinutes = (int)minutesUpDown.Value;
            };

            this.Controls.Add(label);
            this.Controls.Add(minutesUpDown);
            this.Controls.Add(hourLabel);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }
    }
}