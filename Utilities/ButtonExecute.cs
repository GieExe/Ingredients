using Ingredients.Class;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Ingredients.Utilities
{
    internal class ButtonExecute
    {
       
        private fetchIngredientsTable ingredientsTableFetcher = new fetchIngredientsTable();
        public ButtonExecute(fetchIngredientsTable fetcher) // Pass in the fetcher object for database operations
        {
            ingredientsTableFetcher = fetcher;
        }

        // Main method to handle button logic
        public void HandleButtonClick(string ingredientsID, string qty, string itemInventoryID, string type, TextBox txtIngredients, TextBox txtqty, Action<string> loadIngredientData, Action clearFields)
        {
            // Validation 1: Ensure an ingredient is selected
        

            // Validation 3: Ensure the quantity is a valid non-negative decimal number
            if (!decimal.TryParse(qty, out decimal parsedQty) || parsedQty < 0)
            {
                MessageBox.Show("Please enter a valid non-negative decimal number for quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtqty.Focus();
                return;
            }

    
            // If all validations pass, proceed to check for existing ingredient
            try
            {
                if (ingredientsTableFetcher.CheckIfIngredientExists(ingredientsID, itemInventoryID))
                {
                    // Retrieve the current quantity and update the total
                    decimal currentQty = ingredientsTableFetcher.GetCurrentQuantity(ingredientsID, itemInventoryID);
                    decimal newQty = currentQty + parsedQty; // Add the new quantity to the existing one

                    // Update the ingredient with the new total quantity
                    ingredientsTableFetcher.UpdateIngredientsIfExist(newQty.ToString(), ingredientsID, itemInventoryID);
                }
                else
                {
                    // Insert the new ingredient if it does not exist
                    ingredientsTableFetcher.InsertIngredients(ingredientsID, qty, itemInventoryID,type);
                }

                // Refresh the ingredients data in the DataGridView after operation
                loadIngredientData(itemInventoryID);

                // Optionally, clear the fields after successful insertion or update
                clearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while processing the ingredient: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public string DetermineItemType(string ingredientID)
        {
            string connectionString = ConnectionString.GetConnectionString(); // Your connection string
            string itemType = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                   
                    string queryItemInventory = "SELECT COUNT(*) FROM iteminventory WHERE ListID = @ingredientID";
                    using (MySqlCommand command = new MySqlCommand(queryItemInventory, connection))
                    {
                        command.Parameters.AddWithValue("@ingredientID", ingredientID);
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count > 0)
                        {
                            itemType = "ITEM INVENTORY";
                            return itemType;  // Return immediately if found in iteminventory
                        }
                    }

                    // Check if the ID exists in iteminventoryassembly
                    string queryItemAssembly = "SELECT COUNT(*) FROM iteminventoryassembly WHERE ListID = @ingredientID";
                    using (MySqlCommand command = new MySqlCommand(queryItemAssembly, connection))
                    {
                        command.Parameters.AddWithValue("@ingredientID", ingredientID);
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count > 0)
                        {
                            itemType = "ITEM ASSEMBLY";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while determining the item type: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return itemType;  // Return the found type, or empty if not found in either table
        }

    }
}
