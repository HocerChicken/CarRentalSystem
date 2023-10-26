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
    public partial class Customer : Form
    {
        string connectionString = "Data Source=HOCPAM;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlCommandBuilder commandBuilder;

        public Customer()   
        {
            InitializeComponent();
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            loadCustomer();
        }

        private void resetTextBox()
        {
            tbCusId.Text = "";
            tbCusName.Text = "";
            tbCusAdd.Text = "";
            tbPhone.Text = "";
        }

        private void loadCustomer()
        {
            try
            {
                string query = "SELECT * FROM Customer";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    cusDGV.DataSource = dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //String.IsNullOrEmpty(tbCusId.Text) ||
            if ( String.IsNullOrEmpty(tbCusName.Text) || String.IsNullOrEmpty(tbCusAdd.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "INSERT INTO Customer(CusName, CusAdd, Phone) VALUES (@CusName, @CusAdd, @Phone)";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            //cmd.Parameters.AddWithValue("@CusId", tbCusId.Text);
                            cmd.Parameters.AddWithValue("@CusName", tbCusName.Text);
                            cmd.Parameters.AddWithValue("@CusAdd", tbCusAdd.Text);
                            cmd.Parameters.AddWithValue("@Phone", tbPhone.Text);

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
                    string query = "Update Customer SET CusId = @CusId, CusName = @CusName, CusAdd = @CusAdd, Phone = @Phone Where CusId = @CusId";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@CusId", tbCusId.Text);
                            cmd.Parameters.AddWithValue("@CusName", tbCusName.Text);
                            cmd.Parameters.AddWithValue("@CusAdd", tbCusAdd.Text);
                            cmd.Parameters.AddWithValue("@Phone", tbPhone.Text);

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
                DialogResult result = MessageBox.Show($"Are you sure to delete user with ID: {tbCusId.Text}", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Customer WHERE CusId = @CusId";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(@connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@CusId", tbCusId.Text);

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
    }
}
