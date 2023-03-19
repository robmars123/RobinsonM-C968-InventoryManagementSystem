using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RobinsonM_C968_InventoryManagementSystem
{
    public partial class MainScreen : Form
    {
        public Inventory inventory = new Inventory();
        private AddPart addPartForm;
        private ModifyPart modifyPart;
        private AddProduct addProductForm;
        private ModifyProduct modifyProductForm;
        public MainScreen()
        {
            inventory.LoadSampleData();
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {
            dataGridViewParts.DataSource = inventory.AllParts;
            dataGridViewProducts.DataSource = inventory.Products;
        }

        private void btnAddPart_Click(object sender, EventArgs e)
        {
            addPartForm = new AddPart(this);
            addPartForm.Show();
            this.Visible = false;
        }

        private void btnModifyPart_Click(object sender, EventArgs e)
        {
            Part selectedPart = null;
            foreach (DataGridViewRow item in this.dataGridViewParts.SelectedRows)
            {
                if (item.Selected)
                    selectedPart = item.DataBoundItem as Part;
            }

            if (selectedPart == null)
            {
                string message = "Please select something to modify.";
                MessageBox.Show(message);
                return;
            }

            if (selectedPart.GetType() == typeof(InHouse))
            {
                modifyPart = new ModifyPart(this, (InHouse)selectedPart);
            }
            else
                modifyPart = new ModifyPart(this, (Outsourced)selectedPart);

            modifyPart.Show();
            this.Visible = false;
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            addProductForm = new AddProduct(this);
            addProductForm.Show();
            this.Visible = false;
        }

        private void btnDeletePart_Click(object sender, EventArgs e)
        {
            Part partToBeDeleted = null;

            foreach (DataGridViewRow item in this.dataGridViewParts.SelectedRows)
            {
                if (item.Selected)
                    partToBeDeleted = item.DataBoundItem as Part;
            }
            if (partToBeDeleted == null)
            {
                string message = "Please select something to delete.";
                MessageBox.Show(message);
            }
            else
            {
                string message = "Are you sure you want to delete this part?";
                DialogResult result = MessageBox.Show(message, null, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (partToBeDeleted.GetType() == typeof(InHouse))
                    {
                        inventory.deletePart((InHouse)partToBeDeleted);
                    }
                    else
                        inventory.deletePart((Outsourced)partToBeDeleted);

                    RefreshMainscreen();
                }
            }
        }

        private void btnSearchParts_Click(object sender, EventArgs e)
        {
            var listResult = new List<Part>();
            var word = textBoxSearchParts.Text.Trim().ToLower(); // Search by Part Name or Part ID;
            //PartID
            if (!string.IsNullOrEmpty(word) && word.All(char.IsDigit))
            {
                var part = inventory.lookupPart(Convert.ToInt32(word));
                if (part != null)
                    listResult.Add(part);
            }
            //or Name
            else
                listResult = inventory.AllParts.Where(part => part.Name.ToLower().Contains(word)).ToList();

            if (!string.IsNullOrEmpty(textBoxSearchParts.Text))
                dataGridViewParts.DataSource = listResult;
            else
                dataGridViewParts.DataSource = inventory.AllParts;//restore list if no search term applied.

            if (!listResult.Any())
            {
                string message = "Part could not be found.";
                MessageBox.Show(message);
            }

            textBoxSearchParts.Text = string.Empty;
        }

        public void RefreshMainscreen()
        {
            dataGridViewParts.DataSource = null;
            dataGridViewProducts.DataSource = null;

            dataGridViewParts.DataSource = inventory.AllParts;
            dataGridViewProducts.DataSource = inventory.Products;
            dataGridViewParts.ClearSelection();
            dataGridViewProducts.ClearSelection();
        }

        private void MainScreen_Click(object sender, EventArgs e)
        {
            RefreshMainscreen();
        }

        private void btnModifyProduct_Click(object sender, EventArgs e)
        {
            Product selectedProduct = new Product();
            foreach (DataGridViewRow item in this.dataGridViewProducts.SelectedRows)
            {
                if (item.Selected)
                    selectedProduct = item.DataBoundItem as Product;
            }

            if (selectedProduct.ProductID == 0)
            {
                string message = "Please select something to modify.";
                MessageBox.Show(message);
            }
            else
            {
                ModifyProduct modifyProductForm = new ModifyProduct(this, selectedProduct);
                modifyProductForm.Show();
                this.Visible = false;
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            var selectedProduct = new Product();

            //Foreach is being used for multi-select but SOFTWARE I – C# — C968 only 1 selection is required.
            foreach (DataGridViewRow item in this.dataGridViewProducts.SelectedRows)
            {
                if (item.Selected)
                    selectedProduct = item.DataBoundItem as Product;

                //if this product does have children(parts) do not delete it.
                var children = selectedProduct.AssociatedParts;
                if (children.Any())
                {
                    string message = "This product has associated parts. Could not be deleted.";
                    MessageBox.Show(message);
                    return;
                }
            }

            if (selectedProduct.ProductID == 0)
            {
                string message = "Please select something to delete.";
                MessageBox.Show(message);
            }
            else
            {
                string message = "Are you sure you want to delete this product?";
                DialogResult result = MessageBox.Show(message, null, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    inventory.removeProduct(selectedProduct.ProductID);
                    RefreshMainscreen();
                }
            }
        }

        private void btnSearchProducts_Click(object sender, EventArgs e)
        {
            var listResult = new List<Product>();
            var word = textBoxSearchProducts.Text.Trim().ToLower(); // Search by Part Name or Part ID;
            //PartID
            if (!string.IsNullOrEmpty(word) && word.All(char.IsDigit))
            {
                var product = inventory.lookupProduct(Convert.ToInt32(word));
                if (product != null)
                    listResult.Add(product);
            }
            //or Name
            else
                listResult = inventory.Products.Where(p => p.Name.ToLower().Contains(word)).ToList();

            if (!string.IsNullOrEmpty(textBoxSearchProducts.Text))
                dataGridViewProducts.DataSource = listResult;
            else
                dataGridViewProducts.DataSource = inventory.Products;//restore list if no search term applied.

            if (!listResult.Any())
            {
                string message = "Product could not be found.";
                MessageBox.Show(message);
            }

            textBoxSearchProducts.Text = string.Empty;
        }
    }
}
