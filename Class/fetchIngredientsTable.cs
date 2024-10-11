using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ingredients.Class
{
    internal class fetchIngredientsTable
    {
        public DataTable GetIngredientItem(string itemInventoryID)
        {
            DataTable itemTable = new DataTable();

            string query = "SELECT ID, ingredient_id, qty, iteminventory_id from ingredients_table WHERE iteminventory_id = @iteminventory_id;";

            string connectionString = ConnectionString.GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@iteminventory_id", itemInventoryID);
                       
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
        public void InsertIngredients(string ingredientsID, string qty, string itemInventoryID)
        {
            // SQL query to insert a new ingredient into the ingredients_table
            string query = "INSERT INTO ingredients_table (ingredient_id, qty, iteminventory_id) VALUES (@ingredients_id, @qty, @iteminventory_id);";

            string connectionString = ConnectionString.GetConnectionString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Set parameter values
                    command.Parameters.AddWithValue("@ingredients_id", ingredientsID);
                    command.Parameters.AddWithValue("@qty", qty);
                    command.Parameters.AddWithValue("@iteminventory_id", itemInventoryID);

                    try
                    {
                        // Open the connection
                        connection.Open();
                        // Execute the command
                        command.ExecuteNonQuery();
                        MessageBox.Show("Successful Inserting Ingredients.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (e.g., log them or display a message)
                        MessageBox.Show("Error inserting ingredient: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        public void UpdateIngredientsIfExist(string qty, string ingredientsID, string itemInventoryID)
        {
            string query = "UPDATE ingredients_table SET qty = @qty WHERE ingredient_id = @ingredients_id AND iteminventory_id = @item_inventory_id;";



            string connectionString = ConnectionString.GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add the parameters with their respective values
                      
                        command.Parameters.AddWithValue("@qty", qty);
                        command.Parameters.AddWithValue("@ingredients_id", ingredientsID);
                        command.Parameters.AddWithValue("@item_inventory_id", itemInventoryID);

                        // Execute the update command
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Successful updating Ingredients.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated. Please check the item inventory ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating the ingredient: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public int GetCurrentQuantity(string ingredientsID, string itemInventoryID)
        {
            string query = "SELECT qty FROM ingredients_table WHERE ingredient_id = @ingredients_id AND iteminventory_id = @item_inventory_id;";
            string connectionString = ConnectionString.GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ingredients_id", ingredientsID);
                        command.Parameters.AddWithValue("@item_inventory_id", itemInventoryID);

                        object result = command.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0; // Return 0 if not found
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while retrieving the current quantity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
            }
        }
        public bool CheckIfIngredientExists(string ingredientsID, string itemInventoryID)
        {
            string query = "SELECT COUNT(*) FROM ingredients_table WHERE ingredient_id = @ingredients_id AND iteminventory_id = @item_inventory_id;";
            string connectionString = ConnectionString.GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ingredients_id", ingredientsID);
                        command.Parameters.AddWithValue("@item_inventory_id", itemInventoryID);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0; // Return true if the ingredient exists
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while checking for existing ingredient: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public void UpdateIngredients(string ingredientsID, string qty, string ID)
        {
            string query = "UPDATE ingredients_table SET ingredient_id = @ingredients_id, qty = @qty WHERE ID = @ID;";

            string connectionString = ConnectionString.GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add the parameters with their respective values
                        command.Parameters.AddWithValue("@ingredients_id", ingredientsID);
                        command.Parameters.AddWithValue("@qty", qty);
                        command.Parameters.AddWithValue("@ID", ID);

                        // Execute the update command
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Successful updating Ingredients.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated. Please check the item inventory ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating the ingredient: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void DeleteIngredients(string ID)
        {
            string query = "DELETE FROM ingredients_table WHERE ID = @ID";
            string connectionString = ConnectionString.GetConnectionString();

            using(MySqlConnection connection = new MySqlConnection( connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", ID);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Successfully deleted the ingredient.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Unable to Delete Ingredients, Try Again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

    }
}
