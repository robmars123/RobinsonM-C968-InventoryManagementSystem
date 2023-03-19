using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RobinsonM_C968_InventoryManagementSystem
{
    public partial class AddProduct : Form
    {
        private MainScreen mainScreen = new MainScreen();
        private Product product = new Product();
        public AddProduct(MainScreen _mainScreen)
        {
            mainScreen = _mainScreen;
            InitializeComponent();
        }

        private void AddProduct_Load(object sender, EventArgs e)
        {
            dataGridViewParts.DataSource = mainScreen.inventory.AllParts;
            ControlsValidation();
        }
        private string ValidateWholeNumber(TextBox _textbox)
        {
            string errorMessage = string.Empty;
            if (!ValidateNumbersOnly(_textbox.Text))
            {
                errorMessage = "Please enter whole number for " + _textbox.Name.Replace("textBox", "") + ".";
                MessageBox.Show(errorMessage);
                return "";
            }
            return _textbox.Text;
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

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
            mainScreen.Show();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                //product first fields
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

                mainScreen.inventory.addProduct(product);
                this.Close();
                mainScreen.Show();
                mainScreen.RefreshMainscreen();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Part partToBeAdded = null;
            foreach (DataGridViewRow item in this.dataGridViewParts.SelectedRows)
            {
                if (item.Selected)
                    partToBeAdded = item.DataBoundItem as Part;
            }
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

            RefreshGrids();
        }

        private void RefreshGrids()
        {
            dataGridViewAssociatedParts.DataSource = null;
            dataGridViewAssociatedParts.DataSource = product.AssociatedParts;

            dataGridViewParts.ClearSelection();
            dataGridViewAssociatedParts.ClearSelection();
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

                RefreshGrids();
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            ControlsValidation();
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
