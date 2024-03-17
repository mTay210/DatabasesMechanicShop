using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MechanicShop
{
    public partial class Form3 : Form
    {
        // Connections to the server and database
        private const string connectionString = "Server=localhost\\SQLEXPRESS;Database=MechanicShop;Integrated Security=SSPI;";

        // Constructor
        public Form3()
        {
            InitializeComponent();
            PopulateModelComboBox();
            PopulateMakeComboBox();
            PopulateServicesComboBox();

            // Set the DateTimePicker format to Custom to display both date and time
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            // Set the CustomFormat to a format that includes both date and time
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm";
        }

        // Populate the technicians combo box with all technicians available for the selected service
        private void PopulateTechniciansComboBox(int serviceID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT DISTINCT T.Tech_ID, T.Tech_FN, T.Tech_LN 
                     FROM Technician T 
                     INNER JOIN Tech_to_Services TS ON T.Tech_ID = TS.Tech_ID 
                     WHERE TS.Service_ID = @ServiceID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@ServiceID", serviceID);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dt.Columns.Add("FullName", typeof(string), "Tech_FN + ' ' + Tech_LN");

                    comboBox4.DisplayMember = "FullName";
                    comboBox4.ValueMember = "Tech_ID";
                    comboBox4.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Populates the Services combobox and send the selected service ID to the PopulateTechniciansComboBox method
        private void PopulateServicesComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Service_ID, Service FROM Services";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Create a new row with a blank item
                    DataRow blankRow = dt.NewRow();
                    blankRow["Service_ID"] = DBNull.Value;
                    blankRow["Service"] = string.Empty;
                    dt.Rows.InsertAt(blankRow, 0);

                    comboBox3.DisplayMember = "Service";
                    comboBox3.ValueMember = "Service_ID";
                    comboBox3.DataSource = dt;

                    // Add event handler for SelectedIndexChanged
                    comboBox3.SelectedIndexChanged += (sender, e) =>
                    {
                        if (comboBox3.SelectedValue != null && comboBox3.SelectedValue != DBNull.Value)
                        {
                            int selectedServiceID = Convert.ToInt32(comboBox3.SelectedValue);
                            PopulateTechniciansComboBox(selectedServiceID);
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Populates the Model combobox based on the selected Make
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

        // Method to populate the Make combobox
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

        // Adds a new car to the database  BUT does NOT and links it to the customer if there is more than 1
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

                    // Create a SqlCommand object with the query and connection
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
                    // Get the Cust_ID from the searched customer
                    int custID = GetCustomerID(textBox4.Text);

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

        // Method to go back to the home page
        private void button4_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2 
            Form1 form1 = new Form1();

            // Show Form2
            form1.Show();

            // Optionally, hide or close Form1
            this.Hide();
        }

        // Method to submit the appointment
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the selected customer's last car ID
                int custCarID = GetCustomerCarID(textBox4.Text);

                if (custCarID > 0)
                {
                    // Get the appointment date and time from DateTimePicker
                    DateTime appointmentDateTime = dateTimePicker1.Value;

                    // SQL query to insert appointment data into Cust_Car_Service_Date_Time table
                    string insertQuery = @"
                                    INSERT INTO Cust_Car_Service_Date_Time (Cust_Car_ID, ServiceDate, ServiceTime) 
                                    VALUES (@CustCarID, @AppointmentDate, @AppointmentTime)";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@CustCarID", custCarID);
                            command.Parameters.AddWithValue("@AppointmentDate", appointmentDateTime.Date);
                            command.Parameters.AddWithValue("@AppointmentTime", appointmentDateTime.TimeOfDay);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Appointment scheduled successfully and added to Cust_Car_Service_Date_Time table!");
                            }
                            else
                            {
                                MessageBox.Show("Error adding appointment to Cust_Car_Service_Date_Time table.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error: Could not find a car associated with the customer.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Method to get the Tech_Service_ID based on Tech_ID and Service_ID
        private int GetTechServiceID(int techID, int serviceID)
        {
            int techServiceID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Tech_Service_ID FROM Tech_to_Services WHERE Tech_ID = @TechID AND Service_ID = @ServiceID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TechID", techID);
                        command.Parameters.AddWithValue("@ServiceID", serviceID);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            techServiceID = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting Tech_Service_ID: " + ex.Message);
            }

            return techServiceID;
        }

        // Method to get the technician's ID
        private int GetTechnicianID(string technicianFullName)
        {
            int techID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT Tech_ID FROM Technician WHERE CONCAT(Tech_FN, ' ', Tech_LN) = @FullName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", technicianFullName);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            techID = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting Technician ID: " + ex.Message);
            }

            // Display pop-up message with the technician's ID
            MessageBox.Show("Technician ID: " + techID);

            return techID;
        }

        // Method to get the customer's latest car ID
        private int GetCustomerCarID(string phoneNumber)
        {
            int custID = GetCustomerID(phoneNumber); // Get the customer ID using the phone number

            if (custID > 0)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // SQL query to get the latest car ID associated with the customer
                        string query = @"
                                SELECT TOP 1 Car_ID 
                                FROM CarOwner 
                                WHERE Cust_ID = @CustID 
                                ORDER BY Cust_Car_ID DESC";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CustID", custID);

                            object result = command.ExecuteScalar();

                            if (result != null)
                            {
                                // Display pop-up message with the customer's car ID
                                MessageBox.Show("Customer's Car ID: " + Convert.ToInt32(result));

                                return Convert.ToInt32(result);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error getting customer's car ID: " + ex.Message);
                }
            }

            // Display pop-up message if the customer or their car ID is not found
            MessageBox.Show("Customer or Car ID not found.");

            return 0; // Return 0 if the customer or their car ID is not found
        }

        // Method to search for a customer based on phone number and display their service history and populate comboBox5 with their cars
        private void button3_Click(object sender, EventArgs e)
        {
            string phoneNumber = textBox4.Text;

            // Check if phone number is empty
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show("Phone number is required to search for the customer.");
                return; // Exit the method without further execution
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to select customer information based on phone number
                    string query = "SELECT Cust_ID, Cust_FN, Cust_LN FROM Customer WHERE Phone = @PhoneNumber";

                    // Create a SqlCommand object with the query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameter to prevent SQL injection
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

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

                            // Get the customer ID for further queries
                            int custID = Convert.ToInt32(reader["Cust_ID"]);

                            // Close the reader before executing the second query
                            reader.Close();

                            // SQL query to retrieve service information for the customer
                            string serviceQuery = @"
                        SELECT
                            M.Make AS Car_Make,
                            MD.Model AS Car_Model,
                            Ca.LicensePlate AS Car_License_Plate,
                            CONVERT(VARCHAR(10), CCSDT.ServiceDate, 120) AS Service_Date, -- Format as yyyy-mm-dd
                            CONVERT(VARCHAR(8), CCSDT.ServiceTime, 108) AS Service_Time, -- Format as HH:MM:SS
                            S.Service AS Service_Name,
                            S.Cost AS Service_Cost,
                            CONCAT(T.Tech_FN, ' ', T.Tech_LN) AS Technician_Name
                        FROM
                            CarOwner CO
                            INNER JOIN Car Ca ON CO.Car_ID = Ca.Car_ID
                            INNER JOIN Cust_Car_Service_Date_Time CCSDT ON CO.Cust_Car_ID = CCSDT.Cust_Car_ID
                            INNER JOIN Car_Service_Date_Services CSDS ON CCSDT.Cust_Car_Date_ID = CSDS.Cust_Car_Date_ID
                            INNER JOIN Tech_to_Services TS ON CSDS.Tech_Service_ID = TS.Tech_Service_ID
                            INNER JOIN Services S ON TS.Service_ID = S.Service_ID
                            INNER JOIN Technician T ON TS.Tech_ID = T.Tech_ID
                            INNER JOIN Make M ON Ca.MakeID = M.MakeID
                            INNER JOIN Model MD ON Ca.ModelID = MD.ModelID
                        WHERE
                            CO.Cust_ID = @CustID";

                            // Create a new SqlCommand object for the service query
                            using (SqlCommand serviceCommand = new SqlCommand(serviceQuery, connection))
                            {
                                // Add parameter for customer ID
                                serviceCommand.Parameters.AddWithValue("@CustID", custID);

                                // Execute the service query and populate a DataTable
                                SqlDataAdapter adapter = new SqlDataAdapter(serviceCommand);
                                DataTable dt = new DataTable();
                                adapter.Fill(dt);

                                // Bind the DataTable to the DataGridView
                                dataGridView1.DataSource = dt;

                                // Optional: Display a message if no service records are found
                                if (dt.Rows.Count == 0)
                                {
                                    MessageBox.Show("No service records found for the customer.");
                                }
                            }

                            // Populate comboBox5 with the customer's cars
                            PopulateCarsComboBox(custID);
                        }
                        else
                        {
                            MessageBox.Show("Customer not found.");
                        }

                        // Close the reader
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        // Method to populate the cars in comboBox5 currently not working
        public class CarItem
        {
            public int CarID { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }

            public CarItem(int carID, string make, string model)
            {
                CarID = carID;
                Make = make;
                Model = model;
            }

            public override string ToString()
            {
                return $"{Make} {Model}";
            }
        }

        // Method to populate the cars in comboBox5 displays license plate number
        private void PopulateCarsComboBox(int custID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to retrieve customer's cars with make, model, and license plate details
                    string query = @"
                SELECT Car.Car_ID, Make.Make, Model.Model, Car.LicensePlate 
                FROM Car 
                INNER JOIN Make ON Car.MakeID = Make.MakeID 
                INNER JOIN Model ON Car.ModelID = Model.ModelID 
                INNER JOIN CarOwner ON Car.Car_ID = CarOwner.Car_ID 
                WHERE CarOwner.Cust_ID = @CustID";

                    // Create a SqlCommand object for the query
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameter for customer ID
                        command.Parameters.AddWithValue("@CustID", custID);

                        // Execute the query and populate a DataTable
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Set the DisplayMember and ValueMember for comboBox5
                        comboBox5.DisplayMember = "DisplayText";
                        comboBox5.ValueMember = "Car_ID";

                        // Create a new DataColumn for the display text (Make, Model, LicensePlate)
                        dt.Columns.Add("DisplayText", typeof(string), "Make + ' ' + Model + ' - ' + LicensePlate");

                        // Bind the DataTable to comboBox5
                        comboBox5.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while populating the cars in comboBox5: " + ex.Message);
            }
        }








        /*
         * Could not delete these without cause the Design window to break and complain about errors.
         * Ignore everything below this comment, but DO NOT delete it or comment it out.
         */
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

