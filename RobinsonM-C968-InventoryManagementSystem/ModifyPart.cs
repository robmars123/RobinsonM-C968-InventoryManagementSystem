using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RobinsonM_C968_InventoryManagementSystem
{
    public partial class ModifyPart : Form
    {
        private MainScreen mainScreen = new MainScreen();
        private InHouse inHouse = new InHouse();
        private Outsourced outSourced = new Outsourced();

        private int partID;
        private string name;
        private int max;
        private int min;
        private int inventory;
        private decimal price;
        public ModifyPart(MainScreen _mainScreen, InHouse _part)
        {
            mainScreen = _mainScreen;
            inHouse = _part;
            InitializeComponent();
        }
        public ModifyPart(MainScreen _mainScreen, Outsourced _part)
        {
            mainScreen = _mainScreen;
            outSourced = _part;
            InitializeComponent();
        }

        private void ModifyPart_Load(object sender, EventArgs e)
        {
            if (inHouse.MachineID > 0)
            {
                textBoxID.Text = inHouse.PartID.ToString();
                textBoxName.Text = inHouse.Name;
                textBoxMax.Text = inHouse.Max.ToString();
                textBoxMin.Text = inHouse.Min.ToString();
                textBoxPrice.Text = inHouse.Price.ToString();
                textBoxInventory.Text = inHouse.InStock.ToString();
                textBoxMachineID.Text = inHouse.MachineID.ToString();
                radioButtonInHouse.Checked = true;
            }
            else
            {
                textBoxID.Text = outSourced.PartID.ToString();
                textBoxName.Text = outSourced.Name;
                textBoxMax.Text = outSourced.Max.ToString();
                textBoxMin.Text = outSourced.Min.ToString();
                textBoxPrice.Text = outSourced.Price.ToString();
                textBoxInventory.Text = outSourced.InStock.ToString();
                textBoxCompanyName.Text = (string.IsNullOrEmpty(outSourced.CompanyName)) ? "" : outSourced.CompanyName.ToString();
                radioButtonOutsourced.Checked = true;
            }

            ControlsValidation();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
            mainScreen.Show();
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

        private void radioButtonInHouse_CheckedChanged(object sender, EventArgs e)
        {
            labelCompanyName.Visible = false;
            textBoxCompanyName.Visible = false;

            labelMachineID.Visible = true;
            textBoxMachineID.Visible = true;
        }

        private void radioButtonOutsourced_CheckedChanged(object sender, EventArgs e)
        {
            labelCompanyName.Visible = true;
            textBoxCompanyName.Visible = true;

            labelMachineID.Visible = false;
            textBoxMachineID.Visible = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                partID = int.Parse(textBoxID.Text);
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
                    inHouse = new InHouse()
                    {
                        Name = name,
                        InStock = inventory,
                        MachineID = Convert.ToInt32(ValidateWholeNumber(textBoxMachineID)),
                        Max = max,
                        Min = min,
                        Price = price,
                        PartID = partID
                    };
                    mainScreen.inventory.updatePart(inHouse.PartID, inHouse);
                }
                else if (radioButtonOutsourced.Checked)
                {
                    outSourced = new Outsourced()
                    {
                        Name = name,
                        InStock = inventory,
                        CompanyName = textBoxCompanyName.Text,
                        Max = max,
                        Min = min,
                        Price = price,
                        PartID = partID
                    };
                    mainScreen.inventory.updatePart(outSourced.PartID, outSourced);
                }

                Close();
                mainScreen.Show();
            }
            catch (Exception ex)
            {
                return;
            }
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
