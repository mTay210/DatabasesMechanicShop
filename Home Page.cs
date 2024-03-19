using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MechanicShop
{
    public partial class Form1 : Form
    {
        // Connect to the server and database
        private const string connectionString = "Server=localhost\\SQLEXPRESS;Database=MechanicShop;Integrated Security=SSPI;";

        public Form1()
        {
            InitializeComponent();
        }

        // This method is used to open the NewCustomer form
        private void button1_Click(object sender, EventArgs e)
        {
            NewCustomer newCustomer = new NewCustomer();
            newCustomer.Show();

            // Hide the current form
            this.Hide();
        }

        // This method is used to open the Customer form
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();

            // Hide the current form
            this.Hide();
        }

        // This method is used to open the AddTech form
        private void button3_Click(object sender, EventArgs e)
        {
            AddTech addTech = new AddTech();
            addTech.Show();

            // Hide the current form
            this.Hide();
        }

        // This method is used to open the AddService form
        private void button4_Click(object sender, EventArgs e)
        {
            AddService addService = new AddService();
            addService.Show();
        }

        // This method is used to open the TechServices form
        private void button5_Click(object sender, EventArgs e)
        {
            TechServices techServices = new TechServices();
            techServices.Show();

            // Hide the current form
            this.Hide();
        }



        /*
         * Could not delete these without cause the Design window to break and complain about errors.
         * Ignore everything below this comment, but DO NOT delete it or comment it out.
         */

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
