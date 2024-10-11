﻿using Ingredients.Class;
using Ingredients.Object;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Data;
using System.Windows.Forms;

namespace Ingredients.FORMS
{
    public partial class addIngredients : Form
    {
        private fetchIngredientsTable ingredientsTableFetcher = new fetchIngredientsTable();
        private fetchItemInventory itemInventoryGETSET = new fetchItemInventory();
        private DataTable itemTable;
        private string originalValue = "";
        public addIngredients()
        {
            InitializeComponent();
            LoadItemData();
            txtIngredients.TextChanged += txtIngredients_TextChanged;
            txtHiddenID.Visible = false;
            txtID.Visible = false;
            textBox1.Visible = false;
            btnUpdate.Visible = false;
            btnRetrieve.Visible = false;
            txtqty.KeyPress += new KeyPressEventHandler(txtqty_KeyPress);
            dataGridView1.CellBeginEdit += new DataGridViewCellCancelEventHandler(dataGridView1_CellBeginEdit);

          
            
        }
        private void addIngredients_Activated(object sender, EventArgs e)
        {
            txtIngredients.Focus();
        }
        public void SetFullName(string fullName, string itemInventoryID)
        {
            // Assuming you have a Label or TextBox to display the full name
            txtFullName.Text = fullName;
            txtID.Text = itemInventoryID;
        }
        private void LoadItemData()
        {
            itemTable = itemInventoryGETSET.GetFullName(); // Retrieve data

            // Check if itemTable contains rows
            if (itemTable != null && itemTable.Rows.Count > 0)
            {
                dataGridView2.DataSource = itemTable;
                dataGridView2.Columns["ListID"].HeaderText = "List ID";
                dataGridView2.Columns["FullName"].HeaderText = "Ingredients List";

                foreach (DataGridViewColumn column in dataGridView2.Columns)
                {
                    
                        column.ReadOnly = true;  // Set all other columns as read-only
                    
                }
            }
            else
            {
                // Handle case where no items are retrieved
                MessageBox.Show("No items found in the inventory.");
                dataGridView2.DataSource = null; // Clear the DataGridView if no data
            }

            dataGridView2.Columns["ListID"].Visible = false;
        }
        public void LoadIngredientData(string itemInventoryID)
        {
            DataTable ingredientTable = ingredientsTableFetcher.GetIngredientItem(itemInventoryID);

            // Create an empty DataTable with the same structure
            DataTable emptyTable = new DataTable();
            emptyTable.Columns.Add("Ingredient ID");
            emptyTable.Columns.Add("Quantity");
            emptyTable.Columns.Add("Item Inventory ID");

            // Check if the retrieved DataTable has rows
            if (ingredientTable != null && ingredientTable.Rows.Count > 0)
            {
                dataGridView1.DataSource = ingredientTable; // Bind the data to the DataGridView

                // Set column headers
                dataGridView1.Columns["ID"].HeaderText = "ID";
                dataGridView1.Columns["ingredient_id"].HeaderText = "Ingredient ID";
                dataGridView1.Columns["qty"].HeaderText = "Quantity";
                dataGridView1.Columns["iteminventory_id"].HeaderText = "Item Inventory ID";

                // Hide the ID column
                dataGridView1.Columns["ID"].Visible = false;
                dataGridView1.ClearSelection();
                // Set all columns as read-only except the "Quantity" column
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    if (column.Name == "qty")
                    {
                        column.ReadOnly = false; // Allow editing for "Quantity" column
                    }
                    else
                    {
                        column.ReadOnly = true;  // Set all other columns as read-only
                    }
                }
            }
            else
            {
                // No data available, bind the empty table to the DataGridView
                dataGridView1.DataSource = emptyTable;
            }
        }



        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex]; // Access the selected row

                // Retrieve the full name and item inventory ID from the selected row

                string ingredientsID = selectedRow.Cells["ListID"].Value.ToString();
                string fullName = selectedRow.Cells["FullName"].Value.ToString();
                // Ensure this matches your DataTable column name

                // Set the full name to the txtIngredients TextBox
                txtIngredients.Text = fullName;
                textBox1.Text = ingredientsID;

                txtqty.Focus();

            }

        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex]; // Access the selected row

                // Retrieve the full name and item inventory ID from the selected row

                string ingredientsID = selectedRow.Cells["ListID"].Value.ToString();
                string fullName = selectedRow.Cells["FullName"].Value.ToString();
                // Ensure this matches your DataTable column name

                // Set the full name to the txtIngredients TextBox
                txtIngredients.Text = fullName;
                textBox1.Text = ingredientsID;
                txtqty.Focus();

            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow dataGridViewRow = dataGridView1.Rows[e.RowIndex];

                string hiddenID = dataGridViewRow.Cells["ID"].Value.ToString();
                string ingredientsID = dataGridViewRow.Cells["ingredient_id"].Value.ToString();
                string qty = dataGridViewRow.Cells["qty"].Value.ToString();

                // Retrieve the FullName based on the ingredientsID
                // Retrieve the FullName based on the ingredientsID
                string fullName = itemInventoryGETSET.GetFullNameUsingListID(ingredientsID);

                if (!string.IsNullOrEmpty(fullName)) // If FullName is found
                {
                    txtIngredients.Text = fullName;
                }
                else
                {
                    // Handle the case where no FullName is found
                    txtIngredients.Text = "No FullName found";
                }

            
                txtHiddenID.Text = hiddenID;
                txtqty.Text = qty;
                textBox1.Text = ingredientsID;
                button1.Visible = false;
                btnUpdate.Visible = true;
                btnRetrieve.Visible = true;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow dataGridViewRow = dataGridView1.Rows[e.RowIndex];

                string hiddenID = dataGridViewRow.Cells["ID"].Value.ToString();
                string ingredientsID = dataGridViewRow.Cells["ingredient_id"].Value.ToString();
                string qty = dataGridViewRow.Cells["qty"].Value.ToString();

                // Retrieve the FullName based on the ingredientsID
                // Retrieve the FullName based on the ingredientsID
                string fullName = itemInventoryGETSET.GetFullNameUsingListID(ingredientsID);

                if (!string.IsNullOrEmpty(fullName)) // If FullName is found
                {
                    txtIngredients.Text = fullName;
                }
                else
                {
                    // Handle the case where no FullName is found
                    txtIngredients.Text = "No FullName found";
                }

                txtHiddenID.Text = hiddenID;
                txtqty.Text = qty;
                textBox1.Text = ingredientsID;
                button1.Visible = false;
                btnUpdate.Visible = true;
                btnRetrieve.Visible = true;
            }

        }

        private void txtIngredients_TextChanged(object sender, EventArgs e)
        {
            if (itemTable != null && itemTable.Rows.Count > 0)
            {
                // Prepare the filter expression to search both 'Name' and 'FullName' columns
                string filterExpression = string.Format(" FullName LIKE '%{0}%'", txtIngredients.Text);

                // Create a DataView based on the original DataTable
                DataView dv = new DataView(itemTable);
                dv.RowFilter = filterExpression; // Apply the filter

                // Bind the filtered DataView to the DataGridView
                dataGridView2.DataSource = dv;
            }
            else
            {
                // If there are no items, clear the DataGridView
                dataGridView2.DataSource = null;
            }

            if (txtIngredients.Text == null)
            {
                btnUpdate.Visible = false;
                button1.Visible = true;
            }
        }
        private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (like backspace)
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && txtqty.Text.Contains("."))
            {
                MessageBox.Show("Only one decimal point is allowed.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Handled = true; // Prevent further processing of this character
                return;
            }

            // Allow only digits or decimal point
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                MessageBox.Show("Please enter numbers only.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Handled = true; // Prevent further processing of this character
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string ingredientsID = textBox1.Text;  // Get the ingredient ID from the hidden textbox
            string qty = txtqty.Text;               // Get the quantity from txtqty
            string itemInventoryID = txtID.Text;    // Get the item inventory ID from the hidden textbox

            // Validation 1: Ensure an ingredient is selected
            if (string.IsNullOrEmpty(ingredientsID))
            {
                MessageBox.Show("Please select an ingredient from the list.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validation 2: Ensure the quantity field is not empty
            if (string.IsNullOrEmpty(qty))
            {
                MessageBox.Show("Please enter a quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validation 3: Ensure the quantity is a valid integer number
            if (!int.TryParse(qty, out int parsedQty) || parsedQty < 0)
            {
                MessageBox.Show("Please enter a valid non-negative integer for quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validation 4: Ensure that the item inventory ID is valid
            if (string.IsNullOrEmpty(itemInventoryID))
            {
                MessageBox.Show("No valid item is selected for inventory.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If all validations pass, proceed to check for existing ingredient
            try
            {
                if (ingredientsTableFetcher.CheckIfIngredientExists(ingredientsID, itemInventoryID))
                {
                    // Retrieve the current quantity and update the total
                    int currentQty = ingredientsTableFetcher.GetCurrentQuantity(ingredientsID, itemInventoryID);
                    int newQty = currentQty + parsedQty; // Add the new quantity to the existing one

                    // Update the ingredient with the new total quantity
                    ingredientsTableFetcher.UpdateIngredientsIfExist(newQty.ToString(), ingredientsID, itemInventoryID);
                }
                else
                {
                    // Insert the new ingredient if it does not exist
                    ingredientsTableFetcher.InsertIngredients(ingredientsID, qty, itemInventoryID);
                   
                }

                // Refresh the ingredients data in the DataGridView after operation
                LoadIngredientData(itemInventoryID);

                // Optionally, clear the fields after successful insertion or update
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while processing the ingredient: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Method to check if an ingredient exists in the database

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // Optional helper method to clear fields after insertion
        private void ClearFields()
        {
            txtIngredients.Clear();
            textBox1.Clear(); // Clear the hidden ingredient ID textbox
            txtqty.Clear();
            txtHiddenID.Clear();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string itemInventoryID = txtID.Text;
            LoadItemData();
            LoadIngredientData(itemInventoryID);
            txtIngredients.Clear();
            button1.Visible = true;
            button2.Visible = true;
            btnUpdate.Visible = false;
            btnRetrieve.Visible = false;
            textBox1.Clear();
            txtqty.Clear();
            txtHiddenID.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string ingredientsID = textBox1.Text; // Ingredient ID
            string qty = txtqty.Text; // Quantity
            string itemInventoryID = txtID.Text; // Item Inventory ID
            string hiddenID = txtHiddenID.Text;

            // Ensure that the fields are filled before calling the update
            if (string.IsNullOrEmpty(ingredientsID) || string.IsNullOrEmpty(qty) || string.IsNullOrEmpty(itemInventoryID) || string.IsNullOrEmpty(hiddenID))
            {
                MessageBox.Show("Please ensure all fields are filled.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update the ingredient data
            ingredientsTableFetcher.UpdateIngredients(ingredientsID, qty, hiddenID);

            // Optionally reload data after updating
            LoadIngredientData(itemInventoryID);
            button1.Visible = true;
            btnUpdate.Visible = false;
            // Clear fields after updating
            btnRetrieve.Visible=false;
            button2.Visible = true;
            ClearFields();
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {

            var result = MessageBox.Show("Are you sure you want to delete this ingredient?", "Confirm Delete",
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Assuming you have a way to get the ID of the ingredient to delete
                string ingredientId = txtHiddenID.Text; // Replace with actual ID retrieval logic
               ingredientsTableFetcher.DeleteIngredients(ingredientId);

                string itemInventoryID = txtID.Text;
                LoadItemData();
                LoadIngredientData(itemInventoryID);
                btnUpdate.Visible = false;
                btnRetrieve.Visible = false;
                button1.Visible=true;
                button2.Visible = true;
                ClearFields();
                
            }
        }
      

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Store the original value when editing begins
            if (e.ColumnIndex == dataGridView1.Columns["qty"].Index)
            {
                originalValue = dataGridView1.Rows[e.RowIndex].Cells["qty"].Value.ToString();
            }



        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
     
            if (e.ColumnIndex == dataGridView1.Columns["qty"].Index)
            {
                int rowIndex = e.RowIndex;
                string ingredientID = dataGridView1.Rows[rowIndex].Cells["ingredient_id"].Value.ToString();
                string itemInventoryID = dataGridView1.Rows[rowIndex].Cells["iteminventory_id"].Value.ToString();
                string newQty = dataGridView1.Rows[rowIndex].Cells["qty"].Value.ToString().Trim(); // Trim whitespace

               
               
                if (newQty != originalValue)
                {
                   
                    if (decimal.TryParse(newQty, out decimal parsedQty) && parsedQty >= 0)
                    {
                        var result = MessageBox.Show("Are you sure you want to update this Quantity?", "Confirm Update",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                ingredientsTableFetcher.UpdateIngredientsIfExist(newQty, ingredientID, itemInventoryID);
                                LoadIngredientData(itemInventoryID);
                                // Clear and reset UI controls as needed
                                button1.Visible = true;
                                btnUpdate.Visible = false;
                                // Clear fields after updating
                                btnRetrieve.Visible = false;
                                button2.Visible = true;
                                ClearFields();
                                txtIngredients.Focus();
                               
                              
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error updating quantity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            // Reset the value back to originalValue if user clicks "No"
                            dataGridView1.Rows[rowIndex].Cells["qty"].Value = originalValue;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid non-negative number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridView1.Rows[rowIndex].Cells["qty"].Value = originalValue;
                        dataGridView1.CancelEdit(); // Optionally cancel the edit
                    }
                }

            }

        }


        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Check if we are editing the "Quantity" column
            if (dataGridView1.CurrentCell.ColumnIndex == dataGridView1.Columns["qty"].Index)
            {
                TextBox txtQty = e.Control as TextBox;

                if (txtQty != null)
                {
                    // Unsubscribe from previous handlers if any to avoid multiple event calls
                    txtQty.KeyPress -= TxtQty_KeyPress;

                    // Attach the new KeyPress event handler
                    txtQty.KeyPress += TxtQty_KeyPress;
                }
            }
        }
        private void TxtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txtqty = sender as TextBox;

            // Allow control keys (like backspace)
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && txtqty.Text.Contains("."))
            {
                MessageBox.Show("Only one decimal point is allowed.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Handled = true; // Prevent further processing of this character
                return;
            }

            // Allow only digits or decimal point
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                MessageBox.Show("Please enter numbers only.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Handled = true; // Prevent further processing of this character
            }
        }

        private void addIngredients_Load(object sender, EventArgs e)
        {

        }

        
    }
}
