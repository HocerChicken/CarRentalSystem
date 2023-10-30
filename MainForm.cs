using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void btnCar_Click(object sender, EventArgs e)
        {
            this.Hide();
            CarsFr car = new CarsFr();
            car.Show();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            CustomersFr customer = new CustomersFr();
            customer.Show();
        }

        private void btnRental_Click(object sender, EventArgs e)
        {
            this.Hide();
            BookingsFr rental = new BookingsFr();
            rental.Show();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Return returnF = new Return();
            returnF.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            UsersFr user = new UsersFr();
            user.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginFr loginFr = new LoginFr();
            loginFr.Show(); 
        }
    }
}
