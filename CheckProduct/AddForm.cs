using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CheckProduct
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        SQL_Class SQLClass = new SQL_Class();

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if(txt_model.Text == "" || txt_num.Text == "" || txt_pic.Text == "" || txt_type.Text == "")
            {
                MessageBox.Show("输入框不能为空，请重新输入！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SqlDataReader dr = SQLClass.getcom("select 型号,图号,类型,流水号 from 基本信息表 where 流水号 = '" + txt_num.Text.ToString().Trim() + "'");      //型号 = '" + txt_model.Text.ToString().Trim() + "' or 图号 = '" + txt_pic.Text.ToString().Trim() + "' or 
            bool ifcom = dr.Read();
            if(ifcom)
            {
                MessageBox.Show("你输入的参数已经存在，请重新输入！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SQLClass.getsqlcom("insert into 基本信息表 values ('" + txt_model.Text + "','" + txt_pic.Text + "','" + txt_type.Text + "','" + txt_num.Text + "','" + cBox.Text + "')");
                MessageBox.Show("添加基本信息成功！！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            DataSet DSet = SQLClass.getDataSet("select ID from 数据界定表", "数据库信息");
            DataTable dt = DSet.Tables["数据库信息"];      //创建一个DataTable对象
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                cBox.Items.Add(dt.Rows[i][0]);
                cBox.SelectedIndex = 0;
            }
        }
    }
}
