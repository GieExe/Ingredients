using Ingredients.Class;
using Ingredients.Object;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using Ingredients.Utilities;
using System.Drawing.Printing;
using System.Collections.Generic;

namespace Ingredients.FORMS
{
    public partial class addIngredients : Form
    {
        private fetchIngredientsTable ingredientsTableFetcher = new fetchIngredientsTable();
        private fetchItemInventory itemInventoryGETSET = new fetchItemInventory();
        private fetchItemAssembly itemAssembly = new fetchItemAssembly();
        private DataTable itemInventoryTable;
        private DataTable filteredInventoryTable;

        private DataTable itemAssemblyTable;
        private DataTable filteredAssemblyTable;

        private string originalValue = "";
        private string originalType = "";

        private PrivateFontCollection _pfc = new PrivateFontCollection();

        private int currentPageInventory = 1;
        private int totalRecordsInventory;
        private int totalPagesInventory;
        private int totalPageSizeInventory = 30;

        // Variables for Assembly Table
        private int currentPageAssembly = 1;
        private int totalRecordsAssembly;
        private int totalPagesAssembly;
        private int totalPageSizeAssembly = 30;

        private string selectedIngredientID;
        public addIngredients()
        {
            InitializeComponent();
            LoadItemData();
            LoadAssemblyData();
            txtIngredients.TextChanged += txtIngredients_TextChanged;
            txtAssemblySearch.TextChanged += txtAssemblySearch_TextChanged;
            txtHiddenID.Visible = false;
            txtID.Visible = false;
            textBox1.Visible = false;
            txtqty.KeyPress += new KeyPressEventHandler(txtqty_KeyPress);
            dataGridView1.CellBeginEdit += new DataGridViewCellCancelEventHandler(dataGridView1_CellBeginEdit);

           txtAssemblySearch.Visible = false;
            this.Load += new EventHandler(addIngredients_Load);
            //btnRefresh_Click(this, EventArgs.Empty);
            //tabControl1.SelectedTab = tabPage1;

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(addIngredients_KeyDown);

            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += new DrawItemEventHandler(tabControl1_DrawItem);



        }
        private void addIngredients_Shown(object sender, EventArgs e)
        {
          

        }
        private void addIngredients_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            LoadAssemblyData();

            btnRefresh_Click(this, EventArgs.Empty);

        }
      

        private void addIngredients_Activated(object sender, EventArgs e)
        {
            txtIngredients.Focus();
            tabControl1.SelectedTab = tabPage1;
        }
        public void SetFullName(string fullName, string itemInventoryID)
        {
            // Assuming you have a Label or TextBox to display the full name
            txtFullName.Text = fullName;
            txtID.Text = itemInventoryID;
        }

        private void LoadItemData()
        {
            itemInventoryTable = itemInventoryGETSET.GetFullName(); // Retrieve inventory data

            if (itemInventoryTable != null && itemInventoryTable.Rows.Count > 0)
            {
                filteredInventoryTable = itemInventoryTable.Copy(); // Initially, filteredInventoryTable is a copy of itemInventoryTable
                totalRecordsInventory = filteredInventoryTable.Rows.Count;
                totalPagesInventory = (int)Math.Ceiling((double)totalRecordsInventory / totalPageSizeInventory);
                lblTotalPage.Text = totalPagesInventory.ToString(); // Display total number of pages

                LoadPage(); // Load the page for inventory

                // Configure DataGridView columns for inventory
                dataGridItemInventory.Columns["ListID"].HeaderText = "List ID";
                dataGridItemInventory.Columns["FullName"].HeaderText = "Ingredients List";

                foreach (DataGridViewColumn column in dataGridItemInventory.Columns)
                {
                    column.ReadOnly = true;
                }
            }
            else
            {
                dataGridAssembly.DataSource = null;
                lblTotalAssembly.Text = "0";
            }

            dataGridItemInventory.Columns["ListID"].Visible = false;
        }

        private void LoadAssemblyData()
        {
            // Ensure that itemAssemblyTable is initialized even if the query returns no rows
            itemAssemblyTable = itemAssembly.GetAllItem(); // Retrieve assembly data

            // Check if the table is null or empty
            if (itemAssemblyTable != null && itemAssemblyTable.Rows.Count > 0)
            {
                // If the table has rows, copy it to filteredAssemblyTable
                filteredAssemblyTable = itemAssemblyTable.Copy();
                totalRecordsAssembly = filteredAssemblyTable.Rows.Count;
                totalPagesAssembly = (int)Math.Ceiling((double)totalRecordsAssembly / totalPageSizeAssembly);
                lblTotalAssembly.Text = totalPagesAssembly.ToString(); // Display total number of pages

                LoadAssemblyPage(); // Load the page for assembly

                // Configure DataGridView columns for assembly
                dataGridAssembly.Columns["ListID"].HeaderText = "List ID";
                dataGridAssembly.Columns["FullName"].HeaderText = "Ingredients List";

                foreach (DataGridViewColumn column in dataGridAssembly.Columns)
                {
                    column.ReadOnly = true;
                }
            }
            else
            {
                // If no data is found, set the DataGridView to show an empty state
                dataGridAssembly.DataSource = null;
                lblTotalAssembly.Text = "0"; // Indicate 0 pages or records
            }

            // Ensure that "ListID" column is hidden even if the table is empty
            if (dataGridAssembly.Columns.Contains("ListID"))
            {
                dataGridAssembly.Columns["ListID"].Visible = false;
            }
        }

        public void LoadIngredientData(string itemInventoryID)
        {
            DataTable ingredientTable = ingredientsTableFetcher.GetIngredientItem(itemInventoryID);

            // Create an empty DataTable with the same structure
            DataTable emptyTable = new DataTable();
            emptyTable.Columns.Add("Item Name");
            emptyTable.Columns.Add("Quantity");
            emptyTable.Columns.Add("Item Inventory ID");
            emptyTable.Columns.Add("TYPE");

            // Check if the retrieved DataTable has rows
            if (ingredientTable != null && ingredientTable.Rows.Count > 0)
            {
                dataGridView1.DataSource = ingredientTable; // Bind the data to the DataGridView

                // Set column headers
                dataGridView1.Columns["ID"].HeaderText = "ID";
                dataGridView1.Columns["ingredient_id"].HeaderText = "Ingredient ID";
                dataGridView1.Columns["CombinedName"].HeaderText = "Item Name";
                dataGridView1.Columns["qty"].HeaderText = "Quantity";
                dataGridView1.Columns["iteminventory_id"].HeaderText = "Item Inventory ID";
                dataGridView1.Columns["type"].HeaderText = "TYPE";

                // Hide the ID and Ingredient ID columns
                dataGridView1.Columns["ID"].Visible = false;
                dataGridView1.Columns["ingredient_id"].Visible = false;

                // Set column order explicitly
                dataGridView1.Columns["CombinedName"].DisplayIndex = 0;  // Item Name
                dataGridView1.Columns["qty"].DisplayIndex = 1;           // Quantity
                dataGridView1.Columns["iteminventory_id"].DisplayIndex = 2; // Item Inventory ID
                dataGridView1.Columns["type"].DisplayIndex = 3;          // TYPE

                dataGridView1.ClearSelection();

                // Set all columns as read-only except the "Quantity" column
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    if (column.Name == "qty" || column.Name == "type")
                    {
                        column.ReadOnly = false; // Allow editing for "Quantity" column
                    }
                    else
                    {
                        column.ReadOnly = true;  // Set all other columns as read-only
                    }
                }

                //// Apply cell formatting and force the grid to refresh
                //dataGridView1.CellFormatting += dataGridView1_CellFormatting;
                //dataGridView1.Refresh(); // Force the DataGridView to refresh, ensuring the formatting takes effect

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
                DataGridViewRow selectedRow = dataGridItemInventory.Rows[e.RowIndex]; // Access the selected row

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
                DataGridViewRow selectedRow = dataGridItemInventory.Rows[e.RowIndex]; // Access the selected row

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
                selectedIngredientID = dataGridViewRow.Cells["ID"].Value.ToString();

              
                string ingredientsID = dataGridViewRow.Cells["ingredient_id"].Value.ToString();
                string qty = dataGridViewRow.Cells["qty"].Value.ToString();
                string itemType = dataGridViewRow.Cells["type"].Value.ToString();

                string fullName = itemInventoryGETSET.GetFullNameUsingListID(ingredientsID);
                string fullNameAssembly = itemAssembly.GetFullNameUsingListID(ingredientsID);

                if (!string.IsNullOrEmpty(fullName))
                {
                    tabControl1.SelectedTab = tabPage1;
                    txtIngredients.Text = fullName;
                    txtAssemblySearch.Text = "No FullName found";
                    txtqty.Focus();
                }
                else if (!string.IsNullOrEmpty(fullNameAssembly))
                {
                    tabControl1.SelectedTab = tabPage2;
                    txtAssemblySearch.Text = fullNameAssembly;
                    txtIngredients.Text = "No FullName found";
                    txtqty.Focus();
                }
                else
                {
                    txtIngredients.Text = "No FullName found";
                    txtAssemblySearch.Text = "No FullName found";
                }

                txtHiddenID.Text = selectedIngredientID; // Optional if you still want to display it
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
                string itemType = dataGridViewRow.Cells["type"].Value.ToString(); // Get the type column value

                // Retrieve the FullName based on the ingredientsID
                string fullName = itemInventoryGETSET.GetFullNameUsingListID(ingredientsID);
                string fullNameAssembly = itemAssembly.GetFullNameUsingListID(ingredientsID);

                if (!string.IsNullOrEmpty(fullName)) // If FullName is found
                {
                    tabControl1.SelectedTab = tabPage1;
                    txtIngredients.Text = fullName;
                    txtAssemblySearch.Text = "No FullName found";
                    txtqty.Focus();

                }
                else if (!string.IsNullOrEmpty(fullNameAssembly))
                {
                    tabControl1.SelectedTab = tabPage2;
                    txtAssemblySearch.Text = fullNameAssembly;
                    txtIngredients.Text = "No FullName found";
                    txtqty.Focus();

                }
                else
                {
                    // Handle the case where no FullName is found
                    txtIngredients.Text = "No FullName found";
                    txtAssemblySearch.Text = "No FullName found";
                }


                txtHiddenID.Text = hiddenID;
                txtqty.Text = qty;
                textBox1.Text = ingredientsID;
                button1.Visible = false;
                btnUpdate.Visible = true;
                btnRetrieve.Visible = true;

                //// Check item type and switch to the appropriate tab
                //if (itemType == "ITEM INVENTORY")
                //{
                //    tabControl1.SelectedTab = tabPage1; // Switch to TabPage1

                //}
                //else if (itemType == "ITEM ASSEMBLY")
                //{
                //    tabControl1.SelectedTab = tabPage2; // Switch to TabPage2
                //}
            }

        }

        private void txtIngredients_TextChanged(object sender, EventArgs e)
        {
            if (itemInventoryTable != null && itemInventoryTable.Rows.Count > 0)
            {
                // Escape apostrophes in the search text
                string searchText = txtIngredients.Text.Replace("'", "''");

                // Use DataTable.Select to filter rows by FullName
                DataRow[] filteredRows = itemInventoryTable.Select(string.Format("FullName LIKE '%{0}%'", searchText));

                // Create a new DataTable from the filtered rows
                filteredInventoryTable = itemInventoryTable.Clone(); // Clone the structure of itemInventoryTable
                foreach (DataRow row in filteredRows)
                {
                    filteredInventoryTable.ImportRow(row); // Import each filtered row
                }

                totalRecordsInventory = filteredInventoryTable.Rows.Count;
                totalPagesInventory = (int)Math.Ceiling((double)totalRecordsInventory / totalPageSizeInventory);

                // Reset pagination to the first page after search
                currentPageInventory = 1;
                lblTotalPage.Text = totalPagesInventory.ToString(); // Update total pages label
                LoadPage(); // Reload page with filtered data
              
            }
            else
            {
                dataGridItemInventory.DataSource = null; // Clear DataGridView if no data
            }

            if (string.IsNullOrEmpty(txtIngredients.Text))
            {
                btnUpdate.Visible = false;
                button1.Visible = true;
                btnRetrieve.Visible = false;
                button2.Visible = true;
                ClearFields();
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
            string qty = txtqty.Text;              // Get the quantity from txtqty
            string itemInventoryID = txtID.Text;   // Get the item inventory ID from the hidden textbox

            // Create an instance of ButtonExecute and pass your fetcher
            ButtonExecute buttonExecute = new ButtonExecute(ingredientsTableFetcher);

            // Call the DetermineItemType method to find out if the ingredient is from iteminventory or itemassembly
            string type = buttonExecute.DetermineItemType(ingredientsID);  // Call DetermineItemType and get the type (ITEM INVENTORY or ITEM ASSEMBLY)


            // Validation 1: Ensure an ingredient is selected
            if (string.IsNullOrEmpty(ingredientsID))
            {
                MessageBox.Show("Please select an ingredient from the list.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIngredients.Focus();

                return;
            }

            // Validation 2: Ensure the quantity field is not empty
            if (string.IsNullOrEmpty(qty))
            {
               
                if (txtIngredients.Text == "")
                {
                    MessageBox.Show("Please enter a quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   
                    tabControl1.SelectedTab = tabPage2;
                    txtAssemblySearch.Focus();

                }
                else if(txtAssemblySearch.Text == "")
                {
                    MessageBox.Show("Please enter a quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   
                    tabControl1.SelectedTab = tabPage1;
                    txtqty.Focus();
                }
                return;
              
            }

            // Validation 3: Ensure the quantity is a valid non-negative decimal number
            if (!decimal.TryParse(qty, out decimal parsedQty) || parsedQty < 0)
            {
                MessageBox.Show("Please enter a valid non-negative decimal number for quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtqty.Focus();
                return;
            }

            // Validation 4: Ensure that the item inventory ID is valid
            if (string.IsNullOrEmpty(itemInventoryID))
            {
                MessageBox.Show("No valid item is selected for inventory.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // Call the refactored method and pass necessary values
            buttonExecute.HandleButtonClick(
                ingredientsID,
                qty,
                itemInventoryID,
                type,               // Pass the determined type
                txtIngredients,
                txtqty,
                LoadIngredientData,
                ClearFields
            );

            ClearFields();
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
            txtAssemblySearch.Clear();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string itemInventoryID = txtID.Text;
            LoadItemData();
            LoadAssemblyData();
            LoadIngredientData(itemInventoryID);
            txtIngredients.Clear();
            button1.Visible = true;
            button2.Visible = true;
            btnUpdate.Visible = false;
            btnRetrieve.Visible = false;
            textBox1.Clear();
            txtqty.Clear();
            txtHiddenID.Clear();
            txtIngredients.Focus();
            txtIngredients.Visible = true;
            txtAssemblySearch.Visible = false;
            txtAssemblySearch.Clear();
            tabControl1.SelectedIndex = 0;


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
            // Validation 3: Ensure the quantity is a valid non-negative decimal number
            if (!decimal.TryParse(qty, out decimal parsedQty) || parsedQty < 0)
            {
                MessageBox.Show("Please enter a valid non-negative decimal number for quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtqty.Focus();
                return;
            }


            ButtonExecute buttonExecute = new ButtonExecute(ingredientsTableFetcher);

            string type = buttonExecute.DetermineItemType(ingredientsID);

            try
            {
                if (ingredientsTableFetcher.CheckIfIngredientExists(ingredientsID, itemInventoryID))
                {
                    
                   
                  
                    ingredientsTableFetcher.UpdateIngredientsIfExist(qty, ingredientsID, itemInventoryID);

                    // Optionally reload data after updating
                    LoadIngredientData(itemInventoryID);
                    button1.Visible = true;
                    btnUpdate.Visible = false;
                    // Clear fields after updating
                    btnRetrieve.Visible = false;
                    button2.Visible = true;
                    ClearFields();
                }
                else
                {
                    // Insert the new ingredient if it does not exist
                    ingredientsTableFetcher.UpdateIngredients(ingredientsID, qty, hiddenID, type);

                    // Optionally reload data after updating
                    LoadIngredientData(itemInventoryID);
                    button1.Visible = true;
                    btnUpdate.Visible = false;
                    // Clear fields after updating
                    btnRetrieve.Visible = false;
                    button2.Visible = true;
                    ClearFields();
                }

             
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while processing the ingredient: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
           

        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this ingredient?", "Confirm Delete",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                
                if (!string.IsNullOrEmpty(selectedIngredientID))
                {
                    ingredientsTableFetcher.DeleteIngredients(selectedIngredientID);

                    string itemInventoryID = txtID.Text;
                    LoadItemData();
                    LoadIngredientData(itemInventoryID);
                    btnUpdate.Visible = false;
                    btnRetrieve.Visible = false;
                    button1.Visible = true;
                    button2.Visible = true;
                    ClearFields();
                }
               
            }
        }


        private bool ContainsNumbersOrDecimals(string input)
        {
            foreach (char c in input)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    return true;
                }
            }
            return false;
        }


        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Store the original value when editing begins
            if (e.ColumnIndex == dataGridView1.Columns["qty"].Index)
            {
                originalValue = dataGridView1.Rows[e.RowIndex].Cells["qty"].Value.ToString();
            }

            if (e.ColumnIndex == dataGridView1.Columns["type"].Index)
            {
                originalType = dataGridView1.Rows[e.RowIndex].Cells["type"].Value.ToString();
            }



        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == dataGridView1.Columns["type"].Index)
            {
                int rowIndex = e.RowIndex;
                string ingredientID = dataGridView1.Rows[rowIndex].Cells["ingredient_id"].Value.ToString();
                string itemInventoryID = dataGridView1.Rows[rowIndex].Cells["iteminventory_id"].Value.ToString();

                // Automatically capitalize the new type
                string newType = dataGridView1.Rows[rowIndex].Cells["type"].Value.ToString().Trim().ToUpper(); // Trim whitespace and capitalize

                // Allowed types
                var allowedTypes = new List<string> { "ITEM INVENTORY", "ITEM ASSEMBLY" };

                // Check if the entered type is valid and does not contain any numbers or decimals
                if (allowedTypes.Contains(newType) && !ContainsNumbersOrDecimals(newType))
                {
                    if (newType != originalType)
                    {
                        var result = MessageBox.Show("Are you sure you want to update this Type?", "Confirm Update",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                ingredientsTableFetcher.UpdateType(newType, ingredientID, itemInventoryID);
                                LoadIngredientData(itemInventoryID);

                                // Clear and reset UI controls as needed
                                button1.Visible = true;
                                btnUpdate.Visible = false;
                                btnRetrieve.Visible = false;
                                button2.Visible = true;
                                ClearFields();
                                txtIngredients.Focus();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error updating Type: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            // Reset the value back to originalValue if user clicks "No"
                            dataGridView1.Rows[rowIndex].Cells["type"].Value = originalType;
                        }
                    }
                }
                else
                {
                    // Show validation error and reset the value
                    MessageBox.Show("Invalid format. Allowed values are 'ITEM INVENTORY' or 'ITEM ASSEMBLY', and no numbers or decimals are allowed.", "Invalid Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataGridView1.Rows[rowIndex].Cells["type"].Value = originalType;
                }
            }


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

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (currentPageInventory != totalPagesInventory)
            {
                currentPageInventory = totalPagesInventory; // Go to the last page
                LoadPage();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPageInventory < totalPagesInventory)
            {
                currentPageInventory++;
                LoadPage(); // Load the next page
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPageInventory > 1)
            {
                currentPageInventory--;
                LoadPage(); // Load the previous page
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (currentPageInventory != 1)
            {
                currentPageInventory = 1; // Go to the first page
                LoadPage();
            }
        }

        private void LoadPage()
        {
            DataTable pagedTable = filteredInventoryTable.Clone(); // Create an empty table with the same structure
            int startIndex = (currentPageInventory - 1) * totalPageSizeInventory;

            // Copy the rows from the filtered table to the paged table
            for (int i = startIndex; i < startIndex + totalPageSizeInventory && i < totalRecordsInventory; i++)
            {
                pagedTable.ImportRow(filteredInventoryTable.Rows[i]);
            }

            dataGridItemInventory.DataSource = pagedTable; // Bind the paged data to the DataGridView
            lblCurrent.Text = currentPageInventory.ToString(); // Update the current page label
        }


        private void txtAssemblySearch_TextChanged(object sender, EventArgs e)
        {
            if (itemAssemblyTable != null && itemAssemblyTable.Rows.Count > 0)
            {
                // Escape apostrophes in the search text
                string searchText = txtAssemblySearch.Text.Replace("'", "''");

                // Use DataTable.Select to filter rows by FullName
                DataRow[] filteredRows = itemAssemblyTable.Select(string.Format("FullName LIKE '%{0}%'", searchText));

                // Create a new DataTable from the filtered rows
                filteredAssemblyTable = itemAssemblyTable.Clone(); // Clone the structure of itemAssemblyTable
                foreach (DataRow row in filteredRows)
                {
                    filteredAssemblyTable.ImportRow(row); // Import each filtered row
                }

                totalRecordsAssembly = filteredAssemblyTable.Rows.Count;
                totalPagesAssembly = (int)Math.Ceiling((double)totalRecordsAssembly / totalPageSizeAssembly);

                // Reset pagination to the first page after search
                currentPageAssembly = 1;
                lblTotalAssembly.Text = totalPagesAssembly.ToString(); // Update total pages label
                LoadAssemblyPage(); // Reload page with filtered data
            }
            else
            {
                dataGridAssembly.DataSource = null; // Clear DataGridView if no data
            }

            if (string.IsNullOrEmpty(txtAssemblySearch.Text))
            {

                btnUpdate.Visible = false;
                button1.Visible = true;
                btnRetrieve.Visible = false;
                button2.Visible = true;
                ClearFields();
            }

        }

        private void btnFirstAssembly_Click(object sender, EventArgs e)
        {
            if (currentPageAssembly != 1)
            {
                currentPageAssembly = 1; // Go to the first page
                LoadAssemblyPage();
            }
        }

        private void btnPrevAssembly_Click(object sender, EventArgs e)
        {
            if (currentPageAssembly > 1)
            {
                currentPageAssembly--;
                LoadAssemblyPage();
            }
        }

        private void btnNextAssembly_Click(object sender, EventArgs e)
        {
            if (currentPageAssembly < totalPagesAssembly)
            {
                currentPageAssembly++;
                LoadAssemblyPage(); // Load the next page
            }
        }

        private void btnLastAssembly_Click(object sender, EventArgs e)
        {
            if (currentPageAssembly != totalPagesAssembly)
            {
                currentPageAssembly = totalPagesAssembly; // Go to the last page
                LoadAssemblyPage();
            }
        }

        private void LoadAssemblyPage()
        {
            DataTable pagedTable = filteredAssemblyTable.Clone(); // Create an empty table with the same structure
            int startIndex = (currentPageAssembly - 1) * totalPageSizeAssembly;

            // Copy the rows from the filtered table to the paged table
            for (int i = startIndex; i < startIndex + totalPageSizeAssembly && i < totalRecordsAssembly; i++)
            {
                pagedTable.ImportRow(filteredAssemblyTable.Rows[i]);
            }

            dataGridAssembly.DataSource = pagedTable; // Bind the paged data to the DataGridView
            lblCurrentAssembly.Text = currentPageAssembly.ToString(); // Update the current page label
        }


        private void dataGridAssembly_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow selectedRow = dataGridAssembly.Rows[e.RowIndex]; // Access the selected row

                // Retrieve the full name and item inventory ID from the selected row

                string ingredientsID = selectedRow.Cells["ListID"].Value.ToString();
                string fullName = selectedRow.Cells["FullName"].Value.ToString();
                // Ensure this matches your DataTable column name

                // Set the full name to the txtIngredients TextBox
                txtAssemblySearch.Text = fullName;
                textBox1.Text = ingredientsID;
                txtqty.Focus();

            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1) // Assuming tabPage1 is itemInventory
            {
                LoadItemData();
                txtIngredients.Visible = true;
                txtAssemblySearch.Visible = false;

                txtAssemblySearch.Clear();
                txtIngredients.Focus();
            }
            else if (tabControl1.SelectedTab == tabPage2) // Assuming tabPage2 is itemAssembly
            {
                LoadAssemblyData();
                txtIngredients.Visible = false;
                txtAssemblySearch.Visible = true;

                txtIngredients.Clear();
                txtAssemblySearch.Focus();
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage tabPage = tabControl1.TabPages[e.Index];
            Rectangle tabRect = tabControl1.GetTabRect(e.Index);

            // Set the background color for the selected tab
            Color backColor = (e.Index == tabControl1.SelectedIndex) ? Color.LightSkyBlue : Color.Gainsboro;

            // Fill the background color of the tab header
            using (SolidBrush brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            // Draw the tab text (optional, you can leave this out if you just want the color)
            TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font, tabRect, Color.Black, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

       
        private void addIngredients_KeyDown(object sender, KeyEventArgs e)
        {
            // Check for the F1 and F2 key presses
            if (e.KeyCode == Keys.F1) // F1 key for Tab 1
            {
                if (tabControl1.TabCount > 0) // Ensure there are tabs
                {
                    tabControl1.SelectedIndex = 0; // Switch to Tab 1
                }
                e.Handled = true; // Mark the event as handled
            }
            else if (e.KeyCode == Keys.F2) // F2 key for Tab 2
            {
                if (tabControl1.TabCount > 1) // Ensure there is a second tab
                {
                    tabControl1.SelectedIndex = 1; // Switch to Tab 2
                }
                e.Handled = true; // Mark the event as handled
            }
          
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "type")
            {
                string itemType = e.Value?.ToString();

                if (itemType == "ITEM INVENTORY")
                {
                    e.CellStyle.ForeColor = Color.Green; // Set text color to green for ITEM INVENTORY
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold); // Set font to bold
                }
                else if (itemType == "ITEM ASSEMBLY")
                {
                    e.CellStyle.ForeColor = Color.Orange; // Set text color to orange for ITEM ASSEMBLY
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold); // Set font to bold
                }
            }

            // Formatting for the "CombinedName" or "Item Name" column based on the type
            if (dataGridView1.Columns[e.ColumnIndex].Name == "CombinedName")
            {
                // Retrieve the value from the "type" column to determine formatting
                string itemType = dataGridView1.Rows[e.RowIndex].Cells["type"].Value?.ToString();

                if (itemType == "ITEM INVENTORY")
                {
                    e.CellStyle.ForeColor = Color.Green; // Set text color to green for ITEM INVENTORY
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold); // Set font to bold
                }
                else if (itemType == "ITEM ASSEMBLY")
                {
                    e.CellStyle.ForeColor = Color.Orange; // Set text color to orange for ITEM ASSEMBLY
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold); // Set font to bold
                }
            }
        }

       
    }
}
