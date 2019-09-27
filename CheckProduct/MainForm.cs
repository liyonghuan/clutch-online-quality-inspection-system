using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CheckProduct
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        SQL_Class SQLClass = new SQL_Class();

        private void MainForm_Load(object sender, EventArgs e)      //主窗体，窗体加载时所做动作
        {
            lab_result.Text = "";       //清空检测结果
            updatecBox();       //更新cBox
            timer1.Interval = 100;      //设定定时控件默认时间间隔，默认100毫秒
        }  

        private void 新建检测ToolStripMenuItem_Click(object sender, EventArgs e)        //新建产品检测
        {
            
        }

        private void 删除产品ToolStripMenuItem_Click(object sender, EventArgs e)        //删除产品，如果有检测结果数据则同时删除检测结果数据
        {
            //SQLClass.getsqlcom("delete from 基本信息表 where 流水号 = '" + lab_num.Text + "'");
            //updatecBox();
            //if (lab_num.ForeColor == Color.Red)
            //{
            //    SQLClass.getsqlcom("delete from 测量结果表 where 流水号 = '" + lab_num.Text + "'");
            //}
            //else
            //{
                
            //}
            //MessageBox.Show("检测记录删除成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 删除记录ToolStripMenuItem_Click(object sender, EventArgs e)        //删除检测结果数据
        {
            //if(lab_num.ForeColor == Color.Red)
            //{
            //    SQLClass.getsqlcom("delete from 测量结果表 where 流水号 = '" + lab_num.Text + "'");
            //    MessageBox.Show("检测记录删除成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    updatecBox();
            //}
            //else
            //{
            //    MessageBox.Show("该产品没有检测记录！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)     //退出程序
        {
            Application.Exit();
        }


        private void toolStripButton1_Click(object sender, EventArgs e)     //开始、暂停、继续按钮，按钮文本和动作根据操作自动变化
        {
            if(lab_num.ForeColor == Color.Red)
            {
                MessageBox.Show("已有测量结果数据，删除后才能继续！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(cBox_model.Text == "")
            {
                MessageBox.Show("没有需要测试的产品！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(toolStripButton1.Text == "开始")
            {
                //当点击开始时创建图表系列Series
                /*
                 * 例如：
                 * 图表名.Series.Clear();
                 * 图表名.Series.Add("系列名");
                 * 图表名.Series["系列名"].ChartType = SeriesChartType.Line;
                 * 
                 * 其中图表名为tCon_chart1、tCon_chart2
                 * 系列名为你设置的名称
                 * 
                 */
                tCon_chart1.Series.Clear();     //清除图表中的系列
                tCon_chart2.Series.Clear();
                tCon_chart1.Series.Add("负荷特性");     //添加序列
                tCon_chart2.Series.Add("分离特性");
                tCon_chart1.Series["负荷特性"].ChartType = SeriesChartType.Line;        //设置图表类型
                tCon_chart2.Series["分离特性"].ChartType = SeriesChartType.Line;

                DataSet DSet = SQLClass.getDataSet("select Count(id) from 测量数据表 where 数据界定 = '" + cBox_model.Tag + "'", "数据信息表");
                DataTable dt = DSet.Tables["数据信息表"];

                toolStripButton1.Tag = dt.Rows[0][0].ToString();

                timer1.Enabled = true;
                timer1.Interval = 100;      //设置定时控件执行间隔时间
                toolStripButton1.Text = "暂停";
                startCheck();
            }
            else if(toolStripButton1.Text == "暂停")
            {
                timer1.Interval = 1000000000;       //设置定时控件间隔时间，此处目的是使定时控件处于假停止状态，点击继续后可以接着上次未完成结果继续执行
                toolStripButton1.Text = "继续";
            }
            else
            {
                timer1.Interval = 100;
                toolStripButton1.Text = "暂停";
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)     //停止按钮，停止检测动作
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                timer1.Tag = "0";
                SQLClass.getsqlcom("delete from 测量结果表 where 流水号 = '" + lab_num.Text + "'");
                lab_num.ForeColor = Color.Black;
                toolStripButton1.Text = "开始";
            }
            else
            {
                MessageBox.Show("尚未开始，无法停止！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void stopTimer()
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                timer1.Tag = "0";
                SQLClass.getsqlcom("delete from 测量结果表 where 流水号 = '" + lab_num.Text + "'");
                lab_num.ForeColor = Color.Black;
                toolStripButton1.Text = "开始";
            }
            else
            {
                MessageBox.Show("尚未开始，无法停止！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)        //定时控件，每个时间间隔执行一次
        {
            timer1.Tag = Convert.ToInt32(timer1.Tag) + 1;
            if(Convert.ToInt32(timer1.Tag) > Convert.ToInt32(toolStripButton1.Tag))
            {
                timer1.Tag = "0";
                timer1.Enabled = false;
                lab_result.Text = "合格";
                insertData();
            }
            else
            {
                getNew();
            }
        }

        private void cBox_model_SelectedIndexChanged(object sender, EventArgs e)        //cBox值变化时的动作
        {
            DataSet DSet = SQLClass.getDataSet("select 流水号,图号,类型,数据界定 from 基本信息表 where 型号 = '" + cBox_model.Text + "'", "数据信息表");
            DataTable dt = DSet.Tables["数据信息表"];
            if (dt.Rows.Count > 0)
            {
                lab_num.Text = dt.Rows[0][0].ToString();
                lab_pic.Text = dt.Rows[0][1].ToString();
                lab_type.Text = dt.Rows[0][2].ToString();
                cBox_model.Tag = dt.Rows[0][3].ToString();
            }
            else
            {
                lab_num.Text = "";
                lab_pic.Text = "";
                lab_type.Text = "";
                cBox_model.Tag = "";
            }
            getLimit();
            getResult();
        }

        private void updatecBox()       //更新cBox
        {
            cBox_model.Items.Clear();
            DataSet DSet = SQLClass.getDataSet("select 型号 from 基本信息表", "数据信息表");
            DataTable dt = DSet.Tables["数据信息表"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++ )
                {
                    cBox_model.Items.Add(dt.Rows[i][0].ToString());
                }
                cBox_model.SelectedIndex = 0;
            }
        }

        private void getLimit()     //获取数据界定表中的界定值
        {
            DataSet DSet = SQLClass.getDataSet("select 工作压紧力最大值,工作压紧力最小值,最小分离力最大值,最小分离力最小值,分离点分离力最大值,分离点分离力最小值,峰值分离力最大值,峰值分离力最小值,压盘升程最小值,压盘平行度最大值,分离指高度最大值,分离指高度最小值 from 数据界定表 where id = '" + cBox_model.Tag + "'", "数据信息表");
            DataTable dt = DSet.Tables["数据信息表"];
            if (dt.Rows.Count > 0)
            {
                lab1_max.Text = dt.Rows[0][0].ToString();
                lab1_min.Text = dt.Rows[0][1].ToString();
                lab3_max.Text = dt.Rows[0][2].ToString();
                lab3_min.Text = dt.Rows[0][3].ToString();
                lab4_max.Text = dt.Rows[0][4].ToString();
                lab4_min.Text = dt.Rows[0][5].ToString();
                lab5_max.Text = dt.Rows[0][6].ToString();
                lab5_min.Text = dt.Rows[0][7].ToString();
                lab6_min.Text = dt.Rows[0][8].ToString();
                lab7_max.Text = dt.Rows[0][9].ToString();
                lab8_max.Text = dt.Rows[0][10].ToString();
                lab8_min.Text = dt.Rows[0][11].ToString();
            }
            else
            {
                lab1_max.Text = "- - -";
                lab1_min.Text = "- - -";
                lab3_max.Text = "- - -";
                lab3_min.Text = "- - -";
                lab4_max.Text = "- - -";
                lab4_min.Text = "- - -";
                lab5_max.Text = "- - -";
                lab5_min.Text = "- - -";
                lab6_min.Text = "- - -";
                lab7_max.Text = "- - -";
                lab8_max.Text = "- - -";
                lab8_min.Text = "- - -";
            }
        }

        private void getResult()        //获取检测结果
        {
            DataSet DSet = SQLClass.getDataSet("select 工作压紧力,最小分离力,分离点分离力,峰值分离力,压盘升程,压盘平行度,分离指高度,离合器评价 from 测量结果表 where 流水号 = '" + lab_num.Text + "'", "数据信息表");
            DataTable dt = DSet.Tables["数据信息表"];
            if (dt.Rows.Count > 0)
            {
                lab_num.ForeColor = Color.Red;      //红色代表已有检测结果的产品
                lab1.Text = dt.Rows[0][0].ToString();
                lab3.Text = dt.Rows[0][1].ToString();
                lab4.Text = dt.Rows[0][2].ToString();
                lab5.Text = dt.Rows[0][3].ToString();
                lab6.Text = dt.Rows[0][4].ToString();
                lab7.Text = dt.Rows[0][5].ToString();
                lab8.Text = dt.Rows[0][6].ToString();
                lab_result.Text = dt.Rows[0][7].ToString();
            }
            else
            {
                lab_num.ForeColor = Color.Black;        //黑色代表未检测的产品
                lab1.Text = "- - -";
                lab3.Text = "- - -";
                lab4.Text = "- - -";
                lab5.Text = "- - -";
                lab6.Text = "- - -";
                lab7.Text = "- - -";
                lab8.Text = "- - -";
                lab_result.Text = "";
            }
        }

        private void getNew()       //获取测量数据表中的数据，当前至此顺序获取和随即获取。已注释掉的部分为顺序获取，默认随即获取，推荐使用随即获取方式
        {
            string sql;

            //顺序获取方式
            int x = 0;
            if (cBox_model.Tag.ToString() == "")
            {
                sql = "select 工作压紧力,分离力,压盘升程1,压盘升程2,压盘升程3,分离指高度,分离点分离力,底盘位移量,拉杆升程,压紧力 from 测量数据表 order by 数据编号";
            }
            else
            {
                sql = "select 工作压紧力,分离力,压盘升程1,压盘升程2,压盘升程3,分离指高度,分离点分离力,底盘位移量,拉杆升程,压紧力 from 测量数据表 where 数据界定 = '" + cBox_model.Tag + "' order by 数据编号";
            }
            DataSet DSet = SQLClass.getDataSet(sql, "数据信息表");
            DataTable dt = DSet.Tables["数据信息表"];
            if (dt.Rows.Count > 0)
            {
                if(dt.Rows.Count >= Convert.ToInt32(timer1.Tag))
                {
                    x = Convert.ToInt32(timer1.Tag) - 1;
                }
                else
                {
                    x = (Convert.ToInt32(timer1.Tag) - 1) % Convert.ToInt32(dt.Rows.Count);
                }
                lab_new_1.Text = dt.Rows[x][8].ToString();
                lab_new_2.Text = dt.Rows[x][1].ToString();
                lab_new_3.Text = dt.Rows[x][2].ToString();
                lab_new_4.Text = dt.Rows[x][3].ToString();
                lab_new_5.Text = dt.Rows[x][4].ToString();
                lab_new_6.Text = dt.Rows[x][9].ToString();
                lab_new_7.Text = dt.Rows[x][7].ToString();
                if (timer1.Tag.ToString() == "1")
                {
                    lab3.Text = dt.Rows[x][1].ToString();
                    lab5.Text = dt.Rows[x][1].ToString();
                } 
                float[] num = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int i = 0; i < 10; i++ )
                {
                    num[i] = Convert.ToSingle(dt.Rows[x][i]);
                }
                checkProduct(num);
                //图表控制图添加数据点
                /*
                 * 图表名.Series["系列名"].Points.AddXY(X的值, Y的值);
                 * 
                 */
                tCon_chart1.Series["负荷特性"].Points.AddXY(string.Format("{0:0.00}", Convert.ToSingle(dt.Rows[x][7].ToString())), string.Format("{0:0}", Convert.ToSingle(dt.Rows[x][9].ToString())));
                //tCon_chart2.Series["分离特性"].Points.AddXY(string.Format("{0:0.0}", Convert.ToSingle(dt.Rows[x][7].ToString())), string.Format("{0:0.0}", Convert.ToSingle(dt.Rows[x][9].ToString())));
            }
            else
            {
                if (timer1.Enabled)
                {
                    timer1.Enabled = false;
                    timer1.Tag = "0";
                    SQLClass.getsqlcom("delete from 测量结果表 where 流水号 = '" + lab_num.Text + "'");
                    lab_num.ForeColor = Color.Black;
                    toolStripButton1.Text = "开始";
                }
                else
                {
                    MessageBox.Show("尚未开始，无法停止！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                MessageBox.Show("糟糕！获取不到测量数据！！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            ////随机获取方式
            //if(cBox_model.Tag.ToString() == "")
            //{
            //    sql = "select 工作压紧力,分离力,压盘升程1,压盘升程2,压盘升程3,分离指高度,分离点分离力,底盘位移量 from 测量数据表 order by newid()";
            //}
            //else
            //{
            //    sql = "select 工作压紧力,分离力,压盘升程1,压盘升程2,压盘升程3,分离指高度,分离点分离力,底盘位移量 from 测量数据表 where 数据界定 = '" + cBox_model.Tag + "' order by newid()";
            //}
            //DataSet DSet = SQLClass.getDataSet(sql, "数据信息表");
            //DataTable dt = DSet.Tables["数据信息表"];
            //if (dt.Rows.Count > 0)
            //{
            //    lab_new_2.Text = dt.Rows[0][1].ToString();      //分离力
            //    lab_new_3.Text = dt.Rows[0][2].ToString();      //压盘升程1
            //    lab_new_4.Text = dt.Rows[0][3].ToString();      //压盘升程2
            //    lab_new_5.Text = dt.Rows[0][4].ToString();      //压盘升程3
            //    lab_new_6.Text = dt.Rows[0][0].ToString();      //工作压紧力
            //    if (timer1.Tag.ToString() == "1")
            //    {
            //        lab3.Text = dt.Rows[0][1].ToString();
            //        lab5.Text = dt.Rows[0][1].ToString();
            //    }
            //    float[] num = { 0, 0, 0, 0, 0, 0, 0 };
            //    for (int i = 0; i < 7; i++ )
            //    {
            //        num[i] = Convert.ToSingle(dt.Rows[0][i]);
            //    }
            //    checkProduct(num);

            //    //图表控制图添加数据点
            //    /*
            //     * 图表名.Series["系列名"].Points.AddXY(X的值, Y的值);
            //     * 
            //     */
            //    tCon_chart1.Series["Series1"].Points.AddXY(timer1.Tag.ToString(), dt.Rows[0][1].ToString());
            //}
            //else
            //{
            //    if (timer1.Enabled)
            //    {
            //        timer1.Enabled = false;
            //        timer1.Tag = "0";
            //        SQLClass.getsqlcom("delete from 测量结果表 where 流水号 = '" + lab_num.Text + "'");
            //        lab_num.ForeColor = Color.Black;
            //        toolStripButton1.Text = "开始";
            //    }
            //    else
            //    {
            //        MessageBox.Show("尚未开始，无法停止！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    MessageBox.Show("糟糕！获取不到测量数据！！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void checkProduct(float[] num)      //检测数据是否处于控制界限内
        {
            lab1.Text = num[0].ToString();
            lab4.Text = num[6].ToString();
            getMaxMin(num[1]);
            getX(num[2],num[3],num[4]);
            lab8.Text = num[5].ToString();
            if (Convert.ToSingle(lab1_max.Text) < Convert.ToSingle(lab1.Text) || Convert.ToSingle(lab1_min.Text) > Convert.ToSingle(lab1.Text))      //检测工作压紧力是否处于控制界限内
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab3_max.Text) < Convert.ToSingle(lab3.Text) || Convert.ToSingle(lab3_min.Text) > Convert.ToSingle(lab3.Text))     //检测最小分离力是否处于控制界限内
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab4_max.Text) < Convert.ToSingle(lab4.Text) || Convert.ToSingle(lab4_min.Text) > Convert.ToSingle(lab4.Text))     //检测分离点分离力是否处于控制界限内
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab5_max.Text) < Convert.ToSingle(lab5.Text) || Convert.ToSingle(lab5_min.Text) > Convert.ToSingle(lab5.Text))     //检测峰值分离力是否处于控制界限内
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab6_min.Text) > Convert.ToSingle(lab6.Text))      //检测压盘升程是否处于控制界限内
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab7_max.Text) < Convert.ToSingle(lab7.Text))      //检测压盘升程最大值与最小值的差是否处于控制界限内
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab8_max.Text) < Convert.ToSingle(lab8.Text) || Convert.ToSingle(lab8_min.Text) > Convert.ToSingle(lab8.Text))     //检测分离指高度是否处于控制界限内
            {
                badEnd();
                return;
            }

        }

        private void getMaxMin(float a)     //获取分离力的最大值最小值
        {
            if(a < Convert.ToSingle(lab3.Text))
            {
                lab3.Text = a.ToString();
            }
            else
            {

            }
            if(a > Convert.ToSingle(lab5.Text))
            {
                lab5.Text = a.ToString();
            }
            else
            {

            }
        }
        private void getX(float a, float b, float c)        //处理压盘升程的三个数据，求出最小值以及其最大值与最小值的差
        {
            if(a > b)
            {
                if(b > c)
                {
                    lab6.Text = c.ToString();
                    lab7.Text = (a - c).ToString();
                }
                else
                {
                    lab6.Text = b.ToString();
                    if(a > c)
                    {
                        lab7.Text = (a - b).ToString();
                    }
                    else
                    {
                        lab7.Text = (c - b).ToString();
                    }
                }
            }
            else
            {
                if(a > c)
                {
                    lab6.Text = c.ToString();
                    lab7.Text = (b - c).ToString();
                }
                else
                {
                    lab6.Text = a.ToString();
                    if (b > c)
                    {
                        lab7.Text = (b - a).ToString();
                    }
                    else
                    {
                        lab7.Text = (c - a).ToString();
                    }
                }
            }
        }

        private void insertData()       //插入检测结果数据到检测结果表中
        {
            lab_num.ForeColor = Color.Red;
            toolStripButton1.Text = "开始";
            SQLClass.getcom("insert into 测量结果表 values ('" + lab_num.Text + "', '" + lab1.Text + "', '" + lab7.Text + "', '" + lab3.Text + "', '" + lab6.Text + "', '" + lab5.Text + "', '" + lab8.Text + "', '" + lab4.Text + "', '" + lab_result.Text + "', '" + DateTime.Now + "')");
        }

        private void badEnd()       //不合格的产品终止的处理函数
        {
            lab_result.Text = "不合格";
            timer1.Enabled = false;
            timer1.Tag = "0";
            insertData();
        }

        private void startCheck()       //开始运行检测时的初始化
        {
            lab1.Text = "0";
            lab3.Text = "0";
            lab4.Text = "0";
            lab5.Text = "0";
            lab6.Text = "0";
            lab7.Text = "0";
            lab8.Text = "0";
            lab_result.Text = "";
        }

        private void lab_new_3_Click(object sender, EventArgs e)
        {

        }

        private void 新建检测ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AddForm addform = new AddForm();
            addform.Text = "添加基本信息";
            addform.ShowDialog();
            updatecBox();
        }

        private void 删除产品ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SQLClass.getsqlcom("delete from 基本信息表 where 流水号 = '" + lab_num.Text + "'");
            updatecBox();
            if (lab_num.ForeColor == Color.Red)
            {
                SQLClass.getsqlcom("delete from 测量结果表 where 流水号 = '" + lab_num.Text + "'");
            }
            else
            {

            }
            MessageBox.Show("检测记录删除成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 删除记录ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (lab_num.ForeColor == Color.Red)
            {
                SQLClass.getsqlcom("delete from 测量结果表 where 流水号 = '" + lab_num.Text + "'");
                MessageBox.Show("检测记录删除成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updatecBox();
            }
            else
            {
                MessageBox.Show("该产品没有检测记录！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}