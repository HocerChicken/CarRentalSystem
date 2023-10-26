﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalSystem
{
    public partial class User : Form
    {
        string connectionString = "Data Source=HOCPAM;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlCommandBuilder commandBuilder;

        public User()
        {
            InitializeComponent();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loadUser()
        {
            try
            {
                string query = "SELECT * FROM Users";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    userDGV.DataSource = dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void resetTextBox()
        {
            Id.Text = "";
            Username.Text = "";
            Userpassword.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Id.Text) || String.IsNullOrEmpty(Username.Text) || String.IsNullOrEmpty(Userpassword.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "INSERT INTO Users(Id, Username, Userpassword) VALUES (@Id, @Username, @Userpassword)";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", Id.Text);
                            cmd.Parameters.AddWithValue("@Username", Username.Text);
                            cmd.Parameters.AddWithValue("@Userpassword", Userpassword.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("User successfully Added!!!");
                                loadUser();
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

        private void userDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = userDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                cell.Style.SelectionBackColor = Color.Red;
            }
            if (e.RowIndex >= 0)
            {
                Id.Text = userDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                Username.Text = userDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                Userpassword.Text = userDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Id.Text) || String.IsNullOrEmpty(Username.Text) || String.IsNullOrEmpty(Userpassword.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "Update Users SET Id = @Id, Username = @Username, Userpassword = @Userpassword Where Id = @Id";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", Id.Text);
                            cmd.Parameters.AddWithValue("@Username", Username.Text);
                            cmd.Parameters.AddWithValue("@Userpassword", Userpassword.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Edit successfully!!!");
                                loadUser();
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
            if (String.IsNullOrEmpty(Id.Text))
            {
                MessageBox.Show("Missing ID Information");
            }
            else
            {
                DialogResult result = MessageBox.Show($"Are you sure to delete user with ID: {Id.Text}", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM USERS WHERE Id = @Id";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(@connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@Id", Id.Text);

                                int rowEffected = cmd.ExecuteNonQuery();
                                if (rowEffected > 0)
                                {
                                    MessageBox.Show("Delete user successfully!!!");
                                    loadUser();
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

        private void User_Load(object sender, EventArgs e)
        {
            loadUser();
        }
    }
}
