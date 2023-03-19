using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RobinsonM_C968_InventoryManagementSystem
{
    public partial class ModifyProduct : Form
    {
        private MainScreen mainScreen;
        private Product product = new Product();
        public ModifyProduct(MainScreen _mainScreen, Product _product)
        {
            mainScreen = _mainScreen;
            product = _product;
            InitializeComponent();
        }

        private void ModifyProduct_Load(object sender, EventArgs e)
        {
            dataGridViewParts.DataSource = mainScreen.inventory.AllParts;
            dataGridViewAssociatedParts.DataSource = product.AssociatedParts;

            textBoxID.Text = product.ProductID.ToString();
            textBoxName.Text = product.Name;
            textBoxMax.Text = product.Max.ToString();
            textBoxMin.Text = product.Min.ToString();
            textBoxPrice.Text = product.Price.ToString();
            textBoxInventory.Text = product.InStock.ToString();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
            mainScreen.Show();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var partToBeAdded = dataGridViewParts.SelectedRows[0].DataBoundItem;
            if (partToBeAdded == null)
            {
                string message = "Please select something to add.";
                MessageBox.Show(message);
            }
            else if (partToBeAdded.GetType() == typeof(InHouse))
                product.addAssociatedPart((InHouse)partToBeAdded);
            else if (partToBeAdded.GetType() == typeof(Outsourced))
            {
                product.addAssociatedPart((Outsourced)partToBeAdded);
            }

            Refresh();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            Part partToBeDeleted = null;

            foreach (DataGridViewRow item in this.dataGridViewAssociatedParts.SelectedRows)
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
                    product.removeAssociatedPart(partToBeDeleted.PartID);

                Refresh(); //refresh grids' list 
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                //fields of product to Modify
                product.Name = textBoxName.Text;
                product.Max = Convert.ToInt32(ValidateWholeNumber(textBoxMax));
                product.Min = Convert.ToInt32(ValidateWholeNumber(textBoxMin));
                product.Price = Convert.ToDecimal(textBoxPrice.Text);
                product.InStock = Convert.ToInt32(ValidateWholeNumber(textBoxInventory));

                if (product.Min > product.Max)
                {
                    string message = "Your minimum exceeds your maximum value.";
                    MessageBox.Show(message);
                    return;
                }

                if (product.InStock > product.Max || product.InStock < product.Min)
                {
                    string message = "Your inventory is outside of min/max range.";
                    MessageBox.Show(message);
                    return;
                }
                //update product entity
                mainScreen.inventory.updateProduct(product.ProductID, product);

                Close();
                mainScreen.Show();
            }
            catch (Exception ex)
            {
            }
        }

        private string ValidateWholeNumber(TextBox textbox)
        {
            string errorMessage = string.Empty;
            if (!ValidateNumbersOnly(textbox.Text))
            {
                errorMessage = "Please enter whole number for " + textbox.Name.Replace("textBox", "") + ".";
                MessageBox.Show(errorMessage);
                return "";
            }
            return textbox.Text;
        }

        private bool ValidateLettersOnly(string letters)
        {
            return Regex.IsMatch(letters, @"^[a-zA-Z ]+$");
        }
        private bool ValidateNumbersOnly(string numbers)
        {
            return Regex.IsMatch(numbers, @"^[0-9]+$");
        }
        private bool ValidateDecimalOnly(string numbers)
        {
            return Regex.IsMatch(numbers, @"^[1-9]\d*(\.\d+)?$");
        }

        private void ControlsValidation()
        {
            if (string.IsNullOrEmpty(textBoxName.Text) || !ValidateLettersOnly(textBoxName.Text))
                textBoxName.BackColor = Color.LightPink;
            else
                textBoxName.BackColor = Color.White;

            if (string.IsNullOrEmpty(textBoxInventory.Text) || !ValidateNumbersOnly(textBoxInventory.Text))
                textBoxInventory.BackColor = Color.LightPink;
            else
                textBoxInventory.BackColor = Color.White;

            if (string.IsNullOrEmpty(textBoxPrice.Text) || !ValidateDecimalOnly(textBoxPrice.Text))
                textBoxPrice.BackColor = Color.LightPink;
            else
                textBoxPrice.BackColor = Color.White;

            if (string.IsNullOrEmpty(textBoxMax.Text) || !ValidateNumbersOnly(textBoxMax.Text))
                textBoxMax.BackColor = Color.LightPink;
            else
                textBoxMax.BackColor = Color.White;

            if (string.IsNullOrEmpty(textBoxMin.Text) || !ValidateNumbersOnly(textBoxMin.Text))
                textBoxMin.BackColor = Color.LightPink;
            else
                textBoxMin.BackColor = Color.White;


            //if required fields are empty, disable Save button
            if (string.IsNullOrEmpty(textBoxName.Text) ||
                   string.IsNullOrEmpty(textBoxInventory.Text) ||
                   string.IsNullOrEmpty(textBoxPrice.Text) ||
                   string.IsNullOrEmpty(textBoxMin.Text) ||
                   string.IsNullOrEmpty(textBoxMax.Text))
                buttonSave.Enabled = false;
            else
                buttonSave.Enabled = true;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            ControlsValidation();
        }

        private void textBoxInventory_TextChanged(object sender, EventArgs e)
        {
            ControlsValidation();
        }

        private void textBoxPrice_TextChanged(object sender, EventArgs e)
        {
            ControlsValidation();
        }

        private void textBoxMax_TextChanged(object sender, EventArgs e)
        {
            ControlsValidation();
        }

        private void textBoxMin_TextChanged(object sender, EventArgs e)
        {
            ControlsValidation();
        }

        private void btnSearchParts_Click(object sender, EventArgs e)
        {
            var listResult = new List<Part>();
            var word = textBoxSearchParts.Text.Trim().ToLower(); // Search by Part Name or Part ID;
                                                                 //PartID
            if (!string.IsNullOrEmpty(word) && word.All(char.IsDigit))
            {
                var part = mainScreen.inventory.lookupPart(Convert.ToInt32(word));
                if (part != null)
                    listResult.Add(part);
            }
            //or Name
            else
                listResult = mainScreen.inventory.AllParts.Where(part => part.Name.ToLower().Contains(word)).ToList();

            if (!string.IsNullOrEmpty(textBoxSearchParts.Text))
                dataGridViewParts.DataSource = listResult;
            else
                dataGridViewParts.DataSource = mainScreen.inventory.AllParts;//restore list if no search term applied.

            if (!listResult.Any())
            {
                string message = "Part could not be found.";
                MessageBox.Show(message);
            }

            textBoxSearchParts.Text = string.Empty;
        }
    }
}
