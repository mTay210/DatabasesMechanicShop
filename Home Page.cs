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

        private void button1_Click(object sender, EventArgs e)
        {
            string firstName = textBox4.Text;
            string lastName = textBox5.Text;
            string phoneNumber = textBox6.Text;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show("Please enter all fields.");
                return;
            }

            // SQL query to insert data into the Customer table
            string query = "INSERT INTO Customer (Cust_FN, Cust_LN, Phone) VALUES (@FirstName, @LastName, @PhoneNumber)";

            // Create a SqlConnection object using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create a SqlCommand object with the query and connection
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    try
                    {
                        // Open the connection
                        connection.Open();
                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();
                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer added successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to add customer.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }

        // This method is used to open the Customer form
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();

            // Close the home page
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
