using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalSystem
{
    public partial class Car : Form
    {
        string connectionString = "Data Source=HOCPAM;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlCommandBuilder commandBuilder;
        public Car()
        {
            InitializeComponent();
        }

        private void resetTextBox()
        {
            tbRegNum.Text = string.Empty;
            tbBrand.Text = string.Empty;
            tbModel.Text = string.Empty;
            cbAvailable.Text = string.Empty;
            tbPrice.Text = string.Empty;
        }

        private void loadCars()
        {
            try
            {
                string query = "SELECT * FROM Car";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    carDGV.DataSource = dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Car_Load(object sender, EventArgs e)
        {
            loadCars();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbRegNum.Text) || String.IsNullOrEmpty(tbBrand.Text)
           || String.IsNullOrEmpty(tbModel.Text)
           || String.IsNullOrEmpty(cbAvailable.SelectedItem.ToString())
           || String.IsNullOrEmpty(tbPrice.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "INSERT INTO Car(RegNum, Brand, Model, Available, Price) VALUES (@RegNum, @Brand, @Model, @Available, @Price)";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@RegNum", tbRegNum.Text);
                            cmd.Parameters.AddWithValue("@Brand", tbBrand.Text);
                            cmd.Parameters.AddWithValue("@Model", tbModel.Text);
                            cmd.Parameters.AddWithValue("@Available", cbAvailable.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@Price", tbPrice.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Car successfully Added!!!");
                                loadCars();
                                resetTextBox();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Information!!!");
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbRegNum.Text) || String.IsNullOrEmpty(tbBrand.Text)
            || String.IsNullOrEmpty(tbModel.Text)
            || String.IsNullOrEmpty(cbAvailable.SelectedItem.ToString())
            || String.IsNullOrEmpty(tbPrice.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "Update Car SET RegNum = @RegNum, Brand = @Brand, Available = @Available, Price = @Price Where RegNum = @RegNum";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@RegNum", tbRegNum.Text);
                            cmd.Parameters.AddWithValue("@Brand", tbBrand.Text);
                            cmd.Parameters.AddWithValue("@Model", tbModel.Text);
                            cmd.Parameters.AddWithValue("@Available", cbAvailable.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@Price", tbPrice.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Edit successfully!!!");
                                loadCars();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Information!!!");
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbRegNum.Text))
            {
                MessageBox.Show("Missing ID Information");
            }
            else
            {
                DialogResult result = MessageBox.Show($"Are you sure to delete user with RegNum: {tbRegNum.Text}", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Car WHERE RegNum = @RegNum";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(@connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@RegNum", tbRegNum.Text);

                                int rowEffected = cmd.ExecuteNonQuery();
                                if (rowEffected > 0)
                                {
                                    MessageBox.Show("Delete user successfully!!!");
                                    loadCars();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid Information!!!");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                else if (result == DialogResult.No) { }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void cusDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = carDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                cell.Style.SelectionBackColor = Color.Red;
            }
            if (e.RowIndex >= 0)
            {
                tbRegNum.Text = carDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                tbBrand.Text = carDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                tbModel.Text = carDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                cbAvailable.Text = carDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
                tbPrice.Text = carDGV.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
        }
    }
}
