using Ingredients.Class;
using Ingredients.Object;
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
        public addIngredients()
        {
            InitializeComponent();
            LoadItemData();
            txtIngredients.TextChanged += txtIngredients_TextChanged;
            textBox1.Visible = false;
            txtID.Visible = false;
            txtHiddenID.Visible = false;
            btnUpdate.Visible = false;
            btnRetrieve.Visible = false;
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

                dataGridView1.Columns["ID"].Visible = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            string ingredientsID = textBox1.Text;  // Get the ingredient ID from the hidden textbox
            string qty = txtqty.Text;              // Get the quantity from txtqty
            string itemInventoryID = txtID.Text;   // Get the item inventory ID from the hidden textbox

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

            // Validation 3: Ensure the quantity is a valid number
            if (!int.TryParse(qty, out int parsedQty) || parsedQty <= 0)
            {
                MessageBox.Show("Please enter a valid positive number for quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validation 4: Ensure that the item inventory ID is valid
            if (string.IsNullOrEmpty(itemInventoryID))
            {
                MessageBox.Show("No valid item is selected for inventory.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If all validations pass, proceed to insert the ingredients
            try
            {
                ingredientsTableFetcher.InsertIngredients(ingredientsID, qty, itemInventoryID);

                // Refresh the ingredients data in the DataGridView after inserting
                LoadIngredientData(itemInventoryID);



                // Optionally, clear the fields after successful insertion
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the ingredient: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
    }
}
