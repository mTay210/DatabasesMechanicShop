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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MechanicShop
{
    public partial class Appointment : Form
    {
        // Connection string to the database
        private const string connectionString = "Server=localhost\\SQLEXPRESS;Database=MechanicShop;Integrated Security=SSPI;";

        // Properties to store the data passed from Form3
        public int CustCarID { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }

        private List<int> selectedTechServiceIDs = new List<int>();
        private List<int> selectedTechnicianIDs = new List<int>();

        // Field to store the selected date and time
        private DateTime selectedDateTime;

        // Property to store the selected date and time
        public DateTime DateAndTime { get; set; }

        public Appointment()
        {
            InitializeComponent();
            PopulateServicesComboBox();

            // Initialize the selectedDateTime field to current date and time
            selectedDateTime = DateTime.Now;
        }

        // Populate the Services combobox
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Populate the technicians combo box based on the selected service
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

        // Method to get the Tech_Service_ID based on Tech_ID and Service_ID
        private int GetTechServiceID(System.Windows.Forms.ComboBox techComboBox, System.Windows.Forms.ComboBox serviceComboBox)
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
                        // Assuming the ComboBoxes are bound to a DataTable with appropriate ValueMember set
                        command.Parameters.AddWithValue("@TechID", techComboBox.SelectedValue);
                        command.Parameters.AddWithValue("@ServiceID", serviceComboBox.SelectedValue);
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

            // Debugging message to display the technician's ID
            // MessageBox.Show("Technician ID: " + techID);

            return techID;
        }

        // Add an event handler for comboBox3's SelectedIndexChanged event
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedValue != null && comboBox3.SelectedValue != DBNull.Value)
            {
                int selectedServiceID = Convert.ToInt32(comboBox3.SelectedValue);
                PopulateTechniciansComboBox(selectedServiceID);
            }
        }

        // Method to submit the appointment and link it to both Cust_Car_Service_Date_Time and Car_Service_Date_Services
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the car information was passed from Form3
                if (CustCarID > 0 && !string.IsNullOrEmpty(CustomerFirstName) && !string.IsNullOrEmpty(CustomerLastName))
                {
                    // Get the appointment date and time from selectedDateTime
                    DateTime appointmentDateTime = selectedDateTime;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Loop through the saved services and technicians
                        for (int i = 0; i < selectedTechServiceIDs.Count; i++)
                        {
                            // SQL query to insert appointment data into Cust_Car_Service_Date_Time
                            string insertQuery = @"
                                    INSERT INTO Cust_Car_Service_Date_Time (Cust_Car_ID, ServiceDate, ServiceTime) 
                                    VALUES (@CustCarID, @AppointmentDate, @AppointmentTime);

                                    INSERT INTO Car_Service_Date_Services (Cust_Car_Date_ID, Tech_Service_ID) 
                                    VALUES (SCOPE_IDENTITY(), @TechServiceID)";

                            using (SqlCommand command = new SqlCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@CustCarID", CustCarID);
                                command.Parameters.AddWithValue("@AppointmentDate", appointmentDateTime.Date);
                                command.Parameters.AddWithValue("@AppointmentTime", appointmentDateTime.TimeOfDay);
                                command.Parameters.AddWithValue("@TechServiceID", selectedTechServiceIDs[i]);

                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Debugging messages to display the scheduled services
                                    // MessageBox.Show("Service scheduled successfully: " + comboBox3.Text);
                                }
                                else
                                {
                                    MessageBox.Show("Error adding service: " + comboBox3.Text);
                                }
                            }
                        }

                        // Clear the lists after submission
                        selectedTechServiceIDs.Clear();
                        selectedTechnicianIDs.Clear();
                    }

                    MessageBox.Show("All services scheduled successfully!");
                }
                else
                {
                    MessageBox.Show("Error: Selected car information is missing.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        // Method to add the selected service and technician to the the appointment
        private void button1_Click(object sender, EventArgs e)
        {
            int techServiceID = GetTechServiceID(comboBox4, comboBox3);
            int technicianID = GetTechnicianID(comboBox4.Text);

            // Check if service and technician are valid
            if (techServiceID > 0 && technicianID > 0)
            {
                // Save the selected service and technician
                selectedTechServiceIDs.Add(techServiceID);
                selectedTechnicianIDs.Add(technicianID);

                // Debug message to display the selected service and technician
                //MessageBox.Show("Service added: " + comboBox3.Text + " - Technician: " + comboBox4.Text);
            }
            else
            {
                MessageBox.Show("Error: Please select a valid service and technician.");
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
