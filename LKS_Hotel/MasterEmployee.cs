using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Hotel
{
    public partial class MasterEmployee : Form
    {
        public MasterEmployee()
        {
            InitializeComponent();
            lbladmin.Text = Session.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        void dis()
        {
            btn_edit.Enabled = true;
            btn_insert.Enabled = true;
            btn_del.Enabled = true;
            btn_save.Enabled = false;
            btn_cancel.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox9.Enabled = false;
            button3.Enabled = false;
            dateTimePicker1.Enabled = false;
            comboBox1.Enabled = false;
        }

        void enable()
        {
            btn_edit.Enabled = false;
            btn_insert.Enabled = false;
            btn_del.Enabled = false;
            btn_save.Enabled = true;
            btn_cancel.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox9.Enabled = true;
            button3.Enabled = true;
            dateTimePicker1.Enabled = true;
            comboBox1.Enabled = true;
        }

        void loadgrid()
        {
            string com = "select * from emp_view";
            dataGridView1.DataSource = Command.GetData(com);
        }

        private void panelFd_Click(object sender, EventArgs e)
        {
            MasterFoodsAndDrinks master = new MasterFoodsAndDrinks();
            this.Hide();
            master.ShowDialog();
        }

        private void panelemp_Click(object sender, EventArgs e)
        {
            MasterEmployee master = new MasterEmployee();
            this.Hide();
            master.ShowDialog();
        }

        private void panelroomtype_Click(object sender, EventArgs e)
        {
            MasterRoomType master = new MasterRoomType();
            this.Hide();
            master.ShowDialog();
        }

        private void panelitems_Click(object sender, EventArgs e)
        {
            MasterItem master = new MasterItem();
            this.Hide();
            master.ShowDialog();
        }

        private void panelroom_Click(object sender, EventArgs e)
        {
            MasterRoom master = new MasterRoom();
            this.Hide();
            master.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin master = new MainLogin();
                this.Hide();
                master.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }
    }
}
