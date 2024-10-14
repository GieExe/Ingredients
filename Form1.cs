using Ingredients.Class;
using Ingredients.FORMS;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using Org.BouncyCastle.Asn1.Pkcs;

namespace Ingredients
{
    public partial class Form1 : Form
    {
        private fetchItemInventory itemInventoryGETSET = new fetchItemInventory();
        private fetchIngredientsTable ingredientsTable = new fetchIngredientsTable();
        private DataTable itemTable; // Holds the original data for the DataGridView
        private DataTable filteredTable; // Holds the filtered data after search
        private PrivateFontCollection _pfc = new PrivateFontCollection();

        // Pagination variables
        private int currentPage = 1;
        private int pageSize = 30; // Number of records per page
        private int totalRecords;
        private int totalPages;

        public Form1()
        {
            InitializeComponent();
            LoadItemData(); // Load the data into the DataGridView when the form loads
            txtSearch.TextChanged += txtSearch_TextChanged; // Subscribe to the TextChanged event
        }
        private void LoadCustomFont()
        {
            // Load "Outfit-Regular.ttf" from resources
            string fontPath = Path.Combine(Application.StartupPath, "Resources", "Outfit", "static", "Outfit-Regular.ttf");


            if (File.Exists(fontPath))
            {
                var fontBytes = File.ReadAllBytes(fontPath);
                IntPtr fontPtr = Marshal.AllocCoTaskMem(fontBytes.Length);
                Marshal.Copy(fontBytes, 0, fontPtr, fontBytes.Length);
                _pfc.AddMemoryFont(fontPtr, fontBytes.Length);
                Marshal.FreeCoTaskMem(fontPtr);
            }
            else
            {
                MessageBox.Show("Font file not found: " + fontPath);
            }

            if (_pfc.Families.Length == 0)
            {
                MessageBox.Show("Font loading failed.");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;  // Optional, resize columns to fit
            dataGridView1.ScrollBars = ScrollBars.Both;  // Enable both horizontal and vertical scrollbars
            txtSearch.Focus();

            LoadCustomFont();

            // Apply custom font to all controls
            // Check if the font family is available before applying it
            if (_pfc.Families.Length > 0)
            {
                foreach (Control control in this.Controls)
                {
                    control.Font = new Font(_pfc.Families[0], control.Font.Size, control.Font.Style);
                }
            }
            else
            {
                MessageBox.Show("Custom font could not be applied.");
            }
            SetDataGridViewHeaderFont(dataGridView1);
        }
        private void SetDataGridViewHeaderFont(DataGridView dgv)
        {
            // Set the header font using the custom font
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(_pfc.Families[0], 14, FontStyle.Bold); // Set the desired size and style
            dgv.DefaultCellStyle.Font = new Font(_pfc.Families[0], 12);

        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void LoadItemData()
        {
            itemTable = itemInventoryGETSET.GetAllItem(); // Retrieve data

            // Check if itemTable contains rows
            if (itemTable != null && itemTable.Rows.Count > 0)
            {
                filteredTable = itemTable.Copy(); // Initially, filteredTable is a copy of itemTable
                totalRecords = filteredTable.Rows.Count;
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                lblTotalPage.Text = totalPages.ToString(); // Display total number of pages
                LoadPage(); // Load the first page
            }
            else
            {
                MessageBox.Show("No items found in the inventory.");
                dataGridView1.DataSource = null; // Clear the DataGridView if no data
            }
        }

        private void LoadPage()
        {
            // Get a subset of the rows for the current page
            DataTable pagedTable = filteredTable.Clone(); // Create an empty table with the same structure
            int startIndex = (currentPage - 1) * pageSize;

            // Copy the rows from the filtered table to the paged table
            for (int i = startIndex; i < startIndex + pageSize && i < totalRecords; i++)
            {
                pagedTable.ImportRow(filteredTable.Rows[i]);
            }

            dataGridView1.DataSource = pagedTable; // Bind the paged data to the DataGridView
            dataGridView1.Columns["ListID"].HeaderText = "List ID";
            dataGridView1.Columns["Name"].HeaderText = "Name";
            dataGridView1.Columns["FullName"].HeaderText = "FullName";

            dataGridView1.Columns["ListID"].Visible = false;

            // Update the current page label
            lblCurrent.Text = currentPage.ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (itemTable != null && itemTable.Rows.Count > 0)
            {
                // Prepare the filter expression to search both 'ListID', 'Name', and 'FullName' columns
                string filterExpression = string.Format("ListID LIKE '%{0}%' OR Name LIKE '%{0}%' OR FullName LIKE '%{0}%'", txtSearch.Text);

                // Create a DataView based on the original DataTable
                DataView dv = new DataView(itemTable);
                dv.RowFilter = filterExpression; // Apply the filter

                // Paginate the filtered results
                filteredTable = dv.ToTable(); // Convert DataView back to DataTable
                totalRecords = filteredTable.Rows.Count;
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                // Reset pagination to the first page after search
                currentPage = 1;
                lblTotalPage.Text = totalPages.ToString(); // Update total pages label
                LoadPage(); // Load the filtered data with pagination
            }
            else
            {
                dataGridView1.DataSource = null; // If there are no items, clear the DataGridView
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string fullName = selectedRow.Cells["FullName"].Value.ToString();
                string itemInventoryID = selectedRow.Cells["ListID"].Value.ToString();
                string Name = selectedRow.Cells["Name"].Value.ToString();

                addIngredients addIngredientsForm = new addIngredients();
                addIngredientsForm.SetFullName(fullName, itemInventoryID);
                addIngredientsForm.LoadIngredientData(itemInventoryID);
                addIngredientsForm.ShowDialog();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string fullName = selectedRow.Cells["FullName"].Value.ToString();
                string itemInventoryID = selectedRow.Cells["ListID"].Value.ToString();
                string Name = selectedRow.Cells["Name"].Value.ToString();

                addIngredients addIngredientsForm = new addIngredients();
                addIngredientsForm.SetFullName(fullName, itemInventoryID);
                addIngredientsForm.LoadIngredientData(itemInventoryID);
                addIngredientsForm.ShowDialog();
            }
        }

        // Pagination button handlers
        private void btnNext_Click_1(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPage(); // Load the next page
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPage(); // Load the previous page
            }
        }

        private void btnLast_Click_1(object sender, EventArgs e)
        {
            if (currentPage != totalPages)
            {
                currentPage = totalPages; // Go to the last page
                LoadPage();
            }
        }

        private void btnFirst_Click_1(object sender, EventArgs e)
        {
            if (currentPage != 1)
            {
                currentPage = 1; // Go to the first page
                LoadPage();
            }
        }
       
    }
}
