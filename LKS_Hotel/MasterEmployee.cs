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
    public partial class MasterEmployee : Form
    {
        int cond, id;
        SqlConnection connection = new SqlConnection(Utils.conn);
        public MasterEmployee()
        {
            InitializeComponent();
            dis();
            loadgrid();
            loadjob();

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
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
        }

        void loadjob()
        {
            string com = "select * from job";
            comboBox1.DataSource = Command.GetData(com);
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox9.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            pictureBox1.Image = null;
        }

        bool val()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || textBox5.TextLength < 1 || textBox6.TextLength < 1 || textBox9.TextLength < 1 || dateTimePicker1.Value == null || pictureBox1.Image == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox5.Text != textBox4.Text)
            {
                MessageBox.Show("Confirm Password Does't Same", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(textBox5.TextLength < 8 || textBox4.TextLength < 8)
            {
                MessageBox.Show("Password and Confirm Password field at least has 8 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            SqlCommand command = new SqlCommand("select * from Employee where username = '" + textBox2.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Username's name was used!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();
            return true;
        }
        bool val_up()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox6.TextLength < 1 || textBox9.TextLength < 1 || dateTimePicker1.Value == null || pictureBox1.Image == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            SqlCommand command = new SqlCommand("select * from employee where username = '" + textBox2.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                if (Convert.ToInt32(reader["id"]) != id)
                {
                    connection.Close();
                    MessageBox.Show("Username's name was used!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            connection.Close();
            return true;
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
            if (dataGridView1.CurrentRow.Selected)
            {
                cond = 2;
                enable();
                textBox4.Enabled = false;
                textBox5.Enabled = false;
            }
            else
            {
                MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected == true)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete it?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SqlCommand command = new SqlCommand("delete from Employee where id = " + id, connection);
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
            else
            {
                MessageBox.Show("Please select an item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            dis();
            clear();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (cond == 1 && val())
            {
                ImageConverter converter = new ImageConverter();
                byte[] b = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));

                SqlCommand command = new SqlCommand("insert into employee values('" + textBox3.Text + "', '" + Encrypt.enc(textBox4.Text) + "', '" + textBox2.Text + "', '" + textBox6.Text + "', '" + textBox9.Text + "', '" + Convert.ToDateTime(dateTimePicker1.Value) + "', " + comboBox1.SelectedValue + ", @img)", connection);
                command.Parameters.AddWithValue("@img", b);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Successfully Inserted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else if(cond == 2 && val_up())
            {
                ImageConverter converter = new ImageConverter();
                byte[] b = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));

                SqlCommand command = new SqlCommand("update employee set name = '"+textBox2.Text+"', username = '"+textBox3.Text+"', email = '"+textBox6.Text+"', address = '"+textBox9.Text+"', dateofBirth = '"+dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")+"', jobid = "+comboBox1.SelectedValue+", photo = @img where id = " + id, connection);
                command.Parameters.AddWithValue("@img", b);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Successfully Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == 8);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox9.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            textBox9.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[6].Value);
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[7].Value);

            if(dataGridView1.SelectedRows[0].Cells[8].Value != System.DBNull.Value)
            {
                byte[] b = (byte[])dataGridView1.SelectedRows[0].Cells[8].Value;
                MemoryStream stream = new MemoryStream(b);
                Image img = Image.FromStream(stream);
                Bitmap bmp = (Bitmap)img;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                pictureBox1.Image = null;
                
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from emp_view where name like '%"+textBox1.Text+"%'";
            dataGridView1.DataSource = Command.GetData(com);
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.png; *.jpg;* jpeg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(ofd.FileName);
                Bitmap bmp = (Bitmap)img;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == 8); 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox4.PasswordChar = '\0';
            else
                textBox4.PasswordChar = '*';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }
    }
}
