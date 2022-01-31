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
    public partial class MainAdmin : Form
    {
        public MainAdmin()
        {
            InitializeComponent();

            lbladmin.Text = Session.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
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
            if(result == DialogResult.Yes)
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
