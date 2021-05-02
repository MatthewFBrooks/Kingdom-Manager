using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kingdom_Manager
{
    public partial class AddTroop : Form
    {
        public AddTroop()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {

            if (! (CountyComboBox.SelectedItem is null))
            {
                if (int.TryParse(TroopNumberBox.Text, out _) && int.Parse(TroopNumberBox.Text) > 0)
                {
                    TroopLevy newTroop;
                    if (TroopLevyRadioButton.Checked)
                    {
                        newTroop = new TroopLevy(CountyComboBox.SelectedItem.ToString(), int.Parse(TroopNumberBox.Text)) { UUID = Guid.NewGuid().ToString() };
                        Program.AddNewTroop(newTroop);
                        this.Close();
                    }
                    else
                    {
                        if (int.TryParse(GoldCostBox.Text, out _) && int.Parse(GoldCostBox.Text) > 0)
                        {
                            newTroop = new Mercenary(CountyComboBox.SelectedItem.ToString(), int.Parse(TroopNumberBox.Text), int.Parse(GoldCostBox.Text)) { UUID = Guid.NewGuid().ToString() };
                            Program.AddNewTroop(newTroop);
                            this.Close();
                        }
                        else
                        {
                            TroopNumberLabel.BackColor = default(Color);
                            GoldCostLabel.BackColor = Color.Red;
                            MessageBox.Show("The gold cost must be a positive integer.");
                        }
                    }
                }
                else
                {
                    GoldCostLabel.BackColor = default(Color);
                    TroopNumberLabel.BackColor = Color.Red;
                    MessageBox.Show("The number of troops must be a positive integer.");
                }
            }
            else
            {
                CountyLabel.BackColor = Color.Red;
                MessageBox.Show("You must select a county for the troop to be stationed in.");
            }
        }

        private void CountyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CountyLabel.BackColor = default(Color);
        }

        private void TroopLevyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            showHideGoldCost();
        }

        private void MercenaryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            showHideGoldCost();
        }

        void showHideGoldCost()
        {
            if (TroopLevyRadioButton.Checked)
            {
                GoldCostLabel.Visible = false;
                GoldCostBox.Visible = false;
            }
            else
            {
                GoldCostLabel.Visible = true;
                GoldCostBox.Visible = true;
            }
        }

    }
}
