using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Hotel
{
    public partial class MasterRoomType : Form
    {
        int id, cond;
        SqlConnection connection = new SqlConnection(Utils.conn);
        public MasterRoomType()
        {
            InitializeComponent();
            loadgrid();
            dis();

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
            numericUpDown1.Enabled = false;
            button3.Enabled = false;
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
            numericUpDown1.Enabled = true;
            button3.Enabled = true;
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            numericUpDown1.Value = 0;
            pictureBox1.Image = null;
        }

        bool val()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || numericUpDown1.Value == 0 || pictureBox1.Image == null)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("select * from roomtype where name = '" + textBox2.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Room type's name was used!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();
            return true;
        }

        bool val_up()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || numericUpDown1.Value == 0 || pictureBox1.Image == null)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("select * from roomtype where name = '" + textBox2.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                if(Convert.ToInt32(reader["id"]) != id)
                {
                    connection.Close();
                    MessageBox.Show("Room type's name was used!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            connection.Close();
            return true;
        }

        void loadgrid()
        {
            string sql = "select * from roomType";
            dataGridView1.DataSource = Command.GetData(sql);
            dataGridView1.Columns[4].Visible = false;
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

        private void btn_insert_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow.Selected == true)
            {
                cond = 2;
                enable();
            }
            else
            {
                MessageBox.Show("Please select an item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to delete it?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                SqlCommand command = new SqlCommand("delete from roomtype where id = " + id, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Successfully deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    loadgrid();
                    dis();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            clear();
            dis();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (cond == 1 && val())
            {
                ImageConverter converter = new ImageConverter();
                byte[] b = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));

                SqlCommand com = new SqlCommand("insert into roomtype values('" + textBox2.Text + "', " + Convert.ToInt32(numericUpDown1.Value) + ", " + Convert.ToInt32(textBox3.Text) + ",@img)", connection);
                com.Parameters.AddWithValue("@img", b);
                try
                {
                    connection.Open();
                    com.ExecuteNonQuery();
                    MessageBox.Show("Successfully inserted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid();
                    clear();
                    dis();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            else if(cond == 2 && val_up())
            {
                ImageConverter converter = new ImageConverter();
                byte[] b = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));

                SqlCommand com = new SqlCommand("update roomtype set name = '" + textBox2.Text + "', capacity = " + numericUpDown1.Value + ", roomprice = " + Convert.ToInt32(textBox3.Text) + ", photo = @img where id ="+ id, connection);
                com.Parameters.AddWithValue("@img", b);
                try
                {
                    connection.Open();
                    com.ExecuteNonQuery();
                    MessageBox.Show("Successfully Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid();
                    clear();
                    dis();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.png; *.jpg;* jpeg";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(ofd.FileName);
                Bitmap bmp = (Bitmap)img;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            numericUpDown1.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();

            byte[] b = (byte[])dataGridView1.SelectedRows[0].Cells[4].Value;
            MemoryStream stream = new MemoryStream(b);
            Image img = Image.FromStream(stream);
            Bitmap bmp = (Bitmap)img;
            pictureBox1.Image = bmp;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = "select * from roomType where name like '%"+textBox1.Text+"%'";
            dataGridView1.DataSource = Command.GetData(sql);
            dataGridView1.Columns[4].Visible = false;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }
    }
}
