using Ingredients.Class;
using Ingredients.FORMS;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ingredients
{
    public partial class Form1 : Form
    {
        private fetchItemInventory itemInventoryGETSET = new fetchItemInventory();
        private fetchIngredientsTable ingredientsTable = new fetchIngredientsTable();
        private DataTable itemTable; // Holds the data for the DataGridView

        public Form1()
        {
            InitializeComponent();
            LoadItemData(); // Load the data into the DataGridView when the form loads
            txtSearch.TextChanged += txtSearch_TextChanged; // Subscribe to the TextChanged event
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;  // Optional, resize columns to fit
            dataGridView1.ScrollBars = ScrollBars.Both;  // Enable both horizontal and vertical scrollbars
            txtSearch.Focus();
        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void LoadItemData()
        {
            itemTable = itemInventoryGETSET.GetAllItem(); // Retrieve data

            // Check if itemTable contains rows
            if (itemTable != null && itemTable.Rows.Count >= 0)
            {
                dataGridView1.DataSource = itemTable; // Bind the data directly to the DataGridView
                dataGridView1.Columns["ListID"].HeaderText = "List ID";
                dataGridView1.Columns["Name"].HeaderText = "Name";
                dataGridView1.Columns["FullName"].HeaderText = "FullName";

                dataGridView1.Columns["ListID"].Visible = false;
            }
            else
            {
                // Handle case where no items are retrieved
                MessageBox.Show("No items found in the inventory.");
                dataGridView1.DataSource = null; // Clear the DataGridView if no data
            }
        }



        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (itemTable != null && itemTable.Rows.Count > 0)
            {
                // Prepare the filter expression to search both 'Name' and 'FullName' columns
                string filterExpression = string.Format("ListID LIKE '%{0}%' OR Name LIKE '%{0}%' OR FullName LIKE '%{0}%'", txtSearch.Text);

                // Create a DataView based on the original DataTable
                DataView dv = new DataView(itemTable);
                dv.RowFilter = filterExpression; // Apply the filter

                // Bind the filtered DataView to the DataGridView
                dataGridView1.DataSource = dv;
            }
            else
            {
                // If there are no items, clear the DataGridView
                dataGridView1.DataSource = null;
               
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is valid
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Retrieve the full name and item inventory ID from the selected row
                // Retrieve the full name and item inventory ID from the selected row
                string fullName = selectedRow.Cells["FullName"].Value.ToString(); // Adjust column name if necessary
                string itemInventoryID = selectedRow.Cells["ListID"].Value.ToString(); // Adjust this to match your actual column name
                string Name = selectedRow.Cells["Name"].Value.ToString();
                // Create an instance of the AddIngredients form
                addIngredients addIngredientsForm = new addIngredients();

                // Set the full name in the AddIngredients form
                addIngredientsForm.SetFullName(fullName, itemInventoryID);

                // Load the ingredient data based on the item inventory ID
                addIngredientsForm.LoadIngredientData(itemInventoryID);

                // Show the AddIngredients form
                addIngredientsForm.ShowDialog(); // Show as a modal dialog
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            // Check if the clicked cell is valid
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Retrieve the full name and item inventory ID from the selected row
                string fullName = selectedRow.Cells["FullName"].Value.ToString(); // Adjust column name if necessary
                string itemInventoryID = selectedRow.Cells["ListID"].Value.ToString(); // Adjust this to match your actual column name
                string Name = selectedRow.Cells["Name"].Value.ToString();
                // Create an instance of the AddIngredients form
                addIngredients addIngredientsForm = new addIngredients();

                // Set the full name in the AddIngredients form
                addIngredientsForm.SetFullName(fullName, itemInventoryID);

                // Load the ingredient data based on the item inventory ID
                addIngredientsForm.LoadIngredientData(itemInventoryID);

                // Show the AddIngredients form
                addIngredientsForm.ShowDialog(); // Show as a modal dialog
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
