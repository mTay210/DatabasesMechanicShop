using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MechanicShop
{
    public partial class Form3 : Form
    {
        private const string connectionString = "Server=localhost\\SQLEXPRESS;Database=MechanicShop;Integrated Security=SSPI;";

        public Form3()
        {
            InitializeComponent();
            PopulateModelComboBox();
            PopulateMakeComboBox();
        }

        private void PopulateModelComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ModelID, Model FROM Model";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBox1.DisplayMember = "Model";
                    comboBox1.ValueMember = "ModelID";
                    comboBox1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void PopulateMakeComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT MakeID, Make FROM Make";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBox2.DisplayMember = "Make";
                    comboBox2.ValueMember = "MakeID";
                    comboBox2.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedMakeID = Convert.ToInt32(comboBox2.SelectedValue);
                int selectedModelID = Convert.ToInt32(comboBox1.SelectedValue);
                string licensePlate = textBox1.Text;

                // SQL query to insert data into the Car table and retrieve the Car_ID
                string carQuery = "INSERT INTO Car (MakeID, ModelID, LicensePlate) VALUES (@MakeID, @ModelID, @LicensePlate); SELECT SCOPE_IDENTITY();";

                int carID;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(carQuery, connection))
                    {
                        command.Parameters.AddWithValue("@MakeID", selectedMakeID);
                        command.Parameters.AddWithValue("@ModelID", selectedModelID);
                        command.Parameters.AddWithValue("@LicensePlate", licensePlate);

                        // ExecuteScalar to get the newly inserted Car_ID
                        carID = Convert.ToInt32(command.ExecuteScalar());
                    }
                }

                // Check if Car_ID was retrieved successfully
                if (carID > 0)
                {
                    // Debugging message to check carID
                    MessageBox.Show("Car ID: " + carID);

                    // Get the Cust_ID from the searched customer
                    int custID = GetCustomerID(textBox4.Text);

                    // Debugging message to check custID
                    MessageBox.Show("Cust ID: " + custID);

                    if (custID > 0)
                    {
                        // Insert into CarOwner table
                        string ownerQuery = "INSERT INTO CarOwner (Cust_ID, Car_ID) VALUES (@CustID, @CarID)";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand(ownerQuery, connection))
                            {
                                command.Parameters.AddWithValue("@CustID", custID);
                                command.Parameters.AddWithValue("@CarID", carID);

                                // ExecuteNonQuery to insert into CarOwner table
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Car added successfully and linked to the customer!");
                                }
                                else
                                {
                                    MessageBox.Show("Error linking car to the customer.");
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Customer not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Error adding the car.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        // Method to get Cust_ID from phone number
        private int GetCustomerID(string phoneNumber)
        {
            string query = "SELECT Cust_ID FROM Customer WHERE Phone = @PhoneNumber";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    connection.Close();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Code for button2_Click
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2 
            Form1 form1 = new Form1();

            // Show Form2
            form1.Show();

            // Optionally, hide or close Form1
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string phoneNumber = textBox4.Text;

            // Check if phone number is empty
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show("Phone number is required to search for the customer.");
                return; // Exit the method without further execution
            }

            // SQL query to select customer information based on phone number
            string query = "SELECT Cust_FN, Cust_LN, Phone FROM Customer WHERE Phone = @PhoneNumber";

            // Create a SqlConnection object using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create a SqlCommand object with the query and connection
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameter to prevent SQL injection
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    try
                    {
                        // Open the connection
                        connection.Open();

                        // Execute the command and get the SqlDataReader
                        SqlDataReader reader = command.ExecuteReader();

                        // Check if the reader has rows
                        if (reader.HasRows)
                        {
                            // Read the first row
                            reader.Read();

                            // Set the values in Form3 textboxes
                            textBox2.Text = reader["Cust_FN"].ToString();
                            textBox3.Text = reader["Cust_LN"].ToString();
                            textBox4.Text = reader["Phone"].ToString();

                            MessageBox.Show("Customer found.");
                        }
                        else
                        {
                            MessageBox.Show("Customer not found.");
                        }

                        // Close the reader
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Code for dataGridView1_CellContentClick
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Code for textBox1_TextChanged
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Code for comboBox1_SelectedIndexChanged
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Code for comboBox2_SelectedIndexChanged
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Code for textBox2_TextChanged
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Code for textBox3_TextChanged
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Code for textBox4_TextChanged
        }
    }
}

