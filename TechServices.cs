using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MechanicShop
{
    public partial class TechServices : Form
    {
        // Connect to the server and database
        private const string connectionString = "Server=localhost\\SQLEXPRESS;Database=MechanicShop;Integrated Security=SSPI;";

        // Declare ComboBox controls at the class level
        private ComboBox comboBoxTechnicians;
        private ComboBox comboBoxServices;

        public TechServices()
        {
            InitializeComponent();
            InitializeComboBoxes(); // Call a method to initialize ComboBox controls
        }

        private void InitializeComboBoxes()
        {
            comboBoxTechnicians = new ComboBox();
            comboBoxTechnicians.DropDownStyle = ComboBoxStyle.DropDownList; // Set drop-down style
            comboBoxTechnicians.Location = new System.Drawing.Point(20, 20); // Adjust location as needed
            comboBoxTechnicians.Width = 200; // Adjust width as needed

            comboBoxServices = new ComboBox();
            comboBoxServices.DropDownStyle = ComboBoxStyle.DropDownList; // Set drop-down style
            comboBoxServices.Location = new System.Drawing.Point(20, 60); // Adjust location as needed
            comboBoxServices.Width = 200; // Adjust width as needed

            // Add ComboBox controls to the form's Controls collection
            Controls.Add(comboBoxTechnicians);
            Controls.Add(comboBoxServices);

            // Populate ComboBoxes with data from the database
            PopulateTechniciansComboBox();
            PopulateServicesComboBox();

            // Attach event handler to button2_Click
            button2.Click += button2_Click;
        }

        private void PopulateTechniciansComboBox()
        {
            // Clear existing items in comboBoxTechnicians
            comboBoxTechnicians.Items.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Fetch technician data from your database
                    string query = "SELECT Tech_ID, CONCAT(Tech_FN, ' ', Tech_LN) AS FullName FROM Technician";

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int techID = reader.GetInt32(0);
                        string fullName = reader.GetString(1);
                        ComboBoxItem item = new ComboBoxItem(fullName, techID);
                        comboBoxTechnicians.Items.Add(item);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching technicians: " + ex.Message);
            }
        }

        private void PopulateServicesComboBox()
        {
            // Clear existing items in comboBoxServices
            comboBoxServices.Items.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Fetch service data from your database
                    string query = "SELECT Service_ID, Service FROM Services";

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int serviceID = reader.GetInt32(0);
                        string serviceName = reader.GetString(1);
                        ComboBoxItem item = new ComboBoxItem(serviceName, serviceID);
                        comboBoxServices.Items.Add(item);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching services: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Check if comboBoxTechnicians and comboBoxServices are not null
            if (comboBoxTechnicians != null && comboBoxServices != null &&
                comboBoxTechnicians.SelectedItem != null && comboBoxServices.SelectedItem != null)
            {
                // Handle the assignment of technician to service
                ComboBoxItem selectedTechnician = (ComboBoxItem)comboBoxTechnicians.SelectedItem;
                ComboBoxItem selectedService = (ComboBoxItem)comboBoxServices.SelectedItem;

                int technicianID = selectedTechnician.Value;
                int serviceID = selectedService.Value;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Insert the technicianID and serviceID into the Tech_to_Services table
                        string insertQuery = "INSERT INTO Tech_to_Services (Tech_ID, Service_ID) VALUES (@TechID, @ServiceID)";

                        SqlCommand command = new SqlCommand(insertQuery, connection);
                        command.Parameters.AddWithValue("@TechID", technicianID);
                        command.Parameters.AddWithValue("@ServiceID", serviceID);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Assignment successful.");
                        }
                        else
                        {
                            MessageBox.Show("Assignment failed.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error assigning technician to service: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select both a technician and a service.");
            }
        }


        // Custom class to hold ComboBox item data
        public class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public ComboBoxItem(string text, int value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
