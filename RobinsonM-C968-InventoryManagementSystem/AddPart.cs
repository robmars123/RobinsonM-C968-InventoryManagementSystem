using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RobinsonM_C968_InventoryManagementSystem
{
    public partial class AddPart : Form
    {
        private MainScreen mainScreen = new MainScreen();
        private InHouse inHouse;
        private Outsourced outSourced;
        public AddPart(MainScreen _mainScreen)
        {
            mainScreen = _mainScreen;
            InitializeComponent();
        }
        private string name;
        private int max;
        private int min;
        private int inventory;
        private decimal price;

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            mainScreen.Show();
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                name = textBoxName.Text;
                max = Convert.ToInt32(ValidateWholeNumber(textBoxMax));
                min = Convert.ToInt32(ValidateWholeNumber(textBoxMin));
                price = Convert.ToDecimal(textBoxPrice.Text);
                inventory = Convert.ToInt32(ValidateWholeNumber(textBoxInventory));

                if (min > max)
                {
                    string message = "Your minimum exceeds your maximum value.";
                    MessageBox.Show(message);
                    return;
                }

                if (inventory > max || inventory < min)
                {
                    string message = "Your inventory is outside of min/max range.";
                    MessageBox.Show(message);
                    return;
                }

                if (radioButtonInHouse.Checked)
                {
                    inHouse = new InHouse() { Name = name, InStock = inventory, MachineID = Convert.ToInt32(ValidateWholeNumber(textBoxMachineID)), Max = max, Min = min, Price = price, PartID = int.Parse(mainScreen.inventory.AllParts.Count.ToString()) + 1, };
                    mainScreen.inventory.addPart(inHouse);
                }

                else if (radioButtonOutsourced.Checked)
                {
                    outSourced = new Outsourced() { Name = name, InStock = inventory, CompanyName = textBoxCompanyName.Text, Max = max, Min = min, Price = price, PartID = int.Parse(mainScreen.inventory.AllParts.Count.ToString()) + 1, };
                    mainScreen.inventory.addPart(outSourced);
                }

                mainScreen.RefreshMainscreen();
                Close();
                mainScreen.Show();
            }
            catch(Exception ex)
            {
                return;
            }
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

        private void radioButtonInHouse_CheckedChanged(object sender, EventArgs e)
        {
            labelCompanyName.Visible = false;
            textBoxCompanyName.Visible = false;

            labelMachineID.Visible = true;
            textBoxMachineID.Visible = true;
        }

        private void radioButtonOutsourced_CheckedChanged(object sender, EventArgs e)
        {
            labelMachineID.Visible = false;
            textBoxMachineID.Visible = false;

            labelCompanyName.Visible = true;
            textBoxCompanyName.Visible = true;
        }

        private void AddPart_Load(object sender, EventArgs e)
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

            if (string.IsNullOrEmpty(textBoxMachineID.Text) || !ValidateNumbersOnly(textBoxMachineID.Text))
                textBoxMachineID.BackColor = Color.LightPink;
            else
                textBoxMachineID.BackColor = Color.White;

            if (string.IsNullOrEmpty(textBoxCompanyName.Text))
                textBoxCompanyName.BackColor = Color.LightPink;
            else
                textBoxCompanyName.BackColor = Color.White;


            //if required fields are empty, disable Save button
            if (string.IsNullOrEmpty(textBoxName.Text) ||
                   string.IsNullOrEmpty(textBoxInventory.Text) ||
                   string.IsNullOrEmpty(textBoxPrice.Text) ||
                   string.IsNullOrEmpty(textBoxMin.Text) ||
                   string.IsNullOrEmpty(textBoxMax.Text) ||
                   string.IsNullOrEmpty(textBoxMachineID.Text))
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

        private void textBoxCompanyName_TextChanged(object sender, EventArgs e)
        {
            ControlsValidation();
        }

        private void textBoxMachineID_TextChanged(object sender, EventArgs e)
        {
            ControlsValidation();
        }
    }
}
