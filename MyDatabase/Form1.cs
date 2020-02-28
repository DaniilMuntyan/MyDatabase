using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace MyDatabase
{
    public partial class Form1 : Form
    {
        Info info;
        public Form1()
        {
            InitializeComponent();
            info = new Info(this);
        }
        DataSet ds;
        DataView dv1, dv2;
        public static string nameXMLfile = "Sets.xml";
        public static int currentRow = 0;
        string s = "",s1="", s2 = "", sFilter = "";
        bool isChangesSaved = true;
        void LoadXmlFile()
        {
            ds = new DataSet();
            FileStream fsReasXml = new FileStream(nameXMLfile, FileMode.Open);
            ds.ReadXml(fsReasXml, XmlReadMode.InferTypedSchema);
            fsReasXml.Close();
            dv1 = new DataView(ds.Tables[0]);
            dataGridView1.DataSource = dv1;
            string s = dataGridView1.Rows[0].Cells[0].Value.ToString();
            dv2 = new DataView(ds.Tables[1]);
            dv2.RowFilter = "КодНабора = '" + s + "'";
            dataGridView2.DataSource = dv2;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Имя автора";
            dataGridView1.Columns[2].HeaderText = "Описание";
            dataGridView1.Columns[3].HeaderText = "Год рождения";
            dataGridView1.Columns[4].HeaderText = "Год смерти";

            dataGridView1.Columns[1].Width = 242;
            dataGridView1.Columns[2].Width = 260;
            dataGridView1.Columns[3].Width = 65;
            dataGridView1.Columns[4].Width = 65;
            dataGridView1.Columns[5].Width = 70;
            dataGridView1.Columns[6].Width = 84;
            dataGridView1.Columns[7].Width = 66;
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[7].Visible = false;
            dataGridView2.Columns[1].Width = 120;
            dataGridView2.Columns[3].Width = 300;
            dataGridView2.Columns[4].Width = 115;
            dataGridView2.Columns[5].Width = 117;
            isChangesSaved = true;
            currentRow = 0;
            dataGridView1.Font = new Font(Name,10);
            dataGridView2.Font = new Font(Name, 10);

        }
        void replayData()
        {
            dataGridView1.Columns[1].Width = 242;
            dataGridView1.Columns[2].Width = 260;
            dataGridView1.Columns[3].Width = 65;
            dataGridView1.Columns[4].Width = 65;
            dataGridView1.Columns[5].Width = 70;
            dataGridView1.Columns[6].Width = 84;
            dataGridView1.Columns[7].Width = 66;
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[7].Visible = false;
            dataGridView2.Columns[1].Width = 120;
            dataGridView2.Columns[3].Width = 300;
            dataGridView2.Columns[4].Width = 115;
            dataGridView2.Columns[5].Width = 117;
            isChangesSaved = true;
            currentRow = 0;
            dataGridView1.Font = new Font(Name, 10);
            dataGridView2.Font = new Font(Name, 10);
        }
        void SaveXmlFile()
        {
            FileStream fsWriteXml = new FileStream(nameXMLfile, FileMode.Create);
            ds.WriteXml(fsWriteXml);
            fsWriteXml.Close();
        }
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            string s;
            int i = e.RowIndex;
            currentRow = i;
            if(!dataGridView1.Rows[i].Cells[1].Value.Equals(DBNull.Value))
            {
                s = dataGridView1.Rows[i].Cells[0].Value.ToString();
                dv2 = new DataView(ds.Tables[1]);
                dv2.RowFilter = "КодНабора = '" + s + "'";
                dataGridView2.DataSource = dv2;
                pictureBox1.Image = Image.FromFile("Портреты\\" + dataGridView1.Rows[i].Cells[7].Value.ToString());
                lblName.Text = "Имя автора: " + dataGridView1.Rows[i].Cells[1].Value.ToString();
            }
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadXmlFile();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile1 = new OpenFileDialog();
            openFile1.InitialDirectory = Application.StartupPath;
            openFile1.Filter = "Файлы XML (*.xml)|*.xml";
            openFile1.FileName = "Sets.xml";
            if(openFile1.ShowDialog() == DialogResult.OK)
            {
                nameXMLfile = openFile1.FileName;
                LoadXmlFile();
            }
        }

        private void добавитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dv1.AddNew();
            replayData();
        }

        private void удалитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Вы действительно хотите удалить информацию этого автора?","Удаление данных",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                isChangesSaved = false;
                dv1.Delete(currentRow);
            }
        }
        
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveXmlFile();
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "newFile.xml";
            sfd.Filter = "XML Files(*.xml) | *.xml";
            if (sfd.ShowDialog() == DialogResult.OK)
            {

                FileStream fsWriteDB = new FileStream(sfd.FileName, FileMode.Create);
                ds.WriteXml(fsWriteDB);
                fsWriteDB.Close();
                isChangesSaved = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!isChangesSaved)
            {
                DialogResult result;
                result = MessageBox.Show("Сохранить изменения?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    SaveXmlFile();
                    isChangesSaved = true;
                }
                else
                    if (result == DialogResult.No)
                    isChangesSaved = true;
                else
                    e.Cancel = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                s = comboBox1.Text;
                if (radioButton2.Checked)
                    s += " DESC";
                dv1.Sort = s;
                groupBox3.Enabled = true;
            }
            else
            {
                s = "";
                groupBox4.Enabled = false;
                groupBox3.Enabled = false;
                comboBox3.Text = "";
                comboBox2.Text = "";
            }
            s1 = s;
            replayData();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text != "")
            {
                s = comboBox1.Text + " DESC";
                dv1.Sort = s;
            }
            s1 = s;
            replayData();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                s = comboBox1.Text;
                dv1.Sort = s;
            }
            s1 = s;
            replayData();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.Text != "")
            {
                s += ", " + comboBox2.Text;
                dv1.Sort = s;
                groupBox4.Enabled = true;
                s2 = s;
            }
            replayData();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                if (radioButton4.Checked)
                    s = s1 + ", " + comboBox2.Text;
                else
                    s = s1 + ", " + comboBox2.Text + " DESC";
                dv1.Sort = s;
            }
            s2 = s;
            replayData();
        }
        
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox3.Text != "")
            {
                s += ", " + comboBox3.Text;
            }
            replayData();

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                s = s2 + ", " + comboBox3.Text;
            else
                s = s2 + ", " + comboBox3.Text + " DESC";
            replayData();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

         /*   if (comboBox1.Text != "")
            {
                dv1.Sort = comboBox1.Text + " DESC";
            }
            replayData();*/
        }

        private void сортировкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox5.Visible = false;
        }

        private void фильтроватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox5.Visible = true;
            groupBox1.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                dv1.RowFilter = "Автор Like '%" + textBox1.Text + "%'";
                if(checkBox2.Checked)
                {
                    if (txtFrom.Text != "") dv1.RowFilter += " And ГодРождения >= " + txtFrom.Text;
                    if (txtTo.Text != "") dv1.RowFilter += " And ГодРождения <= " + txtTo.Text;
                }
                if (checkBox3.Checked)
                    if (comboBox4.Text != "") dv1.RowFilter += "And Гражданство = " + comboBox4.Text;
                sFilter = dv1.RowFilter;
            }
            replayData();
        }

        private void сортировкаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox5.Visible = false;
        }

        private void фильтрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox5.Visible = true;
            groupBox1.Visible = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                txtFrom.Enabled = true;
                txtTo.Enabled = true;
            }else
            {
                txtFrom.Enabled = false;
                txtTo.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox1.Enabled = true;
            else
                textBox1.Enabled = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox3.Checked)
                comboBox4.Enabled = true;
            else
                comboBox4.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dv1.RowFilter = "";
            string sFilter0 = sFilter;
            if (txtFrom.Text != "") sFilter0 += " And ГодРождения >= " + txtFrom.Text;
            if (txtTo.Text != "") sFilter0 += " And ГодРождения <= " + txtTo.Text;
            sFilter0.Trim();
            if (sFilter0.StartsWith(" And"))
                sFilter0 = sFilter0.Remove(0,4);
            dv1.RowFilter = sFilter0;
            if (checkBox3.Checked)
            {
                if (sFilter0 == "")
                    sFilter0 += "Гражданство Like '%" + comboBox4.Text + "%'";
                else
                    sFilter0 += " And Гражданство Like '%" + comboBox4.Text + "%'";
                dv1.RowFilter = sFilter0;
                sFilter0 = "";
            }
            sFilter = sFilter0;
            replayData();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            dv1.RowFilter = "";
            sFilter = "";
            replayData();
        }

        private void поToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string s = Microsoft.VisualBasic.Interaction.InputBox("Год смерти:");
            if (s != "")
            {
                string strSort = dv1.Sort;
                dv1.Sort = "ГодСмерти";
                int index = dv1.Find(s);
                if (index != -1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[index].Selected = true;
                }
                else
                {
                    dv1.Sort = strSort;
                    MessageBox.Show("Такого года смерти нет!");
                }
            }

            replayData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void поИмениToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = Microsoft.VisualBasic.Interaction.InputBox("Введите фамилию автора:");
            if(s != "")
            {
                string strSort = dv1.Sort;
                dv1.Sort = "Автор";
                int index = dv1.Find(s);
                if(index != -1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[index].Selected = true;
                }
                else
                {
                    dv1.Sort = strSort;
                    MessageBox.Show("Такого автора нет!");
                }
            }
            replayData();
        }

        private void поГодуРожденияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = Microsoft.VisualBasic.Interaction.InputBox("Год рождения:");
            if(s != "")
            {
                string strSort = dv1.Sort;
                dv1.Sort = "ГодРождения";
                int index = dv1.Find(s);
                if(index != -1)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[index].Selected = true;
                }
                else
                {
                    dv1.Sort = strSort;
                    MessageBox.Show("Такого года рождения нет!");
                }
            }
            replayData();
        }

        private void подробнаяИнформацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            info.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox4.Items.Add("Российская империя");
            comboBox4.Items.Add("США");
            comboBox4.Items.Add("Франция");
            comboBox4.Items.Add("Польша");
            comboBox1.Items.Add("ГодРождения");
            comboBox1.Items.Add("ГодСмерти");
            comboBox1.Items.Add("Автор");
            comboBox2.Items.Add("ГодРождения");
            comboBox2.Items.Add("ГодСмерти");
            comboBox2.Items.Add("Автор");
            comboBox3.Items.Add("ГодРождения");
            comboBox3.Items.Add("ГодСмерти");
            comboBox3.Items.Add("Автор");

            nameXMLfile = "Sets.xml";
            LoadXmlFile();

        }
    }
}
