using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ingredients.Object;
using MySql.Data.MySqlClient;

namespace Ingredients.Class
{
    internal class fetchItemInventory
    {
        // Method to fetch all inventory items from the database and return as a DataTable
       public DataTable GetAllItem()
        {
            DataTable itemTable = new DataTable();

            string query = "SELECT ListID, Name, FullName from iteminventory";

            string connectionString = ConnectionString.GetConnectionString();

            using(MySqlConnection  connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                        adapter.Fill(itemTable);


                    }
                }
                catch (Exception ex)
                { 
                
                     MessageBox.Show(ex.Message);
                }
            }
            return itemTable;
        }
        public DataTable GetFullName()
        {
            DataTable itemTable = new DataTable();

            string query = "SELECT ListID, FullName from iteminventory";

            string connectionString = ConnectionString.GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                        adapter.Fill(itemTable);


                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            return itemTable;
        }
        public DataTable GetID(string fullname)
        {
            DataTable itemTable = new DataTable();

            string query = "SELECT ListID from iteminventory WHERE FullName = @FullName";

            string connectionString = ConnectionString.GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", fullname);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        adapter.Fill(itemTable);


                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            return itemTable;
        }

        public string GetFullNameUsingListID(string listID)
        {
            string fullName = string.Empty; // Default value if nothing is found

            string query = "SELECT FullName FROM iteminventory WHERE ListID = @ListID";

            string connectionString = ConnectionString.GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add the parameter for the ListID
                        command.Parameters.AddWithValue("@ListID", listID);

                        // Use ExecuteScalar to get a single value
                        object result = command.ExecuteScalar();

                        // If result is not null, convert it to string
                        if (result != null)
                        {
                            fullName = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return fullName;
        }

    }
}
