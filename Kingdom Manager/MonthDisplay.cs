using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Kingdom_Manager
{
    class MonthDisplay
    {
        public string MonthName { get; set; }
        //private int foodIntake;
        //public int FoodIntake
        //{
        //    get
        //    {
        //        return foodIntake;
        //    }
        //    set
        //    {
        //        foodIntake = value;
        //        if (IntakeBoxes[0] != null)
        //        {
        //            IntakeBoxes[0].Text = foodIntake.ToString();
        //            Program.UpdateResourceGraph(Program.FOOD_INDEX);
        //        }
        //    }
        //}

        //private int goldIntake;
        //public int GoldIntake
        //{
        //    get
        //    {
        //        return goldIntake;
        //    }
        //    set
        //    {
        //        goldIntake = value;
        //        if (IntakeBoxes[1] != null)
        //        {
        //            IntakeBoxes[1].Text = GoldIntake.ToString();
        //            Program.UpdateResourceGraph(Program.GOLD_INDEX);
        //        }
        //    }
        //}

        private int monthNum;
        public int MonthNum
        {
            get
            {
                return monthNum;
            }
            set
            {
                if (value >= 0)
                {
                    monthNum = value;
                }
            }
        }

        public TextBox[] IntakeBoxes;

        public MonthDisplay()
        {
            IntakeBoxes = new TextBox[2];
            IntakeBoxes[0] = new TextBox();
            IntakeBoxes[0].Validating += new System.ComponentModel.CancelEventHandler(this.FoodTextValidate);
            IntakeBoxes[1] = new TextBox();
            IntakeBoxes[1].Validating += new System.ComponentModel.CancelEventHandler(this.GoldTextValidate);
        }

        private void FoodTextValidate(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (int.TryParse(IntakeBoxes[Program.FOOD_INDEX].Text, out _))
            {
                IntakeBoxes[Program.FOOD_INDEX].BackColor = default(Color);
                //FoodIntake = int.Parse(IntakeBoxes[Program.FOOD_INDEX].Text);
                Program.Years[Program.SelectedYear].Months[MonthNum][Program.FOOD_INDEX] = int.Parse(IntakeBoxes[Program.FOOD_INDEX].Text);
                Program.UpdateMonth(Program.Years[Program.SelectedYear].Months[MonthNum]);
                Program.UpdateResourceGraph(Program.FOOD_INDEX);
            }
            else
            {
                IntakeBoxes[Program.FOOD_INDEX].BackColor = Color.Red;
                MessageBox.Show("Error:\nEntered text does not appear to be numeric.");
                e.Cancel = true;
            }
        }

        private void GoldTextValidate(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (int.TryParse(IntakeBoxes[Program.GOLD_INDEX].Text, out _))
            {
                IntakeBoxes[Program.GOLD_INDEX].BackColor = default(Color);
                //GoldIntake = int.Parse(IntakeBoxes[Program.GOLD_INDEX].Text);
                Program.Years[Program.SelectedYear].Months[MonthNum][Program.GOLD_INDEX] = int.Parse(IntakeBoxes[Program.GOLD_INDEX].Text);
                Program.UpdateMonth(Program.Years[Program.SelectedYear].Months[MonthNum]);
                Program.UpdateResourceGraph(Program.GOLD_INDEX);

            }
            else
            {
                IntakeBoxes[Program.GOLD_INDEX].BackColor = Color.Red;
                MessageBox.Show("Error:\nEntered text does not appear to be numeric.");
                e.Cancel = true;
            }
        }
    }
}
