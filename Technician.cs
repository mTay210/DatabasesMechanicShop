using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MechanicShop
{
    public partial class Technician : Form
    {
        private const string connectionString = "Server=localhost\\SQLEXPRESS;Database=MechanicShop;Integrated Security=SSPI;";

        public Technician()
        {
            InitializeComponent();
            PopulateTechniciansComboBox();
        }

        private void PopulateTechniciansComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Tech_ID, Tech_FN, Tech_LN FROM Technician";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dt.Columns.Add("FullName", typeof(string), "Tech_FN + ' ' + Tech_LN"); // Concatenate first name and last name into FullName column

                    comboBox1.DisplayMember = "FullName"; // Use FullName as the DisplayMember
                    comboBox1.ValueMember = "Tech_ID";
                    comboBox1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void Technician_Load(object sender, EventArgs e)
        {
            PopulateTechniciansComboBox();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
