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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CarRentalSystem
{
    public partial class CustomersFr : Form
    {
        string connectionString = "Data Source=HOCPAM;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlCommandBuilder commandBuilder;

        public CustomersFr()   
        {
            InitializeComponent();
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            tbCusId.Hide();
            loadCustomer();
        }

        private void resetTextBox()
        {
            tbCusId.Text = String.Empty;
            tbCusName.Text = String.Empty;
            tbCusAdd.Text = String.Empty;
            tbPhone.Text = String.Empty;
        }

        private void loadCustomer()
        {
            try
            {
                string query = "SELECT * FROM Customers";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cusDGV.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if ( String.IsNullOrEmpty(tbCusName.Text) || String.IsNullOrEmpty(tbCusAdd.Text) || String.IsNullOrEmpty(tbPhone.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "INSERT INTO Customers(cusName, cusAdd, phone) VALUES (@cusName, @cusAdd, @phone)";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            //cmd.Parameters.AddWithValue("@CusId", tbCusId.Text);
                            cmd.Parameters.AddWithValue("@cusName", tbCusName.Text);
                            cmd.Parameters.AddWithValue("@cusAdd", tbCusAdd.Text);
                            cmd.Parameters.AddWithValue("@phone", tbPhone.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Customer successfully Added!!!");
                                loadCustomer();
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
            if (String.IsNullOrEmpty(tbCusId.Text)
                || String.IsNullOrEmpty(tbCusName.Text)
                || String.IsNullOrEmpty(tbCusAdd.Text)
                || String.IsNullOrEmpty(tbPhone.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "Update Customers SET cusName = @cusName, cusAdd = @cusAdd, phone = @phone Where cusId = @cusId";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@cusId", tbCusId.Text);
                            cmd.Parameters.AddWithValue("@cusName", tbCusName.Text);
                            cmd.Parameters.AddWithValue("@cusAdd", tbCusAdd.Text);
                            cmd.Parameters.AddWithValue("@phone", tbPhone.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Edit successfully!!!");
                                loadCustomer();
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
            if (String.IsNullOrEmpty(tbCusId.Text))
            {
                MessageBox.Show("Missing ID Information");
            }
            else
            {
                DialogResult result = MessageBox.Show($"Are you sure to delete this user", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Customers WHERE cusId = @cusId";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(@connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@cusId", tbCusId.Text);

                                int rowEffected = cmd.ExecuteNonQuery();
                                if (rowEffected > 0)
                                {
                                    MessageBox.Show("Delete customer successfully!!!");
                                    loadCustomer();
                                    resetTextBox();
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

        private void lbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cusDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = cusDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                cell.Style.SelectionBackColor = Color.Red;
            }
            if (e.RowIndex >= 0)
            {
                tbCusId.Text = cusDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                tbCusName.Text = cusDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                tbCusAdd.Text = cusDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                tbPhone.Text = cusDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
        }


        private void lbExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbSearch.SelectedIndex == 0)
            {
                loadCustomer("cusName");
            }
            else if (cbSearch.SelectedIndex == 1)
            {
                loadCustomer("phone");
            }
            else
            {
                loadCustomer();
            }
        }
        private void loadCustomer(string typeSearch)
        {
            try
            {
                string query = "SELECT * FROM Customers WHERE " + typeSearch + " LIKE '%' + @SearchText + '%'";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@SearchText", tbSearch.Text);

                    adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cusDGV.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadCustomer();
        }
    }
}
