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
    public partial class FrmAttribute : Form
    {
        //枚举类型，表示当前窗体状态
        public enum FormMode { ViewOnly, AddNew, EditInfo }
        //用于传出数据的属性
        public string SpotID => txtSpotID.Text;
        public string SpotType => cmbType.Text;
        public int SpotStatus => cmbStatus.SelectedIndex;


        public FrmAttribute(FormMode mode, string id = "", string type = "", int status = 0)
        {
            InitializeComponent();

            //初始化下拉框
            if(type == "VIP")
            {
                cmbType.SelectedIndex = 1;
            }
            else
            {
                cmbType.SelectedIndex = 0;
            }

            if(status >= 0 && status <= cmbStatus.Items.Count)
            {
                cmbStatus.SelectedIndex = status;
            }
            else
            {
                cmbStatus.SelectedIndex = 0;
            }

            txtSpotID.Text = id;

            //根据模式调整界面状态
            switch(mode)
            {
                case FormMode.ViewOnly:
                    this.Text = "车位信息详情";
                    DisableAllInputs();
                    btnSave.Visible = false;
                    btnCancel.Text = "关闭";
                    break;
                case FormMode.EditInfo:
                    this.Text = "修改车位信息";
                    txtSpotID.Enabled = false;
                    break;
                case FormMode.AddNew:
                    this.Text = "录入新车位";
                    break;
            }
        }

        private void DisableAllInputs()
        {
            txtSpotID.Enabled = false;
            cmbType.Enabled = false;
            cmbStatus.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtSpotID.Text))
            {
                MessageBox.Show("车位编号不能为空！");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
