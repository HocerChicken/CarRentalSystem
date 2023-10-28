using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalSystem
{
    public partial class CarsFr : Form
    {
        string connectionString = "Data Source=HOCPAM;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlCommandBuilder commandBuilder;

        public CarsFr()
        {
            InitializeComponent();
        }

        private void resetTextBox()
        {
            tbCarid.Text = string.Empty;
            tbBrand.Text = string.Empty;
            tbModel.Text = string.Empty;
            cbAvailable.Text = string.Empty;
            tbPrice.Text = string.Empty;
        }

        private void loadCars()
        {
            try
            {
                string query = "SELECT * FROM Cars WHERE Available = 'YES'";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    carDGV.DataSource = dt;
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
            tbCarid.Hide();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbBrand.Text)
           || String.IsNullOrEmpty(tbModel.Text)
            || String.IsNullOrEmpty(tbType.Text)
           || String.IsNullOrEmpty(cbAvailable.SelectedItem.ToString())
           || String.IsNullOrEmpty(tbPrice.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "INSERT INTO Cars (brand, model, category, available, price) VALUES (@Brand, @Model, @Category, @Available, @Price)";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Brand", tbBrand.Text);
                            cmd.Parameters.AddWithValue("@Model", tbModel.Text);
                            cmd.Parameters.AddWithValue("@Category", tbType.Text);
                            cmd.Parameters.AddWithValue("@Available", cbAvailable.Text);
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
            if (String.IsNullOrEmpty(tbCarid.Text) || String.IsNullOrEmpty(tbBrand.Text)
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
                    string query = "UPDATE Cars SET brand = @Brand, model = @Model, available = @Available, Price = @Price, category = @Category WHERE carId = @CarId";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@CarId", tbCarid.Text);
                            cmd.Parameters.AddWithValue("@Brand", tbBrand.Text);
                            cmd.Parameters.AddWithValue("@Model", tbModel.Text);
                            cmd.Parameters.AddWithValue("@Available", cbAvailable.Text);
                            cmd.Parameters.AddWithValue("@Price", tbPrice.Text);
                            cmd.Parameters.AddWithValue("@Category", tbType.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Edit successfully!!!");
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbCarid.Text))
            {
                MessageBox.Show("Missing ID Information");
            }
            else
            {
                DialogResult result = MessageBox.Show($"Are you sure to delete this car", "Delete Car", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Cars WHERE carId = @CarId";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(@connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@CarId", tbCarid.Text);

                                int rowEffected = cmd.ExecuteNonQuery();
                                if (rowEffected > 0)
                                {
                                    MessageBox.Show("Delete car successfully!!!");
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
                tbCarid.Text = carDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                tbBrand.Text = carDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                tbModel.Text = carDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                tbType.Text = carDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
                cbAvailable.Text = carDGV.Rows[e.RowIndex].Cells[4].Value.ToString();
                tbPrice.Text = carDGV.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadCars();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbSearch.SelectedIndex == 0)
            {
                loadCars("brand");
            } 
            else if (cbSearch.SelectedIndex == 1)
            {
                loadCars("model");
            } 
            else if (cbSearch.SelectedIndex == 2) 
            {
                loadCars("category");
            } 
            else
            {
                loadCars();
            }
        }

        private void loadCars(string typeSearch)
        {
            try
            {
                string query = "SELECT * FROM Cars WHERE " + typeSearch + " = @SearchText";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@SearchText", tbSearch.Text);

                    adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    carDGV.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
