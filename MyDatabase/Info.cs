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
    public partial class Info : Form
    {
        Form1 frm1;
        public Info(Form1 frm1)
        {
            InitializeComponent();
            this.frm1 = frm1; 
        }
        DataSet ds;
        DataView dv1;
        string nameXMLfile = "Sets.xml";
        public static string writer = "";
        int currentRow = 0;
        bool isChangeSaved = true; 
        void FillTextBox()
        {
            int columnsCount = dv1.Table.Columns.Count;
            string[] sTextBox = new string[columnsCount];
            int i = currentRow;
            txtCurrent.Text = (currentRow+1).ToString();
            for(int j = 0; j < columnsCount;j++)
            {
                if (!dv1.Table.Rows[i].ItemArray[j].Equals(DBNull.Value))
                    sTextBox[j] = dv1.Table.Rows[i].ItemArray[j].ToString();
                else
                    sTextBox[j] = "";
            }
            cmbCode.Text = sTextBox[0];
            txtName.Text = sTextBox[1];
            txtBegin.Text = sTextBox[3];
            txtEnd.Text = sTextBox[4];
            txtSecondName.Text = sTextBox[5];
            txtCountry.Text = sTextBox[6];
            txtPort.Text = sTextBox[7];
            if (sTextBox[7] != "Нет" && sTextBox[7] != "")
                pictureBox1.Load("Портреты\\" + sTextBox[7]);
            else
                pictureBox1.Load("Портреты\\0.bmp");

        }
        void FillComboBox()
        {
            cmbCode.Items.Clear();
            for (int i = 0; i < dv1.Count; i++)
                cmbCode.Items.Add(dv1.Table.Rows[i].ItemArray[0].ToString());
        }
        private void Info_Load(object sender, EventArgs e)
        {
            label4.Visible = false;
            nameXMLfile = Form1.nameXMLfile;
            currentRow = Form1.currentRow;
            ds = new DataSet();
            FileStream fsReadXml = new FileStream(nameXMLfile, FileMode.Open);
            ds.ReadXml(fsReadXml,XmlReadMode.InferTypedSchema);
            fsReadXml.Close();
            dv1 = new DataView(ds.Tables[0]);
            FillComboBox();
            FillTextBox();
            
        }

        private void cmbCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentRow = cmbCode.SelectedIndex;
            FillTextBox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int a;
            if(currentRow < dv1.Count -1)
            {
                currentRow++;
                FillTextBox();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(currentRow > 0)
            {
                currentRow--;
                FillTextBox();
            }
        }

        private void Info_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            frm1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            currentRow = dv1.Count - 1;
            FillTextBox();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            currentRow = 0;
            FillTextBox();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            writer = cmbCode.Text;
            frmAbout frmA = new frmAbout();
            frmA.ShowDialog();
        }

        private void label9_MouseEnter(object sender, EventArgs e)
        {
            label9.Visible = false;
            label4.Visible = true;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label9.Visible = true;
            label4.Visible = false;
        }
    }
}
