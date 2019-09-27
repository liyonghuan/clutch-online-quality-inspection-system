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

        private void MainForm_Load(object sender, EventArgs e)      //�����壬�������ʱ��������
        {
            lab_result.Text = "";       //��ռ����
            updatecBox();       //����cBox
            timer1.Interval = 100;      //�趨��ʱ�ؼ�Ĭ��ʱ������Ĭ��100����
        }  

        private void �½����ToolStripMenuItem_Click(object sender, EventArgs e)        //�½���Ʒ���
        {
            
        }

        private void ɾ����ƷToolStripMenuItem_Click(object sender, EventArgs e)        //ɾ����Ʒ������м����������ͬʱɾ�����������
        {
            //SQLClass.getsqlcom("delete from ������Ϣ�� where ��ˮ�� = '" + lab_num.Text + "'");
            //updatecBox();
            //if (lab_num.ForeColor == Color.Red)
            //{
            //    SQLClass.getsqlcom("delete from ��������� where ��ˮ�� = '" + lab_num.Text + "'");
            //}
            //else
            //{
                
            //}
            //MessageBox.Show("����¼ɾ���ɹ���", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ɾ����¼ToolStripMenuItem_Click(object sender, EventArgs e)        //ɾ�����������
        {
            //if(lab_num.ForeColor == Color.Red)
            //{
            //    SQLClass.getsqlcom("delete from ��������� where ��ˮ�� = '" + lab_num.Text + "'");
            //    MessageBox.Show("����¼ɾ���ɹ���", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    updatecBox();
            //}
            //else
            //{
            //    MessageBox.Show("�ò�Ʒû�м���¼��", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void �˳�EToolStripMenuItem_Click(object sender, EventArgs e)     //�˳�����
        {
            Application.Exit();
        }


        private void toolStripButton1_Click(object sender, EventArgs e)     //��ʼ����ͣ��������ť����ť�ı��Ͷ������ݲ����Զ��仯
        {
            if(lab_num.ForeColor == Color.Red)
            {
                MessageBox.Show("���в���������ݣ�ɾ������ܼ�����", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(cBox_model.Text == "")
            {
                MessageBox.Show("û����Ҫ���ԵĲ�Ʒ��", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(toolStripButton1.Text == "��ʼ")
            {
                //�������ʼʱ����ͼ��ϵ��Series
                /*
                 * ���磺
                 * ͼ����.Series.Clear();
                 * ͼ����.Series.Add("ϵ����");
                 * ͼ����.Series["ϵ����"].ChartType = SeriesChartType.Line;
                 * 
                 * ����ͼ����ΪtCon_chart1��tCon_chart2
                 * ϵ����Ϊ�����õ�����
                 * 
                 */
                tCon_chart1.Series.Clear();     //���ͼ���е�ϵ��
                tCon_chart2.Series.Clear();
                tCon_chart1.Series.Add("��������");     //�������
                tCon_chart2.Series.Add("��������");
                tCon_chart1.Series["��������"].ChartType = SeriesChartType.Line;        //����ͼ������
                tCon_chart2.Series["��������"].ChartType = SeriesChartType.Line;

                DataSet DSet = SQLClass.getDataSet("select Count(id) from �������ݱ� where ���ݽ綨 = '" + cBox_model.Tag + "'", "������Ϣ��");
                DataTable dt = DSet.Tables["������Ϣ��"];

                toolStripButton1.Tag = dt.Rows[0][0].ToString();

                timer1.Enabled = true;
                timer1.Interval = 100;      //���ö�ʱ�ؼ�ִ�м��ʱ��
                toolStripButton1.Text = "��ͣ";
                startCheck();
            }
            else if(toolStripButton1.Text == "��ͣ")
            {
                timer1.Interval = 1000000000;       //���ö�ʱ�ؼ����ʱ�䣬�˴�Ŀ����ʹ��ʱ�ؼ����ڼ�ֹͣ״̬�������������Խ����ϴ�δ��ɽ������ִ��
                toolStripButton1.Text = "����";
            }
            else
            {
                timer1.Interval = 100;
                toolStripButton1.Text = "��ͣ";
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)     //ֹͣ��ť��ֹͣ��⶯��
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                timer1.Tag = "0";
                SQLClass.getsqlcom("delete from ��������� where ��ˮ�� = '" + lab_num.Text + "'");
                lab_num.ForeColor = Color.Black;
                toolStripButton1.Text = "��ʼ";
            }
            else
            {
                MessageBox.Show("��δ��ʼ���޷�ֹͣ��", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void stopTimer()
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                timer1.Tag = "0";
                SQLClass.getsqlcom("delete from ��������� where ��ˮ�� = '" + lab_num.Text + "'");
                lab_num.ForeColor = Color.Black;
                toolStripButton1.Text = "��ʼ";
            }
            else
            {
                MessageBox.Show("��δ��ʼ���޷�ֹͣ��", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)        //��ʱ�ؼ���ÿ��ʱ����ִ��һ��
        {
            timer1.Tag = Convert.ToInt32(timer1.Tag) + 1;
            if(Convert.ToInt32(timer1.Tag) > Convert.ToInt32(toolStripButton1.Tag))
            {
                timer1.Tag = "0";
                timer1.Enabled = false;
                lab_result.Text = "�ϸ�";
                insertData();
            }
            else
            {
                getNew();
            }
        }

        private void cBox_model_SelectedIndexChanged(object sender, EventArgs e)        //cBoxֵ�仯ʱ�Ķ���
        {
            DataSet DSet = SQLClass.getDataSet("select ��ˮ��,ͼ��,����,���ݽ綨 from ������Ϣ�� where �ͺ� = '" + cBox_model.Text + "'", "������Ϣ��");
            DataTable dt = DSet.Tables["������Ϣ��"];
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

        private void updatecBox()       //����cBox
        {
            cBox_model.Items.Clear();
            DataSet DSet = SQLClass.getDataSet("select �ͺ� from ������Ϣ��", "������Ϣ��");
            DataTable dt = DSet.Tables["������Ϣ��"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++ )
                {
                    cBox_model.Items.Add(dt.Rows[i][0].ToString());
                }
                cBox_model.SelectedIndex = 0;
            }
        }

        private void getLimit()     //��ȡ���ݽ綨���еĽ綨ֵ
        {
            DataSet DSet = SQLClass.getDataSet("select ����ѹ�������ֵ,����ѹ������Сֵ,��С���������ֵ,��С��������Сֵ,�������������ֵ,������������Сֵ,��ֵ���������ֵ,��ֵ��������Сֵ,ѹ��������Сֵ,ѹ��ƽ�ж����ֵ,����ָ�߶����ֵ,����ָ�߶���Сֵ from ���ݽ綨�� where id = '" + cBox_model.Tag + "'", "������Ϣ��");
            DataTable dt = DSet.Tables["������Ϣ��"];
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

        private void getResult()        //��ȡ�����
        {
            DataSet DSet = SQLClass.getDataSet("select ����ѹ����,��С������,����������,��ֵ������,ѹ������,ѹ��ƽ�ж�,����ָ�߶�,��������� from ��������� where ��ˮ�� = '" + lab_num.Text + "'", "������Ϣ��");
            DataTable dt = DSet.Tables["������Ϣ��"];
            if (dt.Rows.Count > 0)
            {
                lab_num.ForeColor = Color.Red;      //��ɫ�������м�����Ĳ�Ʒ
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
                lab_num.ForeColor = Color.Black;        //��ɫ����δ���Ĳ�Ʒ
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

        private void getNew()       //��ȡ�������ݱ��е����ݣ���ǰ����˳���ȡ���漴��ȡ����ע�͵��Ĳ���Ϊ˳���ȡ��Ĭ���漴��ȡ���Ƽ�ʹ���漴��ȡ��ʽ
        {
            string sql;

            //˳���ȡ��ʽ
            int x = 0;
            if (cBox_model.Tag.ToString() == "")
            {
                sql = "select ����ѹ����,������,ѹ������1,ѹ������2,ѹ������3,����ָ�߶�,����������,����λ����,��������,ѹ���� from �������ݱ� order by ���ݱ��";
            }
            else
            {
                sql = "select ����ѹ����,������,ѹ������1,ѹ������2,ѹ������3,����ָ�߶�,����������,����λ����,��������,ѹ���� from �������ݱ� where ���ݽ綨 = '" + cBox_model.Tag + "' order by ���ݱ��";
            }
            DataSet DSet = SQLClass.getDataSet(sql, "������Ϣ��");
            DataTable dt = DSet.Tables["������Ϣ��"];
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
                //ͼ�����ͼ������ݵ�
                /*
                 * ͼ����.Series["ϵ����"].Points.AddXY(X��ֵ, Y��ֵ);
                 * 
                 */
                tCon_chart1.Series["��������"].Points.AddXY(string.Format("{0:0.00}", Convert.ToSingle(dt.Rows[x][7].ToString())), string.Format("{0:0}", Convert.ToSingle(dt.Rows[x][9].ToString())));
                //tCon_chart2.Series["��������"].Points.AddXY(string.Format("{0:0.0}", Convert.ToSingle(dt.Rows[x][7].ToString())), string.Format("{0:0.0}", Convert.ToSingle(dt.Rows[x][9].ToString())));
            }
            else
            {
                if (timer1.Enabled)
                {
                    timer1.Enabled = false;
                    timer1.Tag = "0";
                    SQLClass.getsqlcom("delete from ��������� where ��ˮ�� = '" + lab_num.Text + "'");
                    lab_num.ForeColor = Color.Black;
                    toolStripButton1.Text = "��ʼ";
                }
                else
                {
                    MessageBox.Show("��δ��ʼ���޷�ֹͣ��", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                MessageBox.Show("��⣡��ȡ�����������ݣ���", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            ////�����ȡ��ʽ
            //if(cBox_model.Tag.ToString() == "")
            //{
            //    sql = "select ����ѹ����,������,ѹ������1,ѹ������2,ѹ������3,����ָ�߶�,����������,����λ���� from �������ݱ� order by newid()";
            //}
            //else
            //{
            //    sql = "select ����ѹ����,������,ѹ������1,ѹ������2,ѹ������3,����ָ�߶�,����������,����λ���� from �������ݱ� where ���ݽ綨 = '" + cBox_model.Tag + "' order by newid()";
            //}
            //DataSet DSet = SQLClass.getDataSet(sql, "������Ϣ��");
            //DataTable dt = DSet.Tables["������Ϣ��"];
            //if (dt.Rows.Count > 0)
            //{
            //    lab_new_2.Text = dt.Rows[0][1].ToString();      //������
            //    lab_new_3.Text = dt.Rows[0][2].ToString();      //ѹ������1
            //    lab_new_4.Text = dt.Rows[0][3].ToString();      //ѹ������2
            //    lab_new_5.Text = dt.Rows[0][4].ToString();      //ѹ������3
            //    lab_new_6.Text = dt.Rows[0][0].ToString();      //����ѹ����
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

            //    //ͼ�����ͼ������ݵ�
            //    /*
            //     * ͼ����.Series["ϵ����"].Points.AddXY(X��ֵ, Y��ֵ);
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
            //        SQLClass.getsqlcom("delete from ��������� where ��ˮ�� = '" + lab_num.Text + "'");
            //        lab_num.ForeColor = Color.Black;
            //        toolStripButton1.Text = "��ʼ";
            //    }
            //    else
            //    {
            //        MessageBox.Show("��δ��ʼ���޷�ֹͣ��", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    MessageBox.Show("��⣡��ȡ�����������ݣ���", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void checkProduct(float[] num)      //��������Ƿ��ڿ��ƽ�����
        {
            lab1.Text = num[0].ToString();
            lab4.Text = num[6].ToString();
            getMaxMin(num[1]);
            getX(num[2],num[3],num[4]);
            lab8.Text = num[5].ToString();
            if (Convert.ToSingle(lab1_max.Text) < Convert.ToSingle(lab1.Text) || Convert.ToSingle(lab1_min.Text) > Convert.ToSingle(lab1.Text))      //��⹤��ѹ�����Ƿ��ڿ��ƽ�����
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab3_max.Text) < Convert.ToSingle(lab3.Text) || Convert.ToSingle(lab3_min.Text) > Convert.ToSingle(lab3.Text))     //�����С�������Ƿ��ڿ��ƽ�����
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab4_max.Text) < Convert.ToSingle(lab4.Text) || Convert.ToSingle(lab4_min.Text) > Convert.ToSingle(lab4.Text))     //�������������Ƿ��ڿ��ƽ�����
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab5_max.Text) < Convert.ToSingle(lab5.Text) || Convert.ToSingle(lab5_min.Text) > Convert.ToSingle(lab5.Text))     //����ֵ�������Ƿ��ڿ��ƽ�����
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab6_min.Text) > Convert.ToSingle(lab6.Text))      //���ѹ�������Ƿ��ڿ��ƽ�����
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab7_max.Text) < Convert.ToSingle(lab7.Text))      //���ѹ���������ֵ����Сֵ�Ĳ��Ƿ��ڿ��ƽ�����
            {
                badEnd();
                return;
            }

            if (Convert.ToSingle(lab8_max.Text) < Convert.ToSingle(lab8.Text) || Convert.ToSingle(lab8_min.Text) > Convert.ToSingle(lab8.Text))     //������ָ�߶��Ƿ��ڿ��ƽ�����
            {
                badEnd();
                return;
            }

        }

        private void getMaxMin(float a)     //��ȡ�����������ֵ��Сֵ
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
        private void getX(float a, float b, float c)        //����ѹ�����̵��������ݣ������Сֵ�Լ������ֵ����Сֵ�Ĳ�
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

        private void insertData()       //�����������ݵ����������
        {
            lab_num.ForeColor = Color.Red;
            toolStripButton1.Text = "��ʼ";
            SQLClass.getcom("insert into ��������� values ('" + lab_num.Text + "', '" + lab1.Text + "', '" + lab7.Text + "', '" + lab3.Text + "', '" + lab6.Text + "', '" + lab5.Text + "', '" + lab8.Text + "', '" + lab4.Text + "', '" + lab_result.Text + "', '" + DateTime.Now + "')");
        }

        private void badEnd()       //���ϸ�Ĳ�Ʒ��ֹ�Ĵ�����
        {
            lab_result.Text = "���ϸ�";
            timer1.Enabled = false;
            timer1.Tag = "0";
            insertData();
        }

        private void startCheck()       //��ʼ���м��ʱ�ĳ�ʼ��
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

        private void �½����ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AddForm addform = new AddForm();
            addform.Text = "��ӻ�����Ϣ";
            addform.ShowDialog();
            updatecBox();
        }

        private void ɾ����ƷToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SQLClass.getsqlcom("delete from ������Ϣ�� where ��ˮ�� = '" + lab_num.Text + "'");
            updatecBox();
            if (lab_num.ForeColor == Color.Red)
            {
                SQLClass.getsqlcom("delete from ��������� where ��ˮ�� = '" + lab_num.Text + "'");
            }
            else
            {

            }
            MessageBox.Show("����¼ɾ���ɹ���", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ɾ����¼ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (lab_num.ForeColor == Color.Red)
            {
                SQLClass.getsqlcom("delete from ��������� where ��ˮ�� = '" + lab_num.Text + "'");
                MessageBox.Show("����¼ɾ���ɹ���", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updatecBox();
            }
            else
            {
                MessageBox.Show("�ò�Ʒû�м���¼��", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}